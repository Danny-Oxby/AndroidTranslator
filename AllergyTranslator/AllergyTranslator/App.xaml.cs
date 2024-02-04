using Xamarin.Forms;

namespace AllergyTranslator
{
    public partial class App : Application
    {

        public App() // Add the create list query here
        {
            InitializeComponent();

            //SqlLibrary.CreateTables.DropTables();
            SqlLibrary.CreateTables.CreateDefaultTables();
            //SqlLibrary.User.InsertUser(new UserMdl() { Password = "123", Recovery = "email@gmail.com" });

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            //this is called after the app shell loads
        }

        protected override void OnSleep()
        {
            //move back to login page and log out
            //SqlLibrary.User.ClearSelectedUser(); //reset to user on logout
            //await Shell.Current.GoToAsync("//LoginPage");
        }

        protected override void OnResume()
        {
        }
    }
}
