using System.Collections.Generic;
using System.Linq;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class EmployeeTechniquesViewModel
    {
        public string CabinetId { get; }
        public string EmployeeId { get; }
        public List<Technique> Techniques { get; } = new List<Technique>();
        public TypeTechnique TypeTechnique { get; }

        public EmployeeTechniquesViewModel(List<Technique> techniques, TypeTechnique typeTechnique, string cabinetId, string empId)
        {
            Techniques = techniques.Where(t => t.TypeTechnique == typeTechnique).ToList();
            TypeTechnique = typeTechnique;
            CabinetId = cabinetId;
            EmployeeId = empId;
        }
    }
}