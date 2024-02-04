using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using AllergyTranslator.Models;
using AllergyTranslator.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllergyTranslator.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {

        #region commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            AllergyColour = GenerateColourList.GetColours();
            SelectedColour = AllergyColour[0];

            AllergyDis = SqlLibrary.Display.FindDispalyValue("Allergy", SqlLibrary.User.CurrentUser.User_Id);
            ColourDis = SqlLibrary.Display.FindDispalyValue("Colour", SqlLibrary.User.CurrentUser.User_Id);
            CancleDis = SqlLibrary.Display.FindDispalyValue("Cancle", SqlLibrary.User.CurrentUser.User_Id);
            SaveDis = SqlLibrary.Display.FindDispalyValue("Save", SqlLibrary.User.CurrentUser.User_Id);
            ErrorMessage = SqlLibrary.Display.FindDispalyValue("Invalid Name Input", SqlLibrary.User.CurrentUser.User_Id);
        }

        #region display values
        public string AllergyDis { get; private set; } = "Allergy";
        public string ColourDis { get; private set; } = "Colour";
        public string CancleDis { get; private set; } = "Cancle";
        public string SaveDis { get; private set; } = "Save";
        public string ErrorMessage { get; private set; } = "Invalid Name Input";

        #endregion

        #region properties
        public string AllergyName { get; set; }
        public List<ColourModel> AllergyColour { get; set; }
        public ColourModel SelectedColour { get; set; }
        public bool DisplayError { get; protected set; }

        #endregion

        private bool ValidateSave()
        {
            //bool bool1 = !String.IsNullOrWhiteSpace(AllergyName);
            //bool bool2 = SqlLibrary.Allergy.SelectAllergy(AllergyName) == null;
            //return bool1 && bool2;
            return !String.IsNullOrWhiteSpace(AllergyName) && SqlLibrary.Allergy.SelectAllergy(AllergyName, SqlLibrary.User.CurrentUser.User_Id) == null;
        }

        protected async void OnCancel()
        {
            ClearProperties();
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        protected async void OnSave()
        {
            if (ValidateSave())
            {
                IAllergyMdl NewValue = new AllergyMdl()
                {
                    //Name = AllergyName,
                    Name = CultureService.GetCultureInfo().TextInfo.ToTitleCase(AllergyName.ToLower()), //Culure will change based on display language
                    User_Id = SqlLibrary.User.CurrentUser.User_Id,
                    Colour = SelectedColour.ColourHex.ToHex(),
                    Search = true,
                };

                SqlLibrary.Allergy.InsertAllergy(NewValue);

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

        protected void ClearProperties() //clear the inputted values for the next opening
        {
            AllergyName = string.Empty;
            SelectedColour = AllergyColour[0];
            DisplayError = false;

            OnPropertyChanged(nameof(AllergyName));
            OnPropertyChanged(nameof(SelectedColour));
            OnPropertyChanged(nameof(DisplayError));
        }
    }
}
