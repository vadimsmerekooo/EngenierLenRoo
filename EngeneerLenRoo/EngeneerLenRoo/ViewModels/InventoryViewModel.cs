using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using EngeneerLenRoo.Models;
using EngeneerLenRoo.ViewModels.CabinetModel;
using EngeneerLenRoo.Views;
using Xamarin.Forms;

namespace EngeneerLenRoo.ViewModels
{
    public class InventoryViewModel : BaseViewModel
    {
        private Cabinet _selectedCabinet;
        
        public ObservableCollection<Cabinet> Cabinets { get; }
        
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        
        public Command<Cabinet> ItemTapped { get; }


        public InventoryViewModel()
        {
            Title = "Кабинеты";
            Cabinets = new ObservableCollection<Cabinet>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Cabinet>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Cabinets.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Cabinets.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public Cabinet SelectedItem
        {
            get => _selectedCabinet;
            set
            {
                SetProperty(ref _selectedCabinet, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewCabinetPage));
        }

        async void OnItemSelected(Cabinet item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(CabinetDetailPage)}?{nameof(CabinetDetailViewModel.ItemId)}={item.Id}");
        }
    }
}