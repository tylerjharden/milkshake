using Milkshake;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSWD
{
    class Program
    {        
        static void Main(string[] args)
        {
            // Initialize Milkshake helpers into memory
            Store.Initialize();

            // Reset the index
            Search.Reset();

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("10.0.0.40:6379");
            redis.PreserveAsyncOrder = true;

            //ISubscriber sub = redis.GetSubscriber();
            IDatabase db = redis.GetDatabase();
            //sub.Subscribe("milkshake_newproduct", (channel, message) =>
            //{
            //var context = TaskScheduler.FromCurrentSynchronizationContext();

            while (true)
            {
                RedisValue item = db.ListLeftPop("milkshake_product_parse_queue");
                                        
                if (item.HasValue && !item.IsNullOrEmpty)
                {
                    try
                    {
                        //Console.Write("Deserializing product...");

                        Stopwatch sw = new Stopwatch();

                        //sw.Start();
                        RawProduct p = JsonConvert.DeserializeObject<RawProduct>(item);
                        //sw.Stop();

                        //Console.WriteLine("Deserialize: " + sw.ElapsedMilliseconds + "ms");
                        //sw.Reset();

                        ////
                        // Step 1: Check if store exists, if not, create it!
                        ////
                        sw.Start();
                        Store s = Store.Get(p.Store);
                        if (s == null)
                        {
                            s = new Store();
                            s.Name = p.Store;

                            if (!String.IsNullOrEmpty(p.StoreWebsite))
                                s.Website = p.StoreWebsite;
                            else
                                s.Website = p.Store;

                            Store.Create(s);
                        }

                        p.StoreId = s.Id;
                        p.Store = s.Name;
                        sw.Stop();

                        //Console.WriteLine("Store: " + sw.ElapsedMilliseconds + "ms");
                        //sw.Reset();

                        // Step 2: Check if brand exists, if not, create it
                        //sw.Start();
                        //Brand b = Brand.Get(p.Brand);
                        /*if (b == null)
                        {
                            b = new Brand();
                            b.Name = p.Brand;

                            Brand.Create(b);
                        }*/
                        //if (b!=null)
                        //    p.Brand = b.Name;

                        //sw.Stop();

                        //Console.WriteLine("Brand: " + sw.ElapsedMilliseconds + "ms");
                        //sw.Reset();

                        // Step 3: Check if manufacturer exists, if not, create it
                        //sw.Start();
                        //Manufacturer m = Manufacturer.Get(p.Manufacturer);
                        /*if (m == null)
                        {
                            m = new Manufacturer();
                            m.Name = p.Manufacturer;

                            Manufacturer.Create(m);
                        }*/
                        //if (m!=null)
                        //    p.Manufacturer = m.Name;
                        //sw.Stop();

                        //Console.WriteLine("Manufacturer: " + sw.ElapsedMilliseconds + "ms");
                        //sw.Reset();

                        try
                        {
                            sw.Start();
                            ProductManager.AddProduct(p).Wait();
                            sw.Stop();
                            Console.WriteLine("Indexed product in " + sw.ElapsedMilliseconds + "ms");
                            sw.Reset();
                        }
                        catch (Exception e)
                        {                            
                            Console.WriteLine("Unhandled exception occurred in Milkshake database pipeline: " + e.ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unhandled exception while deserializing RawProduct from JSON: " + e.ToString());
                    }
                }
            }
                
            //});

            //while (true)
            //{                
            //    Thread.Sleep(1);
            //}
        }               
    }
}
