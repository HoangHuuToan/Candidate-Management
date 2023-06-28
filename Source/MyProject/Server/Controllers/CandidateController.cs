using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyProject.Server.Repository;
using MyProject.Shared.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        // GET: api/<CandidateController>
        [HttpGet]
        public IActionResult Get()
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return Ok(candidateRepo.GetCandidates()); //Statuscode 
        }

        // GET api/<CandidateController>/5
        [HttpGet("{id}")]
        public Candidate Get(int id)
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return candidateRepo.GetCandidateByID(id); //Statuscode 
        }

        // GET api/<CandidateController>/5
        [HttpGet("status/{id}")]
        public IActionResult getBySTT(int id)
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return Ok(candidateRepo.GetCandidatesBySTT(id)); //Statuscode 
        }

        [HttpGet("AddTTInterview")]
        public IActionResult getCandidateAddTTInterview()
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return Ok(candidateRepo.GetCandidatesAddTTInterview()); //Statuscode 
        }


        [HttpGet("screencalendarinterview")]
        public IActionResult getCandidatescreencalendarinterview()
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return Ok(candidateRepo.GetCandidatesEvaluate()); //Statuscode 
        }

        [HttpGet("sendoffer")]
        public IActionResult getCandidateSendOffer()
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return Ok(candidateRepo.GetCandidateSendOffer()); //Statuscode 
        }

        [HttpGet("screenevaluated")]
        public IActionResult getCandidatesEvaluated()
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return Ok(candidateRepo.GetCandidatesEvaluated()); //Statuscode 
        }

        [HttpGet("gradetestCandidate/{id_candidate}")]
        public IActionResult getGradeTestCandidates(int id_candidate)
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return Ok(candidateRepo.GetGradeTestOfCandidate(id_candidate)); //Statuscode 
        }


        [HttpGet("evaluatedofCandidate/{id_candidate}")]
        public IActionResult GetEvaluateOfCandidate(int id_candidate)
        {
            CandidateRepo candidateRepo = new CandidateRepo();

            return Ok(candidateRepo.GetEvaluateOfCandidate(id_candidate)); //Statuscode 
        }

        // POST api/<CandidateController>
        [HttpPost("add")]
        public void Post([FromBody] Candidate candidate)
        {
            CandidateRepo candidateRepo = new CandidateRepo();
            candidateRepo.addCandidate(candidate);

            byte[] bytes = Convert.FromBase64String(candidate.strBase64pdf);
            //File.WriteAllBytes(@"C:\ToanHH\intership_Blazor_ToanHH_202303\Source\MyProject\Server\cvCandidate\abc.pdf", bytes);

            if (!candidate.pathCV.IsNullOrEmpty())
            {
                System.IO.FileStream stream =
                new FileStream(candidate.pathCV, FileMode.CreateNew);
                System.IO.BinaryWriter writer =
                    new BinaryWriter(stream);
                writer.Write(bytes, 0, bytes.Length);
                writer.Close();
            }

        }

        // PUT api/<CandidateController>/5
        [HttpPut("{id}")]
        public void Put([FromBody] Candidate candidate)
        {
            CandidateRepo candidateRepo = new CandidateRepo();
            candidateRepo.update(candidate);
        }

        // DELETE api/<CandidateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            CandidateRepo candidateRepo = new CandidateRepo();
            candidateRepo.DeleteCandidate(id);
        }
    }
}
