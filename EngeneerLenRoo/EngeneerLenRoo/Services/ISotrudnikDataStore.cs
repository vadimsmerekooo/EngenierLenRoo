using System.Collections.Generic;
using System.Threading.Tasks;

namespace EngeneerLenRoo.Services
{
    public interface ISotrudnikDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id, string idSotrudnik);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        
    }
}