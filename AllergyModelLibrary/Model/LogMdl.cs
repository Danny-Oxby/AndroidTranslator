using AllergyModelLibrary.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllergyModelLibrary.Model
{
    //enum IssueTypes // consider matching the enums to the Issue_Type table
    //{
    //    Proformance,
    //    Translation,
    //    Image_Reading,
    //    Settings,
    //    Display,
    //    Usability,
    //    Other,
    //};

    [Table("Issue_Log")]
    public class LogMdl : ILogMdl
    {
        [Column("Issue_Id"), PrimaryKey, AutoIncrement] public int Issue_ID { get; set; }
        [Column("User_Id"), NotNull] public int User_Id { get; set; }
        [Column("Issue_Type_Id"), NotNull] public int Issue_Type { get; set; } // consider making this an enum, how would that interact with SQLite
        [Column("Method_Name")] public string Issue_Location { get; set; }
        [Column("Issue_Date")] public string Issue_Date { get; set; } // Sting as unsire if SQLite can handle date's
        [Column("Method_Params")] public string Issue_Parameters { get; set; }
    }
}
