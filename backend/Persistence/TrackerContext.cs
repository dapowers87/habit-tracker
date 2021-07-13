using System;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence
{
    public class TrackerContext : DbContext
    {
        public TrackerContext()
        {
            
        }
        public TrackerContext(DbContextOptions<TrackerContext> options) : base(options) { }

        public DbSet<LoginAudit> LoginAudits { get; set; }
        public DbSet<Window> Windows { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=/tmp/tracker.db");
    }
}
