using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.App
{
    public static class OutputConsole
    {
        public static void DisplayTitle(string title)
        {
            Console.WriteLine(Tabulate("*** " + title + " ****\n"));
        }
        public static void DisplayText(string text)
        {
            Console.WriteLine(Tabulate(text));
        }
        public static void DisplayStringList(List<string> list)
        {
            foreach (string s in list)
            {
                DisplayText(s);
            }
        }

        static string Tabulate(string text)
        {
            text = "\t" + text.Replace("\n", "\n\t");
            return text;
        }
    }
}
