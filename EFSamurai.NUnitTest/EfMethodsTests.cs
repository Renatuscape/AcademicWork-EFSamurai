using EFSamurai.Domain;
using EFSamurai.DataAccess;

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
            EfMethods.CreateSamurai("Zelda");
            EfMethods.CreateSamurai("Link");

            // Act
            List<string> result = EfMethods.ReadAllSamuraiNames();

            // Assert
            CollectionAssert.AreEqual(new List<string>() { "Zelda", "Link" }, result);
        }
    }
}