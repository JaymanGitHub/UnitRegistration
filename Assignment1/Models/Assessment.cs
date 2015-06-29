using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public string StuName { get; set; }
        public string FileName { get; set; }
        public DateTime DateUpload { get; set; }
        public string Grade { get; set; }
        public string Guid { get; set; }

    }
}