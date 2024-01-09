using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class ChatViewModel
    {
        public Employee User { get; set; }
        public Employee UserDirect { get; set; }
        public Chat ChatActive { get; set; }
        public bool IsAllMessageLoad { get; set; }
        public int CountMessageLoad { get; set; }
        public string Search { get; set; }
        public List<Chat> Chats { get; set; } = new List<Chat>();
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Cabinet> Cabinets { get; set;} = new List<Cabinet>();
    }
}
