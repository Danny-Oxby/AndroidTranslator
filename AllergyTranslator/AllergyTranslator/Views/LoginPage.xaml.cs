using AllergyModelLibrary.Model;
using AllergyTranslator.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllergyTranslator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            this.BindingContext = new LoginViewModel();
            InitializeComponent();
        }

        private async void Button_Creat_User(object sender, EventArgs e)
        {
            string Password = null, Recovery = null;

            LoginViewModel viewModel = (LoginViewModel)BindingContext;
            try
            {
                Password = await DisplayPromptAsync(viewModel.PasswordDis, viewModel.CreateDis, viewModel.AcceptDis, viewModel.FailureDis);
                if (Password != null) //skip if cancle was pressed
                {
                    do
                    {
                        Recovery = await DisplayPromptAsync(viewModel.EmailRecoveryDis, viewModel.EmailValidationDis, viewModel.AcceptDis, viewModel.FailureDis);
                    } while (Recovery != null && !IsValidEmail(Recovery)); //exit on null or valid emails
                }
            }
            catch (Exception ex)
            {
                SqlLibrary.Log.LogIssue(ex.Message, nameof(Button_Creat_User));
            }

            if (Password != null && Recovery != null)
            {
                if (SqlLibrary.User.UniqueUserCheck(Password))
                {
                    await DisplayAlert(viewModel.FailureDis, viewModel.PasswordFailureDis, viewModel.AcceptDis);
                }
                else
                {
                    viewModel.CreateAccount(Recovery, Password);
                    //SqlLibrary.User.InsertUser(new UserMdl(Recovery, Password));
                    await DisplayAlert(viewModel.SuccessDis, viewModel.UserCreationDis, viewModel.AcceptDis);
                }
            }
        }

        private async void Button_Send_Email(object sender, EventArgs e)
        {
            string Recovery = null;
            LoginViewModel viewModel = (LoginViewModel)BindingContext;

            try
            {
                do
                {
                    Recovery = await DisplayPromptAsync(viewModel.EmailRecoveryDis, viewModel.EmailValidationDis);
                } while (Recovery != null && !IsValidEmail(Recovery)); //exit on null or valid emails

                if (Recovery != null)
                {
                    string password = SqlLibrary.User.DoesUserExist(Recovery);
                    if (password != null)
                    {
                        await DisplayAlert("Email Mock", password, "OK");

                        //List<string> recipients = new List<string>
                        //{ Recovery //add the email to the list
                        //}; // get the list of emails

                        //var message = new EmailMessage
                        //{
                        //    Subject = "Email Recovery",
                        //    Body = password,
                        //    To = recipients,
                        //};
                        //await Email.ComposeAsync(message);
                    }
                    else
                    {
                        //user dosn't exist
                        await DisplayAlert(viewModel.FailureDis, viewModel.MatchingErrorDis, viewModel.AcceptDis);

                    }
                } //else password not valid
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                await DisplayAlert(viewModel.FailureDis, viewModel.EmailSupportDis, viewModel.AcceptDis);
                SqlLibrary.Log.LogIssue(fbsEx.Message, nameof(Button_Send_Email), SqlLibrary.IssueList.Usability);
                
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                SqlLibrary.Log.LogIssue(ex.Message, nameof(Button_Send_Email), SqlLibrary.IssueList.Display);

            }
        }

        //code snipper taken from https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }


    }
}