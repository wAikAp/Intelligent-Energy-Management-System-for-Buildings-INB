using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers
{
    public class RoomDetailController : Controller
    {
        
       
        //[Route("RoomDetail/RoomDetail/{roomID}")]
        public IActionResult RoomDetail(String roomID)
        {
            ViewData["roomID"] = roomID;
            Debug.WriteLine("roomID = " + roomID);
            return View();
        }
    }
}
