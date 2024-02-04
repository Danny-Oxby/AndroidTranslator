using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibUnitTest.Tests
{
    [TestClass]
    public class OCRMethods //the commneted code works just removedd it to save limited AWS calls
    {
        private string ImagePath = "";
        private readonly string envo = Environment.CurrentDirectory; ////C:\Users\oxbyd\OneDrive - Sheffield Hallam University\FYP\DependencyUnitTest

        [TestMethod]
        public void DoseImageExist() //images need copy ot ouput dictoinary, copy if newer property to be found
        {
            bool result = false;

            ImagePath = string.Concat(envo, @"\Images\EngImg.jpg"); //C:\Users\oxbyd\OneDrive - Sheffield Hallam University\FYP\DependencyUnitTest\Images\EngImg.jpg
            result = File.Exists(ImagePath);
            Assert.IsTrue(result, "The English Image should exist");

            ImagePath = string.Concat(envo, @"\Images\EnglishImgShort.jpg");
            result = File.Exists(ImagePath);
            Assert.IsTrue(result, "The English Short Image should exist");

            ImagePath = string.Concat(envo, @"\Images\FrenchImg.jpg");
            result = File.Exists(ImagePath);
            Assert.IsTrue(result, "The French Image should exist");

            ImagePath = string.Concat(envo, @"\Images\GermanImg.jpg");
            result = File.Exists(ImagePath);
            Assert.IsTrue(result, "The German Image should exist");

            ImagePath = string.Concat(envo, @"\Images\ItalianImg.jpg");
            result = File.Exists(ImagePath);
            Assert.IsTrue(result, "The Italian Image should exist");

            ImagePath = string.Concat(envo, @"\Images\SpanishImg.jpg");
            result = File.Exists(ImagePath);
            Assert.IsTrue(result, "The Spanish Image should exist");
        }

        //[TestMethod]
        //public void ReadEnglish()
        //{
        //    ImagePath = string.Concat(envo, @"\Images\EnglishImgShort.jpg");
        //    string result = null;

        //    Task.Run(async () =>
        //    {
        //        // Actual test code here.
        //        result = await OCRLibrary.ReadImage.ReturnImageText(ImagePath);
        //    }).GetAwaiter().GetResult();

        //    Assert.IsNotNull(result); //did the image path pass the try catch
        //    Assert.IsTrue(result.Contains("An assortment of chocolates. \n Ingredients: Milk**,sugar, cocoa butter, cocoa mass, \n vegetable fats (palm, shea), whey powder (from milk),"));
        //}

        //[TestMethod]
        //public void ReadItalianAsync()
        //{
        //    ImagePath = string.Concat(envo, @"\Images\ItalianImg.jpg");
        //    string result = null;

        //    Task.Run(async () =>
        //    {
        //        // Actual test code here.
        //        result = await OCRLibrary.ReadImage.ReturnImageText(ImagePath);
        //    }).GetAwaiter().GetResult();

        //    Assert.IsNotNull(result); //did the image path pass the try catch
        //    Assert.IsTrue(result.Contains("[IT] Mochi al gusto di tè al latte con bolle \n Ingredienti: Maltosio, pasta al gusto di tè al latte [36,6%]"));
        //}
    }
}
