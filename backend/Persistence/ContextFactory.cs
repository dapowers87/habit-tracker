using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence
{
    public interface IContextFactory<TContext> where TContext : DbContext
    {
        TContext CreateContext();
    }

    public class ContextFactory<TContext> : IContextFactory<TContext> where TContext : DbContext
    {
        private readonly string _connectionString;

        public ContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TContext>()
                .UseSqlite(_connectionString)
                .Options;

            return (TContext)Activator.CreateInstance(typeof(TContext), options);
        }
    }
}
