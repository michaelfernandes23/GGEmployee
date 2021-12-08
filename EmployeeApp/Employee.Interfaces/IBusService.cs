using System.Threading.Tasks;

namespace Employee.Interfaces
{
    public interface IBusService
    {
        Task Publish(object message);
    }
}
