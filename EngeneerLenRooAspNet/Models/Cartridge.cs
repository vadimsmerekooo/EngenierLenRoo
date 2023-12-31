using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class Cartridge
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        public DateTime DateGet { get; set; }
        public DateTime? DateSet { get; set; }
        public bool IsIssued { get; set; }
        public string EmployeeId { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
        [Display(Name = "Сотрудник")]
        public Employee Employee { get; set; }
    }
}
