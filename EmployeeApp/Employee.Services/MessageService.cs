using Employee.Domain.Entities;
using Employee.Interfaces;
using Employee.SQL.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Employee.Services
{
    public class MessageService : IMessageService
    {
        private readonly ServiceDbContext _context;

        public MessageService(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task CreateEmployee(EmployeeTransaction transaction)
        {
            User employee = transaction.User;
            employee.Id = Guid.NewGuid().ToString();
            _context.User.Add(employee);

            _context.Notification.Add(transaction.Notification);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployee(EmployeeTransaction transaction)
        {
            User employee = transaction.User;
            _context.Entry(employee).State = EntityState.Modified;

            _context.Notification.Add(transaction.Notification);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployee(EmployeeTransaction transaction)
        {
            var employee = await _context.User.FindAsync(transaction.User.Id);

            _context.User.Remove(employee);
            _context.Notification.Add(transaction.Notification);

            await _context.SaveChangesAsync();
        }
    }
}
