using Employee.Domain;
using Employee.Domain.Entities;
using Employee.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;

        public EmployeesController(IEmployeeService employeeService, IHubContext<BroadcastHub, IHubClient> hubContext)
        {
            _employeeService = employeeService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetEmployee()
        {
            return Ok(await _employeeService.GetAllEmployees());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetEmployee(string id)
        {
            var employee = await _employeeService.GetEmployeeInfoBasedOnId(id);

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

            try
            {
                await _employeeService.UpdateEmployee(employee);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            await _hubContext.Clients.All.BroadcastMessage();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostEmployee(User employee)
        {
            try
            {
                await _employeeService.CreateEmployee(employee);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            await _hubContext.Clients.All.BroadcastMessage();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            await _employeeService.DeleteEmployee(id);
            await _hubContext.Clients.All.BroadcastMessage();

            return NoContent();
        }
    }
}
