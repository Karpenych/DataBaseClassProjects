@echo off
REM Create an instance of LocalDB
"C:\Program Files\Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe" create LocalDB_MY_MSSQL
REM Start the instance of LocalDB
"C:\Program Files\Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe" start LocalDB_MY_MSSQL
REM Gather information about the instance of LocalDB
"C:\Program Files\Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe" info LocalDB_MY_MSSQL