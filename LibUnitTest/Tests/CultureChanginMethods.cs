using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using AllergyTranslator.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibUnitTest.Tests
{
    [TestClass]
    public class CultureChanginMethods
    {
        private static IUserMdl Uservalues = new UserMdl();

        [ClassInitialize]
        public static void Initialize(TestContext context) //class not method as we don't need a unique user for each test
        {
            //create a user to test
            SqlLibrary.CreateTables.CreateDefaultTables(); //make sure the tables exist
            //Uservalues = SqlLibrary.User.InsertUser(new UserMdl() { Password = "Culture", Recovery = "email@my.shu.ac.uk" });
        }

        [ClassCleanup]
        public static void Cleanup() //class not method as we don't need a unique user for each test
        {
            //if (Uservalues != null) //remove the created user
            //    SqlLibrary.User.DeleteUser(Uservalues);
        }

        [TestMethod]
        public void TestChangingCulture() //this method will fail if another user is created before this method is called
        {
            Uservalues = SqlLibrary.User.InsertUser(new UserMdl() { Password = "Culture", Recovery = "email@my.shu.ac.uk" });

            CultureInfo temp = CultureService.GetCultureInfo();

            Assert.IsNotNull(temp);
            Assert.IsTrue(temp.Name == "en-GB", $"Base was not english was instead {temp.Name}");
            Assert.IsFalse(temp.Name == "fr-FR", "Base thinks its french");

            //langauge id will normally be know before this method call, hence the weird double SelectLanguage method call 
            temp = CultureService.UpdateCultureInfo(SqlLibrary.Language.SelectLanguage("de-DE").Language_Id);

            Assert.IsNotNull(temp);
            Assert.IsTrue(temp.Name == "de-DE", $"Updated is no german was instead {temp.Name}");
            Assert.IsFalse(temp.Name == "en-GB", "Updated thinks its english");
            Assert.IsFalse(temp.Name == "fr-FR", "Updated thinks its french");

            if (Uservalues != null) //remove the created user
                SqlLibrary.User.DeleteUser(Uservalues);
        }
    }
}
