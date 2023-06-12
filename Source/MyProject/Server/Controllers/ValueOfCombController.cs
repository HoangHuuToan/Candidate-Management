using Microsoft.AspNetCore.Mvc;
using MyProject.Server.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueOfCombController : ControllerBase
    {
        // GET: api/<ValueOfCombController>
        [HttpGet]
        public IActionResult Get()
        {
            ValuesComb valuesComb = new ValuesComb();
            return Ok(valuesComb.getValuesComb());
        }

        // GET api/<ValueOfCombController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValueOfCombController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValueOfCombController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValueOfCombController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
