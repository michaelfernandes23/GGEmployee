using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Domain.Repositories
{
    public interface IUnitOfWork
    {
        int AffectedRows { get; }

        int Commit();

        Task<int> CommitAsync();
    }
}
