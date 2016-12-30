namespace GmwApp.Server


open GmwApp.Data.Types
open GmwApp.Database

module Db =
    let bootstrap databaseFilePath = 
        let connectionString = Helper.buildConnectionString databaseFilePath 
        connectionString |> Setup.createDatabase |> ignore
        connectionString |> Setup.createOffersTable |> ignore

    let loadMenus connectionString yearAndWeek =
        Menus.load connectionString yearAndWeek

    let saveMenus connectionString yearAndWeek data =
        Menus.save connectionString yearAndWeek data

    let loadOffers connectionString yearAndWeek =
        Offers.load connectionString yearAndWeek

    let saveOffers connectionString yearAndWeek data =
        Offers.save connectionString yearAndWeek data