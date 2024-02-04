using AllergyModelLibrary.Interface;
using AllergyTranslator.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllergyTranslator.ViewModels
{
    public class AllergyPageViewModel : BaseViewModel
    {
        #region commands
        public ICommand AddNewItem { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public Command<IAllergyMdl> UpdateItem { get; private set; }
        #endregion

        public AllergyPageViewModel()
        {
            Title = "Allergy List";
            Title = string.Concat(SqlLibrary.Display.FindDispalyValue("Allergy", SqlLibrary.User.CurrentUser.User_Id), " ",
                SqlLibrary.Display.FindDispalyValue("List", SqlLibrary.User.CurrentUser.User_Id));

            AddNewItem = new Command(GoToCreatePage);
            RefreshCommand = new Command(GetAllergyList);
            UpdateItem = new Command<IAllergyMdl>(GoToUpdatePage);

            AddDis = SqlLibrary.Display.FindDispalyValue("Add", SqlLibrary.User.CurrentUser.User_Id);
            SearchDis = SqlLibrary.Display.FindDispalyValue("Search", SqlLibrary.User.CurrentUser.User_Id);

            GetAllergyList();
        }

        #region Display values
        public string AddDis { get; private set; } = "Add";
        public string SearchDis { get; private set; } = "Search";
        #endregion

        #region Properties
        public ICollection<IAllergyMdl> AllergyList { get; private set; }
        public bool IsRefreshing { get; private set; } = false;
        #endregion

        private void GetAllergyList() //get the current list of allergies
        {
            if (!IsRefreshing) // only allow one refreash at a time
            {
                IsRefreshing = true;
                OnPropertyChanged(nameof(IsRefreshing));

                AllergyList = SqlLibrary.Allergy.SelectAllAllergys(SqlLibrary.User.CurrentUser.User_Id);
                OnPropertyChanged(nameof(AllergyList));

                IsRefreshing = false;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        private async void GoToCreatePage(object obj) //move on the navigation stack
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
            //GetAllergyList(); // this method is being called before the GoToAsync finishes
        }
        async void GoToUpdatePage(IAllergyMdl allergy)
        {
            if (allergy == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack, you can only push strings using this method
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.SelectedAllergy)}={allergy.Name}");
            //GetAllergyList(); // this method is being called before the GoToAsync finishes
        }
    }
}
