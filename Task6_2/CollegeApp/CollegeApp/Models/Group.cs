using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using CollegeApp.Models;

namespace CollegeApp.Models
{
    public class Group
    {
        [PrimaryKey, AutoIncrement]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int SpecialtyId { get; set; }
    }
}
