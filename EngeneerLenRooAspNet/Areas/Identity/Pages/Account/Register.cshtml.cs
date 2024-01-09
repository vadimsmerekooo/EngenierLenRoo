using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EngeneerLenRooAspNet.Areas.Identity.Data;
using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace EngeneerLenRooAspNet.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MainContext _context;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            MainContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Поле {0} должно быть заполнено.")]
            [StringLength(25, ErrorMessage = "{0} должен содержать не менее {2} символов и не более {1}.", MinimumLength = 8)]
            [Display(Name = "Логин")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Поле {0} должно быть заполнено.")]
            [StringLength(50, ErrorMessage = "{0} должен состоять из {2} и не более {1} символов латинского алфавита, содержать заглавные и строчные буквы, цифры, символы и знаки препинания ! @ $ % ^ & * ( ) _ - +", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Поле {0} должно быть заполнено.")]
            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
            [Required(ErrorMessage = "Поле {0} должно быть заполнено.")]
            [DataType(DataType.Password)]
            [Display(Name = "Секретный код для регистрации")]
            public string SecureCode { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            if (ModelState.IsValid && Input.SecureCode.Equals("system_300"))
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    Employee employee = new Employee()
                    {
                        Fio = user.Email,
                        Id = user.Id
                    };
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
