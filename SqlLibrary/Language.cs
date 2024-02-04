using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using SQLite;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SqlLibrary
{
    public class Language
    {
        private static List<ILanguageMdl> AvailableLanguage { get; set; }// the list of available language packs
        private static readonly SQLiteConnection db = FindConnection.CreateConnection();

        /*
        A cropped culture info list for brevity.

        CULTURE ISO ISO WIN DISPLAYNAME                              ENGLISHNAME
        ar      ar  ara ARA Arabic                                   Arabic
        bg      bg  bul BGR Bulgarian                                Bulgarian
        ca      ca  cat CAT Catalan                                  Catalan
        zh-Hans zh  zho CHS Chinese (Simplified)                     Chinese (Simplified)
        */
        internal static List<ILanguageMdl> SetLanguageList()
        {
            //https://learn.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/supported-culture-codes
            List<ILanguageMdl> Languages = new List<ILanguageMdl>
            {
                //create the languages
                new LanguageMdl("English", "en", "eng", "en-GB"),
                new LanguageMdl("Français", "fr", "fra", "fr-FR"),
                new LanguageMdl("Deutsch", "de", "frk", "de-DE"),
                new LanguageMdl("Italiano", "it", "ita", "it-it"),
                new LanguageMdl("Português-brasil", "pt", "por", "pt-BR"),
                new LanguageMdl("Español", "es", "spa", "es-ES"),
                //new LanguageMdl("Arabic", "ar", "ara"),
                //new LanguageMdl("Bengali", "bn", "ben"),
                //new LanguageMdl("Chinese-Simple", "zh", "chi_sim"),
                //new LanguageMdl("Dutch", "nl", "nld"),
                //new LanguageMdl("Hebrew", "he", "heb"),
                //new LanguageMdl("Hindi", "hi", "hin"),
                //new LanguageMdl("Japanese", "ja", "jpn"),
                //new LanguageMdl("Korean", "ko", "kor"),
                //new LanguageMdl("Russian", "ru", "rus"),
                //new LanguageMdl("Thai", "th", "Thai")
            }; // this can't use AvaibleLanguages since we need the Language ID

            return Languages;
        }
        public static List<ILanguageMdl> SelectLanguageList()
        {
            if(AvailableLanguage == null)
            {
                // get the language list from the DB
                var temp = db.Table<LanguageMdl>().ToList();
                AvailableLanguage = temp.ToList<ILanguageMdl>(); //no direct interface due to .net security quirk
            }
            return AvailableLanguage;
        }
        //public static ILanguageMdl SelectLanguage(string full_name)
        //{
        //    LanguageMdl ReturnValue = null;
        //    if (!FindConnection.TableNotExist("Language"))
        //    {
        //        ReturnValue = db.FindWithQuery<LanguageMdl>("SELECT * FROM Language WHERE Full_Name = ?;",full_name);
        //    }

        //    return ReturnValue;
        //} // full name changes after language selection
        public static ILanguageMdl SelectLanguage(string culture_name)
        {
            LanguageMdl ReturnValue = null;
            if (!FindConnection.TableNotExist("Language"))
            {
                ReturnValue = db.FindWithQuery<LanguageMdl>("SELECT * FROM Language WHERE Culture_Name = ?;", culture_name);
            }

            return ReturnValue;
        }
        public static ILanguageMdl SelectLanguage(int language_id)
        {
            LanguageMdl ReturnValue = null;
            if (!FindConnection.TableNotExist("Language"))
            {
                ReturnValue = db.FindWithQuery<LanguageMdl>("SELECT * FROM Language WHERE Language_Id = ?;", language_id);
            }

            return ReturnValue;
        }

        public static void UpdateLanguageDispalyNames(List<ILanguageMdl> languageMdl) //given a list of all languages update
        {
            if (!FindConnection.TableNotExist("Language"))
            {
                foreach(ILanguageMdl language in languageMdl) //update all the languages
                {
                    db.Update(language);
                }
                //reset the language list
                AvailableLanguage = null;
                SelectLanguageList();
            }
        }
    }
}
