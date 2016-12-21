namespace GmwApp.SuaveHost

open GmwApp.Database

module Bootstrap =
    let database databaseName =
        let connectionString = Helper.buildConnectionString databaseName 
        connectionString |> Setup.createDatabase 
        connectionString |> Setup.createVersionTable