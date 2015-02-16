using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Milkshake
{
    public class Manufacturer
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Alternatives")]
        public List<String> Alternatives { get; set; }

        // Static Variables
        private static object m_lock = new object();
        private static List<Manufacturer> m_manufacturers;

        [JsonIgnore]
        public static List<Manufacturer> Manufacturers
        {
            get
            {
                lock (m_lock)
                {
                    if (m_manufacturers == null)
                        Initialize();

                    return m_manufacturers;
                }                
            }
        }

        public Manufacturer()
        {
            Name = "";
            Id = Guid.Empty;
            Alternatives = new List<string>();
        }

        public static void Initialize()
        {
            lock (m_lock)
            {
                if (m_manufacturers != null)
                    return;

                try
                {
                    m_manufacturers = Graph.Instance.Cypher
                        .Match("(m:Manufacturer)")
                        .ReturnDistinct(m => m.As<Manufacturer>())
                        .Results.ToList();

                    Logger.Log("Retrieved manufacturers from milkshake database.");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }            
        }

        public static void Add(Manufacturer m)
        {
            Manufacturers.Add(m);
        }

        public static void Create(Manufacturer m)
        {
            try
            {
                m.Id = Guid.NewGuid();

                Graph.Instance.Cypher
                    .Create("(n:Manufacturer {man})")
                    .WithParam("man", m)
                    .ExecuteWithoutResults();

                Add(m);
            }
            catch { }
        }

        public static string GetName(Guid id)
        {
            Manufacturer m = GetById(id);

            if (m == null)
                return "";

            return m.Name;
        }

        public bool AddAlternative(string name)
        {
            try
            {
                if (this.Alternatives.Contains(name)) // no duplicates
                    return false;

                if (Exists(name)) // no other store's alternative or store name
                    return false;

                this.Alternatives.Add(name);
                this.Set("Alternatives", this.Alternatives);

                return true;
            }
            catch { return false; }
        }

        public bool RemoveAlternative(string name)
        {
            if (!this.Alternatives.Contains(name))
                return false;

            try
            {
                this.Alternatives.Remove(name);
                this.Set("Alternatives", this.Alternatives);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Manufacturer Get(string name, bool partial = false)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            // Cleanup potential store name
            name = name.Replace("http://", "").Replace("https://", "").Replace("www.", "");

            if (name.EndsWith("/"))
                name = name.Remove(name.Length - 1);

            name = name.ToLower().Trim();

            try
            {
                if (Exists(name))
                {
                    try
                    {
                        return Manufacturers.Where(item => item.Name.ToLower().Trim() == name).Single();
                    }
                    catch
                    {
                        return Manufacturers.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0).Single();
                    }
                }
                else
                {
                    if (partial) // return stores with partial matches
                    {
                        try
                        {
                            return Manufacturers.Where(item => item.Name.ToLower().Trim().Contains(name)).Single();
                        }
                        catch
                        {
                            return Manufacturers.Where(item => item.Alternatives.Count(x => x.ToLower().Trim().Contains(name)) > 0).Single();
                        }
                    }

                    return null;
                }
            }
            catch { return null; }
        }

        public static List<Manufacturer> GetAll(string name, bool partial = false)
        {
            if (String.IsNullOrEmpty(name))
                return new List<Manufacturer>();

                // Cleanup potential store name
                name = name.Replace("http://", "").Replace("https://", "").Replace("www.", "");

            if (name.EndsWith("/"))
                name = name.Remove(name.Length - 1);

            name = name.ToLower().Trim();

            List<Manufacturer> res = new List<Manufacturer>();

            try
            {
                if (Exists(name))
                {
                    try
                    {
                        res.AddRange(Manufacturers.Where(item => item.Name.ToLower().Trim() == name));
                    }
                    catch
                    {
                        res.AddRange(Manufacturers.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0));
                    }
                }
                else
                {
                    if (partial) // return manufacturers with partial matches
                    {
                        try
                        {
                            res.AddRange(Manufacturers.Where(item => item.Name.ToLower().Trim().Contains(name)));
                        }
                        catch
                        {
                            res.AddRange(Manufacturers.Where(item => item.Alternatives.Count(x => x.ToLower().Trim().Contains(name)) > 0));
                        }
                    }
                }

                return res;
            }
            catch { return res; }
        }

        public void Set(string property, object value)
        {
            Graph.Instance.Cypher
                .Match("(m:Manufacturer)")
                 .Where((Manufacturer m) => m.Id == this.Id)
                 .Set("m." + property + " = {p}")
                 .WithParam("p", value)
                 .ExecuteWithoutResults();
        }

        public static bool Exists(Manufacturer m)
        {
            return Manufacturers.Contains(m);
        }

        public static bool Exists(string name)
        {
            try
            {
                if (String.IsNullOrEmpty(name))
                    return false;

                Initialize(); // always get fresh manufacturers?

                name = name.ToLower().Trim();

                if (Manufacturers.Where(item => item.Name.ToLower().Trim() == name).Count() > 0) // check "normal" name
                    return true;
                else if (Manufacturers.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0).Count() > 0) // check alternatives (misspellings, abbreviations, extra words)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

        public static List<Manufacturer> Find(string query)
        {
            List<Manufacturer> result = new List<Manufacturer>();

            if (String.IsNullOrEmpty(query))
                return result; // empty list for null/empty query

            query = query.ToLower().Trim();

            try
            {
                if (query.Contains(" ")) // delimited by spaces
                {
                    foreach (string s in query.Split(' '))
                    {
                        result.AddRange(GetAll(s));
                    }
                }
                else
                {
                    result.AddRange(GetAll(query));
                }
            }
            catch { }

            return result.Distinct().ToList();
        }

        public static List<Manufacturer> GetAll()
        {            
            try
            {                
                return Graph.Instance.Cypher
                .Match("(m:Manufacturer)")
                .Return(m => m.As<Manufacturer>())
                .Results.ToList();
            }
            catch
            {
                return new List<Manufacturer>();
            }
        }

        public List<Product> GetProducts(int page = 0)
        {
            try
            {
                List<Product> res = Graph.Instance.Cypher
                    .Match("(p:Product)-[:MANUFACTURER]-(m:Manufacturer)")
                    .Where((Manufacturer m) => m.Id == this.Id)
                    .ReturnDistinct(p => p.As<Product>())
                    .OrderByDescending("p.Name")
                    .Skip(50 * page)
                    .Limit(50)
                    .Results.ToList();

                Logger.Log("Returned " + res.Count + " products manufacturer by " + this.Name);

                return res;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());

                return new List<Product>();
            }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                return Graph.Instance.Cypher
                    .Match("(p:Product)-[:MANUFACTURER]-(m:Manufacturer)")
                    .Where((Manufacturer m) => m.Id == this.Id)
                    .ReturnDistinct(p => p.As<Product>())
                    .Results.ToList();
            }
            catch
            {
                return new List<Product>();
            }
        }

        public static Manufacturer GetById(Guid id)
        {            
            try
            {
                return Graph.Instance.Cypher
                    .Match("(m:Manufacturer)")
                    .Where((Manufacturer m) => m.Id == id)
                    .Return(m => m.As<Manufacturer>())
                    .Results.Single();                                        
            }
            catch
            {
                return null;
            }
        }

        public static int Count()
        {
            try
            {
                return (int)Graph.Instance.Cypher
                 .Match("(m:Manufacturer)")
                 .Return(m => m.Count())
                 .Results.Single();
            }
            catch
            {
                Logger.Error("There was an error when retrieving the manufacturer count.");
                return 0;
            }
        }
    }
}
