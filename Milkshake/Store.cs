using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Milkshake
{
    // Represent's a store in the Milkshake database.
    public class Store
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Handle")]
        public string Handle { get; set; }

        [JsonProperty("Website")]
        public string Website { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Logo")]
        public string Logo { get; set; }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        // alternative / misspellings of the store name
        [JsonProperty("Alternatives")]
        public List<String> Alternatives { get; set; }

        // milkshake analytics
        [JsonProperty("ProductCount")]
        public int ProductCount { get; set; }

        [JsonProperty("LastCrawl")]
        public DateTimeOffset LastCrawl { get; set; }

        [JsonProperty("Followers")]
        public int Followers { get; set; }

        [JsonProperty("Following")]
        public int Following { get; set; }


        // wishlu Attributes
        [JsonProperty("IsVerified")]
        public bool IsVerified { get; set; }

        [JsonProperty("AdministratorUserId")]
        public Guid AdministratorUserId { get; set; } // Should we make shops have an administrator User, or allow shops to utilize their own login system? TODO

        [JsonProperty("AdministratorEmail")]
        public string AdministratorEmail { get; set; }

        [JsonProperty("AdministratorUsername")]
        public string AdministratorUsername { get; set; }

        [JsonProperty("AdministratorPasswordHash")]
        public string AdministratorPasswordHash { get; set; }

        [JsonProperty("AdministratorPasswordSalt")]
        public string AdministratorPasswordSalt { get; set; }

        [JsonProperty("FacebookId")]
        public string FacebookId { get; set; }

        [JsonProperty("TwitterId")]
        public string TwitterId { get; set; }

        [JsonProperty("GooglePlusId")]
        public string GooglePlusId { get; set; }

        [JsonProperty("WaneloId")]
        public string WaneloId { get; set; }

        [JsonProperty("PinterestId")]
        public string PinterestId { get; set; }

        [JsonProperty("InstagramId")]
        public string InstagramId { get; set; }

        [JsonProperty("YoutubeId")]
        public string YoutubeId { get; set; }

        [JsonProperty("TumblrId")]
        public string TumblrId { get; set; }

        [JsonProperty("IsBrickAndMortar")]
        public bool IsBrickAndMortar { get; set; }

        [JsonProperty("IsOnline")]
        public bool IsOnline { get; set; }

        [JsonProperty("IsChain")]
        public bool IsChain { get; set; }

        [JsonProperty("IsBoutique")]
        public bool IsBoutique { get; set; }

        [JsonProperty("Level")]
        public StoreLevel Level { get; set; }

        [JsonProperty("IsFeatured")]
        public bool IsFeatured { get; set; }

        // online stores
        [JsonProperty("IsShopify")]
        public bool IsShopify { get; set; }

        [JsonProperty("IsPrestashop")]
        public bool IsPrestashop { get; set; }

        [JsonProperty("IsBigcommerce")]
        public bool IsBigcommerce { get; set; }

        [JsonProperty("IsTictail")]
        public bool IsTictail { get; set; }

        [JsonProperty("IsMagento")]
        public bool IsMagento { get; set; }

        [JsonProperty("IsWordpress")]
        public bool IsWordpress { get; set; }

        [JsonProperty("IsBlogger")]
        public bool IsBlogger { get; set; }

        [JsonProperty("IsTumblr")]
        public bool IsTumblr { get; set; }

        // TwoTap
        [JsonProperty("SupportsTwoTap")]
        public bool SupportsTwoTap { get; set; }
        
        // Static Variables
        private static object m_lock = new object();
        private static List<Store> m_stores;
        
        [JsonIgnore]
        public static List<Store> Stores
        {                   
            get
            {
                lock (m_lock)
                {
                    if (m_stores == null)
                        Initialize();
                    
                    return m_stores;
                }                
            }        
        }

        public Store()
        {
            Logo = "";
            Website = "";
            Id = Guid.NewGuid();
            ProductCount = 0;
            Alternatives = new List<string>();

            // wishlu Attributes
            IsVerified = false;            
            FacebookId = "";
            TwitterId = "";
            GooglePlusId = "";
            WaneloId = "";
            PinterestId = "";
            InstagramId = "";
            YoutubeId = "";
            TumblrId = "";
            IsBrickAndMortar = false;
            IsOnline = true;
            IsChain = false;
            IsBoutique = false;
            Level = StoreLevel.Free;
            IsFeatured = false;

            // TwoTap
            SupportsTwoTap = false;

            // admin account settings
            AdministratorEmail = "";
            AdministratorUserId = Guid.Empty;
            AdministratorUsername = "";
            AdministratorPasswordHash = "";
            AdministratorPasswordSalt = "";
        }
        
        public static void Initialize()
        {
            lock (m_lock)
            {
                if (m_stores != null)
                    return;

                try
                {
                    m_stores = Graph.Instance.Cypher
                        .Match("(s:Store)")
                        .ReturnDistinct(s => s.As<Store>())
                        .Results.ToList();

                    Logger.Log("Retrieved stores from milkshake database.");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }            
        }

        public static void AddProduct(Store s)
        {
            s.ProductCount += 1;

            Graph.Instance.Cypher
                .Match("(sn:Store)")
                .Where((Store sn) => sn.Id == s.Id)
                .Set("sn.ProductCount = {pc}")
                .WithParam("pc", s.ProductCount)
                .ExecuteWithoutResults();
        }

        public static void RemoveProduct(Store s)
        {
            s.ProductCount -= 1;

            Graph.Instance.Cypher
                .Match("(sn:Store)")
                .Where((Store sn) => sn.Id == s.Id)
                .Set("sn.ProductCount = {pc}")
                .WithParam("pc", s.ProductCount)
                .ExecuteWithoutResults();
        }

        public static void UpdateLastCrawl(Store s)
        {
            s.LastCrawl = DateTimeOffset.Now;

            Graph.Instance.Cypher
                .Match("(sn:Store)")
                .Where((Store sn) => sn.Id == s.Id)
                .Set("sn.LastCrawl = {lc}")
                .WithParam("lc", s.LastCrawl)
                .ExecuteWithoutResults();
        }

        public static void Add(Store s)
        {
            Stores.Add(s);
        }

        public static void Create(Store s)
        {
            try
            {
                s.Id = Guid.NewGuid();

                Graph.Instance.Cypher
                    .Create("(n:Store {st})")
                    .WithParam("st", s)
                    .ExecuteWithoutResults();

                Store.Add(s);                
            }
            catch { }
        }

        public static string GetName(Guid id)
        {
            Store s = GetById(id);

            if (s == null)
                return "";

            return s.Name;
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
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

        public static Store Get(string name, bool partial = false)
        {
            if (String.IsNullOrEmpty(name))

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
                        return Stores.Where(item => item.Name.ToLower().Trim() == name).Single();
                    }
                    catch
                    {
                        return Stores.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0).Single();
                    }
                }                                    
                else
                {
                    if (partial) // return stores with partial matches
                    {
                        try
                        {
                            return Stores.Where(item => item.Name.ToLower().Trim().Contains(name)).Single();
                        }
                        catch
                        {
                            return Stores.Where(item => item.Alternatives.Count(x => x.ToLower().Trim().Contains(name)) > 0).Single();
                        }
                    }

                    return null;
                }
            }
            catch { return null; }
        }

        public static List<Store> GetAll(string name, bool partial = false)
        {
            if (String.IsNullOrEmpty(name))

                // Cleanup potential store name
                name = name.Replace("http://", "").Replace("https://", "").Replace("www.", "");

            if (name.EndsWith("/"))
                name = name.Remove(name.Length - 1);

            name = name.ToLower().Trim();

            List<Store> res = new List<Store>();

            try
            {
                if (Exists(name))
                {
                    try
                    {
                        res.AddRange(Stores.Where(item => item.Name.ToLower().Trim() == name));
                    }
                    catch
                    {
                        res.AddRange(Stores.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0));
                    }
                }
                else
                {
                    if (partial) // return stores with partial matches
                    {
                        try
                        {
                            res.AddRange(Stores.Where(item => item.Name.ToLower().Trim().Contains(name)));
                        }
                        catch
                        {
                            res.AddRange(Stores.Where(item => item.Alternatives.Count(x => x.ToLower().Trim().Contains(name)) > 0));
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
                .Match("(s:Store)")
                 .Where((Store s) => s.Id == this.Id)
                 .Set("s." + property + " = {p}")
                 .WithParam("p", value)
                 .ExecuteWithoutResults();                        
        }

        public static bool Exists(Store s)
        {
            return Stores.Contains(s);
        }

        public static bool Exists(string name)
        {
            try
            {
                if (String.IsNullOrEmpty(name))
                    return false;

                Initialize(); // always get fresh stores?

                name = name.ToLower().Trim();

                if (Stores.Where(item => item.Name.ToLower().Trim() == name).Count() > 0) // check "normal" name
                    return true;
                else if (Stores.Where(item => item.Alternatives.Count(x => x.ToLower().Trim() == name) > 0).Count() > 0) // check alternatives (misspellings, abbreviations, extra words)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

        public static List<Store> Find(string query)
        {
            List<Store> result = new List<Store>();

            if (String.IsNullOrEmpty(query))
                return result; // empty list for null/empty query

            query = query.ToLower().Trim();

            try
            {
                if (query.Contains(" ")) // delimited by spaces
                {
                    foreach (string s in query.Split(' '))
                    {                        
                        result.AddRange(GetAll(s, true));
                    }
                }
                else
                {
                    result.AddRange(GetAll(query, true));
                }
            }
            catch { }
                        
            return result.Distinct().ToList();
        }

        public static List<Store> GetStores()
        {            
            try
            {                
                return Graph.Instance.Cypher
                .Match("(s:Store)")
                .ReturnDistinct(s => s.As<Store>())
                .Results.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new List<Store>();
            }            
        }

        public static List<Store> GetStores(IEnumerable<Guid> ids)
        {
            try
            {
                return Graph.Instance.Cypher
                .Match("(s:Store)")
                .Where("s.Id IN {ids}")
                .WithParam("ids", ids)
                .ReturnDistinct(s => s.As<Store>())
                .Results.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new List<Store>();
            }        
        }

        public List<Product> GetProducts(int page = 0)
        {
            try
            {
                /*List<Product> res = Graph.Instance.Cypher
                    .Match("(p:Product)-[:STORE]-(s:Store)")                    
                    .Where((Store s) => s.Id == this.Id)
                    .ReturnDistinct(p => p.As<Product>())
                    .OrderByDescending("p.Name")
                    .Skip(50 * page)
                    .Limit(50)
                    .Results.ToList();*/

                List<Product> res = Search.StoreId(this.Id, page).Documents.ToList();

                Logger.Log("Returned " + res.Count + " products from " + this.Name);

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
                    .Match("(p:Product)-[:STORE]-(s:Store)")
                    .Where((Store s) => s.Id == this.Id)
                    .ReturnDistinct(p => p.As<Product>())
                    .Results.ToList();
            }
            catch
            {
                return new List<Product>();
            }
        }

        public static Store GetById(Guid id)
        {
            Store shop = (Store)System.Web.HttpContext.Current.Cache["shop_" + id];

            if (shop == null)
            {
                try
                {
                    shop = Graph.Instance.Cypher
                     .Match("(s:Store)")
                     .Where((Store s) => s.Id == id)
                     .Return(s => s.As<Store>())
                     .Results.Single();

                    try
                    {
                        System.Web.HttpContext.Current.Cache.Insert("shop_" + id, shop);
                    }
                    catch { }
                }
                catch
                {
                    return null;
                }
            }

            return shop;
        }

        public static int GetStoreCount()
        {
            try
            {
                return (int)Graph.Instance.Cypher
                 .Match("(s:Store)")
                 .Return(s => s.Count())
                 .Results.First();
            }
            catch
            {
                Logger.Error("There was an error when retrieving the shop count.");
                return 0;
            }
        }
    }

    public enum StoreLevel
    {
        Free = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Platinum = 4,
        Diamond = 5,
        None = 9
    }
}
