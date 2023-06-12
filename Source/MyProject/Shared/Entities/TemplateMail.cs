using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class TemplateMail
    {
        public int id { get; set; }
        public string nameMail { get; set; }
        public string tittleMail { get; set; }
        public string contentMail { get; set; }

        public TemplateMail() { }
        public TemplateMail(int id , string namemail, string tittlemail, string contentmail) 
        {
            this.id = id;
            this.nameMail = namemail;
            this.tittleMail = tittlemail;
            this.contentMail = contentmail;
        
        }
    }
}
