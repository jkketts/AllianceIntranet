using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models.CEClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Controllers
{
    public class CEClassController : Controller
    {
        private readonly IAdRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CEClassController> _logger;

        public CEClassController(IAdRepository repo, UserManager<AppUser> userManager, ILogger<CEClassController> logger)
        {
            _repo = repo;
            _userManager = userManager;
            _logger = logger;
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

        public IActionResult Classes()
        {
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

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            ceClass.RegisteredAgents.Add(new RegisteredAgent { AppUser = currentUser, CEClass = ceClass });

            _repo.SaveChanges();

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

            var editCEClass = new EditViewModel(ceClass);

            return View(editCEClass);
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

                _repo.SaveChanges();
            }

            return Redirect("/CEClass/classes");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CEClass/Delete/{id}")]
        public IActionResult Delete(int id, EditViewModel model)
        {
            var ceClass = _repo.GetClassById(id);

            _repo.RemoveClass(ceClass);

            _repo.SaveChanges();

            return Redirect("/CEClass/classes");
        }
    }
}
