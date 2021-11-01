using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize(Roles = "Программист")]
    public class UsersController : Controller
    {
        UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        [Route("users")]
        public IActionResult Index() => View(_userManager.Users.ToList());

        [Route("users/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if(!await _userManager.Users.AnyAsync(u => u.Id == id))
                return RedirectToAction(nameof(Index));

            IdentityUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
        
    }
}