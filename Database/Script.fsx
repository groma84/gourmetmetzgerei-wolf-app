#I __SOURCE_DIRECTORY__

#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "../packages/System.Data.SQLite.Core/lib/net40/System.Data.SQLite.dll"
#r "../packages/SQLProvider/lib/FSharp.Data.SqlProvider.dll"

#load "../Data/Types.fs"
#load "../Data/Errors.fs"
#load "../paket-files/fsprojects/Chessie/src/Chessie/ErrorHandling.fs"
#load "Helper.fs"
// #load "Menus.fs"
#load "Setup.fs"


open FSharp.Data.Sql

open System.Data
open System.Data.SQLite

open GmwApp.Database
// open GmwApp.Data.Dates

// let yaw = { Year = 2016; Week = 51; }
let cs = Helper.buildConnectionString @"E:\git\gourmetmetzgerei-wolf-app\gmwapp.sqlite" 

let menusTable = @"CREATE TABLE IF NOT EXISTS Menus (
                                _id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Year INTEGER,
                                IsoWeek INTEGER,
                                ContentJson TEXT)"
                                
let cn = new SQLiteConnection(cs)
cn.Open()

let createTableScript = menusTable
let cmd = new SQLiteCommand(createTableScript, cn)
let result = cmd.ExecuteNonQuery()
cn.Close()



// let result = Menus.loadMenus cs yaw
// result
// type sql = SqlDataProvider<DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
//                             ConnectionString = @"Data Source=D:\git-repos\groma84@github\gourmetmetzgerei-wolf-app\gmwapp.sqlite;Version=3",
//                             ResolutionPath = @"D:\git-repos\groma84@github\gourmetmetzgerei-wolf-app\packages\System.Data.SQLite.Core\lib\net46",
//                             CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL>

// let ctx = sql.GetDataContext


// let data =
//     query {
//         for item in ctx.Main.Menus do
//         where (item.Year = int64 2016 && item.IsoWeek = int64 50)
//         select (item)
//         head
//     }

// data.ContentJson
