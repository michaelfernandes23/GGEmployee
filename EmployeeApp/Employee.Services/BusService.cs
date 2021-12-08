using Employee.Interfaces;
using MassTransit;
using System.Threading.Tasks;

namespace Employee.Services
{
    public class BusService : IBusService
    {
        private readonly IBusControl _busControl;

        public BusService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task Publish(object message)
        {
            await _busControl.Publish(message);
        }
    }
}
