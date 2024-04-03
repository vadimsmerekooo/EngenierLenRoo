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
        [Required]
        [Display(Name = "Класс")]
        public TypePost? Post { get; set; } = TypePost.Не_определен;
        [Required]
        [Display(Name = "Отдел")]
        public TypeDepartment? Department { get; set; } = TypeDepartment.Не_определен;
        public string HashCode { get; set; }
        public string CabinetId { get; set; }
        public Cabinet Cabinet { get; set; }
        [NotMapped]
        public List<Cartridge> Cartridges { get; set; } = new List<Cartridge>();
        public virtual List<Chat> Chats { get; set; } = new List<Chat>();



        public bool IsCanIWriteUser(Employee userDirect)
        {
            switch (Post)
            {
                case TypePost.Администратор: return true;
                case TypePost.Не_определен: return false;
                case TypePost.Начальник: return true;
                case TypePost.Сотрудник:
                    {
                        if(userDirect.Department != Department && userDirect.Post == TypePost.Начальник) return false;
                        return true;
                    }
                default: return false;
            }
        }


        public string GetShortFio()
        {
            string[] splitFio = Fio.Split(' ');
            string Name;
            string LastName;
            string SecondName;
            if (splitFio.Length == 3)
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
        public enum TypePost
        {
            Не_определен,
            Администратор,
            Начальник,
            Сотрудник
        }
        public enum TypeDepartment
        {
            Не_определен,
            Управляющий_персонал,
            Отдел_материалов,
            Бухгалтерия,
            Экономисты,
            Финансовый_отдел,
            Родительская_плата
        }
    }
}