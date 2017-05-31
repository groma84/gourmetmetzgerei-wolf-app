namespace GmwApp.Database

open FSharp.Data.Sql
open Newtonsoft.Json

open GmwApp.Data.Constants
open GmwApp.Data.Types
open GmwApp.Data.Errors

module Offers =

    type SqlData = SqlDataProvider<Common.DatabaseProviderTypes.SQLITE,
                                    ConnectionString = DB_TYPEPROVIDER_CONNECTIONSTRING,
                                    ResolutionPath = @"..\packages\System.Data.SQLite.Core\lib\net46",
                                    CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL,
                                    UseOptionTypes = true>

    


    let load connectionString yearAndWeek =
        let fromJson json : Angebot [] = 
            JsonConvert.DeserializeObject<Angebot []> json

        printfn "Offers -> load: ConnectionString: %s" connectionString

        let ctx = SqlData.GetDataContext connectionString

        let data =
            query {
                for item in ctx.Main.Offers do
                where (item.Year = Some(int64 yearAndWeek.Year) && item.IsoWeek = Some(int64 yearAndWeek.Week))
                select (item)
                headOrDefault
            }
        let result = if isNull data then None else Some data.ContentJson
        let parsed = match result with
                        | Some json -> Option.map fromJson json
                        | _ -> None

        (parsed, FromDatabase)
        
    let save connectionString yearAndWeek (data : Angebot []) =
        let toJson o =
            JsonConvert.SerializeObject (o, Formatting.Indented)

        let ctx = SqlData.GetDataContext connectionString
        
        let table = ctx.Main.Offers

        let item = table.Create()
        item.ContentJson <- Some (data |> toJson)
        item.IsoWeek <- Some (int64 yearAndWeek.Week)
        item.Year <- Some (int64 yearAndWeek.Year)
        ctx.SubmitUpdates()

        data