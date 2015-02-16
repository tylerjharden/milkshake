using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake
{
    public class Category
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("ParentId")]
        public Guid ParentId { get; set; }

        [JsonProperty("Alternatives")]
        public List<string> Alternatives { get; set; }

        [JsonIgnore]
        public List<Category> Subcategories { get; set; }

        [JsonProperty("IsRootCategory")]
        public bool IsRootCategory { get; set; }

        public Category()
        {

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

        public static void Create(Category c)
        {
            try
            {
                if (c.IsRootCategory)
                {
                    Graph.Instance.Cypher
                        .Merge("(cat:Category {Name: {name}})")
                        .WithParam("name", c.Name)
                        .OnCreate()
                        .Set("cat = {c}")
                        .WithParam("c", c)
                        .ExecuteWithoutResults();
                }
                else
                {
                    if (Exists(c.ParentId))
                    {
                        Graph.Instance.Cypher
                            .Match("(p:Category)")
                            .Where((Category p) => p.Id == c.ParentId)
                            .Merge("(cat:Category {Name: {name}})<-[:SUBCATEGORY]-(p)")
                            .WithParam("name", c.Name)
                            .OnCreate()
                            .Set("cat = {c}")
                            .WithParam("c", c)                        
                        .ExecuteWithoutResults();
                    }
                    else
                    {
                        throw new NullReferenceException("Specified parent category ID does not exist.");
                    }
                }
            }
            catch (Exception ex) { Logger.Error(ex.ToString()); }
        }

        public static bool HasSubcategories(Guid id)
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)-[:SUBCATEGORY]->(s:Category)")
                        .Where((Category c) => c.Id == id)
                        .Return(s => s.Count())
                        .Results.Single() > 0;
            }
            catch { return false; }
        }

        public bool HasSubcategories()
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)-[:SUBCATEGORY]->(s:Category)")
                        .Where((Category c) => c.Id == this.Id)
                        .Return(s => s.Count())
                        .Results.Single() > 0;
            }
            catch { return false; }
        }

        public static bool Exists(string name)
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")
                        .Where((Category c) => c.Name == name)
                        .Return(c => c.Count())
                        .Results.Single() > 0;
            }
            catch { return false; }
        }

        public static bool Exists(Guid id)
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")
                        .Where((Category c) => c.Id == id)
                        .Return(c => c.Count())
                        .Results.Single() > 0;
            }
            catch { return false; }
        }

        public static Category GetCategory(string name)
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")
                        .Where((Category c) => c.Name == name)
                        .Return(c => c.As<Category>())
                        .Results.Single();
            }
            catch { return null; }
        }

        public static Category GetCategory(Guid id)
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")
                        .Where((Category c) => c.Id == id)
                        .Return(c => c.As<Category>())
                        .Results.Single();
            }
            catch { return null; }
        }

        public static List<Category> GetRootCategories()
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")
                        .Where((Category c) => c.IsRootCategory == true)
                        .Return(c => c.As<Category>())
                        .Results.ToList();
            }
            catch { return new List<Category>(); }
        }

        public static List<Category> GetAllCategories()
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")                        
                        .Return(c => c.As<Category>())
                        .Results.ToList();
            }
            catch { return new List<Category>(); }
        }

        public static List<Category> GetSubCategories(Guid id)
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)-[:SUBCATEGORY]->(s:Category)")
                        .Where((Category c) => c.Id == id)                        
                        .Return(s => s.As<Category>())
                        .Results.ToList();
            }
            catch { return new List<Category>(); }
        }

        public List<Category> GetSubCategories()
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")
                        .Where((Category c) => c.Id == this.Id)
                        .Match("(c)-[:SUBCATEGORY]->(s:Category)")
                        .Return(s => s.As<Category>())
                        .Results.ToList();
            }
            catch { return new List<Category>(); }
        }

        public static List<Product> GetProducts(Guid id, int page = 0)
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")
                        .Where((Category c) => c.Id == id)
                        .Match("(c)-[:CATEGORY]-(p:Product)")
                        .Return(p => p.As<Product>())
                        .Skip(50*page)
                        .Limit(50)
                        .Results.ToList();
            }
            catch { return new List<Product>(); }
        }

        public List<Product> GetProducts()
        {
            try
            {
                return Graph.Instance.Cypher
                        .Match("(c:Category)")
                        .Where((Category c) => c.Id == this.Id)
                        .Match("(c)-[:CATEGORY]-(p:Product)")
                        .Return(p => p.As<Product>())
                        .Results.ToList();
            }
            catch { return new List<Product>(); }
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
    }
}
