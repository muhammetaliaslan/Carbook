using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using UdemyCarBook.Persistence.Context;
using UdemyCarBook.WebUI.Models;
 // 🔥 bunu ekle

namespace UdemyCarBook.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CarBookContext _context; // 🔥 DOĞRU CONTEXT

        public HomeController(ILogger<HomeController> logger, CarBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var locations = _context.Locations.ToList();

            List<SelectListItem> values = locations.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.LocationID.ToString()
            }).ToList();

            ViewBag.LocationID = values;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        [HttpPost]
        public IActionResult Index(
       string book_pick_date,
       string book_off_date,
       string time_pick,
       string PickUpLocationID,
       string DropOffLocationID)
        {
            TempData["bookpickdate"] = book_pick_date;
            TempData["bookoffdate"] = book_off_date;
            TempData["timepick"] = time_pick;

            TempData["locationID"] = PickUpLocationID; // 🔥 KRİTİK

            return RedirectToAction("Index", "RentACarList");
        }
    }
}