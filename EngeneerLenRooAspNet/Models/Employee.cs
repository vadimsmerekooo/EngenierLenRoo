using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class Employee
    {
        public string Id { get; set; }
        [Display(Name = "Фамилия Имя Отчество")]
        [Required(ErrorMessage = "Поле ФИО не заполнено!")]
        public string Fio { get; set; }

        public List<Technique> Techniques { get; set; } = new List<Technique>();
        [Display(Name = "Дополнительно")]
        public string Description { get; set; }
        public string CabinetId { get; set; }
        public Cabinet Cabinet { get; set; }
        public List<Cartridge> Cartridges { get; set; } = new List<Cartridge>();
    }
}