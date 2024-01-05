using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EngeneerLenRooAspNet.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace EngeneerLenRooAspNet.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly MainContext _context;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            MainContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Логин")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);
                 
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    if (User.IsInRole("admin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return RedirectToAction("Index", "Chat");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Ваш профиль не активирован! Обратитесь к системному администратору.");
                    return Page();
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Ваш профиль заблокирован.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    var users = await _context.Users.ToListAsync();
                    foreach (var userItem in users)
                    {
                        if(userItem.Email.Split(' ').First() == Input.Email)
                        {
                            result = await _signInManager.PasswordSignInAsync(userItem.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                            if (result.Succeeded)
                            {
                                _logger.LogInformation("User logged in.");
                                if (User.IsInRole("admin"))
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                    return RedirectToAction("Index", "Chat");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Ошибка авторизации. Введен неверный логин или пароль.");
                                return Page();
                            }
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Ошибка авторизации. Введен неверный логин или пароль.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
