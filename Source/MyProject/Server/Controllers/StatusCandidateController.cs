using Microsoft.AspNetCore.Mvc;
using MyProject.Server.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusCandidateController : ControllerBase
    {
        // GET: api/<StatusCandidateController>
        [HttpGet]
        public IActionResult Get()
        {
            StatusCandidateRepo statusCandidateRepo = new StatusCandidateRepo();
            return Ok(statusCandidateRepo.GetStatusCandidates());
        }

        // GET api/<StatusCandidateController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StatusCandidateController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StatusCandidateController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StatusCandidateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
