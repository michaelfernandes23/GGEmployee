using Employee.Interfaces;
using MassTransit;
using System.Threading.Tasks;

namespace Employee.API.EventConsumer
{
    public class EmployeeUpdatedEvent : IConsumer<EmployeeTransaction>
    {
        private readonly IMessageService _messageService;
        public EmployeeUpdatedEvent(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task Consume(ConsumeContext<EmployeeTransaction> context)
        {
            try
            {
                await _messageService.UpdateEmployee(context.Message);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
