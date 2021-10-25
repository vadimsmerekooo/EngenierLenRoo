using System.ComponentModel.DataAnnotations;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.Models
{
    public class EmployeeFormModel : IValidFormModel
    {
        [Display(Name = "ФИО")]
        public string Fio { get; set; }
        [Display(Name = "Ip компьютера")]
        public int? IpComputer { get; set; }
        [Display(Name = "Номер пк в МАП")]
        public int? NumberPcMap { get; set; }
        [Display(Name = "User в МАП")]
        public string UserMap { get; set; }

        public bool IsNull() => string.IsNullOrWhiteSpace(Fio)
                                && IpComputer == 0
                                && NumberPcMap == 0
                                && string.IsNullOrWhiteSpace(UserMap);
    }
}