using ISSF2020.Models;
using ISSF2020.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ISSF2020.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public ActionResult<List<ScheduleModel>> Get() =>
            _scheduleService.Get();

        [HttpGet("{id:length(24)}", Name = "GetSchedule")]
        public ActionResult<ScheduleModel> Get(string id)
        {
            var schedule = _scheduleService.Get(id);

            if (schedule == null)
            {
                return NotFound();
            }

            return schedule;
        }

        [HttpPost]
        public ActionResult<ScheduleModel> Create(ScheduleModel schedule)
        {
            _scheduleService.Create(schedule);

            return CreatedAtRoute("GetSchedule", new { id = schedule.Id.ToString() }, schedule);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ScheduleModel scheduleIn)
        {
            var schedule = _scheduleService.Get(id);

            if (schedule == null)
            {
                return NotFound();
            }

            _scheduleService.Update(id, scheduleIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var schedule = _scheduleService.Get(id);

            if (schedule == null)
            {
                return NotFound();
            }

            _scheduleService.Remove(schedule.Id);

            return NoContent();
        }
    }
}
