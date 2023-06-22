using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using CollegeApp.Models;

namespace CollegeApp.Models
{
    public class Specialty
    {
        [PrimaryKey, AutoIncrement]
        public int SpecialtyId { get; set; }
        public string Name { get; set; }
    }
}
