using Employee.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<User>> GetAllEmployees();
        Task<User> GetEmployeeInfoBasedOnId(string id);
        Task CreateEmployee(User employee);
        Task UpdateEmployee(User employee);
        Task DeleteEmployee(string id);
    }
}
