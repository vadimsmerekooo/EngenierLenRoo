using EngeneerLenRoo.ViewModels.CabinetModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EngeneerLenRoo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SotrudnikDetailPage : ContentPage
    {
        public SotrudnikDetailPage()
        {
            InitializeComponent();
            BindingContext = new SotrudnikDetailsViewModel();
        } 
    }
}