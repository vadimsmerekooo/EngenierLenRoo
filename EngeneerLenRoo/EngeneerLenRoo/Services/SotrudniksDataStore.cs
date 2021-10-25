using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngeneerLenRoo.Models;
using EngeneerLenRoo.ViewModels;

namespace EngeneerLenRoo.Services
{
    public class SotrudniksDataStore: BaseViewModel, ISotrudnikDataStore<Sotrudnik>
    {
        private readonly List<Sotrudnik> _sotrudniks;
        
        public async Task<bool> AddItemAsync(Sotrudnik item)
        {
            _sotrudniks.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Sotrudnik item)
        {

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            

            return await Task.FromResult(true);
        }

        public async Task<Sotrudnik> GetItemAsync(string id, string idSotrudnik)
        {
            return await Task.FromResult(DataStore.GetItemAsync(id).Result.Sotrudniks.FirstOrDefault(s => s.Id == idSotrudnik));
        }

        public async Task<IEnumerable<Sotrudnik>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_sotrudniks);
        }
        
    }
}