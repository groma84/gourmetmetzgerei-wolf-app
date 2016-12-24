namespace GmwApp.Database

open FSharp.Data.Sql
open Microsoft.FSharpLu.Json

open GmwApp.Data.Types
open GmwApp.Data.Errors

module Menus =

    type SqlData = SqlDataProvider<Common.DatabaseProviderTypes.SQLITE,
                                    ConnectionString = @"Data Source=D:\git-repos\groma84@github\gourmetmetzgerei-wolf-app\gmwapp.sqlite;Version=3",
                                    ResolutionPath = @"..\packages\System.Data.SQLite.Core\lib\net46",
                                    CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL,
                                    UseOptionTypes = true>

    let loadMenus connectionString yearAndWeek =
        printfn "loadMenus ConnectionString: %s" connectionString

        let ctx = SqlData.GetDataContext connectionString

        FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent |> Event.add (printfn "loadMenus Executing SQL: %s")

        // let _,_,_, contentJson =
        //     query {
        //         for item in ctx.Main.Menus do
        //         where (item.Year = Some(int64 yearAndWeek.Year) && item.IsoWeek = Some(int64 yearAndWeek.Week))
        //         select (item.Id, item.Year, item.IsoWeek, item.ContentJson)
        //         headOrDefault
        //     }

        // let parsed = Option.map Compact.deserialize<Tagesmenu []> contentJson

        let data =
            query {
                for item in ctx.Main.Menus do
                where (item.Year = Some(int64 yearAndWeek.Year) && item.IsoWeek = Some(int64 yearAndWeek.Week))
                select (item)
                headOrDefault
            }
        let result = if isNull data then None else Some data.ContentJson
        let parsed = match result with
                        | Some json -> Option.map Compact.deserialize<Tagesmenu []> json
                        | _ -> None

        (parsed, FromDatabase)
        
    let saveMenus connectionString yearAndWeek (data : Tagesmenu []) =
        let ctx = SqlData.GetDataContext connectionString
        FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent |> Event.add (printfn "saveMenus Executing SQL: %s")
        
        let menuTable = ctx.Main.Menus

        let item = menuTable.Create()
        item.ContentJson <- Some (Compact.serialize data)
        item.IsoWeek <- Some (int64 yearAndWeek.Week)
        item.Year <- Some (int64 yearAndWeek.Year)
        ctx.SubmitUpdates()

        data
