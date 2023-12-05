using EFSamurai.DataAccess;
using EFSamurai.Domain.Entities;

namespace EFSamurai.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SamuraiDbContext.ClearAllData();
            //EfMethods.CreateSamurai("Tomoe Gozen");
            OutputConsole.DisplayTitle("Samurai");
            OutputConsole.DisplayStringList(EfMethods.ReadAllSamuraiNames());

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
                            Summary = "Morimoto no Yoritomo appointed himself as the Sei-i Taishogun.",
                        }
                    }
                }
            };

            EfMethods.CreateBattle(battle);
        }
    }
}
