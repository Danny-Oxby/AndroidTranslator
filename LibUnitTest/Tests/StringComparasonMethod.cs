using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;

namespace LibUnitTest.Tests
{
    [TestClass]
    public class StringComparasonMethod
    {
        private static IUserMdl Uservalues = new UserMdl();
        private static string ShoppingList = "Ingredients List: whole milk***, ORANGES, pineAppleA, SoyMilk, CrUtOnEs, sugar";

        [ClassInitialize]
        public static void Initialize(TestContext context) //class not method as we don't need a unique user for each test
        {
            //create a user to test
            SqlLibrary.CreateTables.CreateDefaultTables(); //make sure the tables exist
            Uservalues = SqlLibrary.User.InsertUser(new UserMdl() { Password = "password", Recovery = "email@my.shu.ac.uk" });
        }

        [ClassCleanup]
        public static void Cleanup() //class not method as we don't need a unique user for each test
        {
            if (Uservalues != null) //remove the created user
                SqlLibrary.User.DeleteUser(Uservalues);
        }

        [TestMethod]
        public void PartialWordTest()
        {
            List<IAllergyMdl> foundlistP = AllergyTranslator.Services.SearchText.ContainsAllergies(ShoppingList, false);

            Assert.IsTrue(foundlistP.Any(), "list is empty"); //there is something in the found list
            //milk and soy are the partial matches
            Assert.IsTrue(foundlistP.Count == 2, "more than 2 words found");

            if (foundlistP.Count == 2)
            {
                //MStest can't hadel true equals so this method has been employed
                Assert.IsTrue(foundlistP[0].Name == "Milk", "not milk");
                Assert.IsTrue(foundlistP[0].User_Id == Uservalues.User_Id);
                Assert.IsTrue(foundlistP[0].Colour == "#FF00FF00");
                Assert.IsTrue(foundlistP[0].Search == true);

                Assert.IsTrue(foundlistP[1].Name == "Soy", "not soy");
                Assert.IsTrue(foundlistP[1].User_Id == Uservalues.User_Id);
                Assert.IsTrue(foundlistP[1].Colour == "#FF00FFFF");
                Assert.IsTrue(foundlistP[1].Search == true);
            }

            //CollectionAssert.AreEquivalent(foundlistP, new List<IAllergyMdl>()  //default list containa, Wheat, Milk, Soy, Nut, Egg
            //{ 
            //    new AllergyMdl(){Name="milk",User_Id = Uservalues.User_Id,Colour = "#FF00FF00",Search = true},
            //    new AllergyMdl(){Name="soy",User_Id = Uservalues.User_Id,Colour = "#FF00FFFF",Search = true},
            //});
        }

        [TestMethod]
        public void FullWordTest()
        {
            List<IAllergyMdl> foundlistF = AllergyTranslator.Services.SearchText.ContainsAllergies(ShoppingList, true);

            Assert.IsTrue(foundlistF.Any(), "List is empty"); //there is something in the found list
            //milk is the only full match
            Assert.IsTrue(foundlistF.Count == 1, "more than one word found");

            if (foundlistF.Count == 1)
            {
                //MStest can't hadel true equals so this method has been employed
                Assert.IsTrue(foundlistF[0].Name == "Milk", "not milk");
                Assert.IsTrue(foundlistF[0].User_Id == Uservalues.User_Id);
                Assert.IsTrue(foundlistF[0].Colour == "#FF00FF00");
                Assert.IsTrue(foundlistF[0].Search == true);
            }
            //Assert.AreEqual(foundlistF, new List<IAllergyMdl>()  //default list containa, Wheat, Milk, Soy, Nut, Egg
            //{
            //    new AllergyMdl(){Name="milk",User_Id = Uservalues.User_Id,Colour = "#FF00FF00",Search = true},
            //});
        }
    }
}
