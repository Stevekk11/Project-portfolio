# Introduction
This app is used as an GUI for a database in MS SQL Server 2022.
You can use it to open any database, or the Transport database for which it has dedicated
functionality.

# Setup
Prerequisites: Microsoft SQL Server 2022, enabled.
Imported database. Download here.
In the App.Config file, modify the connection string so it fits your use.
The database name must not be changed.
Launch the app

## Example
# <configuration>
    <connectionStrings>
        <clear />
        <add name="Doprava"
             providerName="System.Data.SqlClient"
             connectionString="Server=localhost;Database=Doprava;Integrated Security = true;TrustServerCertificate=true;" />
    </connectionStrings>
# </configuration>

# Import CSV

In the chooser window, you can import a csv with data that matches the rows in the provided database. Use ; as delimiter.
In the Transport DB, you must import a CSV with this data:

# <data>
    StationName,StationType,HasShelter,HasBench,HasTrashBin,HasInfoPanel,RequestStop,BarrierFree,LineNumber,LineName
    Patočkova,bus,true,true,false,false,true,false,147, ,
# </data>