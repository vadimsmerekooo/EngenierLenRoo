using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.Services;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize]
    public class TechniqueController : Controller
    {
        private MainContext _context;

        public TechniqueController(MainContext context)
        {
            _context = context;
        }


        [Route("technique/{empId}")]
        public async Task<IActionResult> Index(string empId)
        {
            if (!await _context.Employees.AnyAsync(emp => emp.Id == empId))
            {
                return NotFound();
            }

            Employee employee = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == empId);
            Cabinet cabinet = await _context.Cabinets.FirstOrDefaultAsync(cab => cab.Id == employee.CabinetId);

            TechniquesViewModel viewModel = new TechniquesViewModel()
            {
                Cabinet = cabinet,
                Employee = employee,
                EmployeeId = employee.Id
            };

            return View(viewModel);
        }

        [Route("technique/create/{empId}/{typeTechnique}")]
        public async Task<IActionResult> Create(string empId, TypeTechnique typeTechnique)
        {
            if (!await _context.Employees.AnyAsync(emp => emp.Id == empId))
            {
                return NotFound();
            }

            Employee employee = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == empId);
            Cabinet cabinet = await _context.Cabinets.FirstOrDefaultAsync(cab => cab.Id == employee.CabinetId);

            TechniquesViewModel viewModel = new TechniquesViewModel()
            {
                Cabinet = cabinet,
                Employee = employee,
                EmployeeId = employee.Id,
                Technique = new Technique()
                {
                    TypeTechnique = typeTechnique
                }
            };

            return View(viewModel);
        }

        [Route("technique/createmodel")]
        [HttpPost]
        public async Task<IActionResult> CreateModel(TechniquesViewModel model)
        {
            if (!await _context.Employees.AnyAsync(emp => emp.Id == model.EmployeeId))
            {
                return NotFound();
            }

            Employee employee = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == model.EmployeeId);
            Cabinet cabinet = await _context.Cabinets.FirstOrDefaultAsync(cab => cab.Id == employee.CabinetId);

            if (ModelState.IsValid)
            {
                if (model.Technique.InventoryNumber == 0)
                {
                    model.Technique.InventoryNumber = 071;
                }

                employee.Techniques.Add(model.Technique);
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Info", "Home", new {id = employee.CabinetId});
            }


            TechniquesViewModel viewModel = new TechniquesViewModel()
            {
                Cabinet = cabinet,
                Employee = employee,
                EmployeeId = model.EmployeeId,
                Technique = model.Technique
            };
            return View(nameof(Create), viewModel);
        }

        [Route("technique/edit/{id}/{empId}")]
        public async Task<IActionResult> Edit(string id, string empId)
        {
            if (!await _context.Techniques.AnyAsync(t => t.Id == id))
            {
                return NotFound();
            }

            Employee employee = await _context.Employees.Include(t => t.Techniques)
                .FirstOrDefaultAsync(emp => emp.Id == empId);
            Cabinet cabinet = await _context.Cabinets.FirstOrDefaultAsync(cab => cab.Id == employee.CabinetId);
            Technique technique = employee.Techniques.FirstOrDefault(t => t.Id == id);

            TechniquesViewModel viewModel = new TechniquesViewModel()
            {
                Cabinet = cabinet,
                Employee = employee,
                EmployeeId = employee.Id,
                Technique = technique
            };


            return View(viewModel);
        }

        [Route("technique/edit/model")]
        [HttpPost]
        public async Task<IActionResult> EditModel(TechniquesViewModel model)
        {
            if (!await _context.Employees.AnyAsync(emp => emp.Id == model.EmployeeId))
            {
                return NotFound();
            }

            Employee employee = await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == model.EmployeeId);
            Cabinet cabinet = await _context.Cabinets.FirstOrDefaultAsync(cab => cab.Id == employee.CabinetId);

            if (ModelState.IsValid)
            {
                if (model.Technique.InventoryNumber == 0)
                {
                    model.Technique.InventoryNumber = 071;
                }

                _context.Techniques.Update(model.Technique);
                await _context.SaveChangesAsync();
                return RedirectToAction("Info", "Home", new {id = employee.CabinetId});
            }


            TechniquesViewModel viewModel = new TechniquesViewModel()
            {
                Cabinet = cabinet,
                Employee = employee,
                EmployeeId = model.EmployeeId,
                Technique = model.Technique
            };
            return View(nameof(Create), viewModel);
        }


        [Route("technique/change-employee/{id}")]
        public async Task<IActionResult> ChangeEmployee(string id)
        {
            if (!await _context.Techniques.AnyAsync(th => th.Id == id))
                return NotFound();

            Technique technique =
                await _context.Techniques.Include(emp => emp.Employee).ThenInclude(cab => cab.Cabinet)
                    .FirstOrDefaultAsync(th => th.Id == id);

            Employee employee = technique.Employee;
            Cabinet cabinet = technique.Employee.Cabinet;

            List<Cabinet> cabinets = await _context.Cabinets
                .Where(cab => cab.Employees.Count > 0)
                .OrderBy(cab => cab.Name)
                .ToListAsync();

            TechniqueChangeEmployeeViewModel viewModel = new TechniqueChangeEmployeeViewModel()
            {
                Cabinets = cabinets,
                Employee = employee,
                Technique = technique,
                SelectCabinetId = cabinet.Id,
                SelectEmployeeId = employee.Id
            };

            return View(viewModel);
        }

        [Route("technique/change-technique/select-employee")]
        [HttpPost]
        public async Task<IActionResult> ChangeTechniqueSelectEmployee(TechniqueChangeEmployeeViewModel model)
        {
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == model.SelectCabinetId) ||
                !await _context.Techniques.AnyAsync(th => th.Id == model.Technique.Id) || 
                !await _context.Employees.AnyAsync(emp => emp.Id == model.Employee.Id))
            {
                return NotFound();
            }

            Cabinet selectCabinet = await _context.Cabinets
                .Include(cab => cab.Employees)
                .FirstOrDefaultAsync(cab => cab.Id == model.SelectCabinetId);

            Employee employee = await _context.Employees.Include(cab =>cab.Cabinet).FirstOrDefaultAsync(emp => emp.Id == model.Employee.Id);
            Technique technique = await _context.Techniques.FirstOrDefaultAsync(th => th.Id == model.Technique.Id);

            TechniqueChangeEmployeeViewModel viewModel = new TechniqueChangeEmployeeViewModel()
            {
                SelectCabinet = selectCabinet,
                Employee = employee,
                Technique = technique,
            };
            
            return View(viewModel);
        }

        [Route("technique/change-employee-model")]
        [HttpPost]
        public async Task<IActionResult> ChangeEmployeeModel(TechniqueChangeEmployeeViewModel model)
        {
            if (!await _context.Employees.AnyAsync(emp => emp.Id == model.SelectEmployeeId) ||
                !await _context.Techniques.AnyAsync(th => th.Id == model.Technique.Id))
                return NotFound();

            Technique technique = await _context.Techniques
                .FirstOrDefaultAsync(th => th.Id == model.Technique.Id);
            technique.EmployeeId = model.SelectEmployeeId;

            _context.Techniques.Update(technique);
            await _context.SaveChangesAsync();

            return RedirectToAction("Info", "Home", new {id = model.SelectCabinetId});
        }


        [Route("technique/delete/{id}/{cabId}")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id, string cabId)
        {
            if (!await _context.Techniques.AnyAsync(th => th.Id == id))
            {
                return RedirectToAction("Info", "Home", new {id = cabId});
            }

            Technique technique = await _context.Techniques.FirstOrDefaultAsync(th => th.Id == id);
            _context.Techniques.Remove(technique);
            await _context.SaveChangesAsync();
            return RedirectToAction("Info", "Home", new {id = cabId});
        }
    }
}