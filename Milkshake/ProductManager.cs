using Schloss.Data.Neo4j.Cypher;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Milkshake
{
    public static class ProductManager
    {
        //static int added = 0;
        //static int updated = 0;
        
        static ProductManager() {}

        public static async Task AddProduct(RawProduct pn)
        {
            if (pn == null)
            {
                await Logger.LogAsync("Received null RawProduct");
                return;
            }

            if (!pn.IsValid())
            {
                await Logger.LogAsync("Received invalid RawProduct");
                await Logger.LogAsync(JsonConvert.SerializeObject(pn));
                return;
            }

            try
            {
                //Stopwatch sw = new Stopwatch();

                ////
                // Step 1. Clean-up / Decode RawProduct fields (to account for non-alphanumeric characters and HTML tags)
                ////   
                pn = GetSafeRawProduct(pn);
                
                ////
                // Step 2. Parse product name and product keywords to create Tokens
                ////
                List<string> tokens = ParseProductTokens(pn);
                
                ////
                // Step 3. Parse product price fields to generate proper pricing information
                ////                
                // Remove $ sign from price fields
                pn.Price = pn.Price.Replace("$","");
                pn.SalePrice = pn.SalePrice.Replace("$","");
                pn.RetailPrice = pn.RetailPrice.Replace("$","");
                pn.FromPrice = pn.FromPrice.Replace("$","");

                if (pn.Price.Contains("-"))
                {
                    string p1 = pn.Price.Split('-')[0];
                    string p2 = pn.Price.Split('-')[1];

                    pn.FromPrice = p1;
                    pn.Price = p2;
                }
                if (!String.IsNullOrEmpty(pn.SalePrice) && pn.SalePrice != pn.Price && pn.SalePrice.Contains("-"))
                {
                    string p1 = pn.SalePrice.Split('-')[0];
                    string p2 = pn.SalePrice.Split('-')[1];

                    pn.FromPrice = p1;
                    pn.SalePrice = p2;
                }
                
                ////
                // Step 4. Parse product category fields to build the correct taxonomy hierarchy
                ////
                List<Category> categories = ParseProductTaxonomy(pn);
                                
                ////
                // Step 5. Separate out Product data
                ////                
                Product p = new Product();
                p.Id = Guid.NewGuid();
                p.Name = pn.Name;
                p.Description = pn.Description;
                p.Image = pn.Image;
                p.Gift = pn.Gift;
                p.GTIN = pn.GTIN;
                p.UPC = pn.UPC;
                p.SKU = pn.SKU;
                p.Manufacturer = pn.Manufacturer;
                p.ManufacturerID = pn.ManufacturerID;
                p.ISBN = pn.ISBN;
                p.Keywords = pn.Keywords;
                p.MPN = pn.MPN;
                p.Title = pn.Title;
                p.Gender = pn.Gender;
                p.Brand = pn.Brand;

                p.CreatedOn = DateTimeOffset.Now;
                p.LastModified = DateTimeOffset.Now;
                
                ////
                // Step 6. Separate out Offer data
                ////
                Offer o = new Offer();
                o.Store = pn.Store;
                o.StoreId = pn.StoreId;
                o.UPC = pn.UPC;
                o.GTIN = pn.GTIN;
                o.AffiliateUrl = pn.AffiliateUrl;
                o.Condition = pn.Condition;
                o.Currency = pn.Currency;
                o.Description = pn.Description;
                o.FromPrice = pn.FromPrice;
                o.Id = Guid.NewGuid();
                o.Image = pn.Image;
                o.ImpressionUrl = pn.ImpressionUrl;
                o.InStock = pn.InStock;
                o.ISBN = pn.ISBN;
                o.MobileUrl = pn.MobileUrl;
                o.Name = pn.Name;
                o.Offline = pn.Offline;
                o.Online = pn.Online;
                o.Price = pn.Price;
                o.ProductId = p.Id; // Id of product created above
                o.RetailPrice = pn.RetailPrice;
                o.SalePrice = pn.SalePrice;
                o.SKU = pn.SKU;
                o.StandardShippingCost = pn.StandardShippingCost;
                o.Url = pn.Url;
                o.MPN = pn.MPN;
                o.Manufacturer = pn.Manufacturer;
                o.ManufacturerID = pn.ManufacturerID;
                o.Brand = pn.Brand;
                o.Warranty = pn.Warranty;
                o.Promotion = pn.Promotion;

                o.StartDate = pn.StartDate;
                o.EndDate = pn.EndDate;
                o.IsActive = true;

                if (o.EndDate < DateTimeOffset.Now) // if we have parsed an expired offer, set it to be expired
                {
                    o.IsActive = false;
                }

                if (String.IsNullOrEmpty(o.SalePrice)) // if offer has no sale price, the sale price is the normal price
                {
                    o.SalePrice = o.Price;
                }

                if (String.IsNullOrEmpty(o.RetailPrice))
                {
                    o.RetailPrice = o.Price;
                }

                o.CreatedOn = DateTimeOffset.Now;
                o.LastModified = DateTimeOffset.Now;
                
                ///////////////////////////////////////////
                // Step 7. Process / Publish to database //
                ///////////////////////////////////////////
                try
                {
                    // Product / Offer
                    /*sw.Start();
                    Graph.Instance.Cypher
                        .ForEach("(cat in {categories} | MERGE (c:Category {Name:cat.Name}) ON CREATE SET c = cat MERGE (par:Category {Id: c.ParentId}) MERGE (c)<-[:SUBCATEGORY]-(par))")
                        .WithParam("categories", categories)
                        .ExecuteWithoutResults();
                    sw.Stop();
                    Logger.Log("Create Categories: " + sw.ElapsedMilliseconds + "ms");
                    sw.Reset();

                    sw.Start();
                    Graph.Instance.Cypher
                        .ForEach("(token in {tokens} | MERGE (tok:Token {Word:token}))")
                        .WithParam("tokens", tokens)
                        .ExecuteWithoutResults();
                    sw.Stop();
                    Logger.Log("Create Tokens: " + sw.ElapsedMilliseconds + "ms");
                    sw.Reset();*/

                    //sw.Start();
                    
                    p.Offers.Add(o);

                    await Search.IndexAsync(p);
                    //await Search.IndexAsync(o);

                    /*await Graph.Instance.Cypher
                        .Match("(s:Store {Id: {sid}})") // match the store, brand, and manufacturer
                        .Merge("(brand:Brand {Name: {b}})")
                        .Merge("(man:Manufacturer {Name: {m}})")
                        .Merge("(pro:Product {Name: {pna}, Manufacturer: {m}, Brand: {b}})") // find or create the product
                        .OnCreate()
                        .Set("pro = {pp}") // set product/offer data if created
                        .Merge("(off:Offer {StoreId: {sid}, SalePrice: {offp}, Manufacturer: {m}, Brand: {b}})")
                        .OnCreate()
                        .Set("off = {oo}") // set offer data if created
                        .Merge("(off)<-[:HAS_OFFER]-(pro)") // link product and offer
                        .Merge("(pro)-[:STORE]->(s)") // link to store
                        .Merge("(brand)<-[:BRAND]-(pro)-[:MANUFACTURER]->(man)") // link to brand and manufacturer                        
                        .ForEach("(token in {tokens} | MERGE (tok:Token {Word:token}) MERGE (pro)-[:NAME]->(tok))") // merge/create all tokens and create relationship between product/token(s)
                        .ForEach("(cat in {categories} | MERGE (c:Category {Name:cat.Name}) ON CREATE SET c = cat MERGE (pro)-[:CATEGORY]-(c) MERGE (par:Category {Id: c.ParentId}) MERGE (c)<-[:SUBCATEGORY]-(par))")
                        .WithParams(new
                        {
                            b = p.Brand,
                            m = p.Manufacturer,
                            pna = p.Name,
                            pp = p,
                            oo = o,
                            sid = pn.StoreId,
                            offp = o.SalePrice,
                            tokens,
                            categories
                        })
                        //.ExecuteWithoutResults();
                        .ExecuteWithoutResultsAsync();*/
                    
                    //sw.Stop();

                    //Logger.Log("Milkshake Magic - " + sw.ElapsedMilliseconds + " ms");
                    //sw.Reset();
               
                    //Logger.Log("Successfully added product - " + p.Name);
                                                            
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error most likely due to duplicate product constraint.");
                    Logger.Error(e);
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("(Error) Failed to add product: " + e.ToString());
                Logger.Error(e);
                return;
            }
        }

        private static RawProduct GetSafeRawProduct(RawProduct pn)
        {
            // HTML Tag Decode
            pn.Name = WebUtility.HtmlDecode(pn.Name);
            pn.Price = WebUtility.HtmlDecode(pn.Price);

            if (String.IsNullOrEmpty(pn.Description))
            {
                if (!String.IsNullOrEmpty(pn.LongDescription))
                {
                    pn.Description = pn.LongDescription;
                }
                else if (!String.IsNullOrEmpty(pn.ShortDescription))
                {
                    pn.Description = pn.ShortDescription;                
                }
                else
                {
                    pn.Description = "";
                }
            }

            pn.Description = WebUtility.HtmlDecode(pn.Description);
            pn.AdvertiserCategory = WebUtility.HtmlDecode(pn.AdvertiserCategory);
            pn.Brand = WebUtility.HtmlDecode(pn.Brand);
            pn.Category = WebUtility.HtmlDecode(pn.Category);
            pn.Keywords = WebUtility.HtmlDecode(pn.Keywords);
            pn.Manufacturer = WebUtility.HtmlDecode(pn.Manufacturer);
            pn.Promotion = WebUtility.HtmlDecode(pn.Promotion);
            pn.SubCategory = WebUtility.HtmlDecode(pn.SubCategory);
            pn.ThirdPartyCategory = WebUtility.HtmlDecode(pn.ThirdPartyCategory);
            pn.Label = WebUtility.HtmlDecode(pn.Label);
            pn.Warranty = WebUtility.HtmlDecode(pn.Warranty);

            // Take care of &amp; encoded URLs
            pn.Url = WebUtility.UrlDecode(pn.Url);
            pn.AffiliateUrl = WebUtility.UrlDecode(pn.AffiliateUrl);
            pn.ImpressionUrl = WebUtility.UrlDecode(pn.ImpressionUrl);
            pn.MobileUrl = WebUtility.UrlDecode(pn.MobileUrl);

            // Fix &nbsp; and double spaces in name and description
            pn.Name = Regex.Replace(pn.Name, @"<[^>]+>|&nbsp;", " ").Trim();
            pn.Name = Regex.Replace(pn.Name, @"\s{2,}", " ");

            pn.Description = Regex.Replace(pn.Description, @"<[^>]+>|&nbsp;", " ").Trim();
            pn.Description = Regex.Replace(pn.Description, @"\s{2,}", " ");

            // Fix invalid UPC/GTIN
            if (pn.UPC == "000000000000" || !IsValidGtin(pn.UPC))            
                pn.UPC = string.Empty;            
            //if (pn.GTIN == "000000000000" || !IsValidGtin(pn.GTIN))            
            //    pn.GTIN = null;
            
            if (String.IsNullOrEmpty(pn.Brand))
                pn.Brand = string.Empty;

            if (String.IsNullOrEmpty(pn.Manufacturer))
                pn.Manufacturer = string.Empty;

            return pn;
        }

        private static List<string> ParseProductTokens(RawProduct pn)
        {
            // Remove characters we are globally not using in tokens from the product name
            string name = pn.Name.Replace("\'", "").Replace("\"", "").Replace(".", "").Replace(",", "").Replace(":", "")
                   .Replace(";", "").Replace("(", " ").Replace(")", " ").Replace("/", " ").Replace("\\", " ")
                   .Replace("[", " ").Replace("]", " ").Replace("{", " ").Replace("}", " ")
                   .Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("+", "").Replace("=", "")
                   .Replace("`", "").Replace("~", "").Replace("?", "").Replace("|", "")
                   .Replace("  ", " ")
                   .Replace(" the ", " ").Replace(" and ", " ")
                   .Trim();

            List<string> tokens = name.ToLower().Trim().Split(' ').Where(x => x.Length > 2).Distinct().ToList();

            // Check if raw product has keywords specified (delimited by commas or simply one keyword
            if (!String.IsNullOrEmpty(pn.Keywords) && pn.Keywords.Length > 2)
            {
                string keywords = pn.Keywords.Replace("\'", "").Replace("\"", "").Replace(".", "").Replace(":", "")
                   .Replace(";", "").Replace("(", " ").Replace(")", " ").Replace("/", " ").Replace("\\", " ")
                   .Replace("[", " ").Replace("]", " ").Replace("{", " ").Replace("}", " ")
                   .Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("+", "").Replace("=", "")
                   .Replace("`", "").Replace("~", "").Replace("?", "").Replace("|", "")
                   .Replace("  ", " ")
                   .Replace(" the ", " ").Replace(" and ", " ")
                   .Trim();

                if (keywords.Contains(",")) // split multiple keywords
                {
                    tokens.AddRange(keywords.ToLower().Trim().Split(',').Where(x => x.Length > 2).Distinct().ToList());
                }
                else // singular keyword
                {
                    tokens.Add(keywords.ToLower().Trim());
                }
            }

            return tokens;
        }

        private static List<Category> ParseProductTaxonomy(RawProduct pn)
        {
            if (pn == null)
                return new List<Category>();

            List<Category> categories = new List<Category>();

            // pn.Category
            if (!String.IsNullOrEmpty(pn.Category))
            {
                pn.Category = pn.Category.Replace(",", ">").Replace("/",">").Replace("\\",">").Replace("~~",">");
                // Taxonomy Hierarchy
                if (pn.Category.Contains(">"))
                {
                    List<string> cats = pn.Category.Split('>').ToList();

                    Category parent = null;
                    Category root = null;
                    foreach (string cc in cats)
                    {
                        string s = cc.Trim();
                        Category c = new Category();
                        c.Id = Guid.NewGuid();
                        c.Name = s;

                        if (parent == null && root == null) // first category in order is root that is actually added to list
                        {
                            c.IsRootCategory = true;
                            c.ParentId = Guid.Empty;
                            c.Subcategories = new List<Category>();
                            parent = c;
                            root = c;
                        }
                        else
                        {
                            c.ParentId = parent.Id;
                            c.IsRootCategory = false;
                            if (parent.Subcategories == null)
                                parent.Subcategories = new List<Category>();
                            parent.Subcategories.Add(c);
                            parent = c;
                        }
                    }
                    categories.Add(root);
                }
                // Potentially Root Category
                else
                {
                    Category rc = new Category();
                    rc.Id = Guid.NewGuid();
                    rc.Name = pn.Category.Trim();
                    rc.ParentId = Guid.Empty;
                    rc.IsRootCategory = true;
                    categories.Add(rc);
                }
            }

            // pn.SubCategory
            if (!String.IsNullOrEmpty(pn.SubCategory))
            {
                pn.SubCategory = pn.SubCategory.Replace(",", ">").Replace("/", ">").Replace("\\", ">").Replace("~~", ">");
                // Taxonomy Hierarchy
                if (pn.SubCategory.Contains(">"))
                {
                    List<string> cats = pn.SubCategory.Split('>').ToList();

                    Category parent = null;
                    Category root = null;
                    foreach (string cc in cats)
                    {
                        string s = cc.Trim();
                        Category c = new Category();
                        c.Id = Guid.NewGuid();
                        c.Name = s;

                        if (parent == null && root == null) // first category in order is root
                        {
                            c.IsRootCategory = true;
                            c.ParentId = Guid.Empty;
                            c.Subcategories = new List<Category>();
                            parent = c;
                            root = c;
                        }
                        else
                        {
                            c.ParentId = parent.Id;
                            c.IsRootCategory = false;
                            if (parent.Subcategories == null)
                                parent.Subcategories = new List<Category>();
                            parent.Subcategories.Add(c);
                            parent = c;
                        }
                    }
                    categories.Add(root);
                }
                // Potentially Root Category
                else
                {
                    Category rc = new Category();
                    rc.Id = Guid.NewGuid();
                    rc.Name = pn.SubCategory.Trim();
                    rc.ParentId = Guid.Empty;
                    rc.IsRootCategory = true;
                    categories.Add(rc);
                }
            }

            // pn.AdvertiserCategory
            if (!String.IsNullOrEmpty(pn.AdvertiserCategory))
            {
                pn.AdvertiserCategory = pn.AdvertiserCategory.Replace(",", ">").Replace("/", ">").Replace("\\", ">").Replace("~~", ">");
                // Taxonomy Hierarchy
                if (pn.AdvertiserCategory.Contains(">"))
                {
                    List<string> cats = pn.AdvertiserCategory.Split('>').ToList();

                    Category parent = null;
                    Category root = null;
                    foreach (string cc in cats)
                    {
                        string s = cc.Trim();
                        Category c = new Category();
                        c.Id = Guid.NewGuid();
                        c.Name = s;

                        if (parent == null && root == null) // first category in order is root
                        {
                            c.IsRootCategory = true;
                            c.ParentId = Guid.Empty;
                            c.Subcategories = new List<Category>();
                            parent = c;
                            root = c;
                        }
                        else
                        {
                            c.ParentId = parent.Id;
                            c.IsRootCategory = false;
                            if (parent.Subcategories == null)
                                parent.Subcategories = new List<Category>();
                            parent.Subcategories.Add(c);
                            parent = c;
                        }
                    }
                    categories.Add(root);
                }
                // Potentially Root Category
                else
                {
                    Category rc = new Category();
                    rc.Id = Guid.NewGuid();
                    rc.Name = pn.AdvertiserCategory.Trim();
                    rc.ParentId = Guid.Empty;
                    rc.IsRootCategory = true;
                    categories.Add(rc);
                }
            }

            // pn.ThirdPartyCategory
            if (!String.IsNullOrEmpty(pn.ThirdPartyCategory))
            {
                pn.ThirdPartyCategory = pn.ThirdPartyCategory.Replace(",", ">").Replace("/", ">").Replace("\\", ">").Replace("~~", ">");
                // Taxonomy Hierarchy
                if (pn.ThirdPartyCategory.Contains(">"))
                {
                    List<string> cats = pn.ThirdPartyCategory.Split('>').ToList();

                    Category parent = null;
                    Category root = null;
                    foreach (string cc in cats)
                    {
                        string s = cc.Trim();
                        Category c = new Category();
                        c.Id = Guid.NewGuid();
                        c.Name = s;

                        if (parent == null && root == null) // first category in order is root
                        {
                            c.IsRootCategory = true;
                            c.ParentId = Guid.Empty;
                            c.Subcategories = new List<Category>();
                            parent = c;
                            root = c;
                        }
                        else
                        {
                            c.ParentId = parent.Id;
                            c.IsRootCategory = false;
                            if (parent.Subcategories == null)
                                parent.Subcategories = new List<Category>();
                            parent.Subcategories.Add(c);
                            parent = c;
                        }
                    }
                    categories.Add(root);
                }
                // Potentially Root Category
                else
                {
                    Category rc = new Category();
                    rc.Id = Guid.NewGuid();
                    rc.Name = pn.ThirdPartyCategory.Trim();
                    rc.ParentId = Guid.Empty;
                    rc.IsRootCategory = true;
                    categories.Add(rc);
                }
            }

            return categories;
        }
                        
        public static Product GetProduct(string url)
        {            
            try
            {
                return Graph.Instance.Cypher
                    .Match("(p:Product)-[:HAS_OFFER]-(o:Offer)")
                    .Where((Offer o) => o.Url == url)
                    .ReturnDistinct(p => p.As<Product>())
                    .Results.First();
            }
            catch  { return null; }
        }

        public static Product GetProduct(Guid id)
        {
            try
            {
                return Graph.Instance.Cypher
                    .Match("(p:Product)")
                    .Where((Product p) => p.Id == id)
                    .ReturnDistinct(p => p.As<Product>())
                    .Limit(1)
                    .Results.First();
            }
            catch { return null; }
        }

        public static bool ProductExists(Guid id)
        {
            try
            {
                return Graph.Instance.Cypher
                    .Match("(p:Product)")
                    .Where((Product p) => p.Id == id)
                    .ReturnDistinct(p => p.CountDistinct())
                    .Results.Single() > 0;
            }
            catch { return false; }
        }

        public static int GetProductsCount(string s)
        {
            return 0;
        }

        public static IEnumerable<Product> GetProducts(string s)
        {
            return GetProducts(s, 0);
        }

        private static Regex _gtinRegex = new System.Text.RegularExpressions.Regex("^(\\d{8}|\\d{12,14})$");
        public static bool IsValidGtin(string code)
        {
            if (code == null)
                return false;

            return _gtinRegex.IsMatch(code); // check if all digits and with 8, 12, 13 or 14 digits
            //code = code.PadLeft(14, '0'); // stuff zeros at start to guarantee 14 digits
            //int[] mult = Enumerable.Range(0, 13).Select(i => int.Parse(code[i].ToString()) * ((i % 2 == 0) ? 3 : 1)).ToArray(); // STEP 1: without check digit, "Multiply value of each position" by 3 or 1
            //int sum = mult.Sum(); // STEP 2: "Add results together to create sum"
            //return (10 - (sum % 10)) % 10 == int.Parse(code[13].ToString()); // STEP 3 Equivalent to "Subtract the sum from the nearest equal or higher multiple of ten = CHECK DIGIT"
        }

        public static IEnumerable<Product> GetProducts(string s, int page)
        {
            IEnumerable<Product> products = new List<Product>();
            List<string> keywords = new List<string>();
            List<Guid> stores = new List<Guid>();
            List<string> manufacturers = new List<string>();
            //List<string> brands = new List<string>();
            
            if (IsValidGtin(s))
            {
                Logger.Log("Valid UPC Code: " + s);

                List<Product> pros = Graph.Instance.Cypher
                    .Match("(p:Product)")
                    .Where((Product p) => p.UPC == s)
                    .ReturnDistinct(p => p.As<Product>())
                    .OrderBy("p.Name")
                    .Skip(25 * page)
                    .Limit(25)
                    .Results.ToList();

                return pros;
            }

            s = s.ToLower().Trim();

            /*Parallel.ForEach(Store.Stores, shop =>
            {
                if (s.Contains(shop.Name.ToLower()) || shop.Alternatives.Contains(s))
                {
                    stores.Add(shop.Id);
                    s = s.Replace(shop.Name.ToLower(), "");

                    foreach (string alt in shop.Alternatives)
                        s = s.Replace(alt, "");
                }
            });*/

            stores = Store.Find(s).Select(x => x.Id).ToList();
            manufacturers = Manufacturer.Find(s).Select(x => x.Name).ToList();
            //brands = Brand.Find(s).Select(x => x.Name).ToList();
                        
            s = s.Replace("\'", "").Replace("\"", "").Replace(".", "").Replace(",", "").Replace(":", "")
                        .Replace(";", "").Replace("(", " ").Replace(")", " ").Replace("/", " ").Replace("\\", " ")
                        .Replace("[", " ").Replace("]", " ").Replace("{", " ").Replace("}", " ")
                        .Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("+", "").Replace("=", "")
                        .Replace("`", "").Replace("~", "").Replace("?", "").Replace("|", "")
                        .Replace("  ", " ")
                        .Replace(" the ", " ").Replace(" and ", " ")
                        .Trim();

            keywords = s.Split(' ').Where(x => x.Length > 2).Distinct().ToList();

            if (keywords.Count <= 0)
            {
                keywords.Add(s);
            }
            
            try
            {
                if (keywords.Count > 0 && stores.Count > 0 && manufacturers.Count > 0) // all 3
                {
                    return Graph.Instance.Cypher
                        .Match("(p:Product)", "(tok:Token)", "(s:Store)", "(m:Manufacturer)")
                        .Where("tok.Word IN {tokens}")
                        .AndWhere("s.Id IN {stores}")
                        .AndWhere("m.Name IN {manufacturers}")
                        .WithParams(new
                        {
                            tokens = keywords,
                            stores = stores,
                            manufacturers = manufacturers,
                        })
                        .Match("(p)-[r:NAME]->(tok)")
                        .Match("(p)-[r2:MANUFACTURER]-(m)")
                        .Match("(p)-[r3:STORE]-(s)")
                        .With("p,count(r) as tc, count(r2) as mc, count(r3) as sc")
                        .With("p,(tc + mc*10 + sc*50) as score")
                        .OrderByDescending("score")
                        .Skip(25 * page)
                        .Limit(25)
                        .ReturnDistinct(p => p.As<Product>())
                        .Results.ToList();
                }
                else if (keywords.Count > 0 && stores.Count > 0 && manufacturers.Count == 0) // all but manufacturers
                {
                    return Graph.Instance.Cypher
                        .Match("(p:Product)", "(tok:Token)", "(s:Store)", "(m:Manufacturer)")
                        .Where("tok.Word IN {tokens}")
                        .AndWhere("s.Id IN {stores}")                        
                        .WithParams(new
                        {
                            tokens = keywords,
                            stores = stores,                            
                        })
                        .Match("(p)-[r:NAME]->(tok)")                        
                        .Match("(p)-[r3:STORE]-(s)")
                        .With("p,count(r) as tc, count(r3) as sc")
                        .With("p,(tc + sc*50) as score")
                        .OrderByDescending("score")
                        .Skip(25 * page)
                        .Limit(25)
                        .ReturnDistinct(p => p.As<Product>())
                        .Results.ToList();
                }
                else if (keywords.Count > 0 && stores.Count == 0 && manufacturers.Count > 0) // all but stores
                {
                    return Graph.Instance.Cypher
                        .Match("(p:Product)", "(tok:Token)", "(m:Manufacturer)")
                        .Where("tok.Word IN {tokens}")                        
                        .AndWhere("m.Name IN {manufacturers}")
                        .WithParams(new
                        {
                            tokens = keywords,                            
                            manufacturers = manufacturers,
                        })
                        .Match("(p)-[r:NAME]->(tok)")
                        .Match("(p)-[r2:MANUFACTURER]-(m)")                        
                        .With("p,count(r) as tc, count(r2) as mc")
                        .With("p,(tc + mc*10) as score")
                        .OrderByDescending("score")
                        .Skip(25 * page)
                        .Limit(25)
                        .ReturnDistinct(p => p.As<Product>())
                        .Results.ToList();
                }
                else if (keywords.Count == 0 && stores.Count > 0 && manufacturers.Count > 0) // all but tokens
                {
                    return Graph.Instance.Cypher
                        .Match("(p:Product)", "(s:Store)", "(m:Manufacturer)")                        
                        .Where("s.Id IN {stores}")
                        .AndWhere("m.Name IN {manufacturers}")
                        .WithParams(new
                        {                            
                            stores = stores,
                            manufacturers = manufacturers,
                        })                        
                        .Match("(p)-[r2:MANUFACTURER]-(m)")
                        .Match("(p)-[r3:STORE]-(s)")
                        .With("p, count(r2) as mc, count(r3) as sc")
                        .With("p,(mc*10 + sc*50) as score")
                        .OrderByDescending("score")
                        .Skip(25 * page)
                        .Limit(25)
                        .ReturnDistinct(p => p.As<Product>())
                        .Results.ToList();
                }
                else if (keywords.Count > 0 && stores.Count == 0 && manufacturers.Count == 0) // only tokens
                {
                    return Graph.Instance.Cypher
                        .Match("(p:Product)", "(tok:Token)")
                        .Where("tok.Word IN {tokens}")
                        .WithParam("tokens", keywords)
                        .Match("(p)-[r:NAME]->(tok)")
                        .With("p,count(r) as score")
                        .OrderByDescending("score")
                        .Skip(25 * page)
                        .Limit(25)
                        .ReturnDistinct(p => p.As<Product>())
                        .Results.ToList();
                }
                else if (keywords.Count == 0 && stores.Count > 0 && manufacturers.Count == 0) // only stores
                {
                    return Graph.Instance.Cypher
                        .Match("(p:Product)", "(s:Store)")
                        .Where("s.Id IN {stores}")
                        .WithParam("stores", stores)
                        .Match("(p)-[r:STORE]->(s)")
                        .With("p,count(r) as score")
                        .OrderByDescending("score")
                        .Skip(25 * page)
                        .Limit(25)
                        .ReturnDistinct(p => p.As<Product>())
                        .Results.ToList();
                }
                else if (keywords.Count == 0 && stores.Count == 0 && manufacturers.Count > 0) // only manufacturers
                {
                    return Graph.Instance.Cypher
                        .Match("(p:Product)", "(m:Manufacturer)")
                        .Where("m.Name IN {mans}")
                        .WithParam("mans", manufacturers)
                        .Match("(p)-[r:MANUFACTURER]->(m)")
                        .With("p,count(r) as score")
                        .OrderByDescending("score")
                        .Skip(25 * page)
                        .Limit(25)
                        .ReturnDistinct(p => p.As<Product>())
                        .Results.ToList();
                }
                else
                {
                    return new List<Product>();
                }

                /*return query
                    .OrderByDescending("score")
                    .Skip(25 * page)
                    .Limit(25)
                    .ReturnDistinct(p => p.As<Product>())
                    //.ReturnDistinct((p, score) => new { p = p.As<Product>(), score = score.As<int>() })                        
                    //.Skip(25 * page)
                    //.Limit(25)
                    .Results.ToList();
                    //.Results.Select(x => x.p).ToList();*/
                          
            }
            catch (Exception ex)
            { Logger.Error(ex.ToString()); return new List<Product>(); }
        }

        public static long GetTotalProductCount()
        {
            try
            {
                return Graph.Instance.Cypher
                    .Match("(p:Product)")
                    .ReturnDistinct(p => p.CountDistinct())
                    .Results.Single();
            }
            catch { return 0; }
        }

        public static long GetTotalOfferCount()
        {
            try
            {
                return Graph.Instance.Cypher
                    .Match("(o:Offer)")
                    .ReturnDistinct(p => p.CountDistinct())
                    .Results.Single();
            }
            catch { return 0; }
        }
    }
}
