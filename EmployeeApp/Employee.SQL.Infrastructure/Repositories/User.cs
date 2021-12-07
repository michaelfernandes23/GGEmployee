using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Employee.SQL.Infrastructure.Repositories
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
    }
}
