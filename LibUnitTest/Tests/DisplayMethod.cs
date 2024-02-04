using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;

namespace LibUnitTest.Tests
{
    [TestClass]
    public class DisplayMethod
    {
        private static IUserMdl Uservalues = new UserMdl();

        [ClassInitialize]
        public static void Initialize(TestContext context) //class not method as we don't need a unique user for each test
        {
            //create a user to test
            SqlLibrary.CreateTables.CreateDefaultTables(); //make sure the tables exist
            Uservalues = SqlLibrary.User.InsertUser(new UserMdl() { Password = "Display", Recovery = "email@my.shu.ac.uk" });
        }

        [ClassCleanup]
        public static void Cleanup() //class not method as we don't need a unique user for each test
        {
            if (Uservalues != null) //remove the created user
                SqlLibrary.User.DeleteUser(Uservalues);
        }

        [TestMethod]
        public void DisplayListContinsCheck()
        {
            List<IDisplayMdl> DisplayList = SqlLibrary.Display.RefreshList(Uservalues.User_Id);

            Assert.IsNotNull(DisplayList);
            Assert.IsFalse(DisplayList.Count == 0, "The list is empty");

            Assert.IsTrue(DisplayList.Count == 36, $"The list conatins only {DisplayList.Count} rather than the expected 36");
        }

        [TestMethod]
        public void LoginListContinsCheck()
        {
            List<ILoginDisplayMdl> DisplayList = SqlLibrary.LoginDisplay.RefreshList();

            Assert.IsNotNull(DisplayList);
            Assert.IsFalse(DisplayList.Count == 0, "The list is empty");

            Assert.IsTrue(DisplayList.Count == 13, $"The list conatins only {DisplayList.Count} rather than the expected 13");
        }

        [TestMethod]
        public void UpdateDisplayCheck()
        {
            string before = SqlLibrary.Display.FindDispalyValue("Image", SqlLibrary.User.CurrentUser.User_Id);
            Assert.IsTrue(before == "Image", "Diaplsy value isn't the expected default");

            //update the item
            SqlLibrary.Display.UpdateOneDisplay("Image", "ImageInFrench", SqlLibrary.User.CurrentUser.User_Id);
            //refreash the list
            SqlLibrary.Display.RefreshList(SqlLibrary.User.CurrentUser.User_Id);
            var ImageDis = SqlLibrary.Display.FindDispalyValue("Image", SqlLibrary.User.CurrentUser.User_Id);
            Assert.IsFalse(ImageDis == "Image", "Display value has not updated");
            Assert.IsTrue(ImageDis == "ImageInFrench", "Display value didn't convert to the expected value");

            //reset the value for future testing
            SqlLibrary.Display.UpdateOneDisplay("Image", "Image", SqlLibrary.User.CurrentUser.User_Id);
        }

        [TestMethod]
        public void UpdateLoginDisplayCheck()
        {
            string before = SqlLibrary.LoginDisplay.FindDispalyValue("Login");
            Assert.IsTrue(before == "Login", "Diaplsy value isn't the expected default");

            //update the item
            SqlLibrary.LoginDisplay.UpdateOneLoginDisplay("Login", "LoginInFrench");
            //refreash the list
            SqlLibrary.LoginDisplay.RefreshList();
            var LoginDis = SqlLibrary.LoginDisplay.FindDispalyValue("Login");
            Assert.IsFalse(LoginDis == "Login", "Display value has not updated");
            Assert.IsTrue(LoginDis == "LoginInFrench", "Display value didn't convert to the expected value");

            //reset the value for future testing
            SqlLibrary.LoginDisplay.UpdateOneLoginDisplay("Login", "Login");
        }
    }
}
