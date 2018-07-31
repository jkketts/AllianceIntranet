using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models;
using AllianceIntranet.Models.AdSubmission;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Controllers
{
    public class AdSubmissionController : Controller
    {
        private readonly IAdRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AdSubmissionController> _logger;

        public AdSubmissionController(IAdRepository repo, UserManager<AppUser> userManager, ILogger<AdSubmissionController> logger)
        {
            _repo = repo;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult AdSubmission()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdSubmission(AdViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser AppUser = await _userManager.FindByIdAsync(model.AppUserId);

                var ad = new Ad(model, AppUser);

                _repo.AddEntity(ad);
                _repo.SaveChanges();

                ModelState.Clear();
                return Redirect("/AdSubmission/AdSuccess");
            }
            /* Read http://blog.staticvoid.co.nz/2012/entity_framework-navigation_property_basics_with_code_first/ */
            return View();
        }

        public IActionResult AdSuccess()
        {
            return View();
        }

        [HttpGet("Ads/{id}")]
        public IActionResult Ads(string id)
        {
            if (User.IsInRole("Admin"))
            {
                ICollection<Ad> ads = _repo.GetAllAds();

                return View(ads);
            }
            else
            {
                ICollection<Ad> ads = _repo.GetAdsByUser(id);

                return View(ads);
            }
        }

        public IActionResult Ads()
        {
            var ads = _repo.GetAllAds();

            return View(ads);
        }
    }
}