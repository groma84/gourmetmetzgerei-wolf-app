namespace GmwApp.Server

open Chessie.ErrorHandling
open GmwApp.Data.Errors

module Server = 
    let getTagesmenue connectionString url date =   
        let weekAndYear = DateHelper.getWeekAndYear date
        // Idee: wir packen noch eine Origin DU mit ran für FromDatabase/Downloaded und werten das jeweils mit einem Pattern Matching aus
        Db.loadMenus connectionString weekAndYear    
        >>= Downloader.download url
        >>= Db.saveMenus connectionString weekAndYear
        >>= Parser.parseMittagsmenue
        
    let getAngebote (url) =
        Downloader.download url
        >>= Parser.parseAngebote