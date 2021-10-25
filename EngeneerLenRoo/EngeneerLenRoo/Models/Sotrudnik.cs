using System.Collections.Generic;

namespace EngeneerLenRoo.Models
{
    public class Sotrudnik
    {
        public string Id { get; set; }
        public string Fio { get; set; }
        public List<Technique> Techniques { get; set; }
        public int? IpComputer { get; set; }
    }
}