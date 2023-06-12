using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Kiota.Abstractions;
using MyProject.Shared.Entities;
using System.Net.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendmailController : ControllerBase
    {

        private MailSettings mailSettings;

        public SendmailController(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }

        // GET: api/<SendmailController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SendmailController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SendmailController>
        [HttpPost]
        public void Post([FromBody] MailRequest mailRequest)
        {
            string pathToHTMLFile = @"wwwroot/templatemail/index.html";
            string htmlString = System.IO.File.ReadAllText(pathToHTMLFile);

            //mailRequest.Body = mailRequest.Body.Replace("@nameCandidate", mailRequest.nameCandidate);

            htmlString = htmlString.Replace("@candidateName", mailRequest.nameCandidate);
            htmlString = htmlString.Replace("@contentMail",mailRequest.Body);
            
            mailRequest.Body = htmlString;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(mailSettings.Mail);
                mail.To.Add(mailRequest.ToEmail);
                mail.Subject = mailRequest.Subject;
                mail.Body = mailRequest.Body;
                mail.IsBodyHtml = true;



                if (!mailRequest.StringFileAttachments.IsNullOrEmpty())
                {
                    int i = 0;
                    foreach (var stringBase64file in mailRequest.StringFileAttachments)
                    {   
                        byte[] bytes = Convert.FromBase64String(stringBase64file);

                        var contents = new StreamContent(new MemoryStream(bytes)).ReadAsStream();

                        var fileattachment = new System.Net.Mail.Attachment(contents, mailRequest.nameFileAttachments[i].ToString());

                        mail.Attachments.Add(fileattachment);
                        i ++;
                    }
                   
                }


                using (SmtpClient smtp = new SmtpClient(mailSettings.Host, mailSettings.Port))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(mailSettings.Mail, mailSettings.Password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        // PUT api/<SendmailController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SendmailController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
