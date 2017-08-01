using System;
using Oracle.DataAccess.Types;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

using Oracle.DataAccess.Client;

namespace DllSqlBulkCopy
{
    public enum BBDDType
    {
        SqlServer,
        Oracle
    };

    public class clsCopy
    {
        private String sSourceTableScheme;
        /// <summary>
        /// Source Table Scheme. "dbo" by default.
        /// </summary>
        public String SourceTableScheme
        {
            get { return sSourceTableScheme; }
            set { sSourceTableScheme = value; }
        }

        private String sSourceTable;
        /// <summary>
        /// Source Table name. Should be clustered indexed by numeric field.
        /// </summary>
        public String SourceTable
        {
            get { return sSourceTable; }
            set { sSourceTable = value; }
        }

        private String sDestTable;
        /// <summary>
        /// Destination Table name.
        /// </summary>
        public String DestTable
        {
            get { return sDestTable; }
            set { sDestTable = value; }
        }

        private String sSourceConnectionString;
        /// <summary>
        /// Connection string to Source BBDD.
        /// </summary>
        public String SourceConnectionString
        {
            get { return sSourceConnectionString; }
            set { sSourceConnectionString = value; }
        }

        private BBDDType tSourceBBDDType;
        /// <summary>
        /// Relational BBDD Engine in Source.
        /// </summary>
        public BBDDType SourceBBDDType
        {
            get { return tSourceBBDDType; }
            set { tSourceBBDDType = value; }
        }

        private String sDestinationConnectionString;
        /// <summary>
        /// Connection string to Destination BBDD.
        /// </summary>
        public String DestinationConnectionString
        {
            get { return sDestinationConnectionString; }
            set { sDestinationConnectionString = value; }
        }

        private BBDDType tDestinationBBDDType;
        /// <summary>
        /// Relational BBDD Engine in Destination.
        /// </summary>
        public BBDDType DestinationBBDDType
        {
            get { return tDestinationBBDDType; }
            set { tDestinationBBDDType = value; }
        }

        private int iNotifyAfter = 0;
        /// <summary>
        /// Number of rows to copy before the SqlRowsCopied notification event fires.
        /// </summary>
        public int NotifyAfter
        {
            get { return iNotifyAfter; }
            set { iNotifyAfter = value; }
        }
        private int iNumThreads = 1;
        /// <summary>
        /// Number of SqlBulkCopy instances to copy data in parallel. For better performance it should be equal to the number of destination server's processors.
        /// </summary>
        public int NumThreads
        {
            get { return iNumThreads; }
            set { iNumThreads = value; }
        }

        private bool bTabLock = true;
        /// <summary>
        /// Adds the TABLOCK argument to BULK INSERT command. Enabled by default.
        /// </summary>
        public bool TabLock
        {
            get { return bTabLock; }
            set { bTabLock = value; }
        }

        private bool bCheckConstraints = false;
        /// <summary>
        /// Adds the CHECK_CONSTRAINTS argument to BULK INSERT command. Disabled by default.
        /// </summary>
        public bool CheckConstraints
        {
            get { return bCheckConstraints; }
            set { bCheckConstraints = value; }
        }

        private bool bKeepNulls = false;
        /// <summary>
        /// Adds the KEEP_NULLS argument to BULK INSERT command. Disabled by default.
        /// </summary>
        public bool KeepNulls
        {
            get { return bKeepNulls; }
            set { bKeepNulls = value; }
        }

        private bool bKeepIdentity = false;
        /// <summary>
        /// Adds the KEEP_IDENTITY argument to BULK INSERT command. Disabled by default.
        /// </summary>
        public bool KeepIdentity
        {
            get { return bKeepIdentity; }
            set { bKeepIdentity = value; }
        }

        private bool bFireTriggers = false;
        /// <summary>
        /// Adds the FIRE_TRIGGERS argument to BULK INSERT command. Disabled by default.
        /// </summary>
        public bool FireTriggers
        {
            get { return bFireTriggers; }
            set { bFireTriggers = value; }
        }

        private String sSelectFilter;
        /// <summary>
        /// T-SQL Select statement to filter the data to copy. It should return in the first column
        /// the values of the clustered index field which we want to be transferred.
        /// </summary>
        public String SelectFilter
        {
            get { return sSelectFilter; }
            set { sSelectFilter = value; }
        }

        private bool bNoLock = false;
        /// <summary>
        /// Adds the WITH (READUNCOMMITTED) hint to all reads from source table.
        /// </summary>
        public bool NoLock
        {
            get { return bNoLock; }
            set { bNoLock = value; }
        }

        private bool bHeapMethod = false;
        /// <summary>
        /// if true, skip the index and statistics checks.
        /// </summary>
        public bool HeapMethod
        {
            get { return bHeapMethod; }
            set { bHeapMethod = value; }
        }

        public delegate void SqlRowsCopiedDelegate(object sender, long lRowsCopied, IclsThreadCopy oTc);
        /// <summary>
        /// It fires each time the number of rows specified in ClsCopy.NotifyAfter property has been copied.
        /// </summary>
        public event SqlRowsCopiedDelegate SqlRowsCopied;

        public delegate void SqlThreadFinishedDelegate(object sender, IclsThreadCopy oTc, String sErr);
        /// <summary>
        /// It fires when the copy proccess of any thread come to end.
        /// </summary>
        public event SqlThreadFinishedDelegate ThreadFinished;



        private String[] sValuesFilter = new String[0];
        private clsThreadCopy[] oTc;
        static object oThreadLocker = new object();
        private int iIndexFieldDataType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SourceTable">Source Table name. Should be clustered indexed by numeric field.</param>
        /// <param name="DestTable">Destination Table name.</param>
        /// <param name="sourceConnectionString">Connection string to Source BBDD.</param>
        /// <param name="destinationConnectionString">Connection string to Destination BBDD.</param>
        public clsCopy(String SourceTable, String DestTable, String sourceConnectionString, String destinationConnectionString)
        {
            this.SourceTable = SourceTable;
            this.DestTable = DestTable;
            this.SourceConnectionString = sourceConnectionString;
            this.DestinationConnectionString = destinationConnectionString;
            this.SourceTableScheme = "dbo";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SourceTable">Source Table name. Should be clustered indexed by numeric field.</param>
        /// <param name="DestTable">Destination Table name.</param>
        /// <param name="sourceConnectionString">Connection string to Source BBDD.</param>
        /// <param name="destinationConnectionString">Connection string to Destination BBDD.</param>
        /// <param name="numThreads">Number of SqlBulkCopy instances to copy data in parallel. For better performance it should be equal to the number of destination server's processors.</param>
        public clsCopy(String SourceTable, String DestTable, String sourceConnectionString, String destinationConnectionString, int numThreads)
        {
            this.SourceTable = SourceTable;
            this.DestTable = DestTable;
            this.SourceConnectionString = sourceConnectionString;
            this.DestinationConnectionString = destinationConnectionString;
            this.NumThreads = numThreads;
            this.SourceTableScheme = "dbo";
        }

        /// <summary>
        /// Starts the process to copy data.
        /// </summary>
        /// <param name="bAppendData">If false, the destination table will be truncated.</param>
        /// <returns>Empty string if everything was ok. Else returns the error's message.</returns>
        public String Copy(bool bAppendData)
        {
            
            //We are going to need one destination connection and as many source connections as number of parallel threads.
            DbConnection sqlDest;
            DbConnection[] sqlSource;
            if (tDestinationBBDDType == BBDDType.SqlServer)
            {
                sqlDest = new SqlConnection(sDestinationConnectionString);                
            }
            else
            {
                sqlDest = new OracleConnection(sDestinationConnectionString);                
            }
            if (tSourceBBDDType == BBDDType.SqlServer)
            {
                sqlSource = new SqlConnection[iNumThreads];
            }
            else
            {
                sqlSource = new OracleConnection[iNumThreads];
            }

            byte byConnectionsOpened = 0;
            String sRet = "";

            try
            {
                //We open the connections                
                for (int m = 0; m < iNumThreads; m++)
                {
                    if (tSourceBBDDType == BBDDType.SqlServer)
                        sqlSource[m] = (DbConnection)new SqlConnection(sSourceConnectionString);
                    else
                        sqlSource[m] = (DbConnection)new OracleConnection(sSourceConnectionString);
                    sqlSource[m].Open();
                    byConnectionsOpened++;
                }
                sqlDest.Open();

                //We check the filter select.                
                if (!string.IsNullOrEmpty(sSelectFilter))
                {
                    sRet = FillSelectFilterValues(ref sqlSource[0]);
                    if (sRet != "")
                    {
                        throw new Exception(sRet);
                    }
                }

                String[][] sRangeLowKeys = new String[iNumThreads][];
                String[][] sRangeHighKeys = new String[iNumThreads][];
                int[] iSize = new int[iNumThreads];
                String sIndexName = "";
                String sIndexField = "";
                //We check if destination table exists.
                sRet = CheckDestinationTable(bAppendData, sqlDest);
                if (sRet != "")
                {
                    throw new Exception(sRet);
                }
                //if heap method we skip the index statistics.
                if ((!bHeapMethod))
                {
                    //We check if the source table is clustered indexed.
                    sRet = CheckSourceTable( sqlSource[0], ref sIndexName, ref sIndexField, ref bHeapMethod);
                    if (sRet != "")
                    {
                        throw new Exception(sRet);
                    }
                    //Looking at source table clustered index´s statistics, we calculate nearly equal row 
                    //based ranges to balance the parallel copy of data.     
                    if (!bHeapMethod)
                    {
                        sRet = CalculateRangeKeys(sIndexName, sIndexField, sqlSource[0], ref sRangeLowKeys, ref sRangeHighKeys, ref iSize);
                        if (sRet != "")
                        {
                            throw new Exception(sRet);
                        }
                    }
                }

                //We check which SqlBulkCopy options that has been selected.
                int iBcOptions = 0;
                if (bTabLock)
                    iBcOptions += (int)SqlBulkCopyOptions.TableLock;
                if (bKeepIdentity)
                    iBcOptions += (int)SqlBulkCopyOptions.KeepIdentity;
                if (bKeepNulls)
                    iBcOptions += (int)SqlBulkCopyOptions.KeepNulls;
                if (bCheckConstraints)
                    iBcOptions += (int)SqlBulkCopyOptions.CheckConstraints;
                if (bFireTriggers)
                    iBcOptions += (int)SqlBulkCopyOptions.FireTriggers;


                oTc = new clsThreadCopy[iNumThreads];
                for (int i = 0; i < iNumThreads; i++)
                {
                    //We initialize and configure with different source connection,ranges,size..etc
                    //as many clsThreadCopy´s instances as number of threads will execute in parallel.
                    oTc[i] = new clsThreadCopy();
                    oTc[i].IdThread = i;
                    oTc[i].SqlSource = sqlSource[i];
                    oTc[i].SRangeLowKeys = sRangeLowKeys[i];
                    oTc[i].SRangeHighKeys = sRangeHighKeys[i];
                    oTc[i].SValuesFilter = sValuesFilter;
                    oTc[i].Size = iSize[i];
                    oTc[i].SDest = sDestinationConnectionString;
                    oTc[i].SSourceTable = sSourceTable;
                    oTc[i].SSourceTableScheme = sSourceTableScheme;
                    oTc[i].SDestTable = sDestTable;
                    oTc[i].SIndexField = sIndexField;
                    oTc[i].IBcOptions = iBcOptions;
                    oTc[i].INotifyAfter = iNotifyAfter;
                    oTc[i].IIndexFieldDataType = iIndexFieldDataType;
                    oTc[i].NoLock = bNoLock;
                    oTc[i].HeapMethod = bHeapMethod;
                    oTc[i].NumThreads = iNumThreads;
                    oTc[i].SourcebbddType = SourceBBDDType;
                    oTc[i].DestbbddType = DestinationBBDDType;
                    //Each time the clsThreadCopy fires the SqlRowsCopiedThreadEvent event it will call to the OnSqlRowsCopied function.
                    oTc[i].SqlRowsCopiedThreadEvent += new clsThreadCopy.SqlRowsCopiedEventDelegate(OnSqlRowsCopied);

                    WaitCallback w = new WaitCallback(CopyData);
                    //For each instance of clsThreadCopy we create one thread which call CopyData function.
                    ThreadPool.QueueUserWorkItem(w, oTc[i]);
                }
                //We wait until all threads end.
                lock (oThreadLocker)
                {
                    while (iNumThreads > 0)
                    {
                        Monitor.Wait(oThreadLocker);
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
            finally
            {
                //We close connections.
                for (int i = 0; i < byConnectionsOpened; i++)
                    if (sqlSource[i].State == System.Data.ConnectionState.Open)
                        sqlSource[i].Close();
                if (sqlDest.State == System.Data.ConnectionState.Open)
                    sqlDest.Close();
            }

            return "";
        }

        private String CheckSourceTable( DbConnection sqlSource, ref String sIndexName, ref String sIndexField, ref bool heapmethod)
        {
            try
            {
                if (tSourceBBDDType == BBDDType.SqlServer)
                {
                    using (SqlDataReader drs = new SqlCommand("SP_HELPINDEX [" + sSourceTableScheme + "." + sSourceTable + "]", (SqlConnection)sqlSource).ExecuteReader(System.Data.CommandBehavior.SingleResult))
                    {
                        while (drs.Read())
                        {
                            if (drs.GetString(1).IndexOf("clustered", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                sIndexName = drs.GetString(0);
                                sIndexField = drs.GetString(2).Split(',')[0];
                                break;
                            }
                        }
                    }
                    if (sIndexName == "")
                    {
                        heapmethod = true;
                        //throw new Exception("Source table should be clustered indexed.");
                    }
                    else
                    {
                        using (SqlDataReader drs = new SqlCommand("SELECT XTYPE FROM SYSCOLUMNS WHERE ID = OBJECT_ID('" +
                                            sSourceTableScheme + "." + sSourceTable + "')" +
                                            "AND NAME = '" + sIndexField + "'", (SqlConnection)sqlSource).ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {
                            if (drs.Read())
                            {
                                iIndexFieldDataType = int.Parse(drs.GetValue(0).ToString());
                            }
                            else
                            {
                                throw new Exception("Unknowned clustered index field datatype.");
                            }
                        }
                    }
                }
                else
                {                    
                    //we search for FREQUENCY histograms in some index column
                    using (OracleDataReader drs = new OracleCommand(@"select column_name,data_type,data_scale from 
                                (select tc.column_name,tc.data_type,tc.data_scale from user_indexes u
                                inner join USER_IND_COLUMNS c on u.index_name=c.INDEX_NAME and u.table_name=c.table_name
                                INNER join user_tab_columns tc on tc.column_name=c.column_name and tc.table_name=c.table_name
                                where u.table_name='" + sSourceTable + @"' and u.status='VALID' and u.index_type='NORMAL'
                                and c.column_position=1 and tc.histogram='FREQUENCY'
                                ORDER BY u.DISTINCT_KEYS desc,u.LEAF_BLOCKS asc) 
                                where rownum=1", (OracleConnection)sqlSource).ExecuteReader(System.Data.CommandBehavior.SingleResult))
                    {
                        if (drs.HasRows)
                        {
                            drs.Read();
                            sIndexField = drs.GetString(0);
                            switch (drs.GetString(1))
                            {
                                case "NUMBER":
                                    if (int.Parse(drs.GetValue(2).ToString())>0)
                                        iIndexFieldDataType = 108; //numeric con precision
                                    else
                                        iIndexFieldDataType = 127;
                                    break;
                                default:
                                    iIndexFieldDataType = 0; //string
                                    break;
                            }
                        }
                        else
                        {
                            heapmethod = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
            return "";
        }

        private String CalculateRangeKeys(String sIndexName, String sIndexField,  DbConnection psqlSource, ref String[][] sRangeLowKeys, ref String[][] sRangeHighKeys, ref int[] iSize)
        {
            try
            {   
                OracleConnection oraSource;
                SqlConnection sqlSource ;

       

                String sRet = "";
                int i = 0;
                int iLower = 0;
                int iLowerSize = int.MaxValue;

                String[][] sStatisticsKeys = { new String[0], new String[0], new String[0], new String[0] };
                
                String sSql = "";
                sSql = "SELECT COUNT(*) FROM " + sSourceTableScheme + "." + sSourceTable;

                if (bNoLock)
                {
                    sSql += " WITH (READUNCOMMITTED) ";
                }

                sSql += " WHERE " + sIndexField + " IS NULL";
               

                if (tSourceBBDDType==BBDDType.SqlServer){
                    sqlSource= (SqlConnection)psqlSource;
                    sRet = GetSourceTableStatistics(sIndexName, ref sqlSource, ref sStatisticsKeys);
                    //We check null values.
                    using (SqlDataReader drs = new SqlCommand(sSql, sqlSource).ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (drs.Read())
                        {
                            iSize[0] = int.Parse(drs[0].ToString());
                        }
                        else
                        {
                            throw new Exception("Error counting clustered index field null values.");
                        }
                    }
                }else{
                    oraSource=(OracleConnection)psqlSource;
                    sRet = GetSourceTableStatistics(sIndexField, ref oraSource, ref sStatisticsKeys);
                    //We check null values.
                    using (OracleDataReader drs = new OracleCommand(sSql, oraSource).ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (drs.Read())
                        {
                            iSize[0] = int.Parse(drs[0].ToString());
                        }
                        else
                        {
                            throw new Exception("Error counting clustered index field null values.");
                        }
                    }
                }
                
                
                if (sRet != "")
                {
                    throw new Exception(sRet);
                }

               

                string sFirstKey = "";
                string sLastKey = "";

                sFirstKey = sStatisticsKeys[0][0];
                sLastKey = sStatisticsKeys[0][sStatisticsKeys[0].Length - 1];

                quicksort(sStatisticsKeys, 0, sStatisticsKeys[0].Length - 1, false, 2);

                for (i = 0; i < iNumThreads; i++)
                {
                    sRangeHighKeys[i] = new String[0];
                    sRangeLowKeys[i] = new String[0];
                }
                for (int p = 0; p < sStatisticsKeys[0].Length; p++)
                {
                    i = 0;
                    iLower = 0;
                    iLowerSize = int.MaxValue;
                    while ((i < iNumThreads))
                    {
                        if (iSize[i] < iLowerSize)
                        {
                            iLower = i;
                            iLowerSize = iSize[i];
                        }
                        i++;
                    }
                    if (sStatisticsKeys[0][p] == sLastKey)
                    {
                        AddItemToArray(ref sRangeHighKeys[iLower], "*");
                    }
                    else
                    {
                        AddItemToArray(ref sRangeHighKeys[iLower], sStatisticsKeys[0][p]);
                    }


                    if (sStatisticsKeys[0][p] == sFirstKey)
                    {
                        AddItemToArray(ref sRangeLowKeys[iLower], "*");
                    }
                    else
                    {
                        AddItemToArray(ref sRangeLowKeys[iLower], sStatisticsKeys[3][p]);
                    }

                    iSize[iLower] += int.Parse(sStatisticsKeys[1][p]) + int.Parse(sStatisticsKeys[2][p]);
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
            return "";
        }

        private static void AddItemToArray(ref String[] sArray, String sValue)
        {
            String[] sTemp;
            sTemp = sArray;
            sArray = new String[sArray.Length + 1];
            sTemp.CopyTo(sArray, 0);
            sArray[sArray.Length - 1] = sValue;
            return;
        }

        private String GetSourceTableStatistics(String sIndexName, ref SqlConnection sqlSource, ref String[][] sStatisticsKeys)
        {
            try
            {
               
                using (SqlDataReader drs = new SqlCommand("DBCC SHOW_STATISTICS('" + sSourceTableScheme + "." + sSourceTable + "','" + sIndexName + "')", sqlSource).ExecuteReader())
                {
                    //We move to histogram resultset
                    if (drs.NextResult())
                    {
                        if (!drs.NextResult())
                        { throw new Exception("DBCC SHOW_STATISTICS only returned two resultsets."); }
                    }
                    else { throw new Exception("DBCC SHOW_STATISTICS only returned one resultset."); }

                     FillArrayStatistics(ref sStatisticsKeys,  drs);
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
            return "";
        }

        private String GetSourceTableStatistics(String sIndexField, ref OracleConnection sqlSource, ref String[][] sStatisticsKeys)
        {
            try
            {
                string sql = @"select endpoint_value as column_value,
                        endpoint_number - lag(endpoint_number,1,0) over (order by endpoint_value) as frequency,0 as filler
                        from all_tab_histograms
                        where owner='" + sSourceTableScheme + @"'
                        and table_name='" + sSourceTable + @"'
                        and column_name='" + sIndexField + "'";

                using (OracleDataReader drs = new OracleCommand(sql, sqlSource).ExecuteReader())
                {                   
                    FillArrayStatistics(ref sStatisticsKeys, drs);
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
            return "";
        }

        private void FillArrayStatistics(ref String[][] sStatisticsKeys, DbDataReader drs)
        {
            String sPreviousKey = "";
            //statistics from oracle returns first row as the first previous value.
            if (tSourceBBDDType==BBDDType.Oracle){
                drs.Read();
                sPreviousKey = drs[0].ToString();
            }
            while (drs.Read())
            {
                for (int n = 0; n < 3; n++)
                {
                    if (n > 0)
                    {
                        AddItemToArray(ref sStatisticsKeys[n], int.Parse(Math.Truncate(decimal.Parse(drs[n].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo)).ToString()).ToString());
                    }
                    else
                    {
                        switch (iIndexFieldDataType)
                        {
                            case 59: //real
                            case 60: //money
                            case 62: //float
                            case 106: //decimal
                            case 108: //numeric
                            case 122: //smallmoney
                                AddItemToArray(ref sStatisticsKeys[n], double.Parse(drs[n].ToString()).ToString());
                                break;
                            default:
                                AddItemToArray(ref sStatisticsKeys[n], drs[n].ToString());
                                break;
                        }
                        //We save the lowkey of each range (the previous key of actual high key).                                
                        if (sPreviousKey == "")
                            AddItemToArray(ref sStatisticsKeys[3], sStatisticsKeys[n][sStatisticsKeys[n].Length - 1]);
                        else
                            AddItemToArray(ref sStatisticsKeys[3], sPreviousKey);
                        sPreviousKey = sStatisticsKeys[n][sStatisticsKeys[n].Length - 1];
                    }
                }
            }            
        }

        private String CheckDestinationTable(bool bAppendData,  DbConnection sqlDest)
        {
            try
            {
                DbDataReader drs;
                if (tDestinationBBDDType == BBDDType.SqlServer)
                {
                    String sTableObject = "";
                    if (sqlDest.ServerVersion.IndexOf("08") == 0)
                        sTableObject = "SYSOBJECTS WHERE TYPE='U' AND "; //BBDD 2000
                    else
                        sTableObject = "SYS.TABLES WHERE "; // BBDD 2005/2008

                    drs = new SqlCommand("SELECT NAME FROM " + sTableObject + " NAME='" + sDestTable + "'", (SqlConnection)sqlDest).ExecuteReader(System.Data.CommandBehavior.SingleRow);
                }
                else
                {
                    drs = new OracleCommand("SELECT TABLE_NAME FROM USER_TABLES WHERE TABLE_NAME='" + sDestTable + "'", (OracleConnection)sqlDest).ExecuteReader(System.Data.CommandBehavior.SingleRow);
                }

                if (drs.Read())
                {
                    drs.Close();
                    if (!bAppendData)
                    {
                        if (tDestinationBBDDType == BBDDType.SqlServer)
                        {
                            new SqlCommand("TRUNCATE TABLE  [" + sDestTable + "]",(SqlConnection)sqlDest).ExecuteNonQuery();
                        }
                        else
                        {
                            new OracleCommand("TRUNCATE TABLE  [" + sDestTable + "]",(OracleConnection)sqlDest).ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    drs.Close();
                    throw new Exception("Destination table doesn´t exists");
                }

            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

            return "";
        }



        private String FillSelectFilterValues(ref DbConnection sqlSource)
        {
            try
            {
                String[] sTemp;
                SqlCommand filterCommand = new SqlCommand(sSelectFilter, (SqlConnection)sqlSource);
                filterCommand.CommandTimeout = 0;
                using (SqlDataReader drs = filterCommand.ExecuteReader(System.Data.CommandBehavior.SingleResult))
                {
                    while (drs.Read())
                    {
                        sTemp = sValuesFilter;
                        sValuesFilter = new String[sValuesFilter.Length + 1];
                        sTemp.CopyTo(sValuesFilter, 0);
                        sValuesFilter[sValuesFilter.Length - 1] = drs[0].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

            return "";
        }

        /// <summary>
        /// Function called by each thread created in Copy function.
        /// It calls the clsThreadCopy´s CopyData function 
        /// when the copy process of each thread has come to end through the ThreadLocker object.
        /// By calling to the RaiseThreadFinishedEvent function it fires a public event which can be captured.
        /// </summary>
        /// <param name="o">clsThreadCopy object</param>
        private void CopyData(object o)
        {
            clsThreadCopy oTc = (clsThreadCopy)o;
            String sRet = "";
            sRet = oTc.CopyData();
            RaiseThreadFinishedEvent(oTc, sRet);
            lock (oThreadLocker)
            {
                iNumThreads--;
                Monitor.Pulse(oThreadLocker);
            }
        }

        private void RaiseThreadFinishedEvent(clsThreadCopy oTc, String sErr)
        {
            SqlThreadFinishedDelegate temp = ThreadFinished;

            if (temp != null)
            {
                temp(this, oTc, sErr);
            }

        }

        private void quicksort(String[] array, int iFirst, int iLast, bool bAsc)
        {
            quicksort(array, array, iFirst, iLast, bAsc);
        }

        private void quicksort(String[][] array, int iFirst, int iLast, bool bAsc, int iBaseIndex)
        {
            String[] arrayaux = new String[array[iBaseIndex].Length];
            array[iBaseIndex].CopyTo(arrayaux, 0);
            for (int i = 0; i < array.Length; i++)
            {
                if (i != iBaseIndex)
                {
                    quicksort(array[iBaseIndex], array[i], iFirst, iLast, bAsc);
                    arrayaux.CopyTo(array[iBaseIndex], 0);
                }
            }
            quicksort(array[iBaseIndex], iFirst, iLast, bAsc);
        }


        /// <summary>
        /// Little procedure to sort an array looking at other array values.
        /// </summary>
        /// <param name="arrayBase">Array to compare values.</param>
        /// <param name="array">Array to sort.</param>
        /// <param name="iFirst">iFirst index of the array. </param>
        /// <param name="iLast">iLast index of the array.</param>
        /// <param name="bAsc">True to sort it asc, False to desc.</param>
        private void quicksort(String[] arrayBase, String[] array, int iFirst, int iLast, bool bAsc)
        {
            int i, j, iMedium;
            String iPivot;

            iMedium = (iFirst + iLast) / 2;
            iPivot = arrayBase[iMedium];
            i = iFirst;
            j = iLast;

            do
            {
                if (bAsc)
                {
                    while (Compare(arrayBase[i], iPivot) < 0) i++;
                    while (Compare(arrayBase[j], iPivot) > 0) j--;
                }
                else
                {
                    while (Compare(arrayBase[i], iPivot) > 0) i++;
                    while (Compare(arrayBase[j], iPivot) < 0) j--;
                }

                if (i <= j)
                {
                    String temp;
                    temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    if (!arrayBase.Equals(array))
                    {
                        temp = arrayBase[i];
                        arrayBase[i] = arrayBase[j];
                        arrayBase[j] = temp;
                    }
                    i++;
                    j--;
                }

            } while (i <= j);

            if (iFirst < j)
            {
                quicksort(arrayBase, array, iFirst, j, bAsc);
            }
            if (i < iLast)
            {
                quicksort(arrayBase, array, i, iLast, bAsc);
            }
        }

        /// <summary>
        /// This function will be called each time any clsThreadCopy object fires it SqlRowsCopiedThreadEvent event.        
        /// By calling to the RaiseSqlRowsCopiedEvent function it fires a public event which can be captured.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="lRowsCopied"></param>
        /// <param name="iIdThread"></param>
        private void OnSqlRowsCopied(
        object sender, long lRowsCopied, int iIdThread)
        {
            RaiseSqlRowsCopiedEvent(lRowsCopied, oTc[iIdThread]);
        }

        private void RaiseSqlRowsCopiedEvent(long lRowsCopied, clsThreadCopy oTc)
        {
            SqlRowsCopiedDelegate temp = SqlRowsCopied;

            if (temp != null)
            {
                temp(this, lRowsCopied, oTc);
            }

        }

        private sbyte Compare(String s1, String s2)
        {
            switch (iIndexFieldDataType)
            {
                case 48: //tinyint
                case 52: //smallint
                case 56: //int                                
                    if (int.Parse(s1) > int.Parse(s2))
                        return 1;
                    else if (int.Parse(s1) < int.Parse(s2))
                        return -1;
                    else
                        return 0;

                case 127: //bigint
                    if (long.Parse(s1) > long.Parse(s2))
                        return 1;
                    else if (long.Parse(s1) < long.Parse(s2))
                        return -1;
                    else
                        return 0;

                case 59: //real
                case 60: //money
                case 62: //float
                case 106: //decimal
                case 108: //numeric
                case 122: //smallmoney
                    if (double.Parse(s1) > double.Parse(s2))
                        return 1;
                    else if (double.Parse(s1) < double.Parse(s2))
                        return -1;
                    else
                        return 0;

                default:
                    return (sbyte)string.Compare(s1, s2);
            }
        }
    }

}
