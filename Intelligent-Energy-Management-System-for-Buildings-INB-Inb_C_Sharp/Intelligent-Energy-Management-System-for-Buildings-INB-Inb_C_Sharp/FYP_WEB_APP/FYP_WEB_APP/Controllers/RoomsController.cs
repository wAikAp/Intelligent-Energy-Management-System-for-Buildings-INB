using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FYP_WEB_APP.Controllers
{
    public class RoomsController : Controller
    {
        public IActionResult Rooms()
        {
            return View();
        }
    }
}