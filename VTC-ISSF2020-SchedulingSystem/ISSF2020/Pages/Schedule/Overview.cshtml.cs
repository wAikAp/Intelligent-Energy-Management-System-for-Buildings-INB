using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISSF2020.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ISSF2020.Pages.Schedule
{
    public class OverviewModel : PageModel
    {
        private readonly ScheduleService _scheduleService;

        public OverviewModel(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public void OnGet()
        {

            ViewData["User"] = HttpContext.Session.GetString("User");

            var scheduleList = _scheduleService.Get();
            // foreach (var elem in scheduleList)
            // {
            //     Console.WriteLine(elem.Id);
            // }
            // Console.WriteLine(scheduleList[0].Id);
            ViewData["Schedules"] = scheduleList;
        }
    }
}
