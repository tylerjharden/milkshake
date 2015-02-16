using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Blender
{
    public static class IgnoreList
    {
        public static List<string> Urls { get; set; }

        //private static FileStream stream;
        //private static StreamReader sr;
        //private static StreamWriter sw;

        public static void Initialize()
        {
            Urls = new List<string>();

            /*if (!File.Exists("ignorelist.dat"))
                File.Create("ignorelist.dat").Close();

            stream = File.OpenRead("ignorelist.dat");

            sr = new StreamReader(stream);            
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                Urls.Add(line);
            }

            sr.Close();
            stream.Close();*/
        }

        public static void Add(string url)
        {
            Urls.Add(url);

            //if (stream == null || !stream.CanWrite)
            //    stream = File.Open("ignorelist.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);

           // sw = new StreamWriter(stream);

           // sw.WriteLine(url);
           // sw.Flush();
        }
    }
}
