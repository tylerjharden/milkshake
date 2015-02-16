using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Neo4jClient;
using Newtonsoft.Json;
using Milkshake;

namespace Blender
{
    /*public static class Store
    {
        public static List<StoreNode> Stores { get; set; }

        static Store()
        {            
        }

        public static void Initialize()
        {
            try
            {
                Stores = Graph.Instance.Cypher
                    .Match("(s:Store)")
                    .ReturnDistinct(s => s.As<StoreNode>())
                    .Results.ToList();
            }
            catch
            {
                Stores = new List<StoreNode>();
            }
        }

        public static void AddProduct(StoreNode s)
        {
            s.ProductCount += 1;

            Graph.Instance.Cypher
                .Match("(sn:Store)")
                .Where((StoreNode sn) => sn.Id == s.Id)
                .Set("sn.ProductCount = {pc}")
                .WithParam("pc", s.ProductCount)
                .ExecuteWithoutResults();
        }

        public static void RemoveProduct(StoreNode s)
        {
            s.ProductCount -= 1;

            Graph.Instance.Cypher
                .Match("(sn:Store)")
                .Where((StoreNode sn) => sn.Id == s.Id)
                .Set("sn.ProductCount = {pc}")
                .WithParam("pc", s.ProductCount)
                .ExecuteWithoutResults();
        }

        public static void UpdateLastCrawl(StoreNode s)
        {
            s.LastCrawl = DateTimeOffset.Now;

            Graph.Instance.Cypher
                .Match("(sn:Store)")
                .Where((StoreNode sn) => sn.Id == s.Id)
                .Set("sn.LastCrawl = {lc}")
                .WithParam("lc", s.LastCrawl)
                .ExecuteWithoutResults();
        }

        public static void Add(StoreNode s)
        {
            Stores.Add(s);
        }

        public static StoreNode Get(string name)
        {
            if (Exists(name))
                return Stores.Where(item => item.Name == name).Single();
            else
            {
                StoreNode sn = new StoreNode();
                sn.Name = name;
                sn.Id = Guid.NewGuid();
                return sn;
            }
        }

        public static bool Exists(StoreNode s)
        {
            return Stores.Contains(s);
        }

        public static bool Exists(string name)
        {
            return Stores.Where(item => item.Name == name).Count() > 0;
        }
    }*/
}
