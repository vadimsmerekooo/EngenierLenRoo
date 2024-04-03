using System.ComponentModel.DataAnnotations;
using System.Configuration;
using EngeneerLenRooAspNet.Models;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class TechniquesViewModel
    {
        public Cabinet Cabinet { get; set; }
        public Employee Employee { get; set; }
        [Display(Name = "Количество")]
        [IntegerValidator(MinValue = 1, MaxValue = 10)]
        public int Count { get; set; } = 1;
        [Required]
        public string EmployeeId { get; set; }
        public Technique Technique { get; set; }
        public string CartridgeName { get; set; }
        [Display(Name ="Добавить комплектом?")]
        public bool IsComplect { get; set; }
        [Display(Name = "Добавить еще?")]
        public bool IsReturn { get; set; } = true;
    }
}