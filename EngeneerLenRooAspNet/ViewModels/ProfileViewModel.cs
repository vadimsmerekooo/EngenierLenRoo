using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.Identity;

namespace EngeneerLenRooAspNet.ViewModels
{
    public class ProfileViewModel
    {
        public IdentityUser User { get; set; }
        public Employee Employee { get; set; }
    }
}
