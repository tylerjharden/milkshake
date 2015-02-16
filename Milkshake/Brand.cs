using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Milkshake
{
    public class Brand
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Alternatives")]
        public List<String> Alternatives { get; set; }

        // Static Variables
        private static object m_lock = new object();
        private static List<Brand> m_brands;

        [JsonIgnore]
        public static List<Brand> Brands
        {            
            get
            {
                lock (m_lock)
                {
                    if (m_brands == null)
                        Initialize();

                    return m_brands;
                }
            }
        }

        public Brand()
        {
            Name = "";
            Id = Guid.Empty;
            Alternatives = new List<string>();
        }

        public static void Initialize()
        {
            lock (m_lock)
            {
                if (m_brands != null)
                    return;

                try
                {
                    m_brands = Graph.Instance.Cypher
                        .Match("(b:Brand)")
                        .ReturnDistinct(b => b.As<Brand>())
                        .Results.ToList();

                    Logger.Log("Retrieved brands from milkshake database.");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
        }

        public static void Add(Brand b)
        {
            Brands.Add(b);
        }

        public static void Create(Brand b)
        {
            try
            {
                b.Id = Guid.NewGuid();

                Graph.Instance.Cypher
                    .Create("(n:Brand {br})")
                    .WithParam("br", b)
                    .ExecuteWithoutResults();

                Add(b);
            }
            catch { }
        }

        public static string GetName(Guid id)
        {
            Brand b = GetById(id);

            if (b == null)
                return "";

            return b.Name;
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

        public static Brand Get(string name, bool partial = false)
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
                        return Brands.Where(item => item.Name.ToLower().Trim() == name).Single();
                    }
                    catch
                    {
                        return Brands.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0).Single();
                    }
                }
                else
                {
                    if (partial) // return stores with partial matches
                    {
                        try
                        {
                            return Brands.Where(item => item.Name.ToLower().Trim().Contains(name)).Single();
                        }
                        catch
                        {
                            return Brands.Where(item => item.Alternatives.Count(x => x.ToLower().Trim().Contains(name)) > 0).Single();
                        }
                    }

                    return null;
                }
            }
            catch { return null; }
        }

        public static List<Brand> GetAll(string name, bool partial = false)
        {
            if (String.IsNullOrEmpty(name))
                return new List<Brand>();

            // Cleanup potential store name
            name = name.Replace("http://", "").Replace("https://", "").Replace("www.", "");

            if (name.EndsWith("/"))
                name = name.Remove(name.Length - 1);

            name = name.ToLower().Trim();

            List<Brand> res = new List<Brand>();

            try
            {
                if (Exists(name))
                {
                    try
                    {
                        res.AddRange(Brands.Where(item => item.Name.ToLower().Trim() == name));
                    }
                    catch
                    {
                        res.AddRange(Brands.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0));
                    }
                }
                else
                {
                    if (partial) // return manufacturers with partial matches
                    {
                        try
                        {
                            res.AddRange(Brands.Where(item => item.Name.ToLower().Trim().Contains(name)));
                        }
                        catch
                        {
                            res.AddRange(Brands.Where(item => item.Alternatives.Count(x => x.ToLower().Trim().Contains(name)) > 0));
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
                .Match("(b:Brand)")
                 .Where((Brand b) => b.Id == this.Id)
                 .Set("b." + property + " = {p}")
                 .WithParam("p", value)
                 .ExecuteWithoutResults();
        }

        public static bool Exists(Brand b)
        {
            return Brands.Contains(b);
        }

        public static bool Exists(string name)
        {
            try
            {
                if (String.IsNullOrEmpty(name))
                    return false;

                Initialize(); // always get fresh manufacturers?

                name = name.ToLower().Trim();

                if (Brands.Where(item => item.Name.ToLower().Trim() == name).Count() > 0) // check "normal" name
                    return true;
                else if (Brands.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0).Count() > 0) // check alternatives (misspellings, abbreviations, extra words)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

        public static List<Brand> Find(string query)
        {
            List<Brand> result = new List<Brand>();

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

        public static List<Brand> GetAll()
        {
            try
            {
                return Graph.Instance.Cypher
                .Match("(b:Brand)")
                .Return(b => b.As<Brand>())
                .Results.ToList();
            }
            catch
            {
                return new List<Brand>();
            }
        }

        public List<Product> GetProducts(int page = 0)
        {
            try
            {
                List<Product> res = Graph.Instance.Cypher
                    .Match("(p:Product)-[:BRAND]-(b:Brand)")
                    .Where((Brand b) => b.Id == this.Id)
                    .ReturnDistinct(p => p.As<Product>())
                    .OrderByDescending("p.Name")
                    .Skip(50 * page)
                    .Limit(50)
                    .Results.ToList();

                Logger.Log("Returned " + res.Count + " products marketed under the brand " + this.Name);

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
                    .Match("(p:Product)-[:BRAND]-(b:Brand)")
                    .Where((Brand b) => b.Id == this.Id)
                    .ReturnDistinct(p => p.As<Product>())
                    .Results.ToList();
            }
            catch
            {
                return new List<Product>();
            }
        }

        public static Brand GetById(Guid id)
        {
            try
            {
                return Graph.Instance.Cypher
                    .Match("(b:Brand)")
                    .Where((Brand b) => b.Id == id)
                    .Return(b => b.As<Brand>())
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
                 .Match("(b:Brand)")
                 .ReturnDistinct(b => b.CountDistinct())
                 .Results.Single();
            }
            catch
            {
                Logger.Error("There was an error when retrieving the brand count.");
                return 0;
            }
        }
    }
}
