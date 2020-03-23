using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FYP_APP.Controllers
{
    public class SensorsController : Controller
    {
        public IActionResult Sensors()
        {
            return View();
        }
    }
}