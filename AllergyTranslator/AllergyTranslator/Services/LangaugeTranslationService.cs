using AllergyModelLibrary.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllergyTranslator.Services
{
    public static class LangaugeTranslationService
    {
        //task with useless bool is used else the await dosn't work properly

        public static async Task<bool> TranslateLoginTextDisplay(ILanguageMdl NewLangaugeAWSValue)
        {
            //find display value list
            List<ILoginDisplayMdl> DisplayList = SqlLibrary.LoginDisplay.RefreshList();
            //for ever list item
            foreach (ILoginDisplayMdl DisplayValue in DisplayList)
            {
                //translte the text and update thte language id value
                DisplayValue.DisplayValue = await TranslationLibrary.TranslateText.TranslatingTextAsync(DisplayValue.SearchName, NewLangaugeAWSValue.AWS_Name,
                    SqlLibrary.Language.SelectLanguage("en-GB").AWS_Name); // search names are all in english so base is always englsih
                DisplayValue.Language_Id = NewLangaugeAWSValue.Language_Id;
                //update the text
                SqlLibrary.LoginDisplay.UpdateOneLoginDisplay(DisplayValue);
            }

            //testing for changing only one
            //ILoginDisplayMdl DisplayValue = DisplayList[0];
            //    //translte the text and update thte language id value
            //    DisplayValue.DisplayValue = await TranslationLibrary.TranslateText.TranslatingTextAsync(DisplayValue.SearchName, NewLangaugeAWSValue.AWS_Name,
            //        SqlLibrary.Language.SelectLanguage("English").AWS_Name); // search names are all in english so base is always englsih
            //    DisplayValue.Language_Id = NewLangaugeAWSValue.Language_Id;
            //    //update the text
            //    SqlLibrary.LoginDisplay.UpdateOneLoginDisplay(DisplayValue);


            return true;
        }

        public static async Task<bool> TranslateApplicationTextDisplay(ILanguageMdl NewLangaugeAWSValue, int user_id)
        {
            //find display value list
            List<IDisplayMdl> DisplayList = SqlLibrary.Display.RefreshList(user_id);
            //find the current setting
            var setting = SqlLibrary.Setting.SelectSettigns();

            //for ever list item
            foreach (IDisplayMdl DisplayValue in DisplayList)
            {
                //translte the text and update thte language id value
                DisplayValue.DisplayValue = await TranslationLibrary.TranslateText.TranslatingTextAsync(DisplayValue.SearchName, NewLangaugeAWSValue.AWS_Name,
                    SqlLibrary.Language.SelectLanguage("en-GB").AWS_Name); // search names are all in english so base is always englsih
                //update the text
                SqlLibrary.Display.UpdateOneDisplay(DisplayValue);
            }

            ////testing for changing only one
            //IDisplayMdl DisplayValue = DisplayList[0];
            ////translte the text and update thte language id value
            //DisplayValue.DisplayValue = await TranslationLibrary.TranslateText.TranslatingTextAsync(DisplayValue.SearchName, NewLangaugeAWSValue.AWS_Name,
            //    SqlLibrary.Language.SelectLanguage("English").AWS_Name); // search names are all in english so base is always englsih
            //                                                             //update the text
            //SqlLibrary.Display.UpdateOneDisplay(DisplayValue);

            setting.Language_Id = NewLangaugeAWSValue.Language_Id;
            SqlLibrary.Setting.UpdateSetting(setting);

            return true;
        }
    }
}

//public static async Task<bool> TranslateLanguageTextDisplay(ILanguageMdl NewLangaugeAWSValue)
//{
//    //find the current setting
//    ISettingMdl setting = SqlLibrary.Setting.SelectSettigns();

//    //update language values
//    List<ILanguageMdl> LanguageList = SqlLibrary.Language.SelectLanguageList();
//    foreach (ILanguageMdl language in LanguageList)
//    {
//        language.Full_Name = await TranslationLibrary.TranslateText.TranslatingTextAsync(language.Full_Name, 
//            NewLangaugeAWSValue.AWS_Name, SqlLibrary.Language.SelectLanguage(setting.Language_Id).AWS_Name);
//    }
//    SqlLibrary.Language.UpdateLanguageDispalyNames(LanguageList);

//    //ILanguageMdl language = LanguageList[1];
//    //language.Full_Name = await TranslationLibrary.TranslateText.TranslatingTextAsync(language.Full_Name, NewLangaugeAWSValue.AWS_Name,
//    //    SqlLibrary.Language.SelectLanguage(setting.Language_Id).AWS_Name);


//    return true;
//}
