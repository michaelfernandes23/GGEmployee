using Employee.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Employee.SQL.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ServiceDbContext dbContext;

        public UnitOfWork(ServiceDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public int AffectedRows { get; private set; }

        public int Commit()
        {
            AffectedRows = dbContext.SaveChanges();
            return AffectedRows;
        }

        public async Task<int> CommitAsync()
        {
            AffectedRows = await dbContext.SaveChangesAsync();
            return AffectedRows;
        }
    }
}
