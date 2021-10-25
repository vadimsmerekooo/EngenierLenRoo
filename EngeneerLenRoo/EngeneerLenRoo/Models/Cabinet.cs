using System.Collections.Generic;

namespace EngeneerLenRoo.Models
{
    public class Cabinet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Sotrudnik> Sotrudniks { get; set; } 
    }
}