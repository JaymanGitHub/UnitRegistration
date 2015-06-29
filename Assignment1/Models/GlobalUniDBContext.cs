using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class GlobalUniDBContext : DbContext
    {
        public GlobalUniDBContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Teaching> Teachings { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<EnrolledStudent> EnrolledStudents { get; set; }
    }

}