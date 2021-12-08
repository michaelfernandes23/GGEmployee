using Employee.Interfaces;
using MassTransit;
using System.Threading.Tasks;

namespace Employee.API.EventConsumer
{
    public class EmployeeDeletedEvent : IConsumer<EmployeeTransaction>
    {
        private readonly IMessageService _messageService;
        public EmployeeDeletedEvent(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task Consume(ConsumeContext<EmployeeTransaction> context)
        {
            try
            {
                await _messageService.DeleteEmployee(context.Message);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
