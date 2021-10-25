using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EngeneerLenRoo.Models;
using Xamarin.Forms;

namespace EngeneerLenRoo.ViewModels.CabinetModel
{
    [QueryProperty(nameof(CabinetId), nameof(CabinetId))] 
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class SotrudnikDetailsViewModel : BaseViewModel
    {
        private string itemId;
        private string cabinetID;
        
        public string Id { get; set; }
        public string Title { get; set; }
        
        public ObservableCollection<Technique> Techniques { get; }
        public Command LoadItemsCommand { get; }

        public SotrudnikDetailsViewModel()
        {
            Techniques = new ObservableCollection<Technique>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Techniques.Clear();
                var items = DataStore.GetItemAsync(CabinetId ,true).Result.Sotrudniks;
                foreach (var sotrudnik in items)
                {
                    foreach (var technique in sotrudnik.Techniques)
                    {
                        Techniques.Add(technique);
                    }
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

        public string CabinetId
        {
            get => cabinetID;
            set => cabinetID = value;
        }
        public string ItemId
        {
            get => itemId;
            set
            {
                itemId = value;
                LoadItemId(cabinetID, itemId);
            }
        }
        
        public async void LoadItemId(string cabinetId, string sotrudnikId)
        {
            try
            {
                var item = DataStore.GetItemAsync(cabinetId).Result.Sotrudniks.FirstOrDefault(s => s.Id == sotrudnikId);
                Id = item.Id;
                Title = "Сотрудник: " + item.Fio;
                foreach (var technique in item.Techniques)
                {
                    Techniques.Add(technique);
                }

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}