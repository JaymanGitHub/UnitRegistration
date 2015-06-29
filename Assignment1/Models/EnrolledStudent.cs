using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class EnrolledStudent
    {
        public int Id { get; set; }
        public string StuName { get; set; }
        public DateTime DateEnrolled { get; set; }
        public int TutorialId { get; set; }
    }
}