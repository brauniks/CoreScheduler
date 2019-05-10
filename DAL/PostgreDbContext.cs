using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DAL
{
    public class PostgreDbContext : DbContext
    {
        public PostgreDbContext()
        {

        }

        public DbSet<Hashes> Hashes { get; set; }
        public DbSet<Websites> Websites { get; set; }

        public PostgreDbContext(DbContextOptions<PostgreDbContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
