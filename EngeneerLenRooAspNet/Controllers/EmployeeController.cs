using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private MainContext _context;

        public EmployeeController(MainContext context)
        {
            _context = context;
        }

        [Route("employee/create")]
        public async Task<IActionResult> Create(string cabinetId)
        {
            if (!await _context.Cabinets.AnyAsync(id => id.Id == cabinetId))
            {
                return RedirectToAction("Info", "Home", new {id = cabinetId});
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
                    return RedirectToAction("Info", "Home", new {id = employee.Cabinet.Id});
                }

                employee.Cabinet = await _context.Cabinets.FirstOrDefaultAsync(cab => cab.Id == employee.CabinetId);

                if (((List<string>) isValid).Count == 0)
                {
                    await _context.Employees.AddAsync(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Info", "Home", new {id = employee.CabinetId});
                }

                foreach (string error in ((List<string>) isValid))
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

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Info", "Home", new {id = employee.CabinetId});
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
                    changeEmployee.IpComputer = employee.IpComputer;
                    changeEmployee.UserMap = employee.UserMap;
                    changeEmployee.NumberPcMap = employee.NumberPcMap;
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

            return RedirectToAction("Info", "Home", new {id = model.SelectCabinetId});
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
                _context.Techniques.RemoveRange(employee.Techniques);
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction("Info", "Home", new {id = cabinetId});
            }

            return RedirectToAction("Info", "Home", new {id = cabinetId});
        }

        private async Task<object> IsValidFormEmployeeAsync(Employee employee)
        {
            List<string> errorsModel = new List<string>();
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == employee.CabinetId))
            {
                return RedirectToAction("Info", "Home", new {id = employee.CabinetId});
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

            if (employee.IpComputer != 0 && employee.IpComputer > 255)
            {
                employee.IpComputer = 255;
                errorsModel.Add("Ip адрес превышает допустимое значение!");
            }

            if (await _context.Employees.AnyAsync(emp => emp.IpComputer == employee.IpComputer))
            {
                errorsModel.Add("Конфликт Ip адресов. Данный адрес пренадлежит другому компьютеру!");
            }

            if (employee.NumberPcMap != null && employee.NumberPcMap != 0 &&
                await _context.Employees.AnyAsync(emp => emp.NumberPcMap == employee.NumberPcMap))
            {
                errorsModel.Add($"Номер компьютера МАП {employee.NumberPcMap} зарегистрирован в системе!");
            }

            if (!string.IsNullOrWhiteSpace(employee.UserMap) &&
                await _context.Employees.AnyAsync(emp => emp.UserMap == employee.UserMap))
            {
                errorsModel.Add($"Пользователь МАП{employee.UserMap} зарегистрирован в системе!");
            }

            return errorsModel;
        }
    }
}