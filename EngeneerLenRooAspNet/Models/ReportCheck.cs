using System.Collections.Generic;
using System.Linq;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.Models
{
    public class ReportCheck
    {
        public string Name { get; set; }
        public long InventoryNumber { get; set; }
        public TypeTechnique TypeTechnique { get; set; }
        public List<ReportCheckCabinet> Cabinets { get; set; } = new List<ReportCheckCabinet>();

        public int Count() => Cabinets.Sum(x => x.Count);
    }

    public class ReportCheckCabinet
    {
        public string CabinetId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; } = 1;

        public bool Counter()
        {
            Count++;
            return true;
        }
    }
}