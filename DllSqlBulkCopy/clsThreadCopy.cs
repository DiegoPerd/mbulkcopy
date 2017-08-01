using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;


namespace DllSqlBulkCopy
{

    class clsThreadCopy : DllSqlBulkCopy.IclsThreadCopy
    {
        private DbConnection sqlSource;

        public DbConnection SqlSource
        {
            get { return sqlSource; }
            set { sqlSource = value; }
        }
        private string sDest;

        public string SDest
        {
            get { return sDest; }
            set { sDest = value; }
        }
        private String[] sRangeHighKeys;

        public String[] SRangeHighKeys
        {
            get { return sRangeHighKeys; }
            set { sRangeHighKeys = value; }
        }
        private String[] sRangeLowKeys;

        public String[] SRangeLowKeys
        {
            get { return sRangeLowKeys; }
            set { sRangeLowKeys = value; }
        }
        private String[] sValuesFilter;

        public String[] SValuesFilter
        {
            get { return sValuesFilter; }
            set { sValuesFilter = value; }
        }
        private int iSize;

        public int Size
        {
            get { return iSize; }
            set { iSize = value; }
        }
        private String sSourceTable;

        public String SSourceTable
        {
            get { return sSourceTable; }
            set { sSourceTable = value; }
        }
        private String sSourceTableScheme;

        public String SSourceTableScheme
        {
            get { return sSourceTableScheme; }
            set { sSourceTableScheme = value; }
        }
        private String sDestTable;

        public String SDestTable
        {
            get { return sDestTable; }
            set { sDestTable = value; }
        }
        private String sIndexField;

        public String SIndexField
        {
            get { return sIndexField; }
            set { sIndexField = value; }
        }
        private int iBcOptions;

        public int IBcOptions
        {
            get { return iBcOptions; }
            set { iBcOptions = value; }
        }
        private int iNotifyAfter;

        public int INotifyAfter
        {
            get { return iNotifyAfter; }
            set { iNotifyAfter = value; }
        }
        private int iIdThread;

        public int IdThread
        {
            get { return iIdThread; }
            set { iIdThread = value; }
        }

        private int iIndexFieldDataType;

        public int IIndexFieldDataType
        {
            get { return iIndexFieldDataType; }
            set { iIndexFieldDataType = value; }
        }
        private bool bIsIndexFieldNumeric;

        private bool bNoLock;

        public bool NoLock
        {
            get { return bNoLock; }
            set { bNoLock = value; }
        }

        private bool bHeapMethod = false;
       
        public bool HeapMethod
        {
            get { return bHeapMethod; }
            set { bHeapMethod = value; }
        }

        private int iNumThreads ;
        
        public int NumThreads
        {
            get { return iNumThreads; }
            set { iNumThreads = value; }
        }

        private BBDDType tSourcebbddType;
        public BBDDType SourcebbddType        
        {
            get { return tSourcebbddType; }
            set { tSourcebbddType = value; }
        }

        private BBDDType tDestbbddType;
        public BBDDType DestbbddType
        {
            get { return tDestbbddType; }
            set { tDestbbddType = value; }
        }

        public delegate void SqlRowsCopiedEventDelegate(object sender, long lRowsCopied, int iIdThread);
        /// <summary>
        /// It fires each time the number of rows specified in iNotifyAfter property has been copied.
        /// </summary>
        public event SqlRowsCopiedEventDelegate SqlRowsCopiedThreadEvent;

        /// <summary>
        /// Function which builds the sql to copy data and launch the SqlBulkCopy object to copy it.
        /// </summary>
        /// <returns>Empty string if everything was ok. Else returns the error's message.</returns>
        public String CopyData()
        {
            try
            {
                String sSql = "";
                //check for copy method.
                if ((!bHeapMethod))
                {
                    // We check if there is any range to copy.
                    if (sRangeHighKeys.Length > 0)
                    {

                        // We check if the clustered index field is numeric.
                        switch (iIndexFieldDataType)
                        {
                            case 48: //tinyint
                            case 52: //smallint
                            case 56: //int
                            case 59: //real
                            case 127: //bigint
                            case 122: //smallmoney
                            case 60: //money
                            case 62: //float
                            case 106: //decimal
                            case 108: //numeric
                                bIsIndexFieldNumeric = true;
                                break;
                            default:
                                bIsIndexFieldNumeric = false;
                                break;
                        }
                        //We start to build the sql.

                        sSql = "SELECT * FROM " + sSourceTableScheme + "." + sSourceTable;

                        if (bNoLock)
                        {
                            sSql += " WITH (READUNCOMMITTED) ";
                        }

                        sSql += " WHERE (";

                        //We add to the sql the ranges to copy.
                        for (int i = 0; i < sRangeHighKeys.Length; i++)
                        {
                            if (sRangeHighKeys[i] == sRangeLowKeys[i])
                            {
                                sSql += " (" + sIndexField + " = " + SQLValue(sRangeLowKeys[i], bIsIndexFieldNumeric) + ") OR";
                            }
                            else
                            {
                                sSql += "(";
                                if (sRangeLowKeys[i] != "*")
                                {
                                    sSql += " (" + sIndexField + " > " + SQLValue(sRangeLowKeys[i], bIsIndexFieldNumeric) + ")";
                                    if (sRangeHighKeys[i] != "*")
                                    {
                                        sSql += " AND ";
                                    }
                                }
                                if (sRangeHighKeys[i] != "*")
                                {
                                    sSql += " (" + sIndexField + " <= " + SQLValue(sRangeHighKeys[i], bIsIndexFieldNumeric) + ")";
                                }
                                sSql += ") OR";
                            }
                        }
                        sSql = sSql.Substring(0, sSql.Length - 2) + ")";

                        //We add to the sql the values coming from the selectfilter.
                        if (sValuesFilter.Length > 0)
                        {
                            sSql += " AND (" + sIndexField + " IN (";
                            for (int i = 0; i < sValuesFilter.Length; i++)
                            {
                                sSql += SQLValue(sValuesFilter[i], bIsIndexFieldNumeric) + ",";
                            }
                            sSql = sSql.Substring(0, sSql.Length - 1) + "))";
                        }
                        else
                        {
                            //We assign to the first thread to copy the rows with null values in the clustered index field.
                            if (iIdThread == 0)
                                sSql += " OR (" + sIndexField + " IS NULL)";
                        }
                    }
                    else
                    {
                        if (iIdThread != 0 | sValuesFilter.Length > 0)
                        {
                            return "";
                        }
                        else
                        {
                            sSql = "SELECT * FROM " + sSourceTableScheme + "." + sSourceTable;

                            if (bNoLock)
                            {
                                sSql += " WITH (READUNCOMMITTED) ";
                            }

                            sSql += " WHERE (" + sIndexField + " IS NULL)";
                        }

                    }
                }
                else
                {
                    sSql = "SELECT * FROM " + sSourceTableScheme + "." + sSourceTable;
                    if (SourcebbddType == BBDDType.SqlServer)
                    {
                        if (bNoLock)
                        {
                            sSql += " WITH (READUNCOMMITTED) ";
                        }


                        sSql += " WHERE (ABS(BINARY_CHECKSUM(%%lockres%%)) % " + iNumThreads.ToString() + " = " + IdThread.ToString() + ")";
                    }
                    else
                    {
                        sSql += " WHERE mod(ora_hash(rowid)," + iNumThreads.ToString() + ") = " + IdThread.ToString() ;
                    }
                }

                //If there isn´t any option to add to the SqlBulkCopy object, we assign it the Default one.
                if (iBcOptions == 0)
                    iBcOptions = (int)SqlBulkCopyOptions.Default;

                //We create the source command with the custom sql.
                if (SourcebbddType == BBDDType.SqlServer)
                {
                    SqlCommand sourceCommand = new SqlCommand(sSql, (SqlConnection)sqlSource);

                    sourceCommand.CommandTimeout = 0;

                    //We fill the datareader
                    using (SqlDataReader dr = sourceCommand.ExecuteReader(System.Data.CommandBehavior.SingleResult))
                    {
                        if (DestbbddType == BBDDType.SqlServer)
                        {
                            SqlBulkCopy(dr);
                        }
                        else
                        {
                            OracleBulkCopy(dr);
                        }
                    }
                    
                }
                else
                {
                    OracleCommand sourceCommand = new OracleCommand(sSql, (OracleConnection)sqlSource);

                    sourceCommand.CommandTimeout = 0;

                    //We fill the datareader
                    using (OracleDataReader dr = sourceCommand.ExecuteReader(System.Data.CommandBehavior.SingleResult))
                    {
                        if (DestbbddType == BBDDType.SqlServer)
                        {                            
                            SqlBulkCopy(dr);
                        }
                        else
                        {
                            OracleBulkCopy(dr);
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

        /// <summary>
        /// Function to bulk copy to Oracle
        /// </summary>
        /// <param name="dr"></param>
        private void OracleBulkCopy(DbDataReader dr)
        {
            //We create the SqlBulkCopy object with the Destination Connection String arguments and with the SqlBulkCopy options selected.
            using (OracleBulkCopy oSbc = new OracleBulkCopy(sDest, (OracleBulkCopyOptions)iBcOptions))
            {
                //We configure the SqlBulkCopy object.
                oSbc.DestinationTableName = sDestTable;
                oSbc.BatchSize = iSize;
                oSbc.BulkCopyTimeout = 0;
                if (iNotifyAfter > 0)
                {
                    oSbc.NotifyAfter = iNotifyAfter;
                    //Each time the number of rows specified in NotifyAfter property has been copied it will call to the OnSqlRowsCopied function.
                    oSbc.OracleRowsCopied += new OracleRowsCopiedEventHandler(OnOracleRowsCopied);
                }
                //We start to copy the data to the dest server.                        
                oSbc.WriteToServer(dr);
                oSbc.Close();
            }
        }

        /// <summary>
        /// Function to bulk copy to Sql Server
        /// </summary>
        /// <param name="dr"></param>
        private void SqlBulkCopy(DbDataReader dr)
        {
            //We create the SqlBulkCopy object with the Destination Connection String arguments and with the SqlBulkCopy options selected.
            using (SqlBulkCopy oSbc = new SqlBulkCopy(sDest, (SqlBulkCopyOptions)iBcOptions))
            {
                //We configure the SqlBulkCopy object.
                oSbc.DestinationTableName = sDestTable;
                oSbc.BatchSize = iSize;
                oSbc.BulkCopyTimeout = 0;
                if (iNotifyAfter > 0)
                {
                    oSbc.NotifyAfter = iNotifyAfter;
                    //Each time the number of rows specified in NotifyAfter property has been copied it will call to the OnSqlRowsCopied function.
                    oSbc.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                }
                //We start to copy the data to the dest server.                        
                oSbc.WriteToServer(dr);
                oSbc.Close();
            }
        }

        /// <summary>
        /// This function will be called each time the number of rows specified in iNotifyAfter property 
        /// has been copied.
        /// By calling to the RaiseSqlRowsCopiedEvent function it fires an event which one will 
        /// be captured in clsCopy class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSqlRowsCopied(
        object sender, SqlRowsCopiedEventArgs e)
        {
            RaiseSqlRowsCopiedEvent(e.RowsCopied, iIdThread);
        }
        /// <summary>
        /// Oracle one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOracleRowsCopied(
        object sender, OracleRowsCopiedEventArgs e)
        {
            RaiseSqlRowsCopiedEvent(e.RowsCopied, iIdThread);
        }

        private void RaiseSqlRowsCopiedEvent(long lRowsCopied, int iIdThread)
        {
            SqlRowsCopiedEventDelegate temp = SqlRowsCopiedThreadEvent;

            if (temp != null)
            {
                temp(this, lRowsCopied, iIdThread);
            }
        }

        #region SQL Formatting Functions
        private String SQLNumber(String sNumber)
        {
            String sNum;
            sNum = sNumber;
            sNum = sNum.Replace(".", "");
            sNum = sNum.Replace(",", ".");

            return sNum;
        }

        private String SQLText(String sText)
        {
            String sTxt;
            sTxt = sText.Replace("'", "''");
            sTxt = "'" + sTxt + "'";

            return sTxt;
        }

        private String SQLValue(String sValue, bool bNumeric)
        {
            if (bNumeric)
            {
                return SQLNumber(sValue);
            }
            else
            {
                return SQLText(sValue);
            }
        }
        #endregion
    }
}
