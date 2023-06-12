using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyProject.Server.Repository;
using MyProject.Shared.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarInterviewController : ControllerBase
    {

        // GET: api/<CalendarInterview>
        [HttpGet]
        public IActionResult Get()
        {
            CalendarInterviewRepo calendarInterviewRepo = new CalendarInterviewRepo();
            return Ok(calendarInterviewRepo.GetAllCalendar());
        }

        // GET: api/<CalendarInterview>
        [HttpGet("hasroom")]
        public IActionResult Gethasroom()
        {
            CalendarInterviewRepo calendarInterviewRepo = new CalendarInterviewRepo();
            return Ok(calendarInterviewRepo.GetAllCalendarHasRoom());
        }

        // GET api/<CalendarInterview>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("stt/{stt}")]
        public IActionResult GetInterviewTest(int stt)
        {
            CalendarInterviewRepo calendarInterviewRepo = new CalendarInterviewRepo();
            return Ok(calendarInterviewRepo.GetCalendarStt(stt));
        }

        // POST api/<CalendarInterview>
        [HttpPost("add")]
        public void Post([FromBody] CalendarInterview calendarInterview)
        {
            CalendarInterviewRepo calendarInterviewRepo = new CalendarInterviewRepo();
            calendarInterviewRepo.addCalendarInterview(calendarInterview);
        }

        // PUT api/<CalendarInterview>/5
        [HttpPut()]
        public void Put([FromBody] CalendarInterview calendarInterview)
        {
            CalendarInterviewRepo calendarInterviewRepo = new CalendarInterviewRepo();
            calendarInterviewRepo.UpdateCalendar(calendarInterview);
        }

        // DELETE api/<CalendarInterview>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            CalendarInterviewRepo calendarInterviewRepo = new CalendarInterviewRepo();
            calendarInterviewRepo.DeleteCalendar(id);
        }
    }
}
