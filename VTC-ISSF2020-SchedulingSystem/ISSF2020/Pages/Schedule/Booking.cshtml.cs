using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISSF2020.Models;
using ISSF2020.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ISSF2020.Pages.Schedule
{
    public class BookingModel : PageModel
    {

        [TempData]
        public string Message { get; set; }
        [TempData]
        public string Success { get; set; }
        [TempData]
        public string Error { get; set; }

        private readonly ScheduleService _scheduleService;
        public BookingModel(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public void OnGet()
        {
            ViewData["User"] = HttpContext.Session.GetString("User");
        }

        public IActionResult OnPost()
        {
            var form = HttpContext.Request.Form;

            var userTest = HttpContext.Session.GetString("User");
            Console.Write(userTest);

            var schedule = new ScheduleModel
            {
                Date = form["date"],
                Time = form["time"],
                Duration = form["duration"],
                EventName = form["eventname"],
                EventType = form["eventtype"],
                Location = form["location"],
                UsagePlan = form["usage"],
                User = HttpContext.Session.GetString("User")
            };

            _scheduleService.Create(schedule);

            var bookingId = schedule.Id;
            Success = $"Booking with ID {bookingId} has been successfully registered.";
            return RedirectToPage("/schedule/overview");
        }
    }
}
