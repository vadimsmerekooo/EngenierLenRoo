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
        public string GetSizeToString()
        {
            if(Size / 1000 > 1000)
            {
                return $"{Math.Round(Size / 1000000, 1)} Мб";
            }
            else
            {
                return $"{Math.Round(Size / 1000, 0)} Кб";
            }
        }
    }
    public enum TypeFile
    {
        image, text
    }
}
