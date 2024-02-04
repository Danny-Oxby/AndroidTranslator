using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using SQLite;
using System;
using System.Collections.Generic;

namespace SqlLibrary
{
    public static class Display
    {
        private static readonly SQLiteConnection db = FindConnection.CreateConnection();
        public static List<IDisplayMdl> DisplayMdlMessageList = null;
        private static readonly List<string> MessageList = new List<string>()
        {
            //single words
            "Translate", "Allergies", "Allergy", "Settings",
            "Logout", "Image", "Text", "Add",
            "Take Picture", "List", "Colour", "Language",
            "Save", "Cancle", "Accept", "Reject",
            "Hide", "Show", "Search", "Update",
            "Display", "Warning", "Failure", "Delete",
            "Changes", "Password", "Recovery",
            "Terms And Conditions", "Match Whole Word",
            "Invalid Name Input",
            //Messages
            "As Agreed Upon, We Can Not Garentee That The Scanned Text Or Translated Text Is Correct Continue At Your Own Risk",
            "You Must Enable Both, A WiFi Connection And Camera Permissions To Access This",
            "Terms And Conditions Go Here",
            "Changing Langauge Will Trigger Application Restart, And Will Not Update Your Allergies",
            "Number Of Allergies Found",
            "Invalid Password Select Another",
        };

        internal static void InsertDefaultMessages(int user_id)
        {
            try
            {
                if (!FindConnection.TableNotExist("Display_Text"))
                {
                    foreach (string Message in MessageList)
                    {
                        db.Insert(new DisplayMdl() { User_Id = user_id, SearchName = Message, DisplayValue = Message});
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(InsertDefaultMessages), IssueList.Display);
            }
        }

        //update the table item to have the translated value match the current user display langauge
        internal static void UpdateMessages(IDisplayMdl newdisplay) //this model must have a matching id value
        {
            try
            {
                db.Update(newdisplay);
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateMessages), IssueList.Display);
            }
        }
        internal static List<IDisplayMdl> GetDisplayInformation(int user_id)
        {
            if(DisplayMdlMessageList == null || DisplayMdlMessageList.Count == 0) //if my list is null
            {
                DisplayMdlMessageList = new List<IDisplayMdl>(); //make a list
                try //add values to the list
                {
                    var temp = db.Query<DisplayMdl>("SELECT * FROM Display_Text WHERE User_Id = ?", user_id);
                    DisplayMdlMessageList.AddRange(temp); //no direct interface due to .net security quirk
                }
                catch(Exception ex)
                {
                    Log.LogIssue(ex.Message, nameof(GetDisplayInformation), IssueList.Display);
                }
            }
            return DisplayMdlMessageList;
        }

        public static void UpdateOneDisplay(string ExpectedValue, string NewValue, int user_id)
        {
            try
            {
                //find the item
                DisplayMdl dispalyitem = db.FindWithQuery<DisplayMdl>("SELECT * FROM Display_Text WHERE User_Id = ? AND ExpectedValue = ?", user_id, ExpectedValue);
                //update the item
                dispalyitem.DisplayValue = NewValue;
                if (dispalyitem != null)
                    db.Update(dispalyitem);
            }
            catch(Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateOneDisplay), IssueList.Display);
            }
        }
        public static void UpdateOneDisplay(IDisplayMdl dispalyitem)
        {
            try
            {
                db.Update(dispalyitem);
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateOneDisplay), IssueList.Display);
            }
        }

        public static List<IDisplayMdl> RefreshList(int user_id) //call this after you update thet language display
        {
            if(DisplayMdlMessageList != null)
                DisplayMdlMessageList.Clear(); //clear the list

            DisplayMdlMessageList = null; //set it to null
            return GetDisplayInformation(user_id); //update the list
        }
        public static string FindDispalyValue(string ExpectedValue, int user_id)
        {
            IDisplayMdl DisplayLsit = GetDisplayInformation(user_id).Find(o => o.SearchName == ExpectedValue);

            if (DisplayLsit != null)
            {
                return DisplayLsit.DisplayValue;
            }

            return "Missing Value";
        }
    }

    public static class LoginDisplay //login page specific value
    {
        private static readonly SQLiteConnection db = FindConnection.CreateConnection();

        public static List<ILoginDisplayMdl> LoginMdlMessageList = null;
        private static readonly List<string> LoginList = new List<string>()
        {
            //single words
            "Login", "Accept", "Success", "Failure",
            "Password", 
            //Messages
            "Invalid Password Select Another",
            "Password Recovery",
            "Email Recovery",
            "Create User",
            "Only Valid Emails Are Accepted",
            "User Created",
            "No Matching User",
            "Email Not Supported By Device",
        };
        internal static void InsertLoginDefaultMessages() //default language is english at this point in time
        {
            try
            {
                int defaultLanguageID = Language.SelectLanguage("en-GB").Language_Id;
                if (!FindConnection.TableNotExist("Login_Text"))
                {
                    foreach (string Message in LoginList)
                    {
                        db.Insert(new LoginDisplayMdl() { SearchName = Message, DisplayValue = Message, Language_Id = defaultLanguageID });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(InsertLoginDefaultMessages), IssueList.Display);
            }
        }
        internal static List<ILoginDisplayMdl> GetLoginDisplayInformation()
        {
            if (LoginMdlMessageList == null || LoginMdlMessageList.Count == 0) //if my list is null
            {
                LoginMdlMessageList = new List<ILoginDisplayMdl>(); //make a list
                try //add values to the list
                {
                    var temp = db.Query<LoginDisplayMdl>("SELECT * FROM Login_Text;");
                    LoginMdlMessageList.AddRange(temp); //no direct interface due to .net security quirk
                }
                catch (Exception ex)
                {
                    Log.LogIssue(ex.Message, nameof(GetLoginDisplayInformation), IssueList.Display);
                }
            }
            return LoginMdlMessageList;
        }
        public static void UpdateOneLoginDisplay(string ExpectedValue, string NewValue) //for only the login page
        {
            try
            {
                //find the item
                LoginDisplayMdl dispalyitem = db.FindWithQuery<LoginDisplayMdl>("SELECT * FROM Login_Text WHERE ExpectedValue = ?", ExpectedValue);
                //update the item
                dispalyitem.DisplayValue = NewValue;
                if (dispalyitem != null)
                    db.Update(dispalyitem);
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateOneLoginDisplay), IssueList.Display);
            }
        }
        public static void UpdateOneLoginDisplay(ILoginDisplayMdl display) //for only the login page
        {
            try
            {
                db.Update(display);
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateOneLoginDisplay), IssueList.Display);
            }
        }

        public static int ReturnLoginLangaugeID()
        {
            var temp = db.FindWithQuery<LoginDisplayMdl>("SELECT Language_Id FROM Login_Text;");
            return temp.Language_Id;
        }

        public static List<ILoginDisplayMdl> RefreshList() //call this after you update the language display
        {
            if (LoginMdlMessageList != null)
                LoginMdlMessageList.Clear(); //clear the list

            LoginMdlMessageList = null; //set it to null
            return GetLoginDisplayInformation(); //update the list
        }
        public static string FindDispalyValue(string ExpectedValue)
        {
            ILoginDisplayMdl DisplayLsit = GetLoginDisplayInformation().Find(o => o.SearchName == ExpectedValue);

            if (DisplayLsit.DisplayValue != null)
            {
                return DisplayLsit.DisplayValue;
            }

            return "Missing Value";
        }
    }
}
