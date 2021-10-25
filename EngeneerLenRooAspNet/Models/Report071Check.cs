using System.Collections.Generic;
using System.Linq;
using EngeneerLenRooAspNet.Services;

namespace EngeneerLenRooAspNet.Models
{
    public class Report071Check
    {
        public string Name { get; set; }
        public TypeTechnique TypeTechnique { get; set; }
        public List<Report071CheckCabinet> Cabinets { get; set; } = new List<Report071CheckCabinet>();

        public int Count() => Cabinets.Sum(x => x.Count);
    }

    public class Report071CheckCabinet
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