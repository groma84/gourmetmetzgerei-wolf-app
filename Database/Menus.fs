namespace GmwApp.Database

open FSharp.Data.Sql
open Microsoft.FSharpLu.Json
open Chessie.ErrorHandling

open GmwApp.Data.Types
open GmwApp.Data.Errors

module Menus =

    type SqlData = SqlDataProvider<Common.DatabaseProviderTypes.SQLITE,
                                    ConnectionString = @"Data Source=D:\git-repos\groma84@github\gourmetmetzgerei-wolf-app\gmwapp.sqlite;Version=3",
                                    ResolutionPath = @"..\packages\System.Data.SQLite.Core\lib\net46",
                                    CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL,
                                    UseOptionTypes = true>

    let loadMenus connectionString yearAndWeek =
        try
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
                            | Some json -> Option.map Compact.deserialize<Tagesmenu array> json
                            | _ -> None

            ok parsed
        with
            | ex -> fail (Error.LoadMenuFailed ex)
        
