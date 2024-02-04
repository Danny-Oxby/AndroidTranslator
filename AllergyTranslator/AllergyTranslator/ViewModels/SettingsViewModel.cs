using AllergyModelLibrary.Interface;
using AllergyModelLibrary.Model;
using AllergyTranslator.Helpers;
using AllergyTranslator.Services;
using AllergyTranslator.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllergyTranslator.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Commands
        public ICommand ClearChanges { get; private set; }
        public ICommand SaveChanges { get; private set; }
        #endregion

        public SettingsViewModel()
        {
            Title = "Settings";
            Title = SqlLibrary.Display.FindDispalyValue(Title, SqlLibrary.User.CurrentUser.User_Id);

            ClearChanges = new Command(DefaultSettings);
            SaveChanges = new Command(UpdateInformation);

            DefaultSettingsParam = SqlLibrary.Setting.SelectSettigns(); //seed the values to the local param to recude db calls

            PasswordDis = SqlLibrary.Display.FindDispalyValue("Password", SqlLibrary.User.CurrentUser.User_Id);
            RecoveryDis = SqlLibrary.Display.FindDispalyValue("Recovery", SqlLibrary.User.CurrentUser.User_Id);
            UpdateDis = SqlLibrary.Display.FindDispalyValue("Update", SqlLibrary.User.CurrentUser.User_Id);
            ShowDis = SqlLibrary.Display.FindDispalyValue("Show", SqlLibrary.User.CurrentUser.User_Id);
            DisplayDis = SqlLibrary.Display.FindDispalyValue("Display", SqlLibrary.User.CurrentUser.User_Id);
            ResetDis = SqlLibrary.Display.FindDispalyValue("Cancle", SqlLibrary.User.CurrentUser.User_Id);
            SaveDis = SqlLibrary.Display.FindDispalyValue("Save", SqlLibrary.User.CurrentUser.User_Id);
            ChangesDis = SqlLibrary.Display.FindDispalyValue("Changes", SqlLibrary.User.CurrentUser.User_Id);
            WarningDis = SqlLibrary.Display.FindDispalyValue("Warning", SqlLibrary.User.CurrentUser.User_Id);
            TACDis = SqlLibrary.Display.FindDispalyValue("Terms And Conditions", SqlLibrary.User.CurrentUser.User_Id);
            WordMatchDis = SqlLibrary.Display.FindDispalyValue("Match Whole Word", SqlLibrary.User.CurrentUser.User_Id);
            LanguageDis = SqlLibrary.Display.FindDispalyValue("Language", SqlLibrary.User.CurrentUser.User_Id);
            PasseordErrorMessage = SqlLibrary.Display.FindDispalyValue("Invalid Password Select Another", SqlLibrary.User.CurrentUser.User_Id);
            LanguageWarningDis = SqlLibrary.Display.FindDispalyValue("Changing Langauge Will Trigger Application Restart, And Will Not Update Your Allergies", SqlLibrary.User.CurrentUser.User_Id);

            DefaultSettings();
        }

        #region display values
        private string PasswordDis = "Password";
        private string RecoveryDis = "Recovery";
        private string ShowDis = "Show";
        private string DisplayDis = "Display";
        private string ResetDis = "Cancle";
        private string SaveDis = "Save";
        private string ChangesDis = "Changes";
        private string WarningDis = "Warning";
        private string TACDis = "Terms And Conditions";
        public string WordMatchDis { get; private set; } = "Match Whole Word";
        public string LanguageDis { get; set; } = "Language";
        public string UpdateDis { get; private set; } = "Update";
        public string LanguageWarningDis { get; private set; } = "Changing Langauge Will Trigger Application Restart, And Will Not Update Your Allergies";
        public string PasswordUpdateDis { get { return string.Concat(PasswordDis, " ", UpdateDis); } } // "Update Password";
        public string RecoveryUpdateDis { get { return string.Concat(RecoveryDis, " ", UpdateDis); } } // "Recovery Password";
        public string LanguageDisplayDis { get { return string.Concat(LanguageDis, " ", DisplayDis); } } // "Language Display";
        public string ShowTACDis { get { return string.Concat(ShowDis, " ", TACDis); } } // "Show Terms And Conditions";
        public string ShowWarningDis { get { return string.Concat(ShowDis, " ", WarningDis); } } // "Show Warning";
        public string SaveChangesDis { get { return string.Concat(SaveDis, " ", ChangesDis); } } // "Save Changes";
        public string CancleChangesDis { get { return string.Concat(ResetDis, " ", ChangesDis); } } // "Cancle Changes";
        public string PasseordErrorMessage { get; private set; } // "Invalid Password Select Another"
        #endregion

        #region properties

        private ISettingMdl DefaultSettingsParam;
        private bool UserInfoUpdated = false; //if true upadte the userinfor as well as the setting info

        private string passwordUpdate;
        public string PasswordUpdate { get => passwordUpdate; set { passwordUpdate = value; UserInfoUpdated = true; } }

        private string recoveryUpdate;
        public string RecoveryUpdate { get => recoveryUpdate; set { recoveryUpdate = value; UserInfoUpdated = true; } }
        public List<ILanguageMdl> LanguageList { get; set; }

        private ILanguageMdl selectedLanguage;
        public ILanguageMdl SelectedLanguage
        {
            get => selectedLanguage;
            set { 
                if (value != null)
                    selectedLanguage = value; 
            }
        }
        public bool TAC { get; set; } //terms and conditions
        public bool WarningReset { get; set; } // popup about the OCR and Transaltion may not be accurate
        public bool MatchFullWord { get; set; } //for the search function of the OCR
        public bool HasWIFI { get; private set; } = InternetChecker.CheckInternetAccess();
        public bool WIFIMessage { get { return !HasWIFI; } }
        public bool ShowPasswordError { get; private set; } = false;
        #endregion

        private void DefaultSettings() //restore the default settings information
        {
            RecoveryUpdate = SqlLibrary.User.CurrentUser.Recovery;
            PasswordUpdate = "";
            LanguageList = SqlLibrary.Language.SelectLanguageList();
            SelectedLanguage = LanguageList.Find(o => o.Language_Id == DefaultSettingsParam.Language_Id);
            TAC = DefaultSettingsParam.TC;
            WarningReset = DefaultSettingsParam.Warning_State;
            MatchFullWord = DefaultSettingsParam.Full_Word;
            UserInfoUpdated = false;

            OnPropertyChanged(nameof(RecoveryUpdate));
            OnPropertyChanged(nameof(PasswordUpdate));
            OnPropertyChanged(nameof(LanguageList));
            OnPropertyChanged(nameof(SelectedLanguage));
            OnPropertyChanged(nameof(TAC));
            OnPropertyChanged(nameof(WarningReset));
            OnPropertyChanged(nameof(MatchFullWord));
        }
        private async void UpdateInformation()
        {
            try
            {
                if (PasswordUpdate == "" || SqlLibrary.User.UniquePasswordCheck(PasswordUpdate, SqlLibrary.User.CurrentUser.User_Id) ) //password isn't taken or password hasn't changed
                {
                    bool CloseOnUpdate = false;

                    //langauge changed and there is wifi
                    if (SelectedLanguage.Language_Id != DefaultSettingsParam.Language_Id && InternetChecker.CheckInternetAccess())
                    { //use chose a new language
                        bool wait;
                        //thsi si skipped if you have no wifi
                        wait = await UpdateDispalyMethod.UpdateApplcionDispaly(SelectedLanguage, SqlLibrary.User.CurrentUser.User_Id);

                        //update the close value
                        CloseOnUpdate = true;
                    }


                    ISettingMdl settingMdl = new SettingMdl()
                    {
                        Setting_ID = DefaultSettingsParam.Setting_ID, //id values are needed due to the way SqlLite inbuil update method works
                        User_Id = DefaultSettingsParam.User_Id,
                        Font_Size = DefaultSettingsParam.Font_Size,
                        Font_Type = DefaultSettingsParam.Font_Type,
                        Full_Word = MatchFullWord,
                        Warning_State = WarningReset,
                        TC = TAC,
                        Language_Id = SelectedLanguage.Language_Id,
                    };
                    SqlLibrary.Setting.UpdateSetting(settingMdl);

                    if (UserInfoUpdated)
                    {
                        if (PasswordUpdate == "") //if the password hasnt updated then don't change it
                        {
                            PasswordUpdate = SqlLibrary.User.CurrentUser.Password;
                        }

                        //update the user information here
                        IUserMdl userMdl = new UserMdl()
                        {
                            User_Id = DefaultSettingsParam.User_Id,
                            Recovery = RecoveryUpdate,
                            Password = PasswordUpdate,
                        };
                        SqlLibrary.User.UpdateUser(userMdl);
                    }

                    if (CloseOnUpdate)
                    {
                        //go back to the login page / restart the applcation
                        ForceCloseService.CloseApplication();
                    }
                    else
                    {
                        //reset the page values to match the new values
                        DefaultSettingsParam = SqlLibrary.Setting.SelectSettigns();
                        DefaultSettings();

                        //await Shell.Current.GoToAsync(nameof(AboutPage));
                    }
                    ShowPasswordError = false;
                    OnPropertyChanged(nameof(ShowPasswordError));
                }
                else
                {
                    ShowPasswordError = true;
                    OnPropertyChanged(nameof(ShowPasswordError));
                }
            }
            catch (Exception ex)
            {
                SqlLibrary.Log.LogIssue(ex.Message, nameof(UpdateInformation), SqlLibrary.IssueList.Settings,
                    string.Concat("Params : ", MatchFullWord, " ", WarningReset, " ", TAC, " ", SelectedLanguage.Full_Name, " ",
                    PasswordUpdate, " ", RecoveryUpdate, " "));
            }
        }
    }
}
