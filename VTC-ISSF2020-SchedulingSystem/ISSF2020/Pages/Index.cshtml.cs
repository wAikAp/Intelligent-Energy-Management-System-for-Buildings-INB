using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ISSF2020.Services;
using System.Security.Cryptography.X509Certificates;

namespace ISSF2020.Pages
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly RegionalWeatherService _regionalWeatherService;

        public IndexModel(ILogger<IndexModel> logger, RegionalWeatherService regionalWeatherService)
        {
            _logger = logger;
            _regionalWeatherService = regionalWeatherService;
        }

        public void OnGet()
        {

            ViewData["User"] = HttpContext.Session.GetString("User");

            var weatherData = _regionalWeatherService.GetLast();

            ViewData["WeatherData"] = weatherData;

        }
    }
}
