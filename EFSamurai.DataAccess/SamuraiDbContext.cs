using Microsoft.EntityFrameworkCore;
using EFSamurai.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.DataAccess
{
    public class SamuraiDbContext : DbContext
    {
        public DbSet<Samurai> Samurai => Set<Samurai>();
        public DbSet<Quote> Quote => Set<Quote>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            @"Server = (localdb)\MSSQLLocalDB; " +
            "Database = EFSamurai; " +
            "Trusted_Connection = True;");
        }
    }
}
