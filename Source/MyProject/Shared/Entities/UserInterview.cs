using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class UserInterview
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        
        public bool selected { get; set; } = false;
        public UserInterview(int id, string name,string email) 
        {
            this.id = id;
            this.name = name;
            this.email = email;
        }
        public UserInterview()
        {
        
        }
    }

    
}
