using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models.Account;
using AllianceIntranet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AllianceIntranet.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdRepository _repo;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<AppUser> userManager,
            IAdRepository repo,
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _repo = repo;
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: /<controller>/
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { FirstName = model.FirstName,
                                         LastName = model.LastName,
                                         UserName = model.Email,
                                         Email = model.Email,
                                         Office = model.Office};
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    /*Code to send email confirmation would go here*/
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect("/Home/Index");
                }
                else
                {
                    _logger.LogError("This didn't work...");
                }
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return Redirect("/Home/Index");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Agents()
        {
            List<UserViewModel> UserList = new List<UserViewModel>();

            foreach (var agent in _repo.GetAllAppUsers())
            {
                List<string> roles = _userManager.GetRolesAsync(agent).Result.ToList<string>();

                if (!roles.Any())
                {
                    roles.Add("Agent");
                }

                UserList.Add(new UserViewModel() { UserId = agent.Id, FirstName = agent.FirstName, LastName = agent.LastName, Email = agent.Email, Office = agent.Office, Role = roles.First() });
            }

            return View(UserList);
        }

        [HttpGet("Detail/{id}")]
        public IActionResult Detail(string id)
        {

            var agent = _userManager.FindByIdAsync(id).Result;

            List<string> roles = _userManager.GetRolesAsync(agent).Result.ToList<string>();

            if (!roles.Any())
            {
                roles.Add("Agent");
            }

            UserViewModel newUser = new UserViewModel() { UserId = agent.Id, FirstName = agent.FirstName, LastName = agent.LastName, Email = agent.Email, Office = agent.Office, Role = roles.First() };

            return View(newUser);
        }

        [HttpGet("Account/Edit/{id}")]
        public IActionResult Edit(string id)
        {
            var agent = _userManager.FindByIdAsync(id).Result;

            List<string> roles = _userManager.GetRolesAsync(agent).Result.ToList<string>();

            if (!roles.Any())
            {
                roles.Add("Agent");
            }

            AccountEditViewModel updatedUser = new AccountEditViewModel() {FirstName = agent.FirstName, LastName = agent.LastName, Email = agent.Email, Office = agent.Office, Role = roles.First() };

            return View(updatedUser);
        }
        
        [HttpPost("Account/Edit/{id}")]
        public async Task<IActionResult> Edit(string id, AccountEditViewModel model)
        {
            var agent = _userManager.FindByIdAsync(id).Result;

            agent.FirstName = model.FirstName;
            agent.LastName = model.LastName;
            agent.Email = model.Email;
            agent.Office = model.Office;

            if (model.Password != null)
            {
                var removePassword = await _userManager.RemovePasswordAsync(agent);
                if (removePassword.Succeeded)
                {
                    var AddPassword = await _userManager.AddPasswordAsync(agent, model.Password);
                    if (AddPassword.Succeeded)
                    {
                        return Redirect("/Accounts/Agents");
                    }
                }
            }
            _repo.SaveChanges();

            return Redirect("/Account/Agents");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {


                var user = await _userManager.FindByEmailAsync(model.Email);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                //Change this to my email service through exchange
                var callbackUrl = Url.Link("Default", new { Controller = "Account", Action = "ResetPassword", code });
                _emailSender.SendEmail(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                _logger.LogInformation("User wasn't found");
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            _logger.LogInformation(result.ToString());
            //AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateAddress()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            var userViewModel = new UpdateAddressViewModel(user);

            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult UpdateAddress(UpdateAddressViewModel model)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (ModelState.IsValid)
            {
                user.Street = model.Street;
                user.City = model.City;
                user.State = model.State;
                user.Zip = model.Zip;
                user.PhoneNumber = model.Phone;
                user.LastModified = System.DateTime.Now;

                _repo.SaveChanges();
            }
            else
            {
                return View(model);
            }

            return Redirect("/CEClass/classes");
        }

        [HttpGet]
        public IActionResult MakeAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeAdmin(MakeAdminViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");

                return Redirect("/");
            }

            return RedirectToAction("Account", "MakeAdmin");

        }
    }
}
