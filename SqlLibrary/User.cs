using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using SQLite;
using System;

namespace SqlLibrary
{
    public class User
    {
        private static readonly SQLiteConnection db = FindConnection.CreateConnection();
        public static IUserMdl CurrentUser { get; private set; }

        private static string ReturnUserList() // testing method, not intended functioanlity for end product
        {
            TableQuery<UserMdl> stockList = db.Table<UserMdl>();
            //List<User> users = stockList.ToList(); //convert from query to generic list

            string passwords = "";
            foreach (IUserMdl item in stockList)
            {
                passwords += string.Concat(item.Password, " ");
            }

            return passwords;
        }

        public static void ClearSelectedUser() // for timeout mechinisms and for returning to login
        {
            CurrentUser = null;
        }
        public static string DoesUserExist(string Email)
        {
            string userexists = null;
            try
            {
                if (!FindConnection.TableNotExist("User"))
                {
                    //retrun the first found user
                    var finduser = db.FindWithQuery<UserMdl>("SELECT * FROM User WHERE Username = ?", Email);
                    if (finduser != null)
                        userexists = finduser.Password;
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(DoesUserExist), default, string.Concat("Params: ", Email));
            }

            return userexists; //return the found user password
        }
        public static bool UniqueUserCheck(string password) //does the user already exist, true for yes
        {
            try
            {
                if (!FindConnection.TableNotExist("User"))
                {
                    //retrun the first found user
                    var User = db.FindWithQuery<UserMdl>("SELECT * FROM User WHERE Password = ?", password);
                    if (User != null)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(SelectUser));
                return true; //return true since we can garentee the user dosn't exist
            }

            return false;
        }

        public static bool UniquePasswordCheck(string password, int user_id) //is the password already taken by naother userr
        {
            try
            {
                if (!FindConnection.TableNotExist("User"))
                {
                    //retrun the first found user
                    var User = db.FindWithQuery<UserMdl>("SELECT * FROM User WHERE Password = ? AND User_Id != ?", password, user_id);
                    if (User == null)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UniqueUserCheck));
                return false; //return true since we can garentee the user dosn't exist
            }

            return false;
        }
        public static IUserMdl SelectUser(string password)
        {
            if (CurrentUser == null) // has the user been found before
            {
                try
                {
                    if (!FindConnection.TableNotExist("User"))
                    {
                        //retrun the first found user
                        CurrentUser = db.FindWithQuery<UserMdl>("SELECT * FROM User WHERE Password = ?", password);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogIssue(ex.Message, nameof(SelectUser));
                    return null; //return null since the user is not found
                }
            }
            return CurrentUser; //return the found user
        }

        public static IUserMdl InsertUser(IUserMdl user, string default_language = "en-GB") // this should only be called once per install
        {
            try
            {
                if (!FindConnection.TableNotExist("User"))
                {
                    db.Insert(user);
                    //add the default settings needed for after a user is created
                    Setting.InsertSettingDefault(SelectUser(user.Password).User_Id, Language.SelectLanguage("en-GB").Language_Id);
                    //add the default allergies needed for after a user is created
                    CreateTables.AssignUserDefaults(user);
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(InsertUser), default, string.Concat("Id:", user.User_Id));
            }
            return SelectUser(user.Password);
        }
        public static void UpdateUser(IUserMdl user)
        {
            try
            {
                if (!FindConnection.TableNotExist("User"))
                {
                    if (db.Update(user) < 1)
                    {
                        Log.LogIssue("Someone tries to update a non-exisitn user", nameof(UpdateUser), IssueList.Settings);
                    }
                    else
                    {
                        CurrentUser = user; //update the current user model to match the updated value
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateUser), IssueList.Settings);
            }
        }
        public static string DeleteUser(IUserMdl user) // this should never be called currently (except when testing)
        {
            //DELETE from User where Password = 'abcde';
            //db.Execute($"DELETE FROM User where Password = '{user.Password}' ;");
            try
            {
                db.Delete(user); //delete using PK set in model 
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(DeleteUser));
            }
            return ReturnUserList();
        }
        private static string DeleteAllUsers() // this should never be called currently (only use for testing)
        {
            //DELETE from User where Password = 'abcde';
            try
            {
                db.Execute($"DELETE FROM User;");
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(DeleteUser));
            }
            return ReturnUserList();
        }
    }
}

        //private static int GetUserID(string password) //for getting the user ID after a user is created
        //{
        //    try
        //    {
        //        if (!FindConnection.TableNotExist("User"))
        //        {
        //            //retrun the first found user
        //            int value = db.FindWithQuery<int>("SELECT User_Id FROM User WHERE Password = ?", password);
        //            return value; //this is returning 0 instead of the expected value (of 46 id "123" is input"
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.LogIssue(ex.Message, nameof(GetUserID));
        //    }
        //    return 0; //return 0 since the user is not found
        //}