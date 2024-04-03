using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngeneerLenRooAspNet.Controllers
{
    public class RegistrationRequestController : Controller
    {
        private readonly ILogger _logger;
        private readonly MainContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public RegistrationRequestController(MainContext context, ILogger<RegistrationRequestController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;

        }
        [Route("registration/request")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("registration/request/model")]
        public ActionResult Index(RegistrationRequest model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel(RegistrationRequest model)
        {
            try
            {
                var resultEmployee = await _context.Employees.AnyAsync(e => !e.Fio.Equals(model.Fio));
                var resultCabinet = await _context.Cabinets.AnyAsync(c => c.Name.Equals(model.NumberCabinet));
                if (resultCabinet && resultEmployee)
                {
                    model.DateTime = DateTime.Now;
                    model.Status = TypeRequest.Processing;
                    _context.RegistrationRequests.Add(model);
                    await _context.SaveChangesAsync();
                    return ResultRequest($"Уважаемый пользователь: {model.Fio.Split(' ').First()}, Ваша заявка успешно отправлена. Благодарим Вас за регистрацию!");
                }
                else
                {
                    return ResultRequest("Ошибка Введенные данные не верны.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                return ResultRequest("Ошибка Обратитесь к системному администратору!");
            }
        }

        public async Task<IActionResult> RequestOk(string id)
        {
            RegistrationRequest request = await _context.RegistrationRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
            {
                return RedirectToAction(nameof(RequestList));
            }
            request.Status = TypeRequest.Ok;
            Cabinet cabinet = await _context.Cabinets.FirstOrDefaultAsync(c => c.Name == request.NumberCabinet);
            if (cabinet == null)
            {
                return RedirectToAction(nameof(RequestList));
            }
            string password = $"centr_edu_{DateTime.Now.Hour}{DateTime.Now.Minute}";
            Employee employee = new Employee()
            {
                Cabinet = cabinet,
                Department = request.Department,
                Post = request.Post,
                Fio = request.Fio,
                HashCode = password
            };

            var isValid = IsValidFormEmployeeAsync(employee).Result;
            if (isValid is IActionResult)
            {
                return RedirectToAction(nameof(RequestList));
            }

            if (((List<string>)isValid).Count == 0)
            {
                var user = new IdentityUser { UserName = employee.Fio, Email = employee.Fio, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    List<string> role = new List<string>() { "user" };
                    await _userManager.AddToRolesAsync(user, role);
                    var userId = await _userManager.FindByNameAsync(employee.Fio);
                    if (userId != null)
                    {
                        employee.Id = userId.Id;
                        await _context.Employees.AddAsync(employee);
                        _context.RegistrationRequests.Update(request);
                        await _context.SaveChangesAsync();
                        //EmailService emailService = new EmailService();
                        //await emailService.SendEmailAsync(request.Email, $"Уведомление об успешном ободрении заявки", $"Уважаемый(-ая) {employee.Fio}, Ваша заявка регистраци на сайте <a href=\"http://server-7:6100/account/signin\">Нажмите для авторизации</a> успешно одобрена. \n<b>Ваш логин:</b> {employee.Fio} \n <b>Ваш пароль:</b> {employee.HashCode}") ;
                        return RedirectToAction(nameof(RequestList));
                    }
                }
            }
            return RedirectToAction(nameof(RequestList));
        }
        public async Task<IActionResult> RequestDenied(string id)
        {
            RegistrationRequest request = await _context.RegistrationRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (request != null)
            {
                request.Status = TypeRequest.Denied;
                _context.RegistrationRequests.Update(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(RequestList));
        }


        [Route("registration/request/list")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RequestList()
        {
            return View(await _context.RegistrationRequests.OrderByDescending(d => d.DateTime).ToListAsync());
        }

        [Route("registration/request/result")]
        public IActionResult ResultRequest(string message) => View("ResultRequest", message);

        private async Task<object> IsValidFormEmployeeAsync(Employee employee)
        {
            List<string> errorsModel = new List<string>();
            if (!await _context.Cabinets.AnyAsync(cab => cab.Id == employee.Cabinet.Id))
            {
                return RedirectToAction("Info", "Home", new { id = employee.Cabinet.Id });
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
