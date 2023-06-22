using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using CollegeApp.Models;

namespace CollegeApp.Models
{
    public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsBudget { get; set; }
        public int Course { get; set; }
        public int GroupId { get; set; }
        public string Photo { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
