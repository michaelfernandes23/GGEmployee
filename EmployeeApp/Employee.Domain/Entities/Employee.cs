using System;
using System.Collections.Generic;
using System.Text;

namespace Employee.Domain.Entities
{
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Company { get; set; }
        public string Cityname { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
    }
}
