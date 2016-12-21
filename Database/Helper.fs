namespace GmwApp.Database

module Helper =
    let buildConnectionString dbName =
        sprintf "Data Source=%s;Version=3" dbName
    
    