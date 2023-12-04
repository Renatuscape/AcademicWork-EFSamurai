using EFSamurai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
