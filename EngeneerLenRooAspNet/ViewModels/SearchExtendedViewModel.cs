using System.Collections.Generic;
using EngeneerLenRooAspNet.Models;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class SearchExtendedViewModel
    {
        public List<Cabinet> Cabinets { get; set; } = new List<Cabinet>();
        
        public CabinetFormModel CabinetFormModel { get; set; }
        public EmployeeFormModel EmployeeFormModel { get; set; }
        public TechniqueFormModel TechniqueFormModel { get; set; }

        public bool IsNull() => CabinetFormModel.IsNull() && EmployeeFormModel.IsNull() && TechniqueFormModel.IsNull();
    }
}