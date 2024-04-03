using EngeneerLenRooAspNet.Models;
using System;
using System.Collections.Generic;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class LogViewModel
    {
        public DateTime DateTime { get; set; }
        public List<Log> Logs { get; set; } = new List<Log>();
    }
}
