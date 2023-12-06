using EFSamurai.DataAccess;
using EFSamurai.Domain.Entities;
using System.Collections.Generic;

namespace EFSamurai.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SamuraiDbContext.ClearAllData();
            EfMethods.CreateSamurais(new() {
                    new(){ Name = "Oda Nobunaga"},
                    new(){ Name = "Musashi Miyamoto"},
                    new(){ Name = "Date Masamune"},
                    new(){ Name = "Saido Takamori"},
                  }
             );

            //OutputConsole.DisplayTitle("Samurai");
            //OutputConsole.DisplayStringList(EfMethods.ReadAllSamuraiNames());
            //var printList = EfMethods.ReadSamuraisOrderById();
            //foreach (Samurai samurai in printList)
            //{
            //    Console.WriteLine(samurai.Id + " " + samurai.Name);
            //}

            Battle battle = new()
            {
                Name = "The Genpei War",
                Description = "A power struggle between the Taira and Minamoto clans sparked the war which resulted in the establishment of the Kamakura shogunate.",
                IsBrutal = true,
                StartDate  = new DateTime(1180, 1, 1),
                EndDate = new DateTime(1185, 1, 1),
                BattleLog = new BattleLog()
                {
                    Name = "The Genpei War Log",
                    BattleEvents = new List<BattleEvent>()
                    {
                        new BattleEvent()
                        {
                            Order = 1,
                            Summary = "A struggle begins between the Taira and Minamoto clans.",
                        },
                        new BattleEvent()
                        {
                            Order = 2,
                            Summary = "The Genpei War begins.",
                        },
                        new BattleEvent()
                        {
                            Order = 3,
                            Summary = "The downfall of the Taira clan.",
                        },
                        new BattleEvent()
                        {
                            Order = 4,
                            Summary = "Morimoto no Yoritomo appoints himself as the Sei-i Taishogun.",
                        }
                    }
                }
            };

            EfMethods.CreateBattle(battle);

            List<Quote> quotes = new()
            {
                new()
                {
                    Text = "Blegh!", Style = Domain.QuoteStyle.Lame
                },
                new()
                {
                    Text = "What is a man? A miserable pile of secrets.", Style = Domain.QuoteStyle.Awesome
                },
                new()
                {
                    Text = "Enough talk. Have at you!", Style = Domain.QuoteStyle.Cheesy
                }
            };
            List<Battle> battleList = new() { battle };
            Samurai dracula = new() { Name = "Dracula", HairStyle = Domain.HairStyle.Western};
            dracula = EfMethods.CreateSamuraiWithRelatedData(dracula, "Vlad Tepes", quotes, battleList);

            List<Quote> alQuotes = new()
            {
                new() { Text = " Dracula! In the name of my Mother, I will defeat you again!", Style = Domain.QuoteStyle.Awesome}
            };

            Samurai alucard = new()
            {
                Name = "Alucard",
                HairStyle = Domain.HairStyle.Western,
                Quotes = alQuotes,
                SecretIdentity = new()
            };

            int alucardId = EfMethods.CreateSamuraiWithRelatedData(alucard);
            EfMethods.LinkBattleAndSamurais(1, new() { alucard.Id });
            EfMethods.UpdateSamuraiSetSecretIdentityRealName(alucard.Id, "Adrian Tepes");
            Console.WriteLine(EfMethods.PrintSamuraiWithRelatedData(dracula.Id));
            Console.WriteLine(EfMethods.PrintSamuraiWithRelatedData(alucardId));
        }
    }
}
