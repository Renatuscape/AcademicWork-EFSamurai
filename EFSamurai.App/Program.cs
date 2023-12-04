using EFSamurai.DataAccess;

namespace EFSamurai.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //EfMethods.CreateSamurai("Tomoe Gozen");
            OutputConsole.DisplayTitle("Samurai");
            OutputConsole.DisplayStringList(EfMethods.ReadAllSamuraiNames());
        }
    }
}
