using System.ComponentModel.DataAnnotations;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.Models
{
    public class CabinetFormModel : IValidFormModel
    {
        [Display(Name = "Номер кабинета")]
        public string Name { get; set; }
        [Display(Name = "Номер телефона")]
        public int Phone { get; set; }

        public bool IsNull() => string.IsNullOrWhiteSpace(Name) && Phone == 0;
    }
}