using EngeneerLenRoo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EngeneerLenRoo.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string name;
        private List<Sotrudnik> _sotrudniks;
        public string Id { get; set; }

        
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }
        public string Text
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public List<Sotrudnik> Sotrudniks
        {
            get => _sotrudniks;
            set => SetProperty(ref _sotrudniks, value);
        }


        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Name;
                _sotrudniks = item.Sotrudniks;

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
