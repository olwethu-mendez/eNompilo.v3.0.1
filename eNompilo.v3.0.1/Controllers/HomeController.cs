using eNompilo.v3._0._1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Linq;
using eNompilo.v3._0._1.Models.SystemUsers;
using eNompilo.v3._0._1.Areas.Identity.Data;
using eNompilo.v3._0._1.Constants;

namespace eNompilo.v3._0._1.Controllers
{
	public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;


        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor contextAccessor)
		{
			_logger = logger;
			_userManager = userManager;	
			_context = context;
			_signInManager = signInManager;
			_contextAccessor = contextAccessor;
		}

		

		public IActionResult Index()
		{
			if (User.IsInRole(RoleConstants.Patient))
			{
				var userId = _userManager.GetUserId(User);
				var patient = _context.tblPatient.SingleOrDefault(c => c.UserId == userId);
				var patientId = patient.Id;

				HttpContext.Session.SetInt32("PatientId", patientId);
			}
			

			return View();
		}

		public IActionResult SelfHelp()
		{
			return View();
		}

		public IActionResult EmergencyLine()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

        public IActionResult EmptyPage()
		{
			return View();
		}

        public IActionResult PageNotFound()
		{
			return View();
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}