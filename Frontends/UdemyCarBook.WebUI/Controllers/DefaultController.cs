using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using UdemyCarBook.Dto.LocationDtos;

namespace UdemyCarBook.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<SelectListItem> values2 = new();

            var client = _httpClientFactory.CreateClient();

            var responseMessage = await client.GetAsync("https://localhost:7060/api/Locations");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultLocationDto>>(jsonData);

                values2 = values.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.LocationID.ToString()
                }).ToList();
            }

            ViewBag.LocationID = values2;

            return View();
        }

        [HttpPost]
        public IActionResult Index(
            string book_pick_date,
            string book_off_date,
            string time_pick,
            string time_off,
            string LocationID) // 🔥 büyük harf yaptık
        {
            TempData["bookpickdate"] = book_pick_date;
            TempData["bookoffdate"] = book_off_date;
            TempData["timepick"] = time_pick;
            TempData["timeoff"] = time_off;
            TempData["locationID"] = LocationID;

            return RedirectToAction("Index", "RentACarList");
        }
    }
}