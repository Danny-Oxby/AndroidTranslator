using AllergyTranslator.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllergyTranslator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        bool ShowWarning = false;
        public SettingPage()
        {
            ShowWarning = false;
            BindingContext = new SettingsViewModel();
            InitializeComponent();
            ShowWarning = true;
        }
        private async void LanguageChanged_Picker_Focused(object sender, FocusEventArgs e)
        {
            if (ShowWarning)
            {
                SettingsViewModel viewModel = (SettingsViewModel)BindingContext;
                ShowWarning = false;
                await DisplayAlert(viewModel.LanguageDis, viewModel.LanguageWarningDis, viewModel.UpdateDis);
            }
        }
    }
}