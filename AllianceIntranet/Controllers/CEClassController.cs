using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models.CEClasses;
using AllianceIntranet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using RazorLight;
using AllianceIntranet.Models.EmailTemplates;

namespace AllianceIntranet.Controllers
{
    public class CEClassController : Controller
    {
        private readonly IAdRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CEClassController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IRazorLightEngine _engine;

        public CEClassController(IAdRepository repo, UserManager<AppUser> userManager, ILogger<CEClassController> logger, IEmailSender emailSender, IRazorLightEngine engine)
        {
            _repo = repo;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _engine = engine;
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> Classes()
        {
            var user = await _userManager.GetUserAsync(User);
            //Change to days, but for now use seconds
            var diffInDays = (System.DateTime.Now - user.LastModified).TotalDays;


            if ((!User.IsInRole("Admin") && diffInDays > 180) || user.LastModified == null)
            {
                return Redirect("/Account/UpdateAddress");
            }

            var ceClasses = _repo.GetAllClasses();

            return View(ceClasses);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ceClass = new CEClass(model);

                _repo.AddEntity(ceClass);
                _repo.SaveChanges();

                ModelState.Clear();
                return Redirect("/CEClass/Classes");
            }

            return View();
        }

        [HttpPost("CEClass/Register/{id}")]
        public async Task<IActionResult> Register(int id)
        {
            var ceClass = _repo.GetClassById(id);

            if (ceClass.RegisteredAgents.Count() < ceClass.MaxSize) { 
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                ceClass.RegisteredAgents.Add(new RegisteredAgent { AppUser = currentUser, CEClass = ceClass });

                var emailRegisterViewModel = new EmailRegisterViewModel(ceClass);
                
                _emailSender.SendEmailAsync("justin.ketterman@bhhsall.com", $"Registered for {ceClass.ClassTitle}", "RegisteredClass", emailRegisterViewModel);

                _repo.SaveChanges();
            }

            return Redirect("/CEClass/Classes");
        }

        [HttpPost("CEClass/Unregister/{id}")]
        public async Task<IActionResult> Unregister(int id)
        {
            var ceClass = _repo.GetClassById(id);

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var registeredAgents = _repo.GetRegisteredAgents();

            foreach (var r in registeredAgents.ToList())
            {
                if (r.AppUserId == currentUser.Id && r.CEClassId == ceClass.Id)
                {
                    _repo.RemoveRegisteredAgent(r);

                    var emailRegisterViewModel = new EmailRegisterViewModel(ceClass);
                    _emailSender.SendEmailAsync("justin.ketterman@bhhsall.com", $"Unregistered for {ceClass.ClassTitle}", "UnregisteredClass", emailRegisterViewModel);
                    break;
                }
            }

            _repo.SaveChanges();

            return Redirect("/CEClass/Classes");
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("CEClass/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var ceClass = _repo.GetClassById(id);

            if (ceClass != null)
            {
                var editCEClass = new EditViewModel(ceClass);

                return View(editCEClass);
            }

            return Redirect("/CEClass/Classes");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CEClass/Edit/{id}")]
        public IActionResult Edit(int id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ceClass = _repo.GetClassById(id);

                ceClass.Date = model.Date;
                ceClass.Time = model.Time;
                ceClass.Instructor = model.Instructor;
                ceClass.Type = model.Type;
                ceClass.ClassTitle = model.ClassTitle;
                ceClass.Description = model.Description;
                ceClass.MaxSize = model.MaxSize;

                _repo.SaveChanges();
            }

            return Redirect("/CEClass/classes");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CEClass/Delete/{id}")]
        public IActionResult Delete(int id, EditViewModel model)
        {
            var ceClass = _repo.GetClassById(id);

            if (ceClass != null)
            {
                _repo.RemoveClass(ceClass);

                _repo.SaveChanges();
            }

            return Redirect("/CEClass/classes");
        }

        [HttpGet("CEClass/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            var ceClass = _repo.GetClassById(id);

            if (ceClass != null)
            {
                var registeredAgents = _repo.GetRegisteredAgents().Where(n => n.CEClassId == ceClass.Id);

                List<AppUser> appUsersInRegisteredAgents = new List<AppUser>();

                foreach (var r in registeredAgents)
                {
                    var appUser = _userManager.FindByIdAsync(r.AppUserId).Result;
                    appUsersInRegisteredAgents.Add(appUser);
                }

                var Detail = new DetailViewModel(ceClass, appUsersInRegisteredAgents);

                return View(Detail);
            }

            return Redirect("/CEClass/Classes");
        }
    }
}
