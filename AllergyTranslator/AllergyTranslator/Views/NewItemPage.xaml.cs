using AllergyTranslator.ViewModels;
using Xamarin.Forms;

namespace AllergyTranslator.Views
{
    public partial class NewItemPage : ContentPage
    {
        //public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}