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
            //Arrange
            EfMethods.CreateSamurais(new() {
                    new(){ Name = "Oda Nobunaga"},
                    new(){ Name = "Musashi Miyamoto"},
                    new(){ Name = "Date Masamune"},
                    new(){ Name = "Saido Takamori"},
                  }
             );

            Assert.That(EfMethods.ReadAllSamuraiNames().Count, Is.EqualTo(expected));
        }
        [TestCase(10, false)]
        [TestCase(1, true)]
        public void DeleteSamurai_EnterId_ReturnCorrectBool(int id, bool expected)
        {
            //Arrange
            EfMethods.CreateSamurais(new() {
                    new(){ Name = "Oda Nobunaga"},
                    new(){ Name = "Musashi Miyamoto"},
                    new(){ Name = "Date Masamune"},
                    new(){ Name = "Saido Takamori"},
                  }
             );
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

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Code to run after all tests in the assembly
            SamuraiDbContext.ClearAllData();
        }
    }
}