using EngeneerLenRoo.Services;

namespace EngeneerLenRoo.Models
{
    public class Technique
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TypeTechnique TypeTechnique { get; set; }
        public long InventoryNumber { get; set; }
        
    }
}