using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; } 
        public List<string> StringFileAttachments { get; set; } = new List<string>();
        public List<Attachment> FileAttachments { get; set; } = new List<Attachment>();
        public List<string> nameFileAttachments { get; set; } = new List<string>();

        public string nameCandidate { get; set; } = string.Empty;
        public MailRequest(string toMail , string subject, string body, string nameCandidate)
        {
            this.ToEmail = toMail;
            this.Subject = subject;
            this.Body = body;
            this.nameCandidate = nameCandidate;
        }

        public MailRequest()
        {
        
        }
    }
}
