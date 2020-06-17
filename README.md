# Multithreaded Bulk Copy

A tool for data transfer situations, built only for theorical purposes, that may be useful to transfer a large amount of data from a SQL Server(2000 or higher) and/or Oracle (10g or higher) instance to another (SQL>SQL, SQL>Oracle, Oracle>SQL and Oracle>Oracle).

It is usual to copy SQL Server and/or Oracle tables from one instance to another.  
Usually we likely decide to use the SSMS Import/Export wizard, an Integration Services package or a method to transfer data as a text file.  
But problems may arise if we are talking about tables with more than 300 millions of records or 30gb and more if they must be copied in a minimum time.  
If we are lucky this table may have a clustered index in Sql Server or an index with a frequency histogram in Oracle. That's the scenario where this little project play a role.  
If we do not have a clustered index on source table, there is some hope, we implemented a new method (a little slower than clustered one, but a good one) to copy data from a heap table, in both Oracle and Sql Server.  

Its logic is very simple: it scans clustered index statitics in Sql Server, or histogram in Oracle, and define balanced clusters of data.  
It means that it creates as many queries as threads parametrized, and those queries filter on the clustered index field trying so as to obtain the same volume of data each one. Then it fires them using the sqlbulkcopy .Net fw or oraclebulkcopy from odp.net method.
As heaps, it "partition" the table with a determinictic function and a module (%%lockres%% in Sql Server and rowid in oracle), and launch each query in one thread.  

We must advice that depending on the amount of thread it is a very resource intensive process and in some cases CPU consumption could rise up to 100% and disk queue length may cause long waits.  

Because of this, we recommend running on a different server other than the data source and destination.  

Thus, with a structure of 2 networks with two 100mbit network cards in each server, we have achieved 190MB/seg transfer rates.


You should test it for statistical purposes before thinking on using it in a production environment.

Additional notes:
Performance gain will be achieved under certain circunstances, and sometimes it will not be percieved due to the nature of the data inside the clustered column. First of all it's necessary to clarify that clustered index saves statistics information only of the first column of a compound index, so it became more critical that the clustered index be well design. We mean that it'd better to follow the tip of ordering fields from the one with most granularity to the less one.
Another issue that may be encounter is the amount of Null data. Statistics talk about non null data, so if the table has an 80% of null data multithreading will not benefit from it, as all null data is copied in one thread as a unique batch.
Naturally, we also recommend that the destination table has no indexes (to allow multiple bulk insert threads), no partitions and be uncompressed to speed up the insert process.

This dll could be embebbed into other ETL projects that not depends or that are not built over the SSIS platform.
