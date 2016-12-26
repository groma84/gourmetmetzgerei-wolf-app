namespace GmwApp.Database

open System.Data
open System.Data.SQLite

open GmwApp.Data.Errors

module Setup =
    let createDatabase (connectionString : string) =
        let menusTable = @"CREATE TABLE IF NOT EXISTS Menus (
                                    _id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Year INTEGER,
                                    IsoWeek INTEGER,
                                    ContentJson TEXT)"
                                    
        let cn = new SQLiteConnection(connectionString)
        cn.Open()

        let createTableScript = menusTable
        let cmd = new SQLiteCommand(createTableScript, cn)
        let result = cmd.ExecuteNonQuery()
        cn.Close()

        FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent |> Event.add (printfn "Executing SQL: %s")



    let createVersionTable (connectionString : string) =
        let versionTable = @"CREATE TABLE IF NOT EXISTS Version (
                                    Version INTEGER PRIMARY KEY,
                                    UpdatedAt TEXT)" // Datum in SQLite -> ISO8601-String
                                    
        let cn = new SQLiteConnection(connectionString)
        cn.Open()

        let newTableScript = versionTable    
        let cmd = new SQLiteCommand(newTableScript, cn)
        let result = cmd.ExecuteNonQuery()
        cn.Close()
