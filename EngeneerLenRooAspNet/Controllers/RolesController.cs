using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        MainContext _context;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, MainContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        [Route("/roles/list")]
        public IActionResult Index() => View(_roleManager.Roles.ToList());

        [Route("/roles/create")]
        public IActionResult Create() => View();
        [Route("/roles/create")]
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        [Route("/users/list")]
        public IActionResult UserList() => View(_userManager.Users.ToList());

        [Route("/users/roles/edit")]
        public async Task<IActionResult> Edit(string userId)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [Route("/users/roles/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == user.Id);
            if (user != null && employee != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                if (roles.Contains("admin"))
                {
                    employee.Post = Employee.TypePost.Администратор;
                    _context.Employees.Update(employee);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
