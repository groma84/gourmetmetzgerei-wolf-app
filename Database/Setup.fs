namespace GmwApp.Database

open System.Data
open System.Data.SQLite

open Chessie.ErrorHandling
open GmwApp.Data.Errors

module Setup =
    let createDatabase (connectionString : string) =
        let menusTable = @"CREATE TABLE IF NOT EXISTS Menus (
                                    _id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Year INTEGER,
                                    IsoWeek INTEGER,
                                    ContentJson TEXT)"
                                    
        try
            let cn = new SQLiteConnection(connectionString)
            cn.Open()

            let createTableScript = menusTable
            let cmd = new SQLiteCommand(createTableScript, cn)
            let result = cmd.ExecuteNonQuery()
            cn.Close()

            ok true
        with
            | ex -> fail (Error.CreateDatabaseFailed ex)   

    let createVersionTable (connectionString : string) =
        let versionTable = @"CREATE TABLE IF NOT EXISTS Version (
                                    Version INTEGER PRIMARY KEY,
                                    UpdatedAt TEXT)" // Datum in SQLite -> ISO8601-String
                                    
        try
            let cn = new SQLiteConnection(connectionString)
            cn.Open()

            let newTableScript = versionTable    
            let cmd = new SQLiteCommand(newTableScript, cn)
            let result = cmd.ExecuteNonQuery()
            cn.Close()

            ok true
        with
            | ex -> fail (Error.CreateVersionTableFailed ex)   
