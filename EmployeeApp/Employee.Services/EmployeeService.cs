using Employee.Domain.Entities;
using Employee.Interfaces;
using Employee.SQL.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IBusService _busService;
        private readonly ServiceDbContext _context;

        public EmployeeService(IBusService busService, ServiceDbContext context)
        {
            _busService = busService;
            _context = context;
        }

        public async Task<List<User>> GetAllEmployees()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetEmployeeInfoBasedOnId(string id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task CreateEmployee(User employee)
        {
            employee.Id = Guid.NewGuid().ToString();

            Notification notification = new Notification()
            {
                EmployeeName = employee.Name,
                TransactionType = "Add"
            };

            await _busService.Publish(new EmployeeTransaction { User = employee, Notification = notification });
        }

        public async Task UpdateEmployee(User employee)
        {
            _context.Entry(employee).State = EntityState.Modified;

            Notification notification = new Notification()
            {
                EmployeeName = employee.Name,
                TransactionType = "Edit"
            };

            await _busService.Publish(new EmployeeTransaction { User = employee, Notification = notification });
        }

        public async Task DeleteEmployee(string id)
        {
            var employee = await _context.User.FindAsync(id);

            Notification notification = new Notification()
            {
                EmployeeName = employee.Name,
                TransactionType = "Delete"
            };

            await _busService.Publish(new EmployeeTransaction { User = employee, Notification = notification });
        }

        private bool EmployeeExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
