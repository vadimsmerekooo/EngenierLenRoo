using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class Case
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long? NumberAct { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateSend { get; set; }
        public DateTime? DateGet { get; set; }
        public string Description { get; set; }
        public List<Cartridge> Cartridge { get; set; }
    }
}
