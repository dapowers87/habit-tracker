using System;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence
{
    public class SbrContext : DbContext
    {
        public SbrContext()
        {
            
        }
        public SbrContext(DbContextOptions<SbrContext> options) : base(options) { }

        public DbSet<LoginAudit> LoginAudits { get; set; }
        public DbSet<Window> Windows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=/tmp/sbr.db");
    }
}
