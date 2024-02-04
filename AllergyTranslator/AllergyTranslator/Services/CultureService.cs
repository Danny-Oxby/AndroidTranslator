using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AllergyTranslator.Services
{
    //used the culutre info from the languag emodel to effect the format of text, for example turkish 'i' and english 'i' are diffrent ASCII values
    //this is also sued in date formatting such as england vs USA
    public class CultureService //must be pupblic for unit testings
    {
        private static CultureInfo Culture;

        public static CultureInfo GetCultureInfo() //this may want user_id to be passed in when more than one user can access this per instance
        {
            if (Culture == null)
            {
                Culture = new CultureInfo(SqlLibrary.Language.SelectLanguage(SqlLibrary.Setting.SelectSettigns().Language_Id).Culture_Name);
            }

            return Culture;
        }

        public static CultureInfo UpdateCultureInfo(int language_id)
        {
            Culture = new CultureInfo(SqlLibrary.Language.SelectLanguage(language_id).Culture_Name);

            return GetCultureInfo();
        }
    }
}
