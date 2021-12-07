using System;
using System.Collections.Generic;
using System.Text;
using Employee.SQL.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Employee.SQL.Infrastructure.Data
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }

    }
}
