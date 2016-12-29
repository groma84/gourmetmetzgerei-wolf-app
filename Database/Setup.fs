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


    let createOffersTable (connectionString : string) =
        let sql = @"CREATE TABLE IF NOT EXISTS Offers (
                                    _id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Year INTEGER,
                                    IsoWeek INTEGER,
                                    ContentJson TEXT)"
                                    
        let cn = new SQLiteConnection(connectionString)
        cn.Open()

        let cmd = new SQLiteCommand(sql, cn)
        let result = cmd.ExecuteNonQuery()
        cn.Close()
