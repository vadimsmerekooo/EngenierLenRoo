using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Display(Name = "Фамилия Имя Отчество")]
        [Required(ErrorMessage = "Поле ФИО не заполнено!")]
        public string Fio { get; set; }

        public List<Technique> Techniques { get; set; } = new List<Technique>();
        [Display(Name = "Ip компьютера")]
        public int? IpComputer { get; set; }
        [Display(Name = "Номер пк в МАП")]
        public int? NumberPcMap { get; set; }
        [Display(Name = "User в МАП")]
        public string UserMap { get; set; }
        public string CabinetId { get; set; }
        public Cabinet Cabinet { get; set; }
    }
}