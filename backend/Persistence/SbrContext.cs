using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Persistence.Entities;

namespace Persistence
{
    public class SbrContext : DbContext
    {
        public DbSet<LoginAudit> LoginAudits { get; set; }
        public DbSet<Window> Windows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=./blogging.db");
    }
}
