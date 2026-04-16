using Microsoft.AspNetCore.Mvc;

namespace UdemyCarBook.WebUI.Controllers
{
    [Route("about")]
    public class AboutController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "Hakkımızda";
            ViewBag.SubTitle = "Vizyonumuz & Misyonumuz";

            return View();
        }
    }
}