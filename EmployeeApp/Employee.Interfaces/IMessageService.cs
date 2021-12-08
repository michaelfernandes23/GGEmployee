using Employee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Interfaces
{
    public interface IMessageService
    {
        Task CreateEmployee(EmployeeTransaction transaction);
        Task UpdateEmployee(EmployeeTransaction transaction);
        Task DeleteEmployee(EmployeeTransaction transaction);
    }
}
