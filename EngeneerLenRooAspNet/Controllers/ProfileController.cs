using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private MainContext _context;
        public ProfileController(UserManager<IdentityUser> userManager, MainContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [Route("profile")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Chat");
            }
            var employee = await _context.Employees.Include(c => c.Cabinet).Include(t => t.Techniques).Include(c => c.Cartridges).ThenInclude(c => c.Case).FirstOrDefaultAsync(e => e.Id == user.Id);
            if (employee == null && employee.Cabinet == null)
            {
                return RedirectToAction("Index", "Chat");
            }

            ProfileViewModel model = new ProfileViewModel()
            {
                User = user,
                Employee = employee
            };
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [Route("profile/user_profile")]
        public async Task<IActionResult> UserProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Chat");
            }
            var employee = await _context.Employees.Include(c => c.Cabinet).Include(t => t.Techniques).Include(c => c.Cartridges).ThenInclude(c => c.Case).FirstOrDefaultAsync(e => e.Id == user.Id);
            if (employee == null && employee.Cabinet == null)
            {
                return RedirectToAction("Index", "Chat");
            }

            ProfileViewModel model = new ProfileViewModel()
            {
                User = user,
                Employee = employee
            };
            return View(model);
        }
    }
}
