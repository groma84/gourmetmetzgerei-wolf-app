namespace GmwApp.Server

open GmwApp.Data.Errors
open GmwApp.Data.Types
open GmwApp.Database

module Server = 
    let getTagesmenue dbName url date =
        let connectionString = Helper.buildConnectionString dbName

        let downloadAndParseIfNecessary weekAndYear existingData =
            let getMenusViaDownloadAndStoreInDatabase =
                let storeInDb dataToStore =
                    match dataToStore with
                        | Some data, _ -> Some (Db.saveMenus connectionString weekAndYear data)
                        | _ -> Option<Tagesmenu []>.None
                    |> ignore

                    dataToStore        
                    

                let result =    Downloader.download url
                                |> Parser.parseMittagsmenue
                                |> storeInDb           
                                
                result

            match existingData with
                | (Some x, _) -> existingData
                | (None, _) -> getMenusViaDownloadAndStoreInDatabase
        

        let weekAndYear = DateHelper.getWeekAndYear date

        let data, _ =   Db.loadMenus connectionString weekAndYear    
                        |> downloadAndParseIfNecessary weekAndYear
        data

    let getAngebote (url) =
        Downloader.download url
        |> Parser.parseAngebote