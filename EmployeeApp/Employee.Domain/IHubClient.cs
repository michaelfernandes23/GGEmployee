using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Domain
{
    public interface IHubClient
    {
        Task BroadcastMessage();
    }
}
