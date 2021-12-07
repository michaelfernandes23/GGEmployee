using Employee.Domain;
using Employee.SQL.Infrastructure.Data;
using Employee.SQL.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ServiceDbContext _context;
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;

        public EmployeesController(ServiceDbContext context, IHubContext<BroadcastHub, IHubClient> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetEmployee()
        {
            return await _context.User.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetEmployee(string id)
        {
            var employee = await _context.User.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(string id, User employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            Notification notification = new Notification()
            {
                EmployeeName = employee.Name,
                TransactionType = "Edit"
            };
            _context.Notification.Add(notification);

            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.BroadcastMessage();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostEmployee(User employee)
        {
            employee.Id = Guid.NewGuid().ToString();
            _context.User.Add(employee);

            //Createing Notification
            Notification notification = new Notification()
            {
                EmployeeName = employee.Name,
                TransactionType = "Add"
            };
            _context.Notification.Add(notification);

            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.BroadcastMessage();
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(employee.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _context.User.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            Notification notification = new Notification()
            {
                EmployeeName = employee.Name,
                TransactionType = "Delete"
            };

            _context.User.Remove(employee);
            _context.Notification.Add(notification);

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.BroadcastMessage();

            return NoContent();
        }

        private bool EmployeeExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
