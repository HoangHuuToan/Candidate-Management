using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class CalendarInterview
    {
        public int id { get; set; }
        public string nameInterview { get; set; } = string.Empty;
        public string nameCandidate { get; set; } = string.Empty;
        public int idCandidate { get; set; }
        public DateTime timeInterview { get; set; }

        public string addressInterview { get; set; } = string.Empty;
        public int? roominterview { get; set; }
        
        public List<int> idUserInterviews { get; set; } = new List<int> { };
        public List<string> nameUserInterviews { get; set; } = new List<string> { };

        public CalendarInterview() { }

        public CalendarInterview(int id , string nameInterview, int idCandidate, DateTime timeInterview, string addressInterview, string nameCandidate, int roominterview) 
        {
            this.id = id;
            this.nameInterview = nameInterview;
            this.idCandidate = idCandidate;
            this.timeInterview = timeInterview;
            this.addressInterview = addressInterview;
            this.nameCandidate = nameCandidate;
            this.roominterview = roominterview;
        }

        public CalendarInterview(int id, string nameInterview, int idCandidate, DateTime timeInterview, string addressInterview, string nameCandidate)
        {
            this.id = id;
            this.nameInterview = nameInterview;
            this.idCandidate = idCandidate;
            this.timeInterview = timeInterview;
            this.addressInterview = addressInterview;
            this.nameCandidate = nameCandidate;
        }
        public CalendarInterview(int id, string nameInterview, int idCandidate, DateTime timeInterview, string addressInterview)
        {
            this.id = id;
            this.nameInterview = nameInterview;
            this.idCandidate = idCandidate;
            this.timeInterview = timeInterview;
            this.addressInterview = addressInterview;
            
        }

        public CalendarInterview( string nameInterview, int idCandidate, DateTime timeInterview, string addressInterview)
        {
            this.nameInterview = nameInterview;
            this.idCandidate = idCandidate;
            this.timeInterview = timeInterview;
            this.addressInterview = addressInterview;
        }

        public CalendarInterview(string nameInterview, int idCandidate, DateTime timeInterview, string addressInterview, int roominterview)
        {
            this.nameInterview = nameInterview;
            this.idCandidate = idCandidate;
            this.timeInterview = timeInterview;
            this.addressInterview = addressInterview;
            this.roominterview = roominterview;
        }
    }
}
