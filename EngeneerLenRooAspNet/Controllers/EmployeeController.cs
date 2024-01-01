using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmployeeController : Controller
    {
        private MainContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public EmployeeController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, MainContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("employee/create")]
        public async Task<IActionResult> Create(string cabinetId)
        {
            if (!await _context.Cabinets.AnyAsync(id => id.Id == cabinetId))
            {
                return RedirectToAction("Info", "Home", new { id = cabinetId });
            }

            Employee employee = new Employee()
            {
                Cabinet = await _context.Cabinets.FirstOrDefaultAsync(id => id.Id == cabinetId)
            };

            return View(employee);
        }

        [Route("employee/createmodel")]
        [HttpPost]
        public async Task<IActionResult> CreateModel(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var isValid = IsValidFormEmployeeAsync(employee).Result;
                if (isValid is IActionResult)
                {
                    return RedirectToAction("Info", "Home", new { id = employee.Cabinet.Id });
                }

                employee.Cabinet = await _context.Cabinets.FirstOrDefaultAsync(cab => cab.Id == employee.CabinetId);

                if (((List<string>)isValid).Count == 0)
                {
                    var user = new IdentityUser { UserName = employee.Fio, Email = employee.Fio, EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(user, $"centr_edu");
                    if (result.Succeeded)
                    {
                        List<string> role = new List<string>() { "user" };
                        await _userManager.AddToRolesAsync(user, role);
                        var userId = await _userManager.FindByNameAsync(employee.Fio);
                        if (userId == null)
                        {
                            foreach (string error in ((List<string>)isValid))
                                ModelState.AddModelError("Ошибка привязки Id сотрудника.", error);
                        }
                        else
                        {
                            employee.Id = userId.Id;
                            await _context.Employees.AddAsync(employee);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Info", "Home", new { id = employee.CabinetId });
                        }
                    }
                    else
                    {
                        foreach (string error in ((List<string>)isValid))
                            ModelState.AddModelError("Ошибка создания сотрудника.", error);
                    }
                }

                foreach (string error in ((List<string>)isValid))
                    ModelState.AddModelError("", error);

                return View(nameof(Create), employee);
            }

            ModelState.AddModelError("", "Форма не заполнена!");
            return View(nameof(Create), employee);
        }

        [Route("employee/edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (!await _context.Employees.AnyAsync(emp => emp.Id == id))
            {
                return NotFound();
            }

            Employee employee = await _context.Employees
                .Include(cab => cab.Cabinet)
                .FirstOrDefaultAsync(emp => emp.Id == id);

            return View(employee);
        }

        [Route("employee/editmodel")]
        public async Task<IActionResult> EditModel(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Cabinet = await _context.Cabinets.FirstOrDefaultAsync(cab => cab.Id == employee.CabinetId);
                var user = await _userManager.FindByIdAsync(employee.Id);
                if (user != null)
                {
                    var resultChangeEmail = await _userManager.SetEmailAsync(user, employee.Fio);
                    var resultChnageName = await _userManager.SetUserNameAsync(user, employee.Fio);
                    if (resultChangeEmail.Succeeded && resultChnageName.Succeeded)
                    {
                        _context.Employees.Update(employee);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Info", "Home", new { id = employee.CabinetId });
                    }
                    else
                        ModelState.AddModelError("", "Ошибка изменения информации авторизации!");

                }
                else
                    ModelState.AddModelError("", "Ошибка изменения информации! Авторизация пользователя не найдена.");
            }

            ModelState.AddModelError("", "Форма не заполнена!");
            return View(nameof(Edit), employee);
        }


        [Route("employee/change-cabinet")]
        public async Task<IActionResult> ChangeCabinet(string id)
        {
            if (!await _context.Employees.AnyAsync(emp => emp.Id == id))
                return NotFound();

            Employee employee = await _context.Employees.Include(cab => cab.Cabinet)
                .FirstOrDefaultAsync(emp => emp.Id == id);

            EmployeeChangeCabinetViewModel viewModel = new EmployeeChangeCabinetViewModel()
            {
                Cabinets = await _context.Cabinets.Where(cab => cab.Id != employee.CabinetId).OrderBy(cab => cab.Name)
                    .ToListAsync(),
                Employee = employee
            };

            return View(viewModel);
        }

        [Route("employee/change-cabinet-model")]
        [HttpPost]
        public async Task<IActionResult> ChangeCabinetModel(EmployeeChangeCabinetViewModel model)
        {
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == model.SelectCabinetId) ||
                !await _context.Employees.AnyAsync(emp => emp.Id == model.Employee.Id))
                return NotFound();

            Cabinet newCabinet = await _context.Cabinets.Include(emp => emp.Employees)
                .FirstOrDefaultAsync(cab => cab.Id == model.SelectCabinetId);

            Employee employee = await _context.Employees.Include(th => th.Techniques)
                .FirstOrDefaultAsync(emp => emp.Id == model.Employee.Id);

            Cabinet oldCabinet = await _context.Cabinets.Include(emp => emp.Employees)
                .FirstOrDefaultAsync(cab => cab.Id == model.Employee.CabinetId);

            if (!model.IsWithTechniques && employee.Techniques.Count != 0)
            {

                Employee newEmployee = new Employee()
                {
                    Fio = "Свободный стол",
                    Techniques = employee.Techniques
                };
                oldCabinet.Employees.Add(newEmployee);
                employee.Techniques = new List<Technique>();
            }


            if (newCabinet.Employees.Any(emp => emp.Fio == "Свободный стол"))
            {
                Employee changeEmployee = newCabinet.Employees.FirstOrDefault(emp => emp.Fio == "Свободный стол");
                newCabinet.Employees.Remove(changeEmployee);
                if (changeEmployee != null)
                {
                    changeEmployee.Fio = employee.Fio;
                    changeEmployee.Techniques.AddRange(employee.Techniques);
                    newCabinet.Employees.Add(changeEmployee);
                }
                oldCabinet.Employees.Remove(employee);

            }
            else
            {
                newCabinet.Employees.Add(employee);
                oldCabinet.Employees.Remove(employee);
            }

            _context.Cabinets.UpdateRange(newCabinet, oldCabinet);
            await _context.SaveChangesAsync();

            return RedirectToAction("Info", "Home", new { id = model.SelectCabinetId });
        }


        [Route("employee/delete/{id}/{cabinetId}")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id, string cabinetId)
        {
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == cabinetId))
                return NotFound();


            if (await _context.Employees.AnyAsync(emp => emp.Id == id))
            {
                Employee employee = await _context.Employees
                    .Include(cab => cab.Cabinet)
                    .Include(th => th.Techniques)
                    .FirstOrDefaultAsync(emp => emp.Id == id);
                var user = await _userManager.FindByIdAsync(id);
                if (user != null && employee != null)
                {
                    _context.Techniques.RemoveRange(employee.Techniques);
                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();
                    await _userManager.DeleteAsync(user);
                }
            }

            return RedirectToAction("Info", "Home", new { id = cabinetId });
        }

        private async Task<object> IsValidFormEmployeeAsync(Employee employee)
        {
            List<string> errorsModel = new List<string>();
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == employee.CabinetId))
            {
                return RedirectToAction("Info", "Home", new { id = employee.CabinetId });
            }

            if (await _context.Employees.AnyAsync(emp => emp.Fio == employee.Fio.Trim() && emp.CabinetId != null))
                errorsModel.Add($"Сотрудник {employee.Fio} есть в системе!");
            else
            {
                List<Employee> employees =
                    await _context.Employees.Where(emp => emp.Fio == employee.Fio.Trim() && emp.CabinetId == null)
                        .ToListAsync();
                _context.Employees.RemoveRange(employees);
                await _context.SaveChangesAsync();
            }

            return errorsModel;
        }
    }
}