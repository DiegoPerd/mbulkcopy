using System;
namespace DllSqlBulkCopy
{
    public interface IclsThreadCopy
    {
        /// <summary>
        /// Entry point method
        /// </summary>
        /// <returns></returns>
        string CopyData();
        /// <summary>
        /// Type of destination relational database
        /// </summary>
        BBDDType DestbbddType { get; set; }
        /// <summary>
        /// Copy method.
        /// </summary>
        bool HeapMethod { get; set; }
        /// <summary>
        /// Bulk copy options
        /// </summary>
        int IBcOptions { get; set; }
        /// <summary>
        /// Thread's unique identifier.
        /// </summary>
        int IdThread { get; set; }
        /// <summary>
        /// DataType of source index field.
        /// </summary>
        int IIndexFieldDataType { get; set; }
        /// <summary>
        /// number of records to notify sucess copy.
        /// </summary>
        int INotifyAfter { get; set; }
        /// <summary>
        /// Sql server expecific option to do dirty reads
        /// </summary>
        bool NoLock { get; set; }
        /// <summary>
        /// total number or threads.
        /// </summary>
        int NumThreads { get; set; }
        /// <summary>
        /// destination connection string
        /// </summary>
        string SDest { get; set; }
        /// <summary>
        /// destination table
        /// </summary>
        string SDestTable { get; set; }
        /// <summary>
        /// source index field
        /// </summary>
        string SIndexField { get; set; }
        /// <summary>
        /// Aproximate number of rows which will be copied. If the SelectFilter property of clsCopy has been set, this value may be incorrect.
        /// </summary>
        int Size { get; set; }
        /// <summary>
        /// Type of source relational database
        /// </summary>        
        BBDDType SourcebbddType { get; set; }
        /// <summary>
        /// Thread's DbConnection to source data.
        /// </summary>
        System.Data.Common.DbConnection SqlSource { get; set; }
        /// <summary>
        /// Clustered index field high range keys which will be copied.
        /// </summary>
        String[] SRangeHighKeys { get; set; }
        /// <summary>
        /// Clustered index field low range keys which will be copied.
        /// </summary>
        String[] SRangeLowKeys { get; set; }
        /// <summary>
        /// source tablename
        /// </summary>
        string SSourceTable { get; set; }
        /// <summary>
        /// source table scheme
        /// </summary>
        string SSourceTableScheme { get; set; }
        /// <summary>
        /// array with values to filter
        /// </summary>
        string[] SValuesFilter { get; set; }
    }
}
