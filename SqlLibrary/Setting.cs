using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlLibrary
{
    public static class Setting //delete should be handeled on user delete cascade
    {
        private static ISettingMdl CurrentSettings = null;
        private static readonly SQLiteConnection db = FindConnection.CreateConnection();

        public static ISettingMdl SelectSettigns() //gets the settings of the current user
        {
            if(CurrentSettings == null)
            {
                if(User.CurrentUser != null) //id user is null skip
                {
                    try
                    {
                        if (!FindConnection.TableNotExist("Setting"))
                        {
                            //retrun the first found user
                            CurrentSettings = db.FindWithQuery<SettingMdl>("SELECT * FROM Setting WHERE User_Id = ?", User.CurrentUser.User_Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogIssue(ex.Message, nameof(SelectSettigns), IssueList.Other, String.Concat("Params : ", User.CurrentUser.User_Id));
                        return null; //return null since the user is not found
                    }
                }
                else
                {
                    Log.LogIssue("User id is null", nameof(SelectSettigns), IssueList.Other, String.Concat("User id not exists ", User.CurrentUser.User_Id));
                }
            }

            return CurrentSettings;
        }
        internal static void InsertSettingDefault(int user_id, int language_id)
        {
            try
            {
                if (!FindConnection.TableNotExist("Setting") && !FindConnection.TableNotExist("User") && !FindConnection.TableNotExist("Language")) // link the allergys to a user
                {
                    db.Execute("INSERT or IGNORE INTO Setting (User_Id, Language_Id) VALUES (?,?);", user_id, language_id);
                }
            }
            catch(Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(SelectSettigns), default, String.Concat("Params : ", user_id, " ", language_id));
            }
        }
        public static void UpdateSetting(ISettingMdl newvalue) //the settign id must match the old id that you want to update
        {
            try
            {
                if (db.Update(newvalue) < 1) // SQL change happens here
                {
                    Log.LogIssue("Someone tried to update a non-existing setting", nameof(UpdateSetting), SqlLibrary.IssueList.Settings,
                    string.Concat("Params : ", newvalue.Setting_ID, " "));
                }
                else
                {
                    //update the current settings value to match the updated value
                    CurrentSettings = newvalue;
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateSetting), SqlLibrary.IssueList.Settings,
                string.Concat("Params : ",newvalue.Setting_ID , " ", newvalue.Full_Word, " ", newvalue.Warning_State, " ", newvalue.TC, " ", newvalue.Language_Id, " ",
                newvalue.Font_Size, " ", newvalue.Font_Type));
            }
        }
        public static void SetTACToTrue(int user_id)
        {
            try
            { //SQLite has no bool so here 1 = true
                if (db.Execute("UPDATE Setting SET Terms_And_Condition_Accepted = 1 WHERE User_Id = ?", user_id) < 1)
                {
                    Log.LogIssue("Someone tried to update the TAC of a non-existing user", nameof(SetTACToTrue), 
                        SqlLibrary.IssueList.Settings, string.Concat("Params : ", user_id));
                }
                else
                {
                    CurrentSettings.TC = true; //no check needed as CurrentSettings is set on login / before this method can be called
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(SetTACToTrue), SqlLibrary.IssueList.Settings,
                string.Concat("Params : ", user_id));
            }
        }
        public static void HideWarningState(int user_id)
        {
            try
            { //SQLite has no bool so here 1 = true
                if (db.Execute("UPDATE Setting SET Hide_Warning = 1 WHERE User_Id = ?", user_id) < 1)
                {
                    Log.LogIssue("Someone tried to update the WarningState of a non-existing user", nameof(HideWarningState),
                        SqlLibrary.IssueList.Settings, string.Concat("Params : ", user_id));
                }
                else
                {
                    CurrentSettings.Warning_State = true; //no check needed as CurrentSettings is set on login / before this method can be called
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(HideWarningState), SqlLibrary.IssueList.Settings,
                string.Concat("Params : ", user_id));
            }
        }
    }
}
