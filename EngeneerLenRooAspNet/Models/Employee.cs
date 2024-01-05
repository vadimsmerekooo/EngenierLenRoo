using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        public virtual List<Chat> Chats { get; set; } = new List<Chat>(); 

        public string GetShortFio()
        {
            string[] splitFio = Fio.Split(' ');
            string Name;
            string LastName;
            string SecondName;
            if(splitFio.Length == 3)
            {
                LastName = splitFio[0];
                Name = splitFio[1].First() + ".";
                SecondName = splitFio[2].First() + ".";
                return $"{LastName} {Name} {SecondName}";
            }
            if (splitFio.Length == 1)
            {
                LastName = splitFio[0];
                return $"{LastName}";
            }
            return $"Иванов И.И.";
        }
    }
}