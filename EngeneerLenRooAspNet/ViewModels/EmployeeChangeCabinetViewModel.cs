using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EngeneerLenRooAspNet.Models;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class EmployeeChangeCabinetViewModel
    {
        public Employee Employee { get; set; }
        public List<Cabinet> Cabinets { get; set; } = new List<Cabinet>();
        public string SelectCabinetId { get; set; }

        [Display(Name = "Переместить вместе с техникой?")]
        public bool IsWithTechniques { get; set; } = true;
    }
}