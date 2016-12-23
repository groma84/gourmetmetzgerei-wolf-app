namespace GmwApp.Server


open GmwApp.Data.Types
open GmwApp.Database

module Db =
    let bootstrap databaseFilePath = 
        let connectionString = Helper.buildConnectionString databaseFilePath 
        connectionString |> Setup.createDatabase |> ignore
        //connectionString |> Setup.createVersionTable

    let loadMenus connectionString yearAndWeek =
        Menus.loadMenus connectionString yearAndWeek