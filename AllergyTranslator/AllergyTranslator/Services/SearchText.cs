using AllergyModelLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AllergyTranslator.Services
{
    public static class SearchText
    { // the method should be tested for speed
        public static List<IAllergyMdl> ContainsAllergies(string text, bool fullwordmatch) //what allergies are in the text
        {
            List<IAllergyMdl> foundallergylist = new List<IAllergyMdl>();
            try
            {
                if (!string.IsNullOrEmpty(text)) //make sure the input has a value
                {
                    //get the list of Allergies to search for
                    List<IAllergyMdl> allergylist = SqlLibrary.Allergy.SearchAllAllergys(SqlLibrary.User.CurrentUser.User_Id);
                    if (fullwordmatch) //look for full word
                    {
                        //regex cheet sheet -> https://www.mikesdotnetting.com/article/46/c-regular-expressions-cheat-sheet
                        foreach (var item in allergylist)
                        {
                            //regex find the word with any break charactor surronding it (string start, space, string end, or punctuation)
                            if (Regex.IsMatch(text, $@"\b{item.Name}\b", RegexOptions.IgnoreCase))
                                foundallergylist.Add(item);

                            //if (text.ToLower().Contains(item.Name.ToLower())) //if the text contins item add it to the found list
                            //    foundallergylist.Add(item);
                        }
                    }
                    else //look for partial word
                    {
                        foreach (var item in allergylist)
                        {
                            //if(text.Contains(item.Name)) //if the text contins item add it to the found list
                            //    foundallergylist.Add(item);

                            //ignor the case of the word, 0 or -1 mean not existing
                            if(text.IndexOf(item.Name, StringComparison.OrdinalIgnoreCase) > 0) //if the text contins item add it to the found list
                                foundallergylist.Add(item);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                SqlLibrary.Log.LogIssue(ex.Message, nameof(ContainsAllergies), SqlLibrary.IssueList.Usability, string.Concat("Params : ", text, " ", fullwordmatch));
            }
            return foundallergylist;
        }
    }
}
