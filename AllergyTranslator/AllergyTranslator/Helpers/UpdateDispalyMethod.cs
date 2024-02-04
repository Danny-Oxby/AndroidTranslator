using AllergyModelLibrary.Interface;
using AllergyTranslator.Services;
using System.Threading.Tasks;

namespace AllergyTranslator.Helpers
{
    internal class UpdateDispalyMethod
    {
        internal async static Task<bool> UpdateApplcionDispaly(ILanguageMdl ToLanguege, int user_id)
        {
            if (InternetChecker.CheckInternetAccess()) //only do this if you can access the ASW API / have wifi
            {
                bool wait = false;
                //update display values
                wait = await LangaugeTranslationService.TranslateApplicationTextDisplay(ToLanguege, user_id);
                return true;
            }
            return false;
        }
    }
}

//update language values, << but why?
//wait = await LangaugeTranslationService.TranslateLanguageTextDisplay(ToLanguege);
//await AllergyTranslator.Services.LangaugeTranslationService.TranslateLanguageTextDisplay(SqlLibrary.Language.SelectLanguage("French"));

