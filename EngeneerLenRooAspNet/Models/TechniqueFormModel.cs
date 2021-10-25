using System.ComponentModel.DataAnnotations;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.Models
{
    public class TechniqueFormModel : IValidFormModel
    {
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Тип техники")]
        public TypeTechnique TypeTechnique { get; set; }
        [Display(Name = "Инвентарный номер")]
        public long InventoryNumber { get; set; }

        public bool IsNull() => string.IsNullOrWhiteSpace(Name)
                                && TypeTechnique == TypeTechnique.All
                                && InventoryNumber == 0;
    }
}