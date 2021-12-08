using Employee.Interfaces;
using MassTransit;
using System.Threading.Tasks;

namespace Employee.API.EventConsumer
{
    public class EmployeeCreatedEvent : IConsumer<EmployeeTransaction>
    {
        private readonly IMessageService _messageService;
        public EmployeeCreatedEvent(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task Consume(ConsumeContext<EmployeeTransaction> context)
        {
            try
            {
                await _messageService.CreateEmployee(context.Message);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
