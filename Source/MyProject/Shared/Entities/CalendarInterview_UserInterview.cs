using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class CalendarInterview_UserInterview
    {
        public int id { get; set; }
        public int id_calendarinterview { get; set; }
        public int id_userinterview { get; set; }
        public string nameUser { get; set; } = string.Empty;
        public int evaluate { get; set; }
        public string note_evaluate { get; set; } = string.Empty;
        public CalendarInterview_UserInterview()
        {
            
        }
        public CalendarInterview_UserInterview(int id , int id_calendarinterview,int id_userinterview)
        {
            this.id = id;
            this.id_calendarinterview = id_calendarinterview;
            this.id_userinterview = id_userinterview;
        }
        public CalendarInterview_UserInterview(int id, int id_calendarinterview, int id_userinterview,string userName)
        {
            this.id = id;
            this.id_calendarinterview = id_calendarinterview;
            this.id_userinterview = id_userinterview;
            this.nameUser = userName;
        }

        public CalendarInterview_UserInterview(int id, int id_calendarinterview, int id_userinterview, string userName,int evaluate,string note)
        {
            this.id = id;
            this.id_calendarinterview = id_calendarinterview;
            this.id_userinterview = id_userinterview;
            this.nameUser = userName;
            this.evaluate = evaluate;
            this.note_evaluate = note;
        }
        public CalendarInterview_UserInterview(int id_calendarinterview, int id_userinterview)
        {
            
            this.id_calendarinterview = id_calendarinterview;
            this.id_userinterview = id_userinterview;
        }
    }
}
