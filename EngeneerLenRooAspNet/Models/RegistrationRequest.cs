using static EngeneerLenRooAspNet.Models.Employee;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class RegistrationRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [StringLength(100, ErrorMessage = "{0} должен содержать не менее {2} символов и не более {1}.", MinimumLength = 8)]
        [Display(Name = "Email для отправки пароля")]
        public string Email { get; set; }

        [Display(Name = "Фамилия Имя Отчество")]
        [StringLength(50, ErrorMessage = "{0} должен содержать не менее {2} символов и не более {1}.", MinimumLength = 3)]
        [Required(ErrorMessage = "Поле ФИО не заполнено!")]
        public string Fio { get; set; }

        [Required(ErrorMessage = "Выберите кабинет!")]
        [Display(Name = "Номер кабинета")]
        public string NumberCabinet { get; set; }

        [Required(ErrorMessage = "Выберите Класс!")]
        [Display(Name = "Класс")]
        public TypePost? Post { get; set; } = TypePost.Не_определен;

        [Required(ErrorMessage = "Выберите отдел!")]
        [Display(Name = "Отдел")]
        public TypeDepartment? Department { get; set; } = TypeDepartment.Не_определен;

        public TypeRequest Status { get; set; } = TypeRequest.Processing;
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
    public enum TypeRequest
    {
        Processing,
        Ok,
        Denied
    }
}
