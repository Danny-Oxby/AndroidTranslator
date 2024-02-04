using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using AllergyTranslator.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllergyTranslator.ViewModels
{
    [QueryProperty(nameof(SelectedAllergy), nameof(SelectedAllergy))]
    public class ItemDetailViewModel : NewItemViewModel //inheriting newItem removed method duplication
    {
        #region commands
        public ICommand RemoveItem { get; private set; }
        public new ICommand SaveCommand { get; }

        #endregion
        public ItemDetailViewModel()
        {
            Title = "Update Allergy";
            Title = string.Concat(SqlLibrary.Display.FindDispalyValue("Update", SqlLibrary.User.CurrentUser.User_Id), " ", AllergyDis);
            //Title = string.Concat(SqlLibrary.Display.FindDispalyValue("Update", SqlLibrary.User.CurrentUser.User_Id), " ",
            //    SqlLibrary.Display.FindDispalyValue("Allergy", SqlLibrary.User.CurrentUser.User_Id));

            SaveCommand = new Command(OnSave);
            RemoveItem = new Command(OnDelete);

            SearchDis = SqlLibrary.Display.FindDispalyValue("Search", SqlLibrary.User.CurrentUser.User_Id);
            DeleteDis = SqlLibrary.Display.FindDispalyValue("Delete", SqlLibrary.User.CurrentUser.User_Id);
        }
        #region dispaly values (this page inherets newitem so only properys not there are added here)
        public string SearchDis { get; private set; } = "Search";
        public string DeleteDis { get; private set; } = "Delete";

        #endregion

        #region properties
        private string selectedAllergy;
        public string SelectedAllergy //this is the value passed in on naviagtion, this is not used anywhere else
        { 
            get => selectedAllergy; 
            set 
            {
                selectedAllergy = value; 
                BaseInfo = SqlLibrary.Allergy.SelectAllergy(selectedAllergy, SqlLibrary.User.CurrentUser.User_Id);
                AllergyName = BaseInfo.Name;
                SelectedColour = AllergyColour.Find(o => o.ColourHex == Color.FromHex(BaseInfo.Colour)); //find the colour that matches the hex value
                SearchBool = BaseInfo.Search;

                OnPropertyChanged(nameof(AllergyName));
                OnPropertyChanged(nameof(SelectedColour));
                OnPropertyChanged(nameof(SearchBool));
            } 
        } //I an only pushing one string and using that for an SQL call, this allows the SQLmodel to be extended without page modifications

        public IAllergyMdl BaseInfo { get; private set; }
        public bool SearchBool { get; set; }
        #endregion

        private bool ValidateSave() //name is not invalid and name is not used by another item
        {
            return !string.IsNullOrWhiteSpace(AllergyName) && 
                (SqlLibrary.Allergy.SelectAllergy(AllergyName, SqlLibrary.User.CurrentUser.User_Id) == null || AllergyName == BaseInfo.Name);
        }

        protected new async void OnSave()
        {
            if (ValidateSave())
            {
                if(SelectedColour == null)
                {
                    SelectedColour = AllergyColour[0];
                }

                IAllergyMdl NewValue = new AllergyMdl()
                {
                    //Name = AllergyName,
                    Name = CultureService.GetCultureInfo().TextInfo.ToTitleCase(AllergyName.ToLower()), //Culure will change based on display language
                    User_Id = SqlLibrary.User.CurrentUser.User_Id,
                    Colour = SelectedColour.ColourHex.ToHex(),
                    Search = SearchBool,
                };

                SqlLibrary.Allergy.UpdateAllergy(NewValue, BaseInfo.Name);

                ClearProperties();
                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                //invalid name
                DisplayError = true;
                OnPropertyChanged(nameof(DisplayError));
            }
        }

        private async void OnDelete()
        {
            SqlLibrary.Allergy.DeleteAllergy(BaseInfo);

            ClearProperties();
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
