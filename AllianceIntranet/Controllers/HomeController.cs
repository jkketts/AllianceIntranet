using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models;
using AllianceIntranet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Exchange.WebServices.Data;

namespace AllianceIntranet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAdRepository repo, UserManager<AppUser> userManager, ILogger<HomeController> logger)
        {
            _repo = repo;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

                var ad = new Ad { DateSubmitted = DateTime.Now, MLSNumber = model.MLSNumber, Street = model.Street, City = model.City, Price = model.Price, AppUser = AppUser};

                _repo.AddEntity(ad);
                _repo.SaveChanges();

                ModelState.Clear();
                return Redirect("/Home/AdSuccess");
            }
            /* Read http://blog.staticvoid.co.nz/2012/entity_framework-navigation_property_basics_with_code_first/ */
            return View();
        }

        public IActionResult AdSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AppUsers()
        {
            var agents = _repo.GetAllAppUsers();

            return View(agents);
        }

        [HttpGet("Ads/{id}")]
        public IActionResult Ads(string id)
        {
            IEnumerable<Ad> ads = _repo.GetAdsByUser(id);

            ViewBag.Message = id;

            return View(ads);
        }

        public IActionResult Ads()
        {
            var ads = _repo.GetAllAds();

            return View(ads);
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult AddClass()
        {
            return View();
        }

        public IActionResult Classes()
        {
            var ceClasses = _repo.GetAllClasses();
            return View(ceClasses);
        }

        [HttpPost]
        public IActionResult AddClass(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ceClass = new CEClass()
                {
                    Date = model.Date,
                    Time = model.Time,
                    Instructor = model.Instructor,
                    Type = model.Type,
                    ClassTitle = model.ClassTitle,
                    Description = model.Description
                };

                _repo.AddEntity(ceClass);
                _repo.SaveChanges();

                ModelState.Clear();
                return Redirect("/Home/Classes");
            }

            return View();
        }

        [HttpGet]
        public IActionResult SendAnEmail()
        {
            var exchange = new ExchangeService();
            exchange.Credentials = new WebCredentials("justin.ketterman", "Far1on1~Far1on1~", "prua");
            exchange.Url = new Uri("https://email.bhhsall.com/EWS/Exchange.asmx");

            EmailMessage msg = new EmailMessage(exchange);
            msg.Subject = "Test";
            msg.Body = "Test";
            msg.ToRecipients.Add("justin@ketterman.tv");

            try { 
                 msg.SendAndSaveCopy();
                _logger.LogInformation("Message sent succesfsfuly");
            } catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return Redirect("/");
        }
        

    }
}
