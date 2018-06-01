using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MarvelComicFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            //requires download of must-read library from Marvel Unlimited's website in csv format and must be placed on desktop
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            //insert username here
            string userName = "";

            List<KeyValuePair<string, int>> comics = new List<KeyValuePair<string, int>>();

            string currentTitle = string.Empty;
            int counter = 0;
            char[] comma = ",".ToCharArray();
            char[] asterisk = "*".ToCharArray();

            //parse must-read file
            using (var reader = new StreamReader(Path.Combine(deskDir, $"{userName}`s_Must_Reads-MyMarvel.csv")))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Trim().TrimEnd(comma);

                    if (line.Trim() != String.Empty)
                    {
                        int test;
                        if (line.Contains("*") && int.TryParse(line.TrimEnd(asterisk), out test))
                        {
                            KeyValuePair<string, int> comic = new KeyValuePair<string, int>(currentTitle, test);
                            comics.Add(comic);
                            counter++;
                        }
                        else if (line.Contains(userName) == false && int.TryParse(line, out test) == false)
                        {
                            if (line != currentTitle)
                            {
                                //reached a new title
                                currentTitle = line;
                            }
                        }
                    }
                }

            }

            //create text file with library page numbers for where each title starts to make it easier to find
            var test1 = comics.GroupBy(x => x.Key).Select(g => g.First()).ToList();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(deskDir, "comics.txt"), false))
            {
                int currentPage = 1;

                file.WriteLine("Page 1:");
                foreach (KeyValuePair<string, int> kvp in test1)
                {
                    int index = comics.IndexOf(kvp);
                    int comicPage = (index == 0 ? 1 : (int)Math.Ceiling(((double)index / 50)));
                    
                    if (comicPage == currentPage)
                    {
                        file.WriteLine($"    {kvp.Key}");
                    }
                    else
                    {
                        currentPage = comicPage;
                        file.WriteLine($"Page {currentPage}:");
                        file.WriteLine($"    {kvp.Key}");
                    }


                    Console.WriteLine($"{kvp.Key} should be on page {(index == 0 ? 1 : Math.Ceiling(((double)index / 50)))}");
                }
            }

        }

    }
}

