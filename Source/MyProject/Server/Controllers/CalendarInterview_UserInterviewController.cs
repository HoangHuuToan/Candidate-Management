using Microsoft.AspNetCore.Mvc;
using MyProject.Server.Repository;
using MyProject.Shared.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarInterview_UserInterviewController : ControllerBase
    {
        // GET: api/<CalendarInterview_UserInterviewController>
        [HttpGet]
        public IActionResult Get()
        {
            CalendarInterview_UserInterviewRepo calendarInterview_UserInterviewRepo = new CalendarInterview_UserInterviewRepo();

            return Ok(calendarInterview_UserInterviewRepo.getAllCalendar_User());
        }

        // GET api/<CalendarInterview_UserInterviewController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            CalendarInterview_UserInterviewRepo calendarInterview_UserInterviewRepo = new CalendarInterview_UserInterviewRepo();

            return Ok(calendarInterview_UserInterviewRepo.getAllCalendar_UserById_calendar(id));
        }

        // POST api/<CalendarInterview_UserInterviewController>
        [HttpPost]
        public void Post([FromBody] CalendarInterview_UserInterview calendarInterview_UserInterview)
        {

            CalendarInterview_UserInterviewRepo calendarInterviewRepo = new CalendarInterview_UserInterviewRepo();
            calendarInterviewRepo.Add(calendarInterview_UserInterview);
        }

        // PUT api/<CalendarInterview_UserInterviewController>/5
        [HttpPut("{id}")]
        public void Put([FromBody] CalendarInterview_UserInterview calendarInterview_UserInterview)
        {
            CalendarInterview_UserInterviewRepo calendarInterviewRepo = new CalendarInterview_UserInterviewRepo();
            calendarInterviewRepo.Update(calendarInterview_UserInterview);
        }

        // DELETE api/<CalendarInterview_UserInterviewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            CalendarInterview_UserInterviewRepo calendarInterviewRepo = new CalendarInterview_UserInterviewRepo();
            calendarInterviewRepo.Delete(id);
        }
    }
}
