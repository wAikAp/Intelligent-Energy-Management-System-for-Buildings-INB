using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Controllers
{
    public class RoomDetailController : Controller
    {

        public String roomID;
        //[Route("RoomDetail/RoomDetail/{roomID}")]
        public IActionResult RoomDetail(String roomID)
        {
            ViewData["roomID"] = roomID;
            this.roomID = roomID;
            //Debug.WriteLine("RoomDetail = " + this);
            return View();
        }

        [HttpPost]
        public IActionResult AddDragButton(IFormCollection post) {
            Debug.WriteLine("sensorID = " + post["sensor_type"] + post["sensorNo"]);
            ViewData["roomID"] = post["roomID"];
            Debug.WriteLine("AddDragButton room id = " + ViewData["roomID"]);
            return Redirect("RoomDetail?roomID="+ ViewData["roomID"]+"#floorPlan");
        }

    }
}
