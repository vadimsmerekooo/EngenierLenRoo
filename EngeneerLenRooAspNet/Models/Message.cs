using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngeneerLenRooAspNet.Models
{
    public class Message
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        public string PathFile { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public Employee User { get; set; }
        public Chat Chat { get; set; }
        public StatusMessage Status { get; set; }
    }
    public enum StatusMessage
    {
        NotRead,
        Read,
        ErrorSend
    }
}
