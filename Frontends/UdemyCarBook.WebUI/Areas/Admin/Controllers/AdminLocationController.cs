using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UdemyCarBook.Dto.LocationDtos;

namespace UdemyCarBook.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AdminLocation")]
    public class AdminLocationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminLocationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 🔥 ADMIN CHECK (ORTAK KONTROL)
        private bool IsAdmin()
        {
            var role = User.Claims.FirstOrDefault(x => x.Type == "Role")?.Value;
            var token = User.Claims.FirstOrDefault(x => x.Type == "carbooktoken")?.Value;

            return role == "Admin" && token != null;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            // 🔥 ADMIN KONTROL
            if (!IsAdmin())
                return RedirectToAction("Index", "Login", new { area = "" });

            var token = User.Claims.FirstOrDefault(x => x.Type == "carbooktoken")?.Value;

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.GetAsync("https://localhost:7060/api/Locations");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultLocationDto>>(jsonData);
                return View(values);
            }

            return View();
        }

        [HttpGet]
        [Route("CreateLocation")]
        public IActionResult CreateLocation()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Login", new { area = "" });

            return View();
        }

        [HttpPost]
        [Route("CreateLocation")]
        public async Task<IActionResult> CreateLocation(CreateLocationDto createLocationDto)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Login", new { area = "" });

            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(createLocationDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:7060/api/Locations", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "AdminLocation", new { area = "Admin" });
            }

            return View();
        }

        [Route("RemoveLocation/{id}")]
        public async Task<IActionResult> RemoveLocation(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Login", new { area = "" });

            var client = _httpClientFactory.CreateClient();

            var responseMessage = await client.DeleteAsync("https://localhost:7060/api/Locations?id=" + id);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "AdminLocation", new { area = "Admin" });
            }

            return View();
        }

        [HttpGet]
        [Route("UpdateLocation/{id}")]
        public async Task<IActionResult> UpdateLocation(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Login", new { area = "" });

            var client = _httpClientFactory.CreateClient();

            var resposenMessage = await client.GetAsync($"https://localhost:7060/api/Locations/{id}");

            if (resposenMessage.IsSuccessStatusCode)
            {
                var jsonData = await resposenMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateLocationDto>(jsonData);
                return View(values);
            }

            return View();
        }

        [HttpPost]
        [Route("UpdateLocation/{id}")]
        public async Task<IActionResult> UpdateLocation(UpdateLocationDto updateLocationDto)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Login", new { area = "" });

            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(updateLocationDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync("https://localhost:7060/api/Locations/", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "AdminLocation", new { area = "Admin" });
            }

            return View();
        }
    }
}