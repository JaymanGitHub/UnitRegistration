using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class Enrolment
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string RoomNo { get; set; }
        public string Tutor { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public int SrudentCount { get; set; }
        public int Capacity { get;set;}

    }
}