using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class StatusCandidate
    {
        public int id { get; set; }
        public string statusName { get; set; }

        public StatusCandidate() { }
        public StatusCandidate(int ID, string StatusName) 
        {
            this.id = ID;
            this.statusName = StatusName; 
        }

    }
}
