#I __SOURCE_DIRECTORY__

#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "../packages/System.Data.SQLite.Core/lib/net40/System.Data.SQLite.dll"
#r "../packages/SQLProvider/lib/FSharp.Data.SqlProvider.dll"

#load "../Data/Constants.fs"

open FSharp.Data.Sql

open System.Data
open System.Data.SQLite

open GmwApp.Data.Constants

let cs = DB_TYPEPROVIDER_CONNECTIONSTRING

let menusTable = @"CREATE TABLE IF NOT EXISTS Menus (
                                _id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Year INTEGER,
                                IsoWeek INTEGER,
                                ContentJson TEXT)"
let offersTable = @"CREATE TABLE IF NOT EXISTS Offers (
                                    _id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Year INTEGER,
                                    IsoWeek INTEGER,
                                    ContentJson TEXT)"

let cn = new SQLiteConnection(cs)
cn.Open()

[menusTable; offersTable;] 
|> Seq.iter (fun sql ->
                let cmd = new SQLiteCommand(sql, cn)
                cmd.ExecuteNonQuery() |> ignore
            )

cn.Close()