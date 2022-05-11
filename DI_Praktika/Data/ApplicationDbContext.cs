using DI_Praktika.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI_Praktika.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Car>()
                .HasKey(c => new { c.ID });

            builder.Entity<Query>()
                .HasKey(l => new { l.ID });

            builder.Entity<Status>()
                .HasKey(l => new { l.ID });

            builder.Entity<Car>()
                .HasMany(c => c.Queries)
                .WithOne(t => t.Car);

            builder.Entity<User>()
                .HasMany(l => l.Query)
                .WithOne(t => t.User);

            builder.Entity<Status>()
                .HasMany(l => l.Queries)
                .WithOne(t => t.StatusObject);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DI;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
