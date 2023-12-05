using EFSamurai.Domain;
using EFSamurai.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFSamurai.DataAccess
{
    public static class EfMethods
    {
        public static int CreateSamurai(Samurai samurai)
        {
            using SamuraiDbContext db = new();
            db.Samurai.Add(samurai);
            db.SaveChanges();
            // Id got a new value (is no longer 0) when we did SaveChanges(), above.
            return samurai.Id;
        }

        public static int CreateSamurai(string name)
        {
            // NOTE: We don't want to make two bodies of the same method if we don't
            // have to. So in this (the more restrictive) version, we just reshape the
            // in-parameter and call the other version.
            return CreateSamurai(new Samurai() { Name = name });
        }

        public static List<string> ReadAllSamuraiNames()
        {
            using SamuraiDbContext db = new();
            return db.Samurai.Select(s => s.Name).ToList();
        }

        public static void CreateSamurais(List<Samurai> samurais)
        {
            using SamuraiDbContext db = new();
            db.Samurai.AddRange(samurais);
            db.SaveChanges();
        }

        public static Samurai? ReadSamurai(int id)
        {

            using SamuraiDbContext db = new();

            return db.Samurai.Where(s => s.Id == id)
                .Include(s => s.SecretIdentity)
                .Include(s => s.Quotes)
                .Include(s => s.SamuraiBattles)!
                .ThenInclude(sb => sb.Battle)
                .ThenInclude(b => b!.BattleLog)
                .ThenInclude(bl => bl!.BattleEvents)
                .SingleOrDefault();
        }

        public static void DeleteSamurai(Samurai samurai)
        {
            using SamuraiDbContext db = new();
            db.Remove(samurai);
            db.SaveChanges();
        }

        public static bool DeleteSamurai(int id)
        {
            using SamuraiDbContext db = new();

            Samurai? samurai = db.Samurai.Where(s => s.Id == id).SingleOrDefault();
            if (samurai is not null)
            {
                DeleteSamurai(samurai);
                return true;
            }
            return false;
        }

        public static bool UpdateSamuraiSetSecretIdentityRealName(int samuraiId, string realName)
        {
            using SamuraiDbContext db = new();
            SecretIdentity? secretIdentity = db.SecretIdentity.Where(s => s.SamuraiID == samuraiId).SingleOrDefault();

            if (secretIdentity is not null)
            {
                secretIdentity.RealName = realName;
                db.Update(secretIdentity);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static int CreateBattle(Battle battle)
        {
            using SamuraiDbContext db = new();
            db.Battle.Add(battle);
            db.SaveChanges();
            //If the battle object in the in parameter has battle log with battle events attached
            //those objects will also be added in their respective columns automatically

            return battle.Id;
        }

        public static void LinkBattleAndSamurais(int battleId, List<int> samuraiIds)
        {
            using SamuraiDbContext db = new();

            if (db.Battle.Any(b => b.Id == battleId))
            {
                List<SamuraiBattle> links = new List<SamuraiBattle>();

                foreach (int id in samuraiIds)
                {
                    Samurai? samurai = ReadSamurai(id);

                    if (samurai is not null)
                    {

                        // Check if the link doesn't already exist
                        if (!db.SamuraiBattles.Any(sb => sb.SamuraiId == id && sb.BattleId == battleId))
                        {
                            SamuraiBattle link = new SamuraiBattle() { SamuraiId = id, BattleId = battleId };
                            links.Add(link);
                        }
                    }
                }

                db.SamuraiBattles.AddRange(links);
                db.SaveChanges();
            }
        }

        public static int CountBattlesForSamurai(int samuraiId, bool? isBrutal = null)
        {
            using SamuraiDbContext db = new();

            IQueryable<SamuraiBattle> samuraiBattles = db.SamuraiBattles
                .Where(sb => sb.SamuraiId == samuraiId);

            if (isBrutal != null)
            {
                samuraiBattles = samuraiBattles
                    .Where(sb => sb.Battle!.IsBrutal == isBrutal);
            }

            return samuraiBattles.Count();
        }

        public static List<Samurai> ReadSamuraisOrderById()
        {
            using SamuraiDbContext db = new();
            return db.Samurai.OrderBy(s => s.Id).ToList();
        }

        public static List<Quote> ReadQuotesOfStyle(QuoteStyle quoteStyle)
        {
            using SamuraiDbContext db = new();
            return db.Quote.Where(q => q.Style == quoteStyle).ToList();
        }

        public static Samurai CreateSamuraiWithRelatedData(Samurai samurai, string secretIdentity, List<Quote> quotes, List<Battle> battles)
        {
            using SamuraiDbContext db = new();
            samurai.Quotes = quotes;
            db.Samurai.Add(samurai);
            db.SaveChanges();

            List<int> idList = new() { samurai.Id };

            foreach (Battle battle in battles)
            {
                LinkBattleAndSamurais(battle.Id, idList);
            }

            UpdateSamuraiSetSecretIdentityRealName(samurai.Id, secretIdentity);

            return samurai;
        }
        public static int CreateSamuraiWithRelatedData(Samurai samurai)
        {
            //using SamuraiDbContext db = new();
            //db.Samurai.Add(samurai);

            //if (samurai.SecretIdentity != null)
            //{
            //    db.SecretIdentity.Add(samurai.SecretIdentity);
            //}

            //if (samurai.Quotes != null)
            //{
            //    db.Quote.AddRange(samurai.Quotes);
            //}

            //if (samurai.SamuraiBattles != null)
            //{
            //    foreach (SamuraiBattle link in samurai.SamuraiBattles)
            //    {
            //        LinkBattleAndSamurais(link.BattleId, new() { samurai.Id});
            //    }
            //}

            //db.SaveChanges();
            //return samurai.Id;

            using SamuraiDbContext db = new();
            using var transaction = db.Database.BeginTransaction();

            try
            {
                db.Samurai.Add(samurai);
                db.SaveChanges();

                if (samurai.SecretIdentity != null)
                {
                    db.SecretIdentity.Add(samurai.SecretIdentity);
                }

                if (samurai.Quotes != null)
                {
                    db.Quote.AddRange(samurai.Quotes);
                }

                if (samurai.SamuraiBattles != null)
                {
                    foreach (SamuraiBattle link in samurai.SamuraiBattles)
                    {
                        LinkBattleAndSamurais(link.BattleId, new List<int> { link.SamuraiId });
                    }
                }

                db.SaveChanges();
                transaction.Commit();

                return samurai.Id;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
