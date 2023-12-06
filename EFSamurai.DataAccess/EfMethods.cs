using EFSamurai.Domain;
using EFSamurai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EFSamurai.DataAccess
{
    public static class EfMethods
    {
        #region Create Methods
        public static int CreateSamurai(Samurai samurai)
        {
            using SamuraiDbContext db = new();
            db.Samurai.Add(samurai);
            db.SaveChanges();
            return samurai.Id;
        }

        public static int CreateSamurai(string name)
        {
            return CreateSamurai(new Samurai() { Name = name });
        }

        public static void CreateSamurais(List<Samurai> samurais)
        {
            using SamuraiDbContext db = new();
            db.Samurai.AddRange(samurais);
            db.SaveChanges();
        }

        public static Samurai CreateSamuraiWithRelatedData(Samurai samurai, string realName, List<Quote> quotes, List<Battle> battles)
        {
            using SamuraiDbContext db = new();
            using var transaction = db.Database.BeginTransaction();

            try
            {
                samurai.Quotes = quotes;

                samurai.SamuraiBattles = new List<SamuraiBattle>();
                foreach (Battle battle in battles)
                {
                    samurai.SamuraiBattles.Add(new SamuraiBattle() { BattleId = battle.Id });
                }

                samurai.SecretIdentity = new() { RealName = realName };
                db.Samurai.Add(samurai);
                db.SaveChanges();
                transaction.Commit();

                return samurai;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");

                throw;
            }
        }

        public static int CreateSamuraiWithRelatedData(Samurai samurai)
        {
            using SamuraiDbContext db = new();
            using var transaction = db.Database.BeginTransaction();

            try
            {
                db.Samurai.Add(samurai);

                if (samurai.SecretIdentity == null)
                {
                    samurai.SecretIdentity = new SecretIdentity();
                }

                if (samurai.Quotes == null)
                {
                    samurai.Quotes = new List<Quote>();
                }

                if (samurai.SamuraiBattles == null)
                {
                    samurai.SamuraiBattles = new List<SamuraiBattle>();
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
        public static int CreateBattle(Battle battle)
        {
            using SamuraiDbContext db = new();
            db.Battle.Add(battle);
            db.SaveChanges();
            return battle.Id;
        }
        #endregion

        #region Delete Methods
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
        #endregion

        #region Update Methods
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
        public static bool UpdateSamuraiSetSecretIdentityRealName(int samuraiId, string realName)
        {
            using SamuraiDbContext db = new();
            Samurai? samurai = db.Samurai.Where(s => s.Id == samuraiId)
                .Include(s => s.SecretIdentity).SingleOrDefault();

            if (samurai is not null)
            {
                if (samurai.SecretIdentity is null)
                {
                    samurai.SecretIdentity = new();
                }

                samurai.SecretIdentity.RealName = realName;
                db.Update(samurai.SecretIdentity);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region Read Methods
        public static List<string> StringifyQuotesOfStyleAddSamuraiName(QuoteStyle quoteStyle)
        {
            List<string> stringified = new();
            using SamuraiDbContext db = new();
            List<Quote> quotes = db.Quote.Where(q => q.Style == quoteStyle)
                .Include(q => q.Samurai).ToList();

            foreach (Quote quote in quotes)
            {
                stringified.Add($"'{quote.Text}' is a {quote.Style} quote by {quote.Samurai?.Name ?? "Unknown"}");
            }
            return stringified;
        }

        public static string StringifySamuraiWithRelatedData(int samuraiId)
        {
            StringBuilder samuraiText = new();
            using SamuraiDbContext db = new();
            Samurai? samurai = db.Samurai
                .Where(s => s.Id == samuraiId)
                .Include(s => s.SecretIdentity)
                .Include(s => s.Quotes)
                .Include(s => s.SamuraiBattles)!
                .ThenInclude(sb => sb.Battle)
                .ThenInclude(b => b!.BattleLog)
                .ThenInclude(bl => bl!.BattleEvents)
                .SingleOrDefault();

            if (samurai != null)
            {
                samuraiText.AppendLine(new string('=', 25));
                samuraiText.AppendLine("Name: " + samurai.Name);
                samuraiText.AppendLine("Secret Identity: " + samurai.SecretIdentity?.RealName);
                samuraiText.AppendLine("Hairstyle: " + samurai.HairStyle.ToString());
                if (samurai.Quotes != null)
                {
                    samuraiText.AppendLine(new string('-', 25));
                    samuraiText.AppendLine("Quotes:");
                    foreach (Quote quote in samurai.Quotes)
                    {
                        samuraiText.AppendLine("\t" + quote.Text);
                    }
                }
                if (samurai.SamuraiBattles != null)
                {
                    samuraiText.AppendLine(new string('-', 25));
                    samuraiText.AppendLine("Battles: ");
                    foreach (SamuraiBattle link in samurai.SamuraiBattles)
                    {
                        samuraiText.AppendLine(link.Battle?.Name);
                        samuraiText.AppendLine(link.Battle?.BattleLog?.ToString());
                    }
                }
            }
            return samuraiText.ToString();
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
        public static List<string> ReadAllSamuraiNames()
        {
            using SamuraiDbContext db = new();
            return db.Samurai.Select(s => s.Name).ToList();
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

        public static ICollection<string> StringifySamuraiNamesAddAliases()
        {
            using SamuraiDbContext db = new();
            List<string> stringified = new();

            var samuraiSecrets = db.Samurai.Include(s => s.SecretIdentity);
            foreach (Samurai? samurai in samuraiSecrets)
            {
                stringified.Add($"{samurai.SecretIdentity?.RealName ?? "Unknown"} alias {samurai.Name}");
            }

            return stringified;
        }

        public static ICollection<string> StringifyBattlesWithLog(DateTime from, DateTime to, bool isBrutal)
        {
            using SamuraiDbContext db = new();
            List<string> stringified = new();
            const int Padding = -20;

            var battles = db.Battle.Where(b => b.StartDate >= from && b.StartDate <= to)
                .Include(b => b.BattleLog)
                .ThenInclude(bl => bl!.BattleEvents);

            foreach (Battle b in battles)
            {
                stringified.Add($"----------------------------------------------------------------------" +
                              $"\n{"Name of Battle",Padding}{b.Name,Padding}" +
                              $"\n{"Log Name",Padding}{b.BattleLog?.Name,Padding}");

                if (b.BattleLog != null && b.BattleLog.BattleEvents != null)
                {
                    foreach (BattleEvent bEvent in b.BattleLog.BattleEvents)
                    {
                        stringified.Add($"{"Event",Padding}Order {bEvent.Order}: {bEvent.Summary}");
                    }
                }
            }

            return stringified;
        }
        #endregion
    }
}
