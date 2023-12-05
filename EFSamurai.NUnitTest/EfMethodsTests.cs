using EFSamurai.Domain;
using EFSamurai.DataAccess;
using EFSamurai.Domain.Entities;

namespace EFSamurai.NUnitTest
{
    public class Tests
    {
        [OneTimeSetUp] // Marks a method to be called ONCE, at start of a test-run.
        public void OneTimeSetup()
        {
            SamuraiDbContext.RebuildDatabase();
        }

        [SetUp]
        public void Setup()
        {
            SamuraiDbContext.ClearAllData();
            EfMethods.CreateSamurais(new() {
                    new(){ Name = "Oda Nobunaga"},
                    new(){ Name = "Musashi Miyamoto"},
                    new(){ Name = "Date Masamune"},
                    new(){ Name = "Saido Takamori"},
                  }
             );

            Battle brutalBattle = new()
            {
                Name = "Brutal Battle",
                Description = "A brutal battle.",
                IsBrutal = true,
                StartDate = new DateTime(1180, 1, 1),
                EndDate = new DateTime(1185, 1, 1),
                BattleLog = new BattleLog()
                {
                    Name = "The Brutal Battle Log",
                    BattleEvents = new List<BattleEvent>()
                    {
                        new BattleEvent()
                        {
                            Order = 1,
                            Summary = "The brutal battle begins.",
                        },
                        new BattleEvent()
                        {
                            Order = 2,
                            Summary = "The brutal battle ends.",
                        },
                    }
                }
            };
            EfMethods.CreateBattle(brutalBattle);

            Battle deadlyBattle = new()
            {
                Name = "Deadly Battle",
                Description = "A deadly battle.",
                IsBrutal = true,
                StartDate = new DateTime(1180, 1, 1),
                EndDate = new DateTime(1185, 1, 1),
                BattleLog = new BattleLog()
                {
                    Name = "The Deadly Battle Log",
                    BattleEvents = new List<BattleEvent>()
                    {
                        new BattleEvent()
                        {
                            Order = 1,
                            Summary = "The deadly battle begins.",
                        },
                        new BattleEvent()
                        {
                            Order = 2,
                            Summary = "The deadly battle ends.",
                        },
                    }
                }
            };
            EfMethods.CreateBattle(deadlyBattle);

            Battle mockBattle = new()
            {
                Name = "Mock Battle",
                Description = "A practice run.",
                IsBrutal = false,
                StartDate = new DateTime(1180, 1, 1),
                EndDate = new DateTime(1185, 1, 1),
                BattleLog = new BattleLog()
                {
                    Name = "The Mock Battle Log",
                    BattleEvents = new List<BattleEvent>()
                    {
                        new BattleEvent()
                        {
                            Order = 1,
                            Summary = "The mock battle begins.",
                        },
                        new BattleEvent()
                        {
                            Order = 2,
                            Summary = "The mock battle ends.",
                        },
                    }
                }
            };
            EfMethods.CreateBattle(mockBattle);
        }

        [Test]
        public void ReadSamuraiNames_TwoSamurais_NamesAreCorrect()
        {
            // Arrange
            SamuraiDbContext.ClearAllData();
            EfMethods.CreateSamurai("Zelda");
            EfMethods.CreateSamurai("Link");

            // Act
            List<string> result = EfMethods.ReadAllSamuraiNames();

            // Assert
            CollectionAssert.AreEqual(new List<string>() { "Zelda", "Link" }, result);
        }

        [Test]
        public void CreateSamurais_AddRange_ReturnCorrectCount()
        {
            int expected = 4;

            Assert.That(EfMethods.ReadAllSamuraiNames().Count, Is.EqualTo(expected));
        }

        [TestCase(10, false)]
        [TestCase(1, true)]
        public void DeleteSamurai_EnterId_ReturnCorrectBool(int id, bool expected)
        {
            //Act
            bool isDeleted = EfMethods.DeleteSamurai(id);
            Assert.That(isDeleted, Is.EqualTo(expected));
        }

        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        public void DeleteSamurai_EnterId_ActuallyDeletedFromTable(int id, bool expected)
        {
            SamuraiDbContext.ClearAllData();
            //Arrange
            List<Samurai> samuraiList = new() {
                    new(){ Name = "Oda Nobunaga"},
                    new(){ Name = "Musashi Miyamoto"},
                    new(){ Name = "Date Masamune"},
                    new(){ Name = "Saido Takamori"},
                  };

            EfMethods.CreateSamurais(samuraiList);

            //Act
            EfMethods.DeleteSamurai(samuraiList[id - 1]);
            bool isDeleted = EfMethods.ReadSamurai(id) == null;

            Assert.That(isDeleted, Is.EqualTo(expected));
        }

        [TestCase(null, 3)]
        [TestCase(true, 2)]
        [TestCase(false, 1)]
        public void CountBattlesForSamurai_SearchSettingIn_CorrectCountOut(bool? searchSetting, int expected)
        {
            EfMethods.LinkBattleAndSamurais(1, new() { 1 });
            EfMethods.LinkBattleAndSamurais(2, new() { 1 });
            EfMethods.LinkBattleAndSamurais(3, new() { 1 });

            int battleCount = EfMethods.CountBattlesForSamurai(1, searchSetting);
            Assert.That(battleCount, Is.EqualTo(expected));
        }

        [Test]
        public void LinkBattleAndSamurais_ThreeSamuraiIn_ThreeSamuraisLinked()
        {
            List<int> samuraiIdList = new() { 1 };
            EfMethods.LinkBattleAndSamurais(1, samuraiIdList);
            Samurai? samurai = EfMethods.ReadSamurai(1);
            Assert.That(samurai?.SamuraiBattles is not null);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Code to run after all tests in the assembly
            SamuraiDbContext.ClearAllData();
        }
    }
}