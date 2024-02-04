using AllergyTranslator.Helpers;
using AllergyTranslator.Services;
using AllergyTranslator.Views;
using System;
using Xamarin.Forms;

namespace AllergyTranslator
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private void OnMenuItemClicked(object sender, EventArgs e)
        {
            SqlLibrary.User.ClearSelectedUser(); //reset to user on logout
            //go back to the login page / restart the applcation
            ForceCloseService.CloseApplication();
            //await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
