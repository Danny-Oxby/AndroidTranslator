using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;

namespace LibUnitTest.Tests
{
    [TestClass]
    public class TranslationMethods //the commneted code works just removedd it to save limited AWS calls
    { //https://learn.microsoft.com/en-us/archive/msdn-magazine/2014/november/async-programming-unit-testing-asynchronous-code
    //    [TestMethod]
    //    public void TranslateEnglishToFrench() //working
    //    {
    //        string text = "";
    //        Task.Run(async () =>
    //        {
    //            text = await TranslationLibrary.TranslateText.TranslatingTextAsync("milk 20%, wheat 65%, eggs 15%", "fr", "en");
    //            // Actual test code here.
    //        }).GetAwaiter().GetResult();

    //        Assert.IsFalse(string.IsNullOrEmpty(text), "text was null");
    //        Assert.IsTrue(text.Contains("lait 20 %, blé 65 %, œufs 15 %"), "text did not match expected");
    //    }

    //    [TestMethod]
    //    public void TranslateGermanToEnglish() //working
    //    {
    //        string text = "";
    //        Task.Run(async () =>
    //        {
    //            text = await TranslationLibrary.TranslateText.TranslatingTextAsync("Milch 20%, Weizen 65%, Eier 15%", "en", "de");
    //            // Actual test code here.
    //        }).GetAwaiter().GetResult();

    //        Assert.IsFalse(string.IsNullOrEmpty(text), "text was null");
    //        Assert.IsTrue(text.Contains("milk 20%, wheat 65%, eggs 15%"), "text did not match expected");

    //    }

    //    [TestMethod]
    //    public void TranslateLoginDispalyValues() //working
    //    {
    //        bool complete;
    //        Task.Run(async () =>
    //        {
    //            complete = await AllergyTranslator.Services.LangaugeTranslationService.TranslateLoginTextDisplay(SqlLibrary.Language.SelectLanguage(3));
    //            // Actual test code here.
    //        }).GetAwaiter().GetResult();

    //        List<ILoginDisplayMdl> DisplayList = SqlLibrary.LoginDisplay.RefreshList();

    //        Assert.IsNotNull(DisplayList); //the list has values

    //        //the value has changed
    //        Assert.IsTrue(DisplayList[0].DisplayValue == "Einloggen", "The name hasn't become german");
    //        Assert.IsTrue(DisplayList[0].DisplayValue != "Login", "The name is still the default english");
    //        //the pointer has changed
    //        Assert.IsTrue(DisplayList[0].Language_Id == 3, "The Language ID hasn't become german");
    //        Assert.IsTrue(DisplayList[0].Language_Id != 1, "The Language ID value hasn't changed");

    //        //nothing else has
    //        Assert.IsTrue(DisplayList[0].SearchName == "Login", "The Search value has changed");
    //    }

    //    [TestMethod]
    //    public void TranslateAppDispalyValues() //working
    //    {
    //        IUserMdl Uservalues = SqlLibrary.User.InsertUser(new UserMdl() { Password = "Translate", Recovery = "email@my.shu.ac.uk" });

    //        bool complete;
    //        Task.Run(async () =>
    //        {
    //            complete = await AllergyTranslator.Services.LangaugeTranslationService.TranslateApplicationTextDisplay(SqlLibrary.Language.SelectLanguage(6), Uservalues.User_Id);
    //            // Actual test code here.
    //        }).GetAwaiter().GetResult();

    //        List<IDisplayMdl> DisplayList = SqlLibrary.Display.RefreshList(Uservalues.User_Id);

    //        Assert.IsNotNull(DisplayList); //the list has values

    //        //the value has changed
    //        Assert.IsTrue(DisplayList[0].DisplayValue == "Traduce", "The name hasn't become spanish");
    //        Assert.IsTrue(DisplayList[0].DisplayValue != "Translate", "The name is still the default english");

    //        var setting = SqlLibrary.Setting.SelectSettigns();
    //        //the pointer has changed
    //        Assert.IsTrue(setting.Language_Id == 6, "The Language ID hasn't become spanish");
    //        Assert.IsTrue(setting.Language_Id != 1, "The Language ID value hasn't changed");

    //        //nothing else has
    //        Assert.IsTrue(DisplayList[0].SearchName == "Translate", "The Search value has changed");

    //        if (Uservalues != null) //remove the created user
    //            SqlLibrary.User.DeleteUser(Uservalues);
    //    }


    }
}

        //[TestMethod]
        //public void TranslateLanguageNameToFrench() //Not working << Why am i translating the language name when languge is a shared table?
        //{
        //    bool complete;
        //    Task.Run(async () =>
        //    {
        //        complete = await AllergyTranslator.Services.LangaugeTranslationService.TranslateLanguageTextDisplay(SqlLibrary.Language.SelectLanguage("French"));
        //        // Actual test code here.
        //    }).GetAwaiter().GetResult();

        //    List<ILanguageMdl> languages = SqlLibrary.Language.SelectLanguageList();

        //    Assert.IsNotNull(languages); //the list has values

        //    //the name has changed
        //    Assert.IsTrue(languages[0].Full_Name == "Anglais", "The name hasn't become french");
        //    Assert.IsTrue(languages[0].Full_Name != "English", "The name is still the default english");

        //    //nothing else has
        //    Assert.IsTrue(languages[0].AWS_Name == "en", "The AWS value has changed");
        //    Assert.IsTrue(languages[0].Tes_Name == "eng", "The Tes value has changed");
        //    Assert.IsTrue(languages[0].Culture_Name == "en-GB", "The Culture value has changed");
        //}