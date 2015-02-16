using MilkshakeTester.Parsers;
//using Milkshake;
//using Milkshake.Linkshare;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MilkshakeTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.BufferHeight = 8192;
            Console.BufferWidth = 1024;
            Console.Clear();

            Console.WriteLine("/***********************/");
            Console.WriteLine("/*===== Milkshake =====*/");
            Console.WriteLine("/***********************/");
            Console.WriteLine();
            
            /* LINKSHARE */
            
            //Console.WriteLine("/* Loading Linkshare... */");
            /*
            Console.WriteLine();
            Linkshare ls = new Linkshare();
            dynamic token = ls.GetAccessToken();

            Console.WriteLine("Access Token: " + token.access_token);
            
            ls.LoadAdvertisers();

            foreach (LinkshareMerchant merc in Linkshare.Merchants)
            {
                Console.WriteLine(string.Format("Name: {0} MID: {1}", merc.Name, merc.MID));
            }
            */
            //Console.WriteLine("/* Done. */");
            //Console.WriteLine();
            
            /* CJ AFFILIATE */
            //Console.WriteLine("/* Loading CJ Affiliate... */");
            //Console.WriteLine();
            
            // TODO: Load CJ Affiliate

            //Console.WriteLine("/* Done. */");
            //Console.WriteLine();

            /* Pepperjam (eBay Enterprise) */
            //Console.WriteLine("/* Loading Pepperjam (eBay Enterprise)... */");
            //Console.WriteLine();

            // TODO: Load Pepperjam

            //Console.WriteLine("/* Done. */");
           // Console.WriteLine();

            /* Parsing CJ Affiliate Product Catalogs */
            Console.WriteLine("/* Parsing CJ Affiliate Product Catalogs... */");
            Console.WriteLine();

            CJ cj = new CJ();

            if (args.Contains("fresh"))
                cj.Reset();

            cj.ParseAll();
            Console.WriteLine("Initial files parsed.");
            Console.WriteLine();

            Console.WriteLine("CJ files parsed...press any key to continue");
            Console.ReadKey();

            /* Parsing LinkShare Affiliate Product Catalogs */
            Console.WriteLine("/* Parsing LinkShare Affiliate Product Catalogs... */");
            Console.WriteLine();

            Linkshare ls = new Linkshare();

            if (args.Contains("fresh"))
                ls.Reset();

            ls.ParseAll();
            Console.WriteLine("Initial files parsed.");
            Console.WriteLine();

            Console.WriteLine("LinkShare files parsed...press any key to continue");
            Console.ReadKey();

            /* Parsing PepperJam Affiliate Product Catalogs */
            Console.WriteLine("/* Parsing Pepperjam (eBay Enterprise) Affiliate Product Catalogs... */");
            Console.WriteLine();

            Pepperjam pj = new Pepperjam();

            if (args.Contains("fresh"))
                pj.Reset();

            pj.ParseAll();
            Console.WriteLine("Initial files parsed.");
            Console.WriteLine();

            Console.WriteLine("Pepperjam (eBay Enterprise) files parsed...press any key to continue");
            Console.ReadKey();
            
                                    
            /* Finished */
            Console.Write("Watchdog waiting for files...press F4 to close");

            while (Console.ReadKey().Key != ConsoleKey.F4) { Thread.Sleep(1); }

            //Milkshake.Aggregator agg = new Milkshake.Aggregator();
            //while (true)
            //{
            //    Thread.Sleep(1);
            //}
        }
    }
}
