using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize(Roles = "admin")]
    public class CaseController : Controller
    {
        MainContext _context;
        ILogger<CaseController> _loogger;
        public CaseController(MainContext context, ILogger<CaseController> logger)
        {
            _context = context;
            _loogger = logger;
        }
        [Route("case")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cases
                .Include(c => c.Cartridge)
                .ThenInclude(e => e.EmployeeGet)
                .Include(c => c.Cartridge)
                .ThenInclude(e => e.EmployeeSet)
                .ThenInclude(c => c.Cabinet)
                .Include(c => c.Cartridge)
                .ThenInclude(t => t.Technique)
                .OrderByDescending(d => d.DateCreate)
                .AsSplitQuery()
                .Take(50)
                .ToListAsync());
        }
        [Route("case/create")]
        public async Task<IActionResult> Create()
        {
            await _context.Cases.AddAsync(new Case()
            {
                DateCreate = System.DateTime.Now
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Route("case/add")]
        public async Task<IActionResult> Add(int caseId)
        {
            ViewBag.Employees = new SelectList(await _context.Employees.Include(c => c.Techniques).Where(c => c.Id != "b230b4b5-8ba9-4d95-9b81-d04732e15ddb" && c.Techniques.Where(th => th.TypeTechnique == Services.TypeTechnique.Cartridge).Count() > 0).ToListAsync(), "Id", "Fio");
            CartridgeViewModel model = new CartridgeViewModel()
            {
                CaseId = caseId
            };
            return View(model);
        }
        [Route("case/add")]
        [HttpPost]
        public async Task<IActionResult> Add(CartridgeViewModel model)
        {
            try
            {
                Employee employee = await _context.Employees.Where(e => e.Id == model.EmployeeGetId).Include(t => t.Techniques).FirstOrDefaultAsync();

                if (employee.Techniques.Count(t => t.TypeTechnique == Services.TypeTechnique.Cartridge) == 1)
                {
                    Technique technique = employee.Techniques.Where(t => t.TypeTechnique == Services.TypeTechnique.Cartridge).FirstOrDefault();
                    Employee caseEmployee = await _context.Employees.Include(t => t.Techniques).FirstOrDefaultAsync(c => c.Id == "b230b4b5-8ba9-4d95-9b81-d04732e15ddb");
                    Cartridge cartridge = new Cartridge()
                    {
                        DateGet = System.DateTime.Now,
                        Technique = technique,
                        CaseId = model.CaseId,
                        EmployeeGet = employee
                    };
                    if (model.IsIssuedRight)
                    {
                        cartridge.IsIssuedRight = true;
                    }
                    employee.Techniques.Remove(technique);
                    caseEmployee.Techniques.Add(technique);
                    _context.Employees.UpdateRange(employee, caseEmployee);
                    await _context.Cartridges.AddAsync(cartridge);
                    await _context.SaveChangesAsync();
                }
                if (employee.Techniques.Count(t => t.TypeTechnique == Services.TypeTechnique.Cartridge) >= 1)
                {
                    return RedirectToAction(nameof(AddSelectCartridge), model);
                }
            }
            catch(Exception ex)
            {

            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddSelectCartridge(CartridgeViewModel model)
        {
            ViewBag.Technique = new SelectList(await _context.Techniques.Include(e => e.Employee).Where(t => t.EmployeeId == model.EmployeeGetId && t.TypeTechnique == Services.TypeTechnique.Cartridge).ToListAsync(), "Id", "Name"); ;
            return View(model);
        }

        public async Task<IActionResult> AddSelectCartridgeModel(CartridgeViewModel model)
        {
            try
            {
                Employee employee = await _context.Employees.Include(t => t.Techniques).FirstOrDefaultAsync(e => e.Id == model.EmployeeGetId);

                Technique technique = employee.Techniques.FirstOrDefault(t => t.TypeTechnique == Services.TypeTechnique.Cartridge && t.Id == model.TechniqueId);
                Employee caseEmployee = await _context.Employees.Include(t => t.Techniques).FirstOrDefaultAsync(c => c.Id == "b230b4b5-8ba9-4d95-9b81-d04732e15ddb");
                Cartridge cartridge = new Cartridge()
                {
                    DateGet = System.DateTime.Now,
                    Technique = technique,
                    CaseId = model.CaseId,
                    EmployeeGet = employee
                };
                if (model.IsIssuedRight)
                {
                    cartridge.IsIssuedRight = true;
                }

                employee.Techniques.Remove(technique);
                caseEmployee.Techniques.Add(technique);
                _context.Employees.UpdateRange(employee, caseEmployee);
                await _context.Cartridges.AddAsync(cartridge);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _loogger.LogError(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> IssuedCartridge(int cartridgeId)
        {
            Cartridge cartridge = await _context.Cartridges.Include(e => e.EmployeeGet).ThenInclude(t => t.Techniques).Include(t => t.Technique).ThenInclude(e => e.Employee).AsSplitQuery().FirstOrDefaultAsync(c => c.Id == cartridgeId);
            Employee employee = cartridge.EmployeeGet;
            Employee caseEmployee = cartridge.Technique.Employee;
            cartridge.DateSet = System.DateTime.Now;
            cartridge.IsIssued = true;
            cartridge.EmployeeSet = employee;
            caseEmployee.Techniques.Remove(cartridge.Technique);
            employee.Techniques.Add(cartridge.Technique);
            _context.Employees.Update(employee);
            _context.Cartridges.Update(cartridge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Send(int caseId)
        {
            Case caseResult = await _context.Cases.FirstOrDefaultAsync(c => c.Id == caseId);
            caseResult.DateSend = System.DateTime.Now;
            _context.Cases.Update(caseResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Get(int caseId)
        {
            Case caseResult = await _context.Cases.FirstOrDefaultAsync(c => c.Id == caseId);
            caseResult.DateGet = System.DateTime.Now;
            _context.Cases.Update(caseResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteCartridge(int cartridgeId)
        {
            Cartridge cartridge = await _context.Cartridges.Include(e => e.EmployeeGet).ThenInclude(t => t.Techniques).Include(t => t.Technique).AsSplitQuery().FirstOrDefaultAsync(c => c.Id == cartridgeId);
            Employee employee = cartridge.EmployeeGet;
            employee.Techniques.Add(cartridge.Technique);
            _context.Employees.Update(employee);
            _context.Cartridges.Remove(cartridge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int caseId)
        {
            Case caseResult = _context.Cases.Where(c => c.Id == caseId).FirstOrDefault();
            _context.Cases.Remove(caseResult);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
