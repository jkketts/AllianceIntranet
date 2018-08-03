using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models;
using AllianceIntranet.Models.EmailTemplates;
using AllianceIntranet.Models.OpenHouseSubmission;
using AllianceIntranet.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/***************
 * Still need to add which offices receive emails. Was considering creating list of if thens for if office then send to this admin
 */

namespace AllianceIntranet.Controllers
{
    public class OpenHouseSubmissionController : Controller
    {
        private readonly IAdRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<OpenHouseSubmissionController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IRazorLightEngine _engine;

        public OpenHouseSubmissionController(IAdRepository repo, UserManager<AppUser> userManager, ILogger<OpenHouseSubmissionController> logger, IEmailSender emailSender, IRazorLightEngine engine)
        {
            _repo = repo;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _engine = engine;
        }

        public IActionResult OpenHouseSubmission()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OpenHouseSubmission(OpenHouseViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser AppUser = await _userManager.FindByIdAsync(model.AppUserId);

                var openHouse = new OpenHouse(model, AppUser);

                _repo.AddEntity(openHouse);
                _repo.SaveChanges();

                var emailOpenHouseViewModel = new EmailOpenHouseViewModel(model, AppUser);
                _emailSender.SendEmailAsync("justin.ketterman@bhhsall.com", $"{AppUser.FirstName} {AppUser.LastName} has submitted an Open House", "OpenHouseSubmission", emailOpenHouseViewModel);

                ModelState.Clear();
                return Redirect("/OpenHouseSubmission/OpenHouseSuccess");
            }
            /* Read http://blog.staticvoid.co.nz/2012/entity_framework-navigation_property_basics_with_code_first/ */
            return View();
        }

        public IActionResult OpenHouseSuccess()
        {
            return View();
        }

        [HttpGet("OpenHouses/{id}")]
        public IActionResult OpenHouses(string id)
        {
            if (User.IsInRole("Admin"))
            {
                ICollection<OpenHouse> openHouses = _repo.GetAllOpenHouses();

                return View(openHouses);
            }
            else
            {
                ICollection<OpenHouse> openHouses = _repo.GetOpenHousesByUser(id);

                return View(openHouses);
            }
        }

        public IActionResult OpenHouses()
        {
            var openHouses = _repo.GetAllOpenHouses();

            return View(openHouses);
        }
    }
}