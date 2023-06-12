using Microsoft.AspNetCore.Mvc;
using MyProject.Server.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateMailController : ControllerBase
    {
        // GET: api/<TemplateMailController>
        [HttpGet]
        public IActionResult Get()
        {
            TemplateMailRepo templateMailRepo = new TemplateMailRepo();
            return Ok(templateMailRepo.getAllTemplateMail());
        }

        // GET api/<TemplateMailController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            TemplateMailRepo templateMailRepo = new TemplateMailRepo();
            return Ok(templateMailRepo.getTemplateMail(id));
        }

        // POST api/<TemplateMailController>
        [HttpGet("formMail")]
        public IActionResult GetFormMail()
        {
            string pathToHTMLFile = @"wwwroot/templatemail/index.html";
            string htmlString = System.IO.File.ReadAllText(pathToHTMLFile);
            return Ok(htmlString);
        }

        // PUT api/<TemplateMailController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TemplateMailController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
