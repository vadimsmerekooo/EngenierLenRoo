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
        public File File { get; set; }
        public Employee User { get; set; }
        public Chat Chat { get; set; }
        public StatusMessage Status { get; set; }
        public string GetTypeMessageMessageBox(Employee userDirect)
        {
            string result = "";
            if(this.User == userDirect)
            {
                result += "Вы: ";
            }
            else
            {
                result += $"{User.GetShortFio()}: ";
            }
            if(File != null && Text == "")
            {
                switch (File.TypeFile)
                {
                    case TypeFile.image:
                        result += "Изображение";
                        break;
                    case TypeFile.text:
                        result += "Файл";
                        break;
                }
            }
            else
            {
                result += Text;
            }
            return result;
        }
    }
    public enum StatusMessage
    {
        NotRead,
        Read,
        ErrorSend
    }
}
