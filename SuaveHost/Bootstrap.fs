module Bootstrap

open Database

let database databaseName =
    Database.buildConnectionString databaseName 
    |> Database.createDatabase 