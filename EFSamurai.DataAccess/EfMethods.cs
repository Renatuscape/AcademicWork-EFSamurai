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
            List<SamuraiBattle> linkObjects = db.SamuraiBattles.Where(sb => sb.SamuraiId == samuraiId)
                .Include(sb => sb.Battle)
                .ToList();
            
            if (isBrutal != null)
            {
                foreach (SamuraiBattle link in linkObjects)
                {
                    if (link.Battle?.IsBrutal != isBrutal)
                    {
                        linkObjects.Remove(link);
                    }
                }
            }

            return linkObjects.Count;
        }
    }
}
