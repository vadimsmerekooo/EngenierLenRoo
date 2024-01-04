using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EngeneerLenRooAspNet.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.Services;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MainContext _context;

        public HomeController(ILogger<HomeController> logger, MainContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("cabinets")]
        public async Task<IActionResult> Index() => View(
                await _context.Cabinets
                    .Include(s => s.Employees)
                    .ThenInclude(t => t.Techniques)
                    .OrderBy(c => c.Name)
                    .ToListAsync());
        //{
        //    if (await _context.Users.FirstOrDefaultAsync(c => c.Email == User.Identity.Name) != null)
        //    {
        //        return View(
        //        await _context.Cabinets
        //            .Include(s => s.Employees)
        //            .ThenInclude(t => t.Techniques)
        //            .OrderBy(c => c.Name)
        //            .ToListAsync());
        //    }
        //    return RedirectToPage("/Account/Login", new { area = "Identity" });
        //}

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

        [Route("/cabinet/create")]
        public IActionResult Create()
        {
            return View();
        }


        [Route("/cabinet/create")]
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

        [Route("/cabinet/info")]
        public async Task<IActionResult> Info(string id)
        {
            if (!await _context.Cabinets.AnyAsync(cabinetId => cabinetId.Id == id))
            {
                return NotFound();
            }

            Cabinet cabinet = await _context.Cabinets
                .Include(s => s.Employees)
                .ThenInclude(c => c.Cartridges)
                .ThenInclude(c => c.Case)
                .Include(th => th.Employees)
                .ThenInclude(s => s.Techniques)
                .FirstOrDefaultAsync(cabinetId => cabinetId.Id == id);
            return View(cabinet);
        }


        [Route("/cabinet/employee/search")]
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
                           || emp.Techniques.Any(th =>
                               IsContains(th.InventoryNumber, search)
                               || IsContains(th.Name, search)
                               || IsContains(th.IpComputer, search))).ToList();

                cabinet.Employees = employees;
                return View(nameof(Info), cabinet);
            }
            catch
            {
                return View(nameof(Info), cabinet);
            }
        }

        [Route("cabinets/search/employee")]
        public async Task<IActionResult> SearchExtended()
        {
            SearchExtendedViewModel model = new SearchExtendedViewModel()
            {
                Employees = await _context.Employees.Include(c => c.Cabinet).ToListAsync()
            };
            return View(model);
        }
        [Route("cabinets/search/technique")]
        public async Task<IActionResult> SearchTechnique()
        {
            SearchExtendedViewModel model = new SearchExtendedViewModel()
            {
                Techniques = await _context.Techniques.Include(emp => emp.Employee).ThenInclude(cab => cab.Cabinet).ToListAsync()
            };
            return View(model);
        }



        [Route("cabinets/edit")]
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

        [Route("cabinets/edit/model")]
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


        [Route("cabinets/delete")]
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


        [Route("/info-071-check")]
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
                    if (viewModel.Checks.Any(th => th.TypeTechnique == technique.TypeTechnique))
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



            return View(viewModel);
        }

        [Route("/info-technique-check")]
        public async Task<IActionResult> ReportTechniqueCheck()
        {
            Report071CheckViewModel viewModel = new Report071CheckViewModel();

            List<Technique> techniques =
                await _context.Techniques
                    .Include(th => th.Employee)
                    .ThenInclude(emp => emp.Cabinet)
                    .ToListAsync();

            foreach (Technique technique in techniques)
            {
                if (viewModel.Checks.Any(th => th.Name == technique.Name && technique.InventoryNumber == 71 && th.TypeTechnique == technique.TypeTechnique))
                {
                    viewModel.Checks.FirstOrDefault(th => th.Name == technique.Name).CountAll++;
                }
                else
                {
                    if (viewModel.Checks.Any(th => th.Name == technique.Name && technique.InventoryNumber == th.InventoryNumber))
                    {
                        viewModel.Checks.FirstOrDefault(th => th.Name == technique.Name).CountAll++;
                    }
                    else
                    {
                        viewModel.Checks.Add(new Report071Check()
                        {
                            Name = technique.Name,
                            TypeTechnique = technique.TypeTechnique,
                            Employee = technique.Employee,
                            InventoryNumber = technique.InventoryNumber,
                            IpComputer = technique.IpComputer,
                            CountAll = 1
                        });
                    }
                }
            }

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
    }
}