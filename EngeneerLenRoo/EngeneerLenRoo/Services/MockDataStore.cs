using EngeneerLenRoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngeneerLenRoo.Services
{
    public class MockDataStore : IDataStore<Cabinet>
    {
        private readonly List<Cabinet> cabinet;

        public MockDataStore()
        {
            cabinet = new List<Cabinet>()
            {
                new Cabinet()
                {
                    Id = 1.ToString(),
                    Name = 248.ToString(),
                    Sotrudniks = new List<Sotrudnik>()
                    {
                        new Sotrudnik()
                        {
                            Id = 1.ToString(),
                            Fio = "Программист",
                            Techniques = new List<Technique>()
                            {
                                new Technique()
                                {
                                    Id = 1.ToString(),
                                    InventoryNumber = 071,
                                    TypeTechnique = TypeTechnique.Pc,
                                    Name = "Сервер"
                                },
                                new Technique()
                                {
                                    Id = 2.ToString(), InventoryNumber = 071, TypeTechnique = TypeTechnique.Printer,
                                    Name = "Kyocera m2335dn"
                                }
                            }
                        }
                    }
                },
                new Cabinet()
                {
                    Id = 2.ToString(), Name = 247.ToString(), Sotrudniks = new List<Sotrudnik>()
                    {
                        new Sotrudnik()
                        {
                            Id = 2.ToString(), Fio = "Тыгыдык", IpComputer = 125, Techniques = new List<Technique>()
                            {
                                new Technique()
                                {
                                    Id = 3.ToString(), Name = "Hp lj m14", InventoryNumber = 01300046,
                                    TypeTechnique = TypeTechnique.Printer
                                }
                            }
                        }
                    }
                }
            };
        }

        public async Task<bool> AddItemAsync(Cabinet item)
        {
            cabinet.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Cabinet item)
        {
            var oldItem = cabinet.FirstOrDefault((Cabinet arg) => arg.Id == item.Id);
            cabinet.Remove(oldItem);
            cabinet.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = cabinet.FirstOrDefault((Cabinet arg) => arg.Id == id);
            cabinet.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Cabinet> GetItemAsync(string id)
        {
            return await Task.FromResult(cabinet.FirstOrDefault(s => s.Id == id));
        }

        public async Task<Cabinet> GetItemAsync(string id, bool forceRefresh)
        {
            return await Task.FromResult(cabinet.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Cabinet>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(cabinet);
        }
    }
}