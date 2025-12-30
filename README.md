# Database Project - Transport Management System

## Table of Contents
1. [Introduction](#introduction)
2. [System Requirements](#system-requirements)
3. [Installation and Setup](#installation-and-setup)
4. [Database Schema](#database-schema)
5. [Application Architecture](#application-architecture)
6. [User Guide](#user-guide)
7. [CSV Import Format](#csv-import-format)
8. [Repository Pattern](#repository-pattern)
9. [Report Generation](#report-generation)
10. [Troubleshooting](#troubleshooting)

## Introduction

This application provides a graphical user interface (GUI) for managing public transportation data stored in Microsoft SQL Server 2022. It is specifically designed for the "Doprava" (Transport) database but can also connect to and manage other SQL Server databases with limited functionality.

The system manages:
- Public transport stations (bus, tram, metro, train)
- Transport lines and their associations with stations
- Station amenities (shelters, benches, trash bins, info panels)
- Metro station specific data (depth, cleaning schedules, facilities)
- Train station specific data (platforms, electrification, track gauge)
- Comprehensive reporting capabilities

## System Requirements

### Prerequisites
- Microsoft Windows operating system
- .NET 9.0 or higher
- Microsoft SQL Server 2022 or compatible version
- SQL Server with TCP/IP enabled
- Sufficient permissions to create and modify databases

### Dependencies
The application uses the following NuGet packages:
- Microsoft.Data.SqlClient
- CsvHelper
- Azure.Core
- Azure.Identity
- Microsoft.Extensions.Caching.Memory
- Microsoft.Extensions.Logging.Abstractions

## Installation and Setup

### Step 1: Database Creation

1. Locate the `create-database.sql` file in the project root directory
2. Open Microsoft SQL Server Management Studio (SSMS)
3. Connect to your SQL Server instance
4. Open and execute the `create-database.sql` script
5. Verify that the "Doprava" database has been created with all tables, views, and triggers

### Step 2: Configure Connection String

Edit the `App.config` file in the application root directory to match your SQL Server configuration:

```xml
<configuration>
    <connectionStrings>
        <clear />
        <add name="Doprava"
             providerName="System.Data.SqlClient"
             connectionString="Server=localhost;Database=Doprava;Integrated Security=true;TrustServerCertificate=true;" />
    </connectionStrings>
</configuration>
```

**Configuration Parameters:**
- **Server**: Your SQL Server instance name (e.g., `localhost`, `.\SQLEXPRESS`, or `server-name\instance`)
- **Database**: Must be "Doprava" for full functionality
- **Integrated Security**: Set to `true` for Windows Authentication
- **TrustServerCertificate**: Set to `true` to bypass certificate validation

### Step 3: Launch Application

1. Build the project in Visual Studio or using `dotnet build`
2. Run the executable from `bin\Debug\net9.0-windows\DatabazeProjekt.exe`
3. The main window will appear with connection options

## Database Schema

### Core Tables

#### stanice (Stations)
Primary table for all transport stations with common attributes:
- `id_stanice` (INT, PRIMARY KEY): Unique station identifier
- `nazev` (NVARCHAR(50), UNIQUE): Station name
- `typ_stanice` (NVARCHAR(10)): Station type (bus, metro, tram, vlak)
- `ma_lavicku` (BIT): Has bench
- `ma_kos` (BIT): Has trash bin
- `ma_pristresek` (BIT): Has shelter
- `ma_infopanel` (BIT): Has info panel
- `na_znameni` (BIT): Request stop
- `bezbarierova` (BIT): Barrier-free access

#### linky (Lines)
Transport lines information:
- `id_linky` (INT, PRIMARY KEY): Line identifier
- `cislo_linky` (INT, UNIQUE): Line number
- `nazev_linky` (NVARCHAR(50)): Line name

#### stanice_linka (Station-Line Association)
Many-to-many relationship between stations and lines:
- `id_sl` (INT, PRIMARY KEY)
- `stanice_id` (INT, FOREIGN KEY): References stanice
- `linka_id` (INT, FOREIGN KEY): References linky

#### metro_stanice (Metro Stations)
Additional attributes for metro stations:
- `id` (INT, PRIMARY KEY)
- `stanice_id` (INT, FOREIGN KEY): References stanice
- `hloubka_pod_zemi` (FLOAT): Depth below ground
- `cetnost_uklidu` (NVARCHAR(50)): Cleaning frequency
- `ma_wc` (BIT): Has restroom facilities
- `dat_posl_uklid` (DATETIME): Last cleaning date

#### vlak_stanice (Train Stations)
Additional attributes for train stations:
- `id_vlak` (INT, PRIMARY KEY)
- `stanice_id` (INT, FOREIGN KEY): References stanice
- `pocet_nast` (INT): Number of platforms
- `elektrifikovana` (BIT): Electrified
- `soustava` (NVARCHAR(50)): Electrification system
- `rozchod_kolej` (INT): Track gauge

#### pristresek (Shelters)
Detailed shelter information:
- `id_prist` (INT, PRIMARY KEY)
- `stanice_id` (INT, FOREIGN KEY): References stanice
- `typ` (NVARCHAR(50)): Shelter type
- `barva` (NVARCHAR(50)): Color
- `vlastnik` (NVARCHAR(100)): Owner
- `spravce` (NVARCHAR(100)): Manager
- `datum_vyroby` (DATETIME): Manufacturing date

### Database Views

#### v_stanice_s_linkami
Comprehensive view of stations with their associated lines:
- Station details
- Count of lines serving the station
- Comma-separated list of line numbers
- Comma-separated list of line names
- Equipment level categorization

#### v_linky_s_pokrytim
Overview of lines with coverage statistics:
- Line identification
- Count of stations by type
- Metro depth statistics (min/max)

### Database Triggers

#### TR_metro_station_type
Ensures metro-specific records are only created for stations with type 'metro'

#### TR_train_station_type
Ensures train-specific records are only created for stations with type 'vlak'

## Application Architecture

### Project Structure

The application follows a layered architecture with separation of concerns:

```
DatabazeProjekt/
├── Forms/
│   ├── MainWindow.cs - Database connection window
│   ├── Chooser.cs - Generic database editor
│   ├── Transport.cs - Transport database main window
│   ├── AddStationAndLine.cs - Station and line creation form
│   ├── AddMetroStation.cs - Metro station creation form
│   ├── AddTrainStation.cs - Train station creation form
│   └── AddShelter.cs - Shelter creation form
├── Repositories/
│   ├── IStationRepository.cs - Station data access interface
│   ├── StationRepository.cs - Station data access implementation
│   ├── ILineRepository.cs - Line data access interface
│   ├── LineRepository.cs - Line data access implementation
│   ├── IStationLineRepository.cs - Station-Line relationship interface
│   ├── StationLineRepository.cs - Station-Line relationship implementation
│   ├── IMetroStationRepository.cs - Metro station interface
│   ├── MetroStationRepository.cs - Metro station implementation
│   ├── ITrainStationRepository.cs - Train station interface
│   ├── TrainStationRepository.cs - Train station implementation
│   ├── IShelterRepository.cs - Shelter interface
│   ├── ShelterRepository.cs - Shelter implementation
│   └── IReportRepository.cs - Report generation interface
├── Reports/
│   ├── MarkdownReportWriter.cs - Markdown report formatter
│   └── TransportSummaryReport.cs - Report data model
├── Models/
│   └── StationRecord.cs - CSV import data model
└── Program.cs - Application entry point
```

### Design Patterns

#### Repository Pattern
All database operations are encapsulated in repository classes implementing repository interfaces. This provides:
- Abstraction over data access logic
- Testability through interface implementation
- Separation of concerns
- Reusability of data access code

#### Transaction Management
Complex operations involving multiple tables use SQL transactions to ensure data consistency and integrity.

## User Guide

### Connecting to a Database

#### Method 1: Quick Connect to Transport Database
1. Click the "Doprava" button on the main window
2. The application will use the connection string from App.config
3. Transport window opens with specialized features

#### Method 2: Connection String from Config
1. Check "ConnStr" checkbox
2. Enter the connection string name from App.config (e.g., "Doprava")
3. Click "Load DB"
4. Generic database chooser window opens

#### Method 3: Windows Authentication
1. Check "WinAuth" checkbox
2. Enter server name (e.g., `localhost` or `.\SQLEXPRESS`)
3. Enter database name
4. Click "Load DB"

#### Method 4: SQL Server Authentication
1. Leave both checkboxes unchecked
2. Enter server name
3. Enter database name
4. Enter username
5. Enter password
6. Click "Load DB"

### Working with the Generic Database Editor (Chooser)

The Chooser window provides basic CRUD operations for any database:

1. **Select Table**: Choose a table from the dropdown menu
2. **View Data**: Table contents display in the data grid
3. **Add Row**: Click "Add" to insert a new row
4. **Edit Data**: Click on any cell to modify values
5. **Delete Row**: Select row(s) and click "Delete"
6. **Save Changes**: Click "Save" to persist changes to database
7. **Import CSV**: Click "Import" to load data from CSV file
8. **Exit**: Close the window

**Note**: Database views are read-only and cannot be modified.

### Working with the Transport Database

The Transport window provides specialized functionality:

#### Adding a Station with Line
1. Click "Stanice + Linka" button
2. Fill in station details:
   - Station name
   - Station type (bus/metro/tram/vlak)
   - Amenities checkboxes
3. Fill in line details:
   - Line number
   - Line name
4. Click "Odeslat" to save

#### Adding a Metro Station
1. Click "Metro stanice" button
2. Select existing station or create new
3. Enter metro-specific data:
   - Depth below ground
   - Cleaning frequency
   - WC facilities checkbox
   - Last cleaning date
4. Click submit to save

#### Adding a Train Station
1. Click "Vlak stanice" button
2. Select existing station or create new
3. Enter train-specific data:
   - Number of platforms
   - Electrification checkbox
   - Electrification system type
   - Track gauge
4. Click submit to save

#### Adding a Shelter
1. Click "Pristresek" button
2. Select station
3. Enter shelter details:
   - Type
   - Color
   - Owner
   - Manager
   - Manufacturing date
4. Click submit to save

#### Importing CSV Data
1. Click "CSV" button
2. Select delimiter:
   - Check checkbox for semicolon (;)
   - Uncheck for comma (,)
3. Browse and select CSV file
4. Data is automatically imported

#### Deleting a Station
1. Check "Smazat stanici" checkbox
2. Enter station name exactly as stored
3. Click "Smazat" button
4. Confirm deletion

#### Generating Reports
1. Click "Report" button
2. Choose save location and filename
3. Markdown report is generated with:
   - Station statistics
   - Line coverage data
   - Equipment analysis
   - Metro and train station summaries

#### Opening Database Editor
1. Click "Editor" button
2. Generic Chooser window opens for the Transport database

## CSV Import Format

### Station Import Format

CSV files for station import must have the following columns in order:

```
StationName,StationType,HasShelter,HasBench,HasTrashBin,HasInfoPanel,RequestStop,BarrierFree,LineNumber,LineName
```

### Column Descriptions

- **StationName** (string): Unique name of the station
- **StationType** (string): Must be one of: `bus`, `metro`, `tram`, `vlak`
- **HasShelter** (boolean): `true` or `false`
- **HasBench** (boolean): `true` or `false`
- **HasTrashBin** (boolean): `true` or `false`
- **HasInfoPanel** (boolean): `true` or `false`
- **RequestStop** (boolean): `true` or `false` - indicates if station is on-demand
- **BarrierFree** (boolean): `true` or `false` - wheelchair accessibility
- **LineNumber** (integer): Transport line number
- **LineName** (string): Name/description of the line

### Example CSV Content

Using semicolon delimiter:
```
StationName;StationType;HasShelter;HasBench;HasTrashBin;HasInfoPanel;RequestStop;BarrierFree;LineNumber;LineName
Patockova;bus;true;true;false;false;true;false;147;Ládví - Dejvická
Mustek;metro;false;true;true;true;false;true;1;Metro A
Hlavní nádraží;vlak;true;true;true;true;false;true;5;Hlavní trasa
```

Using comma delimiter:
```
StationName,StationType,HasShelter,HasBench,HasTrashBin,HasInfoPanel,RequestStop,BarrierFree,LineNumber,LineName
Patockova,bus,true,true,false,false,true,false,147,Ládví - Dejvická
Mustek,metro,false,true,true,true,false,true,1,Metro A
Hlavní nádraží,vlak,true,true,true,true,false,true,5,Hlavní trasa
```

### Important Notes

- CSV must include header row
- Boolean values must be `true` or `false` (case-insensitive)
- Station names must be unique
- Line numbers should be numeric integers
- Empty line names are allowed
- Delimiter can be changed in the Transport window before import

## Repository Pattern

### Overview

The application implements the Repository pattern to abstract database operations. Each entity has an interface and implementation:

### IStationRepository

```csharp
void AddStation(StationRecord station);
void DeleteStationByName(string name);
StationRecord GetStationByName(string name);
IEnumerable<StationRecord> GetAllStations();
int AddStationAndReturnId(StationRecord station);
int AddStationAndReturnId(StationRecord station, SqlTransaction transaction);
int GetStationIdByName(string name);
```

### Transaction Support

Repositories support optional transaction parameters for multi-step operations:

```csharp
using (SqlTransaction transaction = connection.BeginTransaction())
{
    try
    {
        int stationId = stationRepository.AddStationAndReturnId(station, transaction);
        int lineId = lineRepository.GetOrCreateLineId(lineNumber, lineName, transaction);
        stationLineRepository.AddStationToLine(stationId, lineId, transaction);
        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}
```

## Report Generation

### Report Types

The application generates comprehensive Markdown reports containing:

1. **Station Summary**
   - Total number of stations
   - Breakdown by type (bus, metro, tram, train)
   - Equipment statistics

2. **Line Coverage**
   - Total number of lines
   - Stations per line
   - Type distribution per line

3. **Equipment Analysis**
   - Stations with benches
   - Stations with trash bins
   - Stations with shelters
   - Stations with info panels
   - Barrier-free stations
   - Request stops

4. **Metro Station Details**
   - Total metro stations
   - Average depth
   - Stations with WC facilities
   - Cleaning schedule overview

5. **Train Station Details**
   - Total train stations
   - Electrified stations
   - Platform statistics
   - Track gauge information

### Generating a Report

1. Open Transport window
2. Click "Report" button
3. Choose save location
4. Default filename format: `doprava-report-YYYYMMDD-HHmm.md`
5. Report opens automatically after generation

## Troubleshooting

### Connection Issues

**Error: "Cannot connect to server"**
- Verify SQL Server is running
- Check server name is correct
- Ensure TCP/IP is enabled in SQL Server Configuration Manager
- Verify firewall allows SQL Server connections

**Error: "Login failed for user"**
- Verify credentials are correct
- Check SQL Server authentication mode (Windows vs Mixed)
- Ensure user has appropriate permissions

**Error: "Database 'Doprava' not found"**
- Run the `create-database.sql` script
- Verify database name in connection string matches actual database name

### Import Issues

**Error: "Chyba načítání tabulek"**
- Verify connection is still active
- Check user has SELECT permissions on INFORMATION_SCHEMA

**CSV Import Fails**
- Verify CSV format matches expected columns
- Check delimiter setting matches CSV file
- Ensure boolean values are `true`/`false`
- Verify station types are valid: bus, metro, tram, vlak

### Data Integrity Issues

**Error: "Stanice must be of type metro"**
- Trigger validation failed
- Attempting to add metro-specific data to non-metro station
- Verify station type before adding specialized data

**Error: "Violation of UNIQUE KEY constraint"**
- Station name already exists
- Line number already exists
- Use unique names/numbers or update existing records

### Application Errors

**Unhandled Exception Dialog**
- Note the error message
- Check database connection is active
- Verify data types match expected values
- Review application logs if available

### Performance Issues

**Slow Data Loading**
- Check database server performance
- Verify network connection to remote servers
- Consider indexing frequently queried columns
- Reduce data set size if viewing large tables

### View Editing Restrictions

**Cannot modify view data**
- Views are read-only by design
- Edit underlying tables directly
- Use specialized forms for Transport database modifications

## Additional Resources

### File Locations

- **Application Config**: `App.config`
- **Database Script**: `create-database.sql`
- **Reports Output**: User-selected location (default: Documents)
- **Test Cases**: `Blackbox testing/` directory

### Support

For issues or questions:
1. Review this documentation
2. Check SQL Server logs
3. Verify application configuration
4. Review test cases in Blackbox testing folder

### Version Information

- Application: Database Project (DatabazeProjekt)
- Target Framework: .NET 9.0 Windows
- Database: Microsoft SQL Server 2022
- Copyright: 2025 Štěpán Végh
