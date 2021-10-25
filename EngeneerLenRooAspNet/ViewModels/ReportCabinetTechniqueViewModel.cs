using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class ReportCabinetTechniqueViewModel
    {
        public Cabinet Cabinet { get; set; }
        public List<ReportCabinetTechnique> CabinetTechnique { get; set; } = new List<ReportCabinetTechnique>();
        public List<ReportEmployeeTechnique> EmployeeTechniques { get; set; } = new List<ReportEmployeeTechnique>();
    }

    public class ReportCabinetTechnique
    {
        public string Name { get; set; }
        private List<Technique> Techniques { get; set; }

        public ReportCabinetTechnique(string name, List<Technique> techniques)
        {
            Name = name;
            Techniques = techniques;
        }

        public int GetCountTechnique() => Techniques.Count;
        public int GetCount071Technique() => Techniques.Count(th => th.InventoryNumber == 71);
    }

    public class ReportEmployeeTechnique
    {
        public string Fio { get; set; }
        public List<ReportCabinetTechnique> CabinetTechniques { get; set; } = new List<ReportCabinetTechnique>();
    }
}
