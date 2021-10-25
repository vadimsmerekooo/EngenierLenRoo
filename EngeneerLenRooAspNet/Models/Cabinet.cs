using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.Models
{
    public class Cabinet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Номер кабинета")]
        public string Name { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Не введен номер телефона")]
        public int Phone { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}