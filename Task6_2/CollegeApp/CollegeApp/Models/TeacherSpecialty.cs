using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CollegeApp.Models
{
    public class TeacherSpecialty
    {
        [PrimaryKey, AutoIncrement]
        public int TeacherSpecialtyId { get; set; }
        public int TeacherId { get; set; }
        public int SpecialtyId { get; set; }
    }
}
