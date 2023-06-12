using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class Candidate
    {
        public int id { get; set; }
        public int id_calendar { get; set; }
        public string strID { get; set; } = string.Empty;
        public string  Name { get; set; } =  string.Empty;
        public int Role { get; set; }= 0;
        public int Position { get; set; } = 0;
        public DateTime BirthDay { get; set; } = DateTime.Now;
        public string Address { get; set; } = string.Empty;
        public int NumberPhone { get; set; } = 0;
        public string Email { get; set; } = string.Empty;   
        public string pathCV { get; set; } = string.Empty;  
        public int Origin { get; set; } = 0;    

        public int Contacting { get; set; } = 0;
        public int Applied { get; set; } = 0;
        public int Status { get; set; } = 1;
        public string? gradeTest { get; set; } = null;
        public string Note { get; set; } = string.Empty;    
        public int FlagDlt { get; set; } = 0;
        public string strBase64pdf { get; set; } = string.Empty;

        public string nameRole { get; set; } = string.Empty;
        public string namePosition { get; set; } = string.Empty;
        public string nameOrigin { get; set; } = string.Empty;
        public string nameStatus { get; set; } = string.Empty;

        public bool Checked { get; set; } = false;

        public DateTime timeInterview { get; set; }
        public string addressInterview { get; set; } = string.Empty;

        public bool selectedFilter = false;


        //calendar

        public int in_interview { get ; set; } 
        public string name_interview { get; set; } = string.Empty;
        public int room_interview { get; set; }

        //Evaluate
        public int evaluate { get; set; }
        public string note_evaluate { get; set; } = string.Empty;
        public int id_userinterview { get; set; }
        public string userinterviewName { get; set; } = string.Empty;

        public Candidate(   int id, 
                            string ID, 
                            string Name, 
                            int Role, 
                            int Position,
                            DateTime BirthDay,
                            string Address,
                            int NumberPhone,
                            string Email,
                            string pathCV,
                            int Origin,
                            int Contacting,
                            int Applied,
                            int Status,
                            string? gradeTest,
                            string Note,
                            int FlagDlt, 
                            string strBase64pdf)
        {
            this.id = id;
            this.strID = ID;
            this.Name = Name;
            this.Role = Role;
            this.Position = Position;
            this.BirthDay = BirthDay;
            this.Address = Address;
            this.NumberPhone = NumberPhone;
            this.Email = Email;
            this.pathCV = pathCV;
            this.Origin = Origin;
            this.Contacting = Contacting;
            this.Applied = Applied;
            this.Status = Status;
            this.gradeTest = gradeTest;
            this.Note = Note;
            this.FlagDlt = FlagDlt;
            this.strBase64pdf = strBase64pdf;
        }
        //import
        public Candidate(   string ID,
                            string Name,
                            int Role,
                            int Position,
                            DateTime BirthDay,
                            string Address,
                            int NumberPhone,
                            string Email,
                            string pathCV,
                            int Origin,
                            int Contacting,
                            int Applied,
                            int Status,
                            string? gradeTest,
                            string Note,
                            int FlagDlt,
                            string strBase64pdf)
        {
            this.strID = ID;
            this.Name = Name;
            this.Role = Role;
            this.Position = Position;
            this.BirthDay = BirthDay;
            this.Address = Address;
            this.NumberPhone = NumberPhone;
            this.Email = Email;
            this.pathCV = pathCV;
            this.Origin = Origin;
            this.Contacting = Contacting;
            this.Applied = Applied;
            this.Status = Status;
            this.gradeTest = gradeTest;
            this.Note = Note;
            this.FlagDlt = FlagDlt;
            this.strBase64pdf = strBase64pdf;
        }

        public Candidate() { }  

        public Candidate(int id , string strID , string nameRole ,string namePosition , string name, DateTime datetime, string address,string nameOrigin ,int applied, string pathCV) 
        {
            this.id = id;
            this.strID =   strID;
            this.nameRole = nameRole;
            this.namePosition = namePosition;
            this.Name=name;
            this.BirthDay=datetime;
            this.Address = address;
            this.nameOrigin = nameOrigin;
            this.Applied=applied;
            this.pathCV=pathCV;
        }

        public Candidate(   int id,
                            string ID,
                            int role,
                            int position,
                            string name,
                            DateTime birthday,
                            string address,         
                            int numberphone,
                            string email,
                            string pathcv,
                            int origin,
                            int contacting,
                            int applied,
                            int status,
                            string? gradetest,
                            string note,
                            int flagdlt,
                            string nameposition,
                            string namerole,                           
                            string nameorigin,
                            string namestatus
                             )
        {
            this.id = id;
            this.strID =ID;
            this.Role = role;
            this.Position   = position;
            this.Name = name;
            this.BirthDay = birthday;
            this.Address = address; 
            this.Origin = origin;
            this.Applied=applied;
            this.NumberPhone = numberphone;
            this.Contacting = contacting;
            this.Note = note;
            this.gradeTest  = gradetest;
            this.Email = email;
            this.pathCV = pathcv;
            this.Status = status;
            this.nameRole = namerole;
            this.namePosition = nameposition;
            this.nameOrigin=nameorigin;
            this.nameStatus     =namestatus;
            this.FlagDlt = flagdlt;
        }

    }

    
}
