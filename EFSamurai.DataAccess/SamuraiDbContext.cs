using Microsoft.EntityFrameworkCore;
using EFSamurai.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EFSamurai.DataAccess
{
    public class SamuraiDbContext : DbContext
    {
        public DbSet<Samurai> Samurai => Set<Samurai>();
        public DbSet<Quote> Quote => Set<Quote>();
        public DbSet<SecretIdentity> SecretIdentity => Set<SecretIdentity>();
        public DbSet<Battle> Battle => Set<Battle>();
        public DbSet<SamuraiBattle> SamuraiBattles => Set<SamuraiBattle>();
        public DbSet<BattleLog> BattleLog => Set<BattleLog>();
        public DbSet<BattleEvent> BattleEvent => Set<BattleEvent>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            @"Server = (localdb)\MSSQLLocalDB; " +
            "Database = EFSamurai; " +
            "Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
        }

        public static void RebuildDatabase()
        {
            using SamuraiDbContext db = new();
            // Deletes the entire database:
            db.Database.EnsureDeleted();
            // Recreates the DB tables, based on the Migrations folder data.
            db.Database.Migrate();
            // NOTE: EnsureCreated(), below, also recreates the DB tables. But
            // it doesn't play well with migrations. Only use it if you
            // don't plan to build further on DB with migrations later.
            //db.Database.EnsureCreated();
        }

        public void ResetIdentityStartingValue(string tableName, int startingValue = 1)
        {
            Database.ExecuteSqlRaw("IF EXISTS(SELECT * FROM sys.identity_columns " +
            "WHERE OBJECT_NAME(OBJECT_ID) = @tableName AND last_value IS NOT NULL) " +
            "DBCC CHECKIDENT(@tableName, RESEED, @startingValueMinusOne);",
            new SqlParameter("tablename", tableName),
            new SqlParameter("startingValueMinusOne", startingValue - 1));
        }

        public static void ClearAllData()
        {
            using SamuraiDbContext db = new();

            // 1) Note: As cascading delete is on (by default), it is enough to remove Samurais and Battles.
            // Deleting the rows in these two tables drags ("cascades") everything else with them.
            db.Samurai.ExecuteDelete();
            db.Battle.ExecuteDelete();
            // Obsolete method: db.RemoveRange(db.Battle);

            // 2) Restart IDENTITY counting at 1 for tables with auto-incrementing PKs (6 of the 7 EfSamurai tables).
            // (Note: Hardcoded tablenames, doublecheck singular/plural vs. your naming convention.)
            db.ResetIdentityStartingValue("Samurai");
            db.ResetIdentityStartingValue("SecretIdentity");
            db.ResetIdentityStartingValue("Quote");
            db.ResetIdentityStartingValue("Battle");
            db.ResetIdentityStartingValue("BattleLog");
            db.ResetIdentityStartingValue("BattleEvent");
            db.SaveChanges();
        }
    }
}
