using System.Collections.Generic;
using System.ComponentModel;

namespace EngeneerLenRooAspNet.Models
{
    public class CartridgeViewModel : Cartridge
    {
        public IEnumerable<Employee> Employees { get; set; }
        [DisplayName("Выдана замена сразу?")]
        public bool IsIssuedRight { get; set; } = false;
    }
}
