using System.Collections.Generic;

namespace EngeneerLenRooAspNet.Models
{
    public class CartridgeViewModel : Cartridge
    {
        public IEnumerable<Employee> Employees { get; set; }
    }
}
