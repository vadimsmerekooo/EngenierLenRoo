using EngeneerLenRoo.ViewModels.CabinetModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EngeneerLenRoo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CabinetDetailPage : ContentPage
    {
        public CabinetDetailPage()
        {
            InitializeComponent();
            BindingContext = new CabinetDetailViewModel();
        }
    }
}