using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using SQLite;
using System.Collections.Generic;

namespace SqlLibrary
{
    public class CreateTables
    {
        private static readonly SQLiteConnection db = FindConnection.CreateConnection();
        private static readonly int Expectedtablenumber = 9;

        public static void DropTables()
        {
            //if (FindConnection.CountTables() == Expectedtablenumber)
            //{
                db.BeginTransaction();
                db.DropTable<LoginDisplayMdl>();
                db.DropTable<DisplayMdl>();
                //issue log table
                db.Execute("DROP TABLE IF EXISTS Issue_Log");
                //issue type table
                db.Execute("DROP TABLE IF EXISTS Issue_Type");
                db.DropTable<AllergyMdl>();
                db.DropTable<SettingMdl>();
                db.DropTable<LanguageMdl>();
                db.DropTable<UserMdl>();
                db.Commit();
            //}
        }
        public static void CreateDefaultTables()
        {
            if (FindConnection.CountTables() != Expectedtablenumber)
            {
                db.BeginTransaction();
                //create section //
                CreateUserTbl();
                CreateLanguageTbl();
                CreateSettingTbl();
                CreateAllergyTbl();
                CreateIssueTbl();
                CreateDisplayTbl();
                CreateLoginPageDisplayTbl();

                //insert section << this could be in the respective methods, but the seperation allows for better testing
                InsertLanguages();
                InsertIssueTypeDefault();
                LoginDisplay.InsertLoginDefaultMessages();
                db.Commit();
                //var temp = conn.Execute("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'table_name'");
            }
        }

        //USER SPECIFIC METHODS
        internal static void AssignUserDefaults(IUserMdl user) 
        {
            Allergy.InsertAllergyDefault(user); // needs user to be created first
            Display.InsertDefaultMessages(user.User_Id); //set the display information
            Display.GetDisplayInformation(user.User_Id); //load the default messages
        }

        // NON-USER SPECIFIC METHODS
        private static void CreateUserTbl()
        {
            if (FindConnection.TableNotExist("User")) 
            {
                db.CreateTable<UserMdl>(); //create the table based on the UserMdl
            }
        }
        private static void CreateLanguageTbl()
        {
            if (FindConnection.TableNotExist("Language"))
            {
                db.CreateTable<LanguageMdl>();
            }
        }
        private static void CreateIssueTypeTbl()
        {
            if (FindConnection.TableNotExist("Issue_Type"))
            {
                db.Execute("CREATE TABLE IF NOT EXISTS \"Issue_Type\" " +
                "(\"Issue_Type_Id\" INTEGER,\"Issue_Name\" TEXT NOT NULL DEFAULT 'Rename' UNIQUE, " +
                "PRIMARY KEY(\"Issue_Type_Id\" AUTOINCREMENT));");
            }
        }
        private static void InsertIssueTypeDefault()
        {
            if (!FindConnection.TableNotExist("Issue_Type"))
            {
                // add the default values
                db.Execute("INSERT OR IGNORE INTO \"main\".\"Issue_Type\"" +
                "(\"Issue_Name\") VALUES(\"Proformace\"), (\"Translation\"), " +
                "(\"Image_Reading\"), (\"Settings\"), (\"Display\"), (\"Usability\"), (\"Other\");");
            }
        }
        private static void CreateSettingTbl()
        {
            if (FindConnection.TableNotExist("Setting")) 
            { 
                db.Execute("CREATE TABLE IF NOT EXISTS \"Setting\" " +
                "(\"Setting_Id\" INTEGER, \"User_Id\" INTEGER NOT NULL, \"Language_Id\" INTEGER NOT NULL, \"Font_Type\" INTEGER NOT NULL DEFAULT 0, " +
                "\"Font_Size\" INTEGER NOT NULL DEFAULT 18, \"Match_Full_Word\" INTEGER NOT NULL DEFAULT 0, " +
                "\"Terms_And_Condition_Accepted\" INTEGER NOT NULL DEFAULT 0, \"Hide_Warning\" INTEGER NOT NULL DEFAULT 0, " +
                "FOREIGN KEY(\"User_Id\") REFERENCES \"User\"(\"User_ID\") ON DELETE CASCADE ON UPDATE NO ACTION, " +
                "FOREIGN KEY(\"Language_Id\") REFERENCES \"Language\"(\"Language_Id\") ON DELETE CASCADE ON UPDATE NO ACTION, " +
                "PRIMARY KEY(\"Setting_Id\" AUTOINCREMENT));");
            }
        }
        private static void CreateAllergyTbl()
        {
            if (FindConnection.TableNotExist("Allergy"))
            {
                db.Execute("CREATE TABLE IF NOT EXISTS \"Allergy\" " +
                "( \"Allergy_Id\" INTEGER NOT NULL UNIQUE, \"Allergy_Name\" TEXT NOT NULL COLLATE NOCASE, \"User_Id\" INTEGER NOT NULL, " +
                "\"Colour\" TEXT NOT NULL DEFAULT 'FFFFFF', \"Search\" INTEGER NOT NULL DEFAULT 1, " +
                "FOREIGN KEY(\"User_Id\") REFERENCES \"User\"(\"User_ID\") ON DELETE CASCADE ON UPDATE NO ACTION, " +
                "PRIMARY KEY(\"Allergy_Id\"));");
            }
        }
        private static void CreateIssueTbl()
        {
            if (FindConnection.TableNotExist("Issue_Log"))
            {
                CreateIssueTypeTbl();
                db.Execute("CREATE TABLE IF NOT EXISTS \"Issue_Log\" " +
                    "(\"Issue_Id\" INTEGER, \"User_Id\" INTEGER NOT NULL, \"Issue_Type_Id\" INTEGER NOT NULL, " +
                    "\"Message\" TEXT, \"Method_Name\" TEXT, \"Method_Params\" TEXT, \"Issue_Date\" TEXT, " +
                    "FOREIGN KEY(\"User_Id\") REFERENCES \"User\"(\"User_ID\") ON DELETE CASCADE ON UPDATE NO ACTION, " +
                    "FOREIGN KEY(\"Issue_Type_Id\") REFERENCES \"Issue_Type\"(\"Issue_Type_Id\") ON DELETE SET NULL ON UPDATE NO ACTION, " +
                    "PRIMARY KEY(\"Issue_Id\" AUTOINCREMENT));");
            }
        }
        private static void CreateDisplayTbl() 
        {
            if (FindConnection.TableNotExist("Display_Text"))
            {
                //db.Execute("CREATE TABLE IF NOT EXISTS \"Display_Text\" " +
                //"(\"ExpectedValue\" TEXT UNIQUE, \"User_Id\" INTEGER NOT NULL, \"TranslatedContent\" TEXT NOT NULL, " +
                //"FOREIGN KEY(\"User_Id\") REFERENCES \"User\"(\"User_ID\") ON DELETE CASCADE ON UPDATE NO ACTION, " +
                //"PRIMARY KEY(\"ExpectedValue\"));");
                db.CreateTable<DisplayMdl>();
            }
        }
        private static void CreateLoginPageDisplayTbl()
        {
            if (FindConnection.TableNotExist("Login_Text"))
            {
                db.CreateTable<LoginDisplayMdl>();
            }
        }
        private static void InsertLanguages()
        {
            IList<ILanguageMdl> languages = Language.SetLanguageList();
            // INSERT OR IGNORE INTO Allergy values("wheat2", 1, "FFFFFF", 1); DELETE FROM Allergy WHERE "Allergy_Name" = "wheat2";
            if (!FindConnection.TableNotExist("Language"))
            {
                foreach(ILanguageMdl language in languages)
                {
                    //check if language already exists, if not add the language
                    db.Execute("INSERT OR IGNORE INTO Language " +
                        "(Full_Name, AWS_Name, Tesseract_Name, Culture_Name) values(?, ?, ?, ?);", 
                        language.Full_Name, 
                        language.AWS_Name, 
                        language.Tes_Name,
                        language.Culture_Name);
                }
            }
        }
    }
}
