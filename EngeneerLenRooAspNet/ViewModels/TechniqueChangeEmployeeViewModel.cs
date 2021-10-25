using System.Collections.Generic;
using EngeneerLenRooAspNet.Models;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class TechniqueChangeEmployeeViewModel
    {
        public List<Cabinet> Cabinets { get; set; } = new List<Cabinet>();
        public Employee Employee { get; set; }
        public Technique Technique { get; set; }
        
        public Cabinet SelectCabinet { get; set; }
        public string SelectCabinetId { get; set; }
        public string SelectEmployeeId { get; set; }
    }
}