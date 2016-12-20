module Database

open System.Data
open System.Data.SQLite

open Chessie.ErrorHandling
open Errors
let firstTable = @"CREATE TABLE IF NOT EXISTS Menus (
                            _id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Year INTEGER,
                            IsoWeek INTEGER,
                            ContentJson TEXT)"

let buildConnectionString dbName =
    sprintf "Data Source=%s;Version=3" dbName

let createDatabase (connectionString : string) =
    //try
        let cn = new SQLiteConnection(connectionString)
        cn.Open()

        let createTableScript = firstTable
        let cmd = new SQLiteCommand(createTableScript, cn)
        let result = cmd.ExecuteNonQuery()
        cn.Close()

        ok true
    //with
      //  | ex -> fail (Error.CreateDatabaseFailed ex)   
