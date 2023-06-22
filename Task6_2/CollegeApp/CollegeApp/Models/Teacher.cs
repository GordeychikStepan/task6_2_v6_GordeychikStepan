using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using CollegeApp.Models;

namespace CollegeApp.Models
{
    public class Teacher
    {
        [PrimaryKey, AutoIncrement]
        public int TeacherId { get; set; }
        public string FullName { get; set; }
        public int Workload { get; set; }
        public int SpecialtyId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmission { get; set; }
    }
}
