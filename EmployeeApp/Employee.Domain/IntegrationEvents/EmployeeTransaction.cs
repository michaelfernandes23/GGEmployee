using Employee.Domain.Entities;

namespace Employee
{
    public class EmployeeTransaction
    {
        public User User { get; set; }
        public Notification Notification { get; set; }
    }
}
