using System;
using System.Collections.Generic;
using System.Text;

namespace SqlLibrary
{
    public enum IssueList //link with the table list
    {
        /* db.Execute("INSERT OR IGNORE INTO \"main\".\"Issue_Type\" (\"Issue_Name\") VALUES(\"Proformace\"), (\"Translation\"), " +
           "(\"Image_Reading\"), (\"Settings\"), (\"Display\"), (\"Usability\"), (\"Other\");"); */
        Proformace,
        Translation,
        Image_Reading,
        Settings,
        Display,
        Usability,
        Other,
    }
    public class Log //this matches the issue table
    {
        public static void LogIssue(string message, string methodname, IssueList issue = IssueList.Other, string paramlist = null)
        {
            var temp = false;
            //log the issue here
        }
    }
}
