using System.ComponentModel.DataAnnotations;
using EngeneerLenRooAspNet.Models;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class TechniquesViewModel
    {
        public Cabinet Cabinet { get; set; }
        public Employee Employee { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public Technique Technique { get; set; }
    }
}