using System;
using System.ComponentModel;
using System.Windows.Input;
using EngeneerLenRoo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EngeneerLenRoo.Views
{
    public partial class AboutPage : ContentPage
    {
        InventoryViewModel _viewModel;
        public AboutPage()
        {
            InitializeComponent();
            
            BindingContext = _viewModel = new InventoryViewModel();
        }
    }
}