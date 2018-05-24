using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models;
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

        public AccountController(
            UserManager<AppUser> userManager,
            IAdRepository repo,
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _repo = repo;
            _logger = logger;
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

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(string id)
        {
            var agent = _userManager.FindByIdAsync(id).Result;

            List<string> roles = _userManager.GetRolesAsync(agent).Result.ToList<string>();

            if (!roles.Any())
            {
                roles.Add("Agent");
            }

            EditViewModel updatedUser = new EditViewModel() {FirstName = agent.FirstName, LastName = agent.LastName, Email = agent.Email, Office = agent.Office, Role = roles.First() };

            return View(updatedUser);
        }
        
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(string id, EditViewModel model)
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

          //  _userManager.change

            return Redirect("/Account/Agents");
        }
        
    }
}
