using System.ComponentModel.DataAnnotations;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.Models
{
    public class EmployeeFormModel : IValidFormModel
    {
        [Display(Name = "ФИО")]
        public string Fio { get; set; }
        [Display(Name = "Дополнительно")]
        public string Description { get; set; }

        public bool IsNull() => string.IsNullOrWhiteSpace(Fio)
                                && string.IsNullOrWhiteSpace(Description);
    }
}