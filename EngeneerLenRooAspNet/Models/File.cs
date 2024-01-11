using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class File
    {
        public string Id { get; set; }
        public string OriginalName { get; set; }
        public string Path { get; set; }
        public double Size { get; set; }
        public TypeFile TypeFile { get; set; }
    }
    public enum TypeFile
    {
        image, text
    }
}
