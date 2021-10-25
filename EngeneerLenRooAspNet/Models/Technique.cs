using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.Models
{
    public class Technique
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Поле Название, не заполнено.")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Тип техники")]
        public TypeTechnique TypeTechnique { get; set; }
        [Display(Name = "Инвентарный номер. Если техника стоит на 071 счете, поле можно оставить пустым!")]
        public long InventoryNumber { get; set; }
        
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}