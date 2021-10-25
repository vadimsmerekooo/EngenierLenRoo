using EngeneerLenRoo.ViewModels;
using EngeneerLenRoo.Views;
using System;
using System.Collections.Generic;
using EngeneerLenRoo.ViewModels.CabinetModel;
using Xamarin.Forms;

namespace EngeneerLenRoo
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CabinetDetailPage), typeof(CabinetDetailPage));
            Routing.RegisterRoute(nameof(NewCabinetPage), typeof(NewCabinetPage));
            Routing.RegisterRoute(nameof(SotrudnikDetailPage), typeof(SotrudnikDetailPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
