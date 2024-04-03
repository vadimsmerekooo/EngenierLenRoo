using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class Cartridge
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateGet { get; set; }
        public DateTime? DateSet { get; set; }
        public bool IsIssued { get; set; }
        public bool IsIssuedRight { get; set; }


        public string TechniqueId { get; set; }
        public string EmployeeGetId { get; set; }
        public string EmployeeSetId { get; set; }
        public int? CaseId { get; set; }


        public virtual Technique Technique { get; set; }
        [Display(Name = "Коробка")]
        public Case Case { get; set; }
        [Display(Name = "Сдал сотрудник")]
        public virtual Employee EmployeeGet { get; set; }
        [Display(Name = "Выдан сотруднику")]
        public virtual Employee EmployeeSet { get; set; }
    }
}
