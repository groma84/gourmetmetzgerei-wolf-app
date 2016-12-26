namespace GmwApp.Server

open System 
open System.Globalization

open GmwApp.Data.Types

module DateHelper =
    let getWeekAndYear (date : DateTime) =
        let getIsoWeek (dt: DateTime) =
            let day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek dt
            let dayModified =   if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) then
                                    dt.AddDays 3.0
                                else
                                    dt

            let isoWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear (dayModified, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
            isoWeek

        // as the parsed webpages are not always updated instantly we want to look at "today" only after 9 AM
        let modifiedDate = date.AddHours -9.0
        
        {
            Year = modifiedDate.Year;
            Week = getIsoWeek modifiedDate; 
        }
