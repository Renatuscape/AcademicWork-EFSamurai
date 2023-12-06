using System;
using System.Collections.Generic;
using System.Linq;
using EFSamurai.Domain;
using EFSamurai.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFSamurai.DataAccess
{
	public class EfTddMethods
	{
		#region Helper methods

		public static void RebuildDatabase()
		{
			using SamuraiDbContext db = new();

			db.Database.EnsureDeleted();
			db.Database.Migrate();
			//db.Database.EnsureCreated();
		}


		public static void ClearAllData()
		{
			using SamuraiDbContext db = new();

			db.RemoveRange(db.Samurai);
			db.RemoveRange(db.Battle);

			db.ResetIdentityStartingValue("Samurai");
			db.ResetIdentityStartingValue("SecretIdentity");
			db.ResetIdentityStartingValue("Quote");
			db.ResetIdentityStartingValue("Battle");
			db.ResetIdentityStartingValue("BattleLog");
			db.ResetIdentityStartingValue("BattleEvent");

			db.SaveChanges();
		}

		#endregion Helper methods

		public static int CreateSamurai(string name, HairStyle? hairStyle)
		{
			Samurai samurai = new() { Name = name, HairStyle = hairStyle };
			return CreateSamurai(samurai);
		}

		public static int CreateSamurai(Samurai samurai)
		{
            using SamuraiDbContext db = new();
            db.Add(samurai);
            db.SaveChanges();
            return samurai.Id;
        }
		public static List<string> ReadAlphabeticallyAllSamuraiNamesWithSpecificHairstyle(HairStyle hairStyle)
		{
			using SamuraiDbContext db = new();
			return db.Samurai.Where(s => s.HairStyle == hairStyle).OrderBy(s => s.Name).Select(s => s.Name).ToList();
		}

		public static List<Quote> ReadAllQuotesWithSpecificQuoteStyle(QuoteStyle quoteStyle)
		{
            using SamuraiDbContext db = new();
            return db.Quote
				.Include(q => q.Samurai)
				.Where(q => q.Style == quoteStyle).ToList();
        }

		public static int CreateBattle(string battleName, bool isBrutal, string description, DateTime startDate, DateTime endDate)
		{
			Battle battle = new Battle() { Name = battleName, IsBrutal = isBrutal, Description = description, StartDate = startDate, EndDate = endDate};
            using SamuraiDbContext db = new();
			db.Battle.Add(battle);
			db.SaveChanges();
			return battle.Id;
		}
        public static Battle? ReadOneBattle(int battleId)
		{
            using SamuraiDbContext db = new();
            return db.Battle.Where(b => b.Id == battleId).SingleOrDefault();
		}

        public static void CreateOrUpdateSecretIdentitySetRealName(int samuraiId, string name)
		{
            using SamuraiDbContext db = new();
			Samurai? samurai = db.Samurai
				.Include(s => s.SecretIdentity)
				.Where(s => s.Id == samuraiId).SingleOrDefault();

			if (samurai is not null)
			{
                samurai.SecretIdentity ??= new SecretIdentity();
                samurai.SecretIdentity.RealName = name;

				db.SaveChanges();
            }
        }

        public static SecretIdentity? ReadSecretIdentityOfSpecificSamurai(int samuraiId)
		{
            using SamuraiDbContext db = new();
            Samurai? samurai = db.Samurai
				.Include(s => s.SecretIdentity)
				.Where(s => s.Id == samuraiId).SingleOrDefault();
			return samurai?.SecretIdentity;
        }
    }
}
