using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize(Roles = "admin")]
    public class CaseController : Controller
    {
        MainContext _context;
        public CaseController(MainContext context)
        {
            _context = context;
        }
        [Route("case")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cases.Include(c => c.Cartridge).ThenInclude(e => e.Employee).ThenInclude(c => c.Cabinet).ToListAsync());
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
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "Fio");
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
            Employee employee = await _context.Employees.Where(e => e.Id == model.EmployeeId).FirstOrDefaultAsync();
            Cartridge cartridge = new Cartridge()
            {
                Name = model.Name,
                CaseId = model.CaseId,
                DateGet = System.DateTime.Now,
                Employee = employee
            };
            await _context.Cartridges.AddAsync(cartridge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> IssuedCartridge(int cartridgeId)
        {
            Cartridge cartridge = await _context.Cartridges.FirstOrDefaultAsync(c => c.Id == cartridgeId);
            cartridge.DateSet = System.DateTime.Now;
            cartridge.IsIssued = true;
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
            Cartridge cartridge = await _context.Cartridges.FirstOrDefaultAsync(c => c.Id == cartridgeId);
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
