using AllergyTranslator.Helpers;
using AllergyTranslator.Services;
using AllergyTranslator.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllergyTranslator.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel();
            TermsAndConditionDisplay();
        }

        private async void TermsAndConditionDisplay()
        {
            if (!SqlLibrary.Setting.SelectSettigns().TC) //if TC has not been accepted
            {
                AboutViewModel viewModel = (AboutViewModel)BindingContext;
                if (await DisplayAlert(viewModel.TermsAndConditionsDis, viewModel.TACListDis, viewModel.AcceptDis, viewModel.RejectDis))
                {
                    //true update settings
                    viewModel.UpdateTermAndCondtionsValue.Execute(null); //null as no param
                }
                else // I want to clear the naviagtion stack here, so change //login to .. after login become the main page
                {
                    //return to login
                    SqlLibrary.User.ClearSelectedUser(); //reset to user on logout

                    ForceCloseService.CloseApplication();
                    //await Shell.Current.GoToAsync("//LoginPage");
                }
            }
        }

        private async void TakePicture_Btn_Pressed(object sender, EventArgs e)
        {
            AboutViewModel viewModel = (AboutViewModel)BindingContext;
            if (PermissionChecker.CheckWiFi() && await PermissionChecker.CheckCamera() 
                && InternetChecker.CheckInternetAccess() && await PermissionChecker.CheckImages())
            {
                if (!SqlLibrary.Setting.SelectSettigns().Warning_State)
                {
                    if (!await DisplayAlert(viewModel.WarningDis, viewModel.AllergyWarningTextDis, viewModel.AcceptDis, viewModel.HideDis))
                    {
                        //stop showing this message
                        viewModel.UpdateWarning.Execute(null); //null as no param
                    }
                }
                viewModel.TakePictureCmd.Execute(null); //null as no param
                OpenSwipeView_Btn_Clicked(sender, e);
            }
            else
                await DisplayAlert(viewModel.FailureDis, viewModel.WifiAndCameraDis, viewModel.AcceptDis);
        }

        //swip function is based on https://medium.com/@helder.debaere/a-simple-swipeable-overlay-view-in-xamarin-forms-49faa7eaa889
        private bool InMotion = false;
        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            if (!InMotion)
            {
                InMotion = true; 
                SwipeView.TranslateTo(0, 800, 200, Easing.Linear);
                InMotion = false;
            }
        }

        private void CloseSwipeView_Btn_Clicked(object sender, EventArgs e)
        {
            SwipeGestureRecognizer_Swiped(sender, null);
        }

        private void OpenSwipeView_Btn_Clicked(object sender, EventArgs e)
        {
            if (!InMotion)
            {
                InMotion = true;
                SwipeView.TranslateTo(0, 0, 200, Easing.Linear);
                InMotion = false;
            }
        }
    }
}