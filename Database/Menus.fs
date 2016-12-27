namespace GmwApp.Database

open FSharp.Data.Sql
open Chiron

open GmwApp.Data.Constants
open GmwApp.Data.Types
open GmwApp.Data.Errors

module Menus =

    type SqlData = SqlDataProvider<Common.DatabaseProviderTypes.SQLITE,
                                    ConnectionString = DB_TYPEPROVIDER_CONNECTIONSTRING,
                                    ResolutionPath = @"..\packages\System.Data.SQLite.Core\lib\net46",
                                    CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL,
                                    UseOptionTypes = true>

    


    let loadMenus connectionString yearAndWeek =
        let fromJsonToTagesmenu json : Tagesmenu [] = 
            json
            |> Json.parse
            |> Json.deserialize

        printfn "loadMenus ConnectionString: %s" connectionString

        let ctx = SqlData.GetDataContext connectionString

        let data =
            query {
                for item in ctx.Main.Menus do
                where (item.Year = Some(int64 yearAndWeek.Year) && item.IsoWeek = Some(int64 yearAndWeek.Week))
                select (item)
                headOrDefault
            }
        let result = if isNull data then None else Some data.ContentJson
        let parsed = match result with
                        | Some json -> Option.map fromJsonToTagesmenu json
                        | _ -> None

        (parsed, FromDatabase)
        
    let saveMenus connectionString yearAndWeek (data : Tagesmenu []) =
        let toJson o =
            o |> Json.serialize |> Json.format 

        let ctx = SqlData.GetDataContext connectionString
        
        let menuTable = ctx.Main.Menus

        let item = menuTable.Create()
        item.ContentJson <- Some (data |> toJson)
        item.IsoWeek <- Some (int64 yearAndWeek.Week)
        item.Year <- Some (int64 yearAndWeek.Year)
        ctx.SubmitUpdates()

        data
