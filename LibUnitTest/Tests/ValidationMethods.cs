using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibUnitTest.Tests
{
    [TestClass]
    public class ValidationMethods
    {
        private readonly string envo = Environment.CurrentDirectory;

        //[TestMethod]
        //public void SpeedTest()
        //{
        //    string ImagePath = string.Concat(envo, @"\Images\20Base.jpg");
        //    string result = null;

        //    Task.Run(async () =>
        //    {
        //        // Actual test code here.
        //        result = await OCRLibrary.ReadImage.ReturnImageText(ImagePath);
        //    }).GetAwaiter().GetResult();

        //    string text = "";
        //    Task.Run(async () =>
        //    {
        //        text = await TranslationLibrary.TranslateText.TranslatingTextAsync(result, "fr", "en");
        //        // Actual test code here.
        //    }).GetAwaiter().GetResult();

        //    Assert.IsFalse(string.IsNullOrEmpty(text), "text was null");
        //    Assert.IsFalse(string.IsNullOrEmpty(result), "result was null");
        //}
    }
}
