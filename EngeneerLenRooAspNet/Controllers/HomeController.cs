using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using EngeneerLenRooAspNet.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.Services;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MainContext _context;

        public HomeController(ILogger<HomeController> logger, MainContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("")]
        [Route("cabinets")]
        public async Task<IActionResult> Index() => View(
            await _context.Cabinets
                .Include(s => s.Employees)
                .ThenInclude(t => t.Techniques)
                .OrderBy(c => c.Name)
                .ToListAsync());

        [Route("cabinets/search")]
        public async Task<IActionResult> Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return RedirectToAction(nameof(Index));

            ViewData["Search"] = true;
            ViewData["SearchString"] = search;
            List<Cabinet> result = await _context.Cabinets
                .Include(s => s.Employees)
                .ThenInclude(t => t.Techniques)
                .ToListAsync();


            return View(nameof(Index),
                result.Where(cab => cab.Name.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase)).ToList());
        }

        [Route("cabinet/create")]
        public IActionResult Create()
        {
            return View();
        }


        [Route("cabinet/create")]
        [HttpPost]
        public async Task<IActionResult> Create(Cabinet cabinet)
        {
            if (ModelState.IsValid)
            {
                cabinet.Name = cabinet.Name.Trim();
                if (await _context.Cabinets.AnyAsync(n => n.Name == cabinet.Name))
                {
                    ModelState.AddModelError("", "Кабинет с данным номером добавлен!");
                    return View(cabinet);
                }

                await _context.Cabinets.AddAsync(cabinet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { });
            }

            ModelState.AddModelError("", "Форма не заполнена!");
            return View(cabinet);
        }

        [Route("cabinet/info")]
        public async Task<IActionResult> Info(string id)
        {
            if (!await _context.Cabinets.AnyAsync(cabinetId => cabinetId.Id == id))
            {
                return NotFound();
            }

            Cabinet cabinet = await _context.Cabinets
                .Include(s => s.Employees)
                .ThenInclude(th => th.Techniques)
                .FirstOrDefaultAsync(cabinetId => cabinetId.Id == id);
            return View(cabinet);
        }

        [Route("cabinet/report-technique")]
        public async Task<IActionResult> ReportCabinetTechnique(string id)
        {
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == id))
                return NotFound();

            Cabinet cabinet = await _context.Cabinets.Include(cab => cab.Employees).ThenInclude(emp => emp.Techniques).ThenInclude(th => th.Employee)
                .FirstOrDefaultAsync(cab => cab.Id == id);

            List<Technique> techniques = new List<Technique>();
            foreach (Employee employee in cabinet.Employees)
                techniques.AddRange(employee.Techniques);

            List<ReportEmployeeTechnique> employeeTechniques = new List<ReportEmployeeTechnique>();

            foreach (Employee employee in cabinet.Employees)
            {
                employeeTechniques.Add(new ReportEmployeeTechnique()
                {
                    Fio = employee.Fio,
                    CabinetTechniques = new List<ReportCabinetTechnique>()
                    {
                        new ReportCabinetTechnique("Системных блоков", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Pc)),
                        new ReportCabinetTechnique("Мониторов", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Monitor)),
                        new ReportCabinetTechnique("Мышек", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Mouse)),
                        new ReportCabinetTechnique("Клавиатур", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Keyboard)),
                        new ReportCabinetTechnique("Принтеров", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Printer)),
                        new ReportCabinetTechnique("ИБП", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Ups)),
                        new ReportCabinetTechnique("Сетевых фильтров", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Filter)),
                        new ReportCabinetTechnique("Модемов", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Modem)),
                        new ReportCabinetTechnique("Другое", employee.Techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Other)),
                    }
                });
            }



            ReportCabinetTechniqueViewModel viewModel = new ReportCabinetTechniqueViewModel()
            {
                Cabinet = cabinet,
                CabinetTechnique = new List<ReportCabinetTechnique>()
                {
                    new ReportCabinetTechnique("Системных блоков", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Pc)),
                    new ReportCabinetTechnique("Мониторов", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Monitor)),
                    new ReportCabinetTechnique("Мышек", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Mouse)),
                    new ReportCabinetTechnique("Клавиатур", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Keyboard)),
                    new ReportCabinetTechnique("Принтеров", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Printer)),
                    new ReportCabinetTechnique("ИБП", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Ups)),
                    new ReportCabinetTechnique("Сетевых фильтров", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Filter)),
                    new ReportCabinetTechnique("Модемов", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Modem)),
                    new ReportCabinetTechnique("Другое", techniques.FindAll(th => th.TypeTechnique == TypeTechnique.Other)),
                },
                EmployeeTechniques = employeeTechniques
            };
            return View(viewModel);
        }


        [Route("cabinet/employee/search")]
        public async Task<IActionResult> SearchInCab(string id, string search)
        {
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == id))
            {
                return NotFound();
            }

            Cabinet cabinet = await _context.Cabinets
                .Include(emp => emp.Employees)
                .ThenInclude(th => th.Techniques)
                .FirstOrDefaultAsync(cab => cab.Id == id);

            if (string.IsNullOrWhiteSpace(search))
                return RedirectToAction(nameof(Info), cabinet);

            ViewData["Search"] = true;
            ViewData["SearchString"] = search;

            try
            {
                List<Employee> employees = cabinet.Employees
                    .Where(emp
                        => IsContains(emp.Fio, search)
                           || IsContains(emp.IpComputer, search)
                           || IsContains(emp.UserMap, search)
                           || IsContains(emp.NumberPcMap, search)
                           || emp.Techniques.Any(th =>
                               IsContains(th.InventoryNumber, search)
                               || IsContains(th.Name, search))).ToList();

                cabinet.Employees = employees;
                return View(nameof(Info), cabinet);
            }
            catch
            {
                return View(nameof(Info), cabinet);
            }
        }

        [Route("cabinets/search/extended")]
        public IActionResult SearchExtended()
        {
            return View(new SearchExtendedViewModel());
        }

        [Route("cabinets/search/extended/model")]
        [HttpPost]
        public async Task<IActionResult> SearchExtendedModel(SearchExtendedViewModel model)
        {
            if (model.IsNull())
                return View(nameof(SearchExtended), model);

            List<Cabinet> databaseCabinetsResult = await _context.Cabinets
                .Include(cab => cab.Employees)
                .ThenInclude(emp => emp.Techniques)
                .ToListAsync();

            // List<Cabinet> searchResult = databaseCabinetsResult
            //     .Where(cab => IsContainsFormSearchExtendedParams(cab.Name, model.CabinetFormModel.Name) != TypeSearchEqualsParams.IsNull
            //     || IsEqualsNumbers(cab.Phone, model.CabinetFormModel.Phone) != TypeSearchEqualsParams.IsNull
            //     && cab.Employees.Any(emp => IsContainsFormSearchExtendedParams(emp.Fio, model.EmployeeFormModel.Fio)!= TypeSearchEqualsParams.IsNull
            //     || IsContainsFormSearchExtendedParams(emp.UserMap, model.EmployeeFormModel.UserMap)!= TypeSearchEqualsParams.IsNull
            //     || IsEqualsNumbers(emp.IpComputer, model.EmployeeFormModel.IpComputer)!= TypeSearchEqualsParams.IsNull
            //     || IsContainsFormSearchExtendedParams(emp.NumberPcMap, model.EmployeeFormModel.NumberPcMap)!= TypeSearchEqualsParams.IsNull
            //     && emp.Techniques.Any(th => IsContainsFormSearchExtendedParams(th.Name, model.TechniqueFormModel.Name)!= TypeSearchEqualsParams.IsNull
            //     || IsEqualsNumbers(th.InventoryNumber, model.TechniqueFormModel.InventoryNumber)!= TypeSearchEqualsParams.IsNull
            //     || model.TechniqueFormModel.TypeTechnique != TypeTechnique.All ? th.TypeTechnique == model.TechniqueFormModel.TypeTechnique : false)))
            //     .ToList();

            List<Cabinet> searchCabResult = databaseCabinetsResult
                .Where(cab
                    => model.CabinetFormModel.IsNull() || (IsOne(cab.Name, model.CabinetFormModel.Name) == TypeSearchEqualsParams.Found
                                                           && IsTwo(cab.Phone, model.CabinetFormModel.Phone) == TypeSearchEqualsParams.Found
                                                           || IsOne(cab.Name, model.CabinetFormModel.Name) == TypeSearchEqualsParams.IsNull
                                                           && IsTwo(cab.Phone, model.CabinetFormModel.Phone) == TypeSearchEqualsParams.Found
                                                           || IsOne(cab.Name, model.CabinetFormModel.Name) == TypeSearchEqualsParams.Found
                                                           && IsTwo(cab.Phone, model.CabinetFormModel.Phone) == TypeSearchEqualsParams.IsNull)
                )
                .ToList();

            foreach (Cabinet cabinet in searchCabResult)
            {
                cabinet.Employees = cabinet.Employees.Where(emp =>
                    model.EmployeeFormModel.IsNull()
                    || (!(IsOne(emp.Fio, model.EmployeeFormModel.Fio) != TypeSearchEqualsParams.IsNull && IsOne(emp.Fio, model.EmployeeFormModel.Fio) == TypeSearchEqualsParams.NotFound)
                        && !(IsTwo(emp.IpComputer, model.EmployeeFormModel.IpComputer) != TypeSearchEqualsParams.IsNull && IsTwo(emp.IpComputer, model.EmployeeFormModel.IpComputer) == TypeSearchEqualsParams.NotFound)
                        && !(IsTwo(emp.NumberPcMap, model.EmployeeFormModel.NumberPcMap) != TypeSearchEqualsParams.IsNull && IsTwo(emp.NumberPcMap, model.EmployeeFormModel.NumberPcMap) == TypeSearchEqualsParams.NotFound)
                        && !(IsOne(emp.UserMap, model.EmployeeFormModel.UserMap) != TypeSearchEqualsParams.IsNull && IsOne(emp.UserMap, model.EmployeeFormModel.UserMap) == TypeSearchEqualsParams.NotFound)))
                    .ToList();

                foreach (Employee employee in cabinet.Employees)
                {
                    employee.Techniques = employee.Techniques.Where(th =>
                            model.TechniqueFormModel.IsNull()
                            || !((IsOne(th.Name, model.TechniqueFormModel.Name) != TypeSearchEqualsParams.IsNull
                            && IsOne(th.Name, model.TechniqueFormModel.Name) == TypeSearchEqualsParams.NotFound)
                            || !(IsTwo(th.InventoryNumber, model.TechniqueFormModel.InventoryNumber) != TypeSearchEqualsParams.IsNull
                            && IsTwo(th.InventoryNumber, model.TechniqueFormModel.InventoryNumber) == TypeSearchEqualsParams.Found)
                            && !(model.TechniqueFormModel.TypeTechnique != TypeTechnique.All && th.TypeTechnique == model.TechniqueFormModel.TypeTechnique)))
                        .ToList();
                }
            }

            if (model.TechniqueFormModel.IsNull())
            {
                model.Cabinets = searchCabResult.Where(cab => cab.Employees.Count > 0).ToList();
            }
            else
            {
                model.Cabinets = searchCabResult.Where(cab => cab.Employees.Count > 0 && cab.Employees.Any(emp => emp.Techniques.Count != 0)).ToList();
            }

            return View(nameof(SearchExtended), model);
        }


        [Route("cabinet/edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == id))
            {
                return RedirectToAction(nameof(Index));
            }

            Cabinet cabinet = await _context.Cabinets
                .Include(s => s.Employees)
                .ThenInclude(th => th.Techniques)
                .FirstOrDefaultAsync(cab => cab.Id == id);
            return View(cabinet);
        }

        [Route("cabinet/edit/model")]
        public async Task<IActionResult> EditModel(Cabinet cabinet)
        {
            if (ModelState.IsValid)
            {
                cabinet.Name = cabinet.Name.Trim();
                _context.Cabinets.Update(cabinet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Info), new { id = cabinet.Id });
            }

            ModelState.AddModelError("", "Форма не заполнена!");
            return View(nameof(Edit), cabinet);
        }


        [Route("cabinet/delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await _context.Cabinets.AnyAsync(cabinetId => cabinetId.Id == id))
            {
                return RedirectToAction(nameof(Index));
            }

            Cabinet cabinet = await _context.Cabinets
                .Include(e => e.Employees)
                .ThenInclude(t => t.Techniques)
                .ThenInclude(e => e.Employee)
                .FirstOrDefaultAsync(cabinetId => cabinetId.Id == id);

            foreach (Employee employee in cabinet.Employees)
            {
                _context.Techniques.RemoveRange(employee.Techniques);
            }

            _context.Employees.RemoveRange(cabinet.Employees);
            _context.Cabinets.Remove(cabinet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Route("info-071-check")]
        public async Task<IActionResult> Report071Check()
        {

            Report071CheckViewModel viewModel = new Report071CheckViewModel();

            List<Technique> techniques =
                await _context.Techniques
                    .Include(th => th.Employee)
                    .ThenInclude(emp => emp.Cabinet)
                    .Where(th => th.InventoryNumber == 71)
                    .ToListAsync();

            foreach (Technique technique in techniques)
            {
                if (technique.TypeTechnique != TypeTechnique.Modem)
                {
                    if (viewModel.Checks.Any(th => th.TypeTechnique == technique.TypeTechnique && th.Cabinets.Any(cab => cab.Name == technique.Employee.Cabinet.Name)))
                    {
                        if (viewModel.Checks.Any(th => th.Cabinets.Any(cab => cab.Name == technique.Employee.Cabinet.Name)))
                        {
                            viewModel.Checks.Where(th => th.TypeTechnique == technique.TypeTechnique).Any(th =>
                                th.Cabinets.FirstOrDefault(cab => cab.Name == technique.Employee.Cabinet.Name).Counter());
                        }
                        else
                        {
                            viewModel.Checks.FirstOrDefault(th => th.TypeTechnique == technique.TypeTechnique)?.Cabinets.Add(new Report071CheckCabinet()
                            {
                                Name = technique.Employee.Cabinet.Name,
                                CabinetId = technique.Employee.CabinetId
                            });
                        }
                    }
                    else
                    {
                        viewModel.Checks.Add(new Report071Check()
                        {
                            Name = technique.Name,
                            TypeTechnique = technique.TypeTechnique,
                            Cabinets = new List<Report071CheckCabinet>()
                            {
                                new()
                                {
                                    Name = technique.Employee.Cabinet.Name,
                                    CabinetId = technique.Employee.CabinetId
                                }
                            }
                        });
                    }
                }
            }


            viewModel.Checks = viewModel.Checks.OrderBy(x => x.Name).ToList();

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private bool IsContains(object firstParm, string search)
        {
            return firstParm != null && firstParm.ToString().Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        private TypeSearchEqualsParams IsOne(object firstParams, object secondParams)
        {
            if (secondParams is null || firstParams is null ||
                String.IsNullOrWhiteSpace(secondParams.ToString() ?? string.Empty))
                return TypeSearchEqualsParams.IsNull;

            return firstParams.ToString()
                .Contains(secondParams.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                ? TypeSearchEqualsParams.Found
                : TypeSearchEqualsParams.NotFound;
        }

        private TypeSearchEqualsParams IsTwo(long? firstParams, long? secondParams)
        {
            if (firstParams is null || secondParams is null || secondParams == 0 || firstParams == 0)
                return TypeSearchEqualsParams.IsNull;

            return firstParams == secondParams ? TypeSearchEqualsParams.Found : TypeSearchEqualsParams.NotFound;
        }
    }
}