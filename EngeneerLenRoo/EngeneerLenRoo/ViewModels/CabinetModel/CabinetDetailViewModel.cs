using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using EngeneerLenRoo.Models;
using EngeneerLenRoo.Views;
using Xamarin.Forms;

namespace EngeneerLenRoo.ViewModels.CabinetModel
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class CabinetDetailViewModel : BaseViewModel
    {
        private string itemId;
        private Sotrudnik _selectedSotrudnik;
        
        
        
        public ObservableCollection<Sotrudnik> Sotrudniks { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Sotrudnik> ItemTapped { get; }

        public string Id { get; set; }
        public string Title { get; set; }


        public CabinetDetailViewModel()
        {
            Sotrudniks = new ObservableCollection<Sotrudnik>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Sotrudnik>(OnItemSelected);

            // AddItemCommand = new Command(OnAddItem);
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Sotrudniks.Clear();
                var item = await DataStore.GetItemAsync(itemId ,true);
                foreach (var sotrudnik in item.Sotrudniks)
                {
                    Sotrudniks.Add(sotrudnik);
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
        public Sotrudnik SelectedItem
        {
            get => _selectedSotrudnik;
            set
            {
                SetProperty(ref _selectedSotrudnik, value);
                OnItemSelected(value);
            }
        }

        // private async void OnAddItem(object obj)
        // {
        //     await Shell.Current.GoToAsync(nameof(NewCabinetPage));
        // }

        async void OnItemSelected(Sotrudnik item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(SotrudnikDetailPage)}?{nameof(SotrudnikDetailsViewModel.CabinetId)}={ItemId}&{nameof(SotrudnikDetailsViewModel.ItemId)}={item.Id}");
        }
        
        public string ItemId
        {
            get => itemId;
            set
            {
                itemId = value;
                LoadItemId(itemId);
            }
        }



        public async void LoadItemId(string cabinetId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(cabinetId);
                Id = item.Id;
                Title = "Кабинет: " + item.Name;
                Sotrudniks.Clear();
                foreach (var sotrudnik in item.Sotrudniks)
                {
                    Sotrudniks.Add(sotrudnik);
                }

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}