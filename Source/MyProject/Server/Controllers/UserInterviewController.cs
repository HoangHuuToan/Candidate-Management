using Microsoft.AspNetCore.Mvc;
using MyProject.Server.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInterviewController : ControllerBase
    {
        // GET: api/<UserInterviewController>
        [HttpGet]
        public IActionResult Get()
        {   
            UserInterviewRepo userInterviewRepo = new UserInterviewRepo();
            return Ok(userInterviewRepo.getUserInterview());
        }

        // GET api/<UserInterviewController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserInterviewController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserInterviewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserInterviewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
