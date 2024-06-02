using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace BadgeConnector.Models
{
    public class LedContext : DbContext
    {
        public DbSet<Badge> Badges { get; set; }
        public DbSet<BadgeGroup> BadgeGroups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("your-connection-string-here", 
                new MySqlServerVersion(new Version(8, 0, 21))
            );
        }
    }
}