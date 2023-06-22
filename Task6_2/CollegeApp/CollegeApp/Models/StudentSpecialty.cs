using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CollegeApp.Models
{
    public class StudentSpecialty
    {
        [PrimaryKey, AutoIncrement]
        public int StudentSpecialtyId { get; set; }
        public int StudentId { get; set; }
        public int SpecialtyId { get; set; }
    }
}
