using AllergyTranslator.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllergyTranslator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllergyPage : ContentPage
    {
        public AllergyPage()
        {
            InitializeComponent();
            BindingContext = new AllergyPageViewModel();
            viewModel = (AllergyPageViewModel)BindingContext;
        }
        private AllergyPageViewModel viewModel;

        protected override void OnAppearing() //when this page appears
        {
            base.OnAppearing();
            if (viewModel != null)
                viewModel.RefreshCommand.Execute(null); //when this page load refreach the list
        }
    }
}