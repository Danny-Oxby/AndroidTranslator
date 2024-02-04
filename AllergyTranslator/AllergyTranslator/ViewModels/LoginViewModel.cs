using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using AllergyTranslator.Helpers;
using AllergyTranslator.Services;
using AllergyTranslator.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllergyTranslator.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region
        public ICommand LoginCommand { get; }
        //public ICommand RecoverPassword { get; private set; }
        public ICommand UpdateDisplayCommand { get; }
        #endregion

        public LoginViewModel()
        {
            LoginCommand = new Command(o => CheckPassword());
            UpdateDisplayCommand = new Command(async o => await UpdateValues());

            LanguageList = SqlLibrary.Language.SelectLanguageList();

            defaultLanguage = LanguageList.Find(o => o.Language_Id == SqlLibrary.LoginDisplay.ReturnLoginLangaugeID());
            SelectedLanguage = defaultLanguage;

            UpdateDisplayValues();

            if(!HasWIFI)
                TranslationFinished = false;
            else
                TranslationFinished = true;

        }

        private void UpdateDisplayValues()
        {
            EmailRecoveryDis = SqlLibrary.LoginDisplay.FindDispalyValue("Email Recovery");
            EmailValidationDis = SqlLibrary.LoginDisplay.FindDispalyValue("Only Valid Emails Are Accepted");
            EmailSupportDis = SqlLibrary.LoginDisplay.FindDispalyValue("Email Not Supported By Device");
            PasswordDis = SqlLibrary.LoginDisplay.FindDispalyValue("Password");
            PasswordRecoveryDis = SqlLibrary.LoginDisplay.FindDispalyValue("Password Recovery");
            FailureDis = SqlLibrary.LoginDisplay.FindDispalyValue("Failure");
            AcceptDis = SqlLibrary.LoginDisplay.FindDispalyValue("Accept");
            SuccessDis = SqlLibrary.LoginDisplay.FindDispalyValue("Success");
            LoginDis = SqlLibrary.LoginDisplay.FindDispalyValue("Login");
            UserCreationDis = SqlLibrary.LoginDisplay.FindDispalyValue("User Created");
            NewUserDis = SqlLibrary.LoginDisplay.FindDispalyValue("Create User");
            PasswordFailureDis = SqlLibrary.LoginDisplay.FindDispalyValue("Invalid Password Select Another");
            MatchingErrorDis = SqlLibrary.LoginDisplay.FindDispalyValue("No Matching User");
            CreateDis = string.Concat(NewUserDis, " ", PasswordDis);
        }
        private void NotifyDispalyChanges()
        {
            OnPropertyChanged(nameof(EmailRecoveryDis));
            OnPropertyChanged(nameof(EmailValidationDis));
            OnPropertyChanged(nameof(EmailSupportDis));
            OnPropertyChanged(nameof(PasswordDis));
            OnPropertyChanged(nameof(PasswordRecoveryDis));
            OnPropertyChanged(nameof(FailureDis));
            OnPropertyChanged(nameof(AcceptDis));
            OnPropertyChanged(nameof(SuccessDis));
            OnPropertyChanged(nameof(LoginDis));
            OnPropertyChanged(nameof(UserCreationDis));
            OnPropertyChanged(nameof(NewUserDis));
            OnPropertyChanged(nameof(PasswordFailureDis));
            OnPropertyChanged(nameof(MatchingErrorDis));
            OnPropertyChanged(nameof(CreateDis));
        }

        #region Display text
        public string EmailRecoveryDis { get; private set; } = "Email Recovery";
        public string EmailValidationDis { get; private set; } = "Only Valid Emails Are Accepted";
        public string EmailSupportDis { get; private set; } = "Email Not Supported By Device";
        public string PasswordDis { get; private set; } = "Password";
        public string PasswordFailureDis { get; private set; } = "Invalid Password Select Another";
        public string PasswordRecoveryDis { get; private set; } = "Password Recovery";
        public string FailureDis { get; private set; } = "Failure";
        public string AcceptDis { get; private set; } = "Accept";
        public string SuccessDis { get; private set; } = "Success";
        public string LoginDis { get; private set; } = "Login";
        public string UserCreationDis { get; private set; } = "User Created";
        public string NewUserDis { get; private set; } = "Create User";
        public string MatchingErrorDis { get; private set; } = "No Matching User";
        public string CreateDis { get; private set; } = "Create User Password"; //combination of create user and password
        #endregion

        #region Properties
        public string PasswordInput { get; set; }
        public string PasswordFailure { get; private set; } = "";
        public List<ILanguageMdl> LanguageList { get; set; }

        private ILanguageMdl defaultLanguage; //this will be used to prevent the AWS call on first load

        public ILanguageMdl SelectedLanguage { get; set; }

        public bool HasWIFI { get; private set; } = InternetChecker.CheckInternetAccess();
        public bool TranslationFinished { get; private set; }
        public bool WIFIMessage { get { return !HasWIFI; } }
     
        #endregion

        private async Task<bool> UpdateValues()
        {
            if (SelectedLanguage != null && SelectedLanguage.Language_Id != defaultLanguage.Language_Id)
            {
                TranslationFinished = false;
                OnPropertyChanged(nameof(TranslationFinished));
                defaultLanguage = SelectedLanguage; //update the default langauge to the new value
                                                    //update the langauge values
                var wait = await LangaugeTranslationService.TranslateLoginTextDisplay(SelectedLanguage);
                System.Threading.Thread.Sleep(1000); // the await method needs time to run else it can crash, give it 1 second extra to finish

                if (wait)
                {
                    _ = SqlLibrary.LoginDisplay.RefreshList();
                    //find the new values
                    UpdateDisplayValues();
                    //notify the change
                    NotifyDispalyChanges();
                }

                TranslationFinished = true;
                OnPropertyChanged(nameof(TranslationFinished));
            }
            return true;
        }

        private void CheckPassword()
        {
            if (PasswordInput == null)
            {
                //not a valid password
                PasswordFailure = string.Concat(LoginDis, " ", FailureDis);
                OnPropertyChanged(nameof(PasswordFailure));
            }
            else
            {
                if (SqlLibrary.User.SelectUser(PasswordInput) == null) // if not null current user is set in this method
                {
                    //not a valid user
                    PasswordFailure = string.Concat(LoginDis, " ", FailureDis);
                    OnPropertyChanged(nameof(PasswordFailure));
                }
                else
                {
                    OnLoginClicked();
                }
            }
        }

        private async void OnLoginClicked()
        {
            if (SqlLibrary.User.CurrentUser != null) //extra chack but should be unneeded
            {
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                SqlLibrary.Display.RefreshList(SqlLibrary.User.CurrentUser.User_Id);
                //AppShellTitles.UpdateAppShellTabNames(SqlLibrary.User.CurrentUser.User_Id); //dosen't work as intended

                //clear values
                PasswordInput = string.Empty;
                PasswordFailure = string.Empty;
                OnPropertyChanged(nameof(PasswordInput));
                OnPropertyChanged(nameof(PasswordFailure));

                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
        }

        public async void CreateAccount(string email, string password)
        {
            if (!SqlLibrary.User.UniqueUserCheck(password)) //onyl create unique users
            {
                SqlLibrary.User.InsertUser(new UserMdl(email, password)); // current user is set in this method
                ISettingMdl setting = SqlLibrary.Setting.SelectSettigns();

                if (SelectedLanguage.Language_Id != setting.Language_Id && SqlLibrary.User.CurrentUser != null) //default display is english GB so if its not englsih on new user, conver the app to the user language
                { //this is skipped if you have no wifi
                    bool wait = await UpdateDispalyMethod.UpdateApplcionDispaly(SelectedLanguage, SqlLibrary.User.CurrentUser.User_Id);
                }
                OnLoginClicked();
            } //password is already taken try again
        }
    }
}
