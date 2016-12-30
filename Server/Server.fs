namespace GmwApp.Server

open GmwApp.Data.Errors
open GmwApp.Data.Types
open GmwApp.Database

module Server = 
    let loadFromDbOrDownloadAndParseIfNecessary connectionString url date (loadFunction : YearAndWeek -> 'a option * DataOrigin) (parseFunction : string -> 'a option * DataOrigin) (saveFunction : YearAndWeek -> 'a -> 'a)  =
        let downloadAndParseIfNecessary weekAndYear =
            let storeInDb dataToStore =
                printfn "Storing new data in database: %A" dataToStore
                match dataToStore with
                    | Some data, _ -> Some (saveFunction weekAndYear data)
                    | _ -> Option<'a>.None
                |> ignore
            
            let getViaDownloadAndStoreInDatabase =        
                printfn "No existing data found, downloading from '%s'" url
                let parsedData = Downloader.download url
                                |> parseFunction
                storeInDb parsedData
                parsedData
                
            getViaDownloadAndStoreInDatabase

        let weekAndYear = DateHelper.getWeekAndYear date
        printfn "Loading data for year %i and week %i" weekAndYear.Year weekAndYear.Week
        let fromDb = loadFunction weekAndYear
        let data, _ = match fromDb with
                        | (Some x, _) -> fromDb
                        | (None, _) -> downloadAndParseIfNecessary weekAndYear

        data

    let getTagesmenue dbName url date =
        let connectionString = Helper.buildConnectionString dbName
        let loadF = Db.loadMenus connectionString
        let saveF = Db.saveMenus connectionString
        let parseF = Parser.parseMittagsmenue
        loadFromDbOrDownloadAndParseIfNecessary connectionString url date loadF parseF saveF

    let getAngebote dbName url date : Angebot [] option =
        let connectionString = Helper.buildConnectionString dbName
        let loadF = Db.loadOffers connectionString
        let saveF = Db.saveOffers connectionString
        let parseF = Parser.parseAngebote
        loadFromDbOrDownloadAndParseIfNecessary connectionString url date loadF parseF saveF
