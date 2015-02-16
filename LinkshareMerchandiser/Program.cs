using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkshareMerchandiser
{
    class Program
    {
        static readonly string userid = "wishlu";
        static readonly string password = "9RYkFFNN";
        static readonly string siteid = "3140526";

        static List<string> merchants = new List<string>()
        {
            "1110",
            "1237",
            "1280",
            "13816",
            "14028",
            "1845",
            "24285",
            "24416",
            "24943",
            "2640",
            "3071",
            "3184",
            "35859",
            "35861",
            "38483",
            "38606",
            "38891",
            "39460",
            "39484",
            "39554",
            "39758",
            "775",
            "38369", // Hurley
            "35543" // The Limited
        };

        static int remaining = 0;
        static int done = 0;

        static List<DownloadProgressChangedEventArgs> active = new List<DownloadProgressChangedEventArgs>();

        static void Main(string[] args)
        {
            remaining = merchants.Count;

            printInfo();

            Parallel.ForEach(merchants, (mid =>
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadProgressChanged += client_DownloadProgressChanged;
                        client.DownloadFileCompleted += client_DownloadFileCompleted;
                                                                        
                        client.DownloadFileAsync(new Uri("ftp://" + userid + ":" + password + "@aftp.linksynergy.com/" + mid + "_" + siteid + "_mp.xml.gz"), mid + "_" + siteid + "_mp.xml.gz", mid);

                    }
                }));

            while (done < merchants.Count)
            {
                Thread.Sleep(100);
                printInfo();
            }

            Console.WriteLine("Done!");
            Console.WriteLine("Press any key to close...");
            Console.Read();
        }

        static void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {            
            lock (active)
            {
                if (active.Exists(x => x.UserState == e.UserState))
                {
                    int index = active.IndexOf(active.Where(x => x.UserState == e.UserState).Single());
                    active.RemoveAt(index);

                    if (e.ProgressPercentage != 100)
                        active.Insert(index, e);
                }
                else
                {
                    active.Add(e);
                }
            }            
        }

        static void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {            
            done++;
            remaining--;        
            lock (active)
            {
                active.RemoveAll(x => x.UserState == e.UserState);
            }
        }

        static void printInfo()
        {
            // Fresh console
            Console.Clear();
            
            // What are we downloading...
            Console.WriteLine("Downloading {0} product catalogs...", merchants.Count);
            Console.WriteLine("Completed {0} of {1} downloads.", done, merchants.Count);
            Console.WriteLine("{0} catalogs remaining.", remaining);
            Console.WriteLine("===================================");
            Console.WriteLine("Active Downloads:");
            lock (active)
            {
                foreach (DownloadProgressChangedEventArgs d in active)
                {
                    Console.WriteLine("Catalog {0} => {1}% ({2} of {3} bytes)", d.UserState, d.ProgressPercentage, d.BytesReceived, d.TotalBytesToReceive);
                }
            }
        }
    }
}