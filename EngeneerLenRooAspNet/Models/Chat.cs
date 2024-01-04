using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class Chat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TypeChat TypeChat { get; set; }
        public virtual List<Employee> ChatUsers { get; set; } = new List<Employee>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
    public enum TypeChat
    {
        Direct,
        Group
    }
}
