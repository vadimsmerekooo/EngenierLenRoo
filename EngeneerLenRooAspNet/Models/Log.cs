using System;

namespace EngeneerLenRooAspNet.Models
{
    public class Log
    {
        public DateTime DateTime { get; set; }
        public string Type { get; set; }
        public string Method { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Action { get; set; }
        public string GetType()
        {
            switch (Type)
            {
                case "ERROR":
                    return "danger";
                case "WARN":
                    return "warning";
                case "DEBUG":
                    return "light";
                case "INFO":
                    return "info";
                default:
                    return "light";
            }
        }
    }
}
