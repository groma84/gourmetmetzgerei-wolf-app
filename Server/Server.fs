namespace GmwApp.Server

open GmwApp.Data.Errors
open GmwApp.Data.Types
open GmwApp.Database

module Server = 
    let getTagesmenue dbName url date =
        let connectionString = Helper.buildConnectionString dbName

        let downloadAndParseIfNecessary weekAndYear =
            let storeInDb dataToStore =
                printfn "Storing new data in database: %A" dataToStore
                match dataToStore with
                    | Some data, _ -> Some (Db.saveMenus connectionString weekAndYear data)
                    | _ -> Option<Tagesmenu []>.None
                |> ignore
            
            let getMenusViaDownloadAndStoreInDatabase =        
                printfn "No existing data found, downloading from '%s'" url
                let parsedData = Downloader.download url
                                |> Parser.parseMittagsmenue
                storeInDb parsedData
                parsedData
                
            getMenusViaDownloadAndStoreInDatabase

        let weekAndYear = DateHelper.getWeekAndYear date
        printfn "Loading data for year %i and week %i" weekAndYear.Year weekAndYear.Week
        let fromDb = Db.loadMenus connectionString weekAndYear
        let data, _ = match fromDb with
                        | (Some x, _) -> fromDb
                        | (None, _) -> downloadAndParseIfNecessary weekAndYear

        data

    let getAngebote (url) =
        Downloader.download url
        |> Parser.parseAngebote