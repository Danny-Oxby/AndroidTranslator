using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using SQLite;
using System;
using System.Collections.Generic;

namespace SqlLibrary
{
    public class Allergy
    {
        private static readonly SQLiteConnection db = FindConnection.CreateConnection();

        public static void InsertAllergy(IAllergyMdl allergy) // not the interace since the SQL attribute tags are needed
        {
            try
            {
                db.Insert(allergy); //table is found through the SQLite attribute in the model
            }
            catch(Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(InsertAllergy), default, string.Concat("params: ", allergy.Name));
            }
        }
        internal static void InsertAllergyDefault(IUserMdl u)
        {
            if (!FindConnection.TableNotExist("Allergy") && !FindConnection.TableNotExist("User")) // link the allergys to a user
            {
                db.Execute("INSERT OR IGNORE INTO Allergy " +
                    "(Allergy_Name, User_Id, Colour, Search) " +
                    "VALUES('Wheat', ?, '#FFFF0000', 1), ('Nut', ?, '#FF0000FF', 1), " +
                    "('Soy', ?, '#FFADD8E6', 1), ('Milk', ?, '#FF008000', 1), ('Egg', ?, '#FFFFFF00', 1)",
                    u.User_Id, u.User_Id, u.User_Id, u.User_Id, u.User_Id);
            }
        }
        public static void UpdateAllergy(IAllergyMdl allergy) //update is based on name PK, currently dosn't support name changes due to this
        {
            try
            {
                db.Update(allergy);
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateAllergy), default, string.Concat("params: ", allergy.Name));
            }
        }
        public static void UpdateAllergy(IAllergyMdl newnamever, string oldallergyname)
        {
            try
            {
                //UPDATE Allergy SET Allergy_Name = "wheat", Colour = "FF0000", Search = 1 where Allergy_Name = "wheat2"; 
                db.Execute("UPDATE Allergy SET Allergy_Name = ?, Colour = ?, Search = ? where Allergy_Name = ?;", 
                    newnamever.Name, newnamever.Colour, newnamever.Search, oldallergyname); //update dosn't have to check for existing
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(UpdateAllergy), default,
                    string.Concat("Params: ", oldallergyname, " ", newnamever.Name));
            }
        }
        public static void DeleteAllergy(IAllergyMdl allergy)
        {
            try
            {
                db.Delete(allergy); //table is found through the SQLite attribute in the model
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(DeleteAllergy), default, string.Concat("params: ", allergy.Name));
            }
        } //user id not needed since allergy id is unique
        public static void DeleteAllergy(string allergy_id)
        {
            try
            {
                db.Execute("DELETE FROM Allergy WHERE Allergy_Name = ?", allergy_id); //table is found through the SQLite attribute in the model
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(DeleteAllergy), default, string.Concat("params:", allergy_id));
            }
        }
        public static IAllergyMdl SelectAllergy(string name, int user_id)
        {
            IAllergyMdl found = null;
            try
            {
                found = db.FindWithQuery<AllergyMdl>("Select * FROM Allergy WHERE Allergy_Name = ? AND User_Id = ?", name, user_id);
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(SelectAllergy), IssueList.Other, string.Concat("Params: " , name));
            }

            return found;
        }
        public static List<IAllergyMdl> SearchAllAllergys(int user)
        {
            List<IAllergyMdl> found = new List<IAllergyMdl>();
            try
            {
                //have to use add range rather than replacing, due to model to interface implict conversion conflict
                var temp = db.Query<AllergyMdl>("Select * FROM Allergy WHERE User_Id = ? AND Search = 1 ORDER BY Allergy_Name ASC", user);
                found.AddRange(temp);
                //found.AddRange(db.Table<AllergyMdl>().ToList());//table is found through the SQLite attribute in the model
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(SearchAllAllergys));
            }

            return found;
        }
        public static List<IAllergyMdl> SelectAllAllergys(int user)
        {
            List<IAllergyMdl> found = new List<IAllergyMdl>();
            try
            {
                //have to use add range rather than replacing, due to model to interface implict conversion conflict
                var temp = db.Query<AllergyMdl>("Select * FROM Allergy WHERE User_Id = ? ORDER BY Allergy_Name ASC", user);
                found.AddRange(temp);
                //found.AddRange(db.Table<AllergyMdl>().ToList());//table is found through the SQLite attribute in the model
            }
            catch (Exception ex)
            {
                Log.LogIssue(ex.Message, nameof(SelectAllAllergys));
            }

            return found;
        }
    }
}
