using AllergyModelLibrary.Interface;
using AllergyTranslator.Helpers;
using SqlLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace AllergyTranslator.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        #region Commands
        public ICommand TakePictureCmd { get; }
        public ICommand TextDisplay { get; }
        public ICommand ImageDisplay { get; }
        public ICommand UpdateTermAndCondtionsValue { get; }
        public ICommand UpdateWarning { get; }
        #endregion

        public AboutViewModel()
        {
            //SqlLibrary.User.SelectUser("123"); // temp until login page is set as default
            //SqlLibrary.Display.RefreshList(SqlLibrary.User.CurrentUser.User_Id); // temp until login page is set as default

            Title = "Translate";
            Title = SqlLibrary.Display.FindDispalyValue(Title, SqlLibrary.User.CurrentUser.User_Id);

            TakePictureCmd = new Command(async o => await TakePicture());
            UpdateTermAndCondtionsValue = new Command(UpdateTAC);
            UpdateWarning = new Command(UpdateWarningState);
            ImageDisplay = new Command(o => UpdateImageTextProperties(true));
            TextDisplay = new Command(o => UpdateImageTextProperties(false));
            LanguageList = SqlLibrary.Language.SelectLanguageList();

            //set the default languages
            ToSelectedLanguage = FromSelectedLanguage = LanguageList.Find(o => o.Language_Id == SqlLibrary.Setting.SelectSettigns().Language_Id);

            //set the dispaly values
            ImageDis = SqlLibrary.Display.FindDispalyValue("Image", SqlLibrary.User.CurrentUser.User_Id);
            TextDis = SqlLibrary.Display.FindDispalyValue("Text", SqlLibrary.User.CurrentUser.User_Id);
            AcceptDis = SqlLibrary.Display.FindDispalyValue("Accept", SqlLibrary.User.CurrentUser.User_Id);
            RejectDis = SqlLibrary.Display.FindDispalyValue("Reject", SqlLibrary.User.CurrentUser.User_Id);
            TakePictureDis = SqlLibrary.Display.FindDispalyValue("Take Picture", SqlLibrary.User.CurrentUser.User_Id);
            TermsAndConditionsDis = SqlLibrary.Display.FindDispalyValue("Terms And Conditions", SqlLibrary.User.CurrentUser.User_Id);
            TACListDis = SqlLibrary.Display.FindDispalyValue("Terms And Conditions Go Here", SqlLibrary.User.CurrentUser.User_Id);
            WarningDis = SqlLibrary.Display.FindDispalyValue("Warning", SqlLibrary.User.CurrentUser.User_Id);
            HideDis = SqlLibrary.Display.FindDispalyValue("Hide", SqlLibrary.User.CurrentUser.User_Id);
            AllergyWarningTextDis = SqlLibrary.Display.FindDispalyValue("As Agreed Upon, We Can Not Garentee That The Scanned Text Or Translated Text Is Correct Continue At Your Own Risk"
                , SqlLibrary.User.CurrentUser.User_Id);
            FailureDis = SqlLibrary.Display.FindDispalyValue("Failure", SqlLibrary.User.CurrentUser.User_Id);
            WifiAndCameraDis = SqlLibrary.Display.FindDispalyValue("You Must Enable Both, A WiFi Connection And Camera Permissions To Access This"
                , SqlLibrary.User.CurrentUser.User_Id);
        }

        #region dispaly values
        public string ImageDis { get; private set; } = "Image";
        public string TextDis { get; private set; } = "Text";
        public string AcceptDis { get; private set; } = "Accept";
        public string RejectDis { get; private set; } = "Reject";
        public string TakePictureDis { get; private set; } = "Take Picture";
        public string TermsAndConditionsDis { get; private set; } = "Terms And Conditions";
        public string TACListDis { get; private set; } = "Terms And Conditions Go Here";
        public string WarningDis { get; private set; } = "Warning";
        public string HideDis { get; private set; } = "Hide";
        public string AllergyWarningTextDis { get; private set; } = "As Agreed Upon, We Can Not Garentee That The Scanned Text Or Translated Text Is Correct Continue At Your Own Risk";
        public string FailureDis { get; private set; } = "Failure";
        public string WifiAndCameraDis { get; private set; } = "You Must Enable Both, A WiFi Connection And Camera Permissions To Access This";
        #endregion

        #region proprties
        public string TranslatedText { get; private set; }
        public string ImagePath { get; private set; }
        public List<IAllergyMdl> FoundAllergies { get; private set; } = new List<IAllergyMdl>();
        public string NumberFoundText { get; private set; }
        public bool DisplayImage { get; private set; } //if error occures set to false
        public bool DisplayText { get { return !DisplayImage; } } //if image is true I MUST be false
        public bool HasWIFI { get; private set; } = InternetChecker.CheckInternetAccess();
        public bool WIFIMessage { get { return !HasWIFI; } }

        public List<ILanguageMdl> LanguageList { get; set; }
        public ILanguageMdl ToSelectedLanguage { get; set; }
        public ILanguageMdl FromSelectedLanguage { get; set; }

        #endregion
        private void UpdateTAC() //in seperate method and not lambda for easier testing 
        {
            SqlLibrary.Setting.SetTACToTrue(SqlLibrary.User.CurrentUser.User_Id);
        }
        private void UpdateWarningState() //in seperate method and not lambda for easier testing 
        {
            SqlLibrary.Setting.HideWarningState(SqlLibrary.User.CurrentUser.User_Id);
        }
        private void UpdateImageTextProperties(bool imgortxt) //True for show image false for show text
        {
            if (imgortxt)
                DisplayImage = true;
            else
                DisplayImage = false;

            OnPropertyChanged(nameof(DisplayImage));
            OnPropertyChanged(nameof(DisplayText));
        }

        private async Task<bool> TakePicture() //WiFi check and error message is handeled in the view codebehind as viewmodel can't display messages
        { //uncomment camaera code later, as the both work
            if(await PermissionChecker.CheckCamera() && PermissionChecker.CheckWiFi() 
                && InternetChecker.CheckInternetAccess() && await PermissionChecker.CheckImages()) // this should request permision if it dosn't have it already
            {
                NumberFoundText = "⟳";
                OnPropertyChanged(nameof(NumberFoundText));
                FoundAllergies.Clear();
                OnPropertyChanged(nameof(FoundAllergies));

                //ImagePath = string.Empty;
                ImagePath = await AccessPhoto.TakePicture(); //path to the last taken image
                if (ImagePath == "Something went wrong" || ImagePath == "Camera permissions are needed")
                {
                    TranslatedText = ImagePath;
                    FoundAllergies.Clear();
                    NumberFoundText = string.Empty;
                }
                else if (ImagePath == null){
                    NumberFoundText = string.Empty;
                    TranslatedText = "nullpath";
                }
                else
                {
                    TranslatedText = await OCRLibrary.ReadImage.ReturnImageText(ImagePath);

                    //TranslatedText = "wheat";

                    //TranslatedText = "Translated Text Example\n Milk** 12%, soy 53%, saturated fat (plam), milk fat" +
                    //    "hazelnut, flavourings.\nMay contain other nuts, wheat.\nBest before see other side of the box";
                    if (ToSelectedLanguage.Language_Id == FromSelectedLanguage.Language_Id)
                    { 
                        if(TranslatedText == null)
                        {
                            TranslatedText = string.Concat(ImageDis, " ", FailureDis);
                        }
                    }//skip the translation process since read and write and the same language
                    else
                    {
                        //bool temp = true;
                        if (TranslatedText != null)
                            TranslatedText = await TranslationLibrary.TranslateText.TranslatingTextAsync(TranslatedText, ToSelectedLanguage.AWS_Name, FromSelectedLanguage.AWS_Name);
                        else
                            TranslatedText = string.Concat(ImageDis, " ", FailureDis);
                    }

                    FoundAllergies = Services.SearchText.ContainsAllergies(TranslatedText, Setting.SelectSettigns().Full_Word);
                    NumberFoundText = string.Concat(SqlLibrary.Display.FindDispalyValue("Number Of Allergies Found", 
                        SqlLibrary.User.CurrentUser.User_Id), " ", FoundAllergies.Count);
                }
                DisplayImage = false;
                OnPropertyChanged(nameof(TranslatedText));
                OnPropertyChanged(nameof(FoundAllergies));
                OnPropertyChanged(nameof(NumberFoundText));
                OnPropertyChanged(nameof(ImagePath));
                OnPropertyChanged(nameof(DisplayImage));
                OnPropertyChanged(nameof(DisplayText));
            } // for IOS an else with a reminder to change camaer settings is needed, as IOS only shows the request message once unlike android
            return true;
        }
    }
}