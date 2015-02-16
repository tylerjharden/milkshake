using Nest;
using Nest.Domain;
using Nest.SerializationExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Milkshake
{    
    public class MappedProduct
    {
        // Product
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }

        // Offer
        public Guid OfferId { get; set; }
        public string Store { get; set; }
        public Guid StoreId { get; set; }
        public string SalePrice { get; set; }
        public string Price { get; set; }
        public string RetailPrice { get; set; }
        public string Url { get; set; }
    }

    // Represents a product in the Milkshake database.
    [ElasticType(Name = "product", IdProperty = "id")]
    public class Product
    {
        public Product()
        {
            Id = Guid.Empty;
            Score = 1;            
            UPC = "";
            GTIN = "";
            IsMilkshake = true;
            Offers = new List<Offer>();
        }
        
        [JsonProperty("Id")]
        [ElasticProperty(Name="id", Store=true, Index=FieldIndexOption.NotAnalyzed)]
        public Guid Id { get; set; }

        [JsonProperty("CreatedOn")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("LastModified")]
        public DateTimeOffset LastModified { get; set; }

        [JsonProperty("Name")]
        [ElasticProperty(Name = "name", Store = true, Index = FieldIndexOption.Analyzed, Analyzer = "snowball", IndexAnalyzer = "snowball")]
        public string Name { get; set; }

        [JsonProperty("Keywords")]
        [ElasticProperty(Name = "keywords", Store = true, Index = FieldIndexOption.Analyzed, Analyzer = "snowball", IndexAnalyzer = "snowball")]
        public string Keywords { get; set; }

        [JsonProperty("Description")]
        [ElasticProperty(Name = "description", Store = true, Index = FieldIndexOption.Analyzed, Analyzer = "snowball", IndexAnalyzer="snowball")]
        public string Description { get; set; }
                
        [JsonProperty("Image")]
        public string Image { get; set; }
                
        [JsonProperty("AdditionalImages")]
        public List<String> AdditionalImages { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

        [ElasticProperty(Name="offers")]
        public List<Offer> Offers { get; set; }
                               
        //[Obsolete("Products should specify a GTIN, of which UPC is a sub-type")]
        [JsonProperty("UPC")]
        [ElasticProperty(Name = "upc", Store = true, Analyzer = "not_analyzed")]
        public string UPC { get; set; }

        // Global Trade Identification Number (US Only: UPC)
        [JsonProperty("GTIN")]
        [ElasticProperty(Name = "gtin", Store = true, Analyzer = "not_analyzed")]
        public string GTIN { get; set; }

        [JsonProperty("ISBN")]
        [ElasticProperty(Name = "isbn", Store = true, Analyzer = "not_analyzed")]
        public string ISBN { get; set; }

        [JsonProperty("SKU")]
        public string SKU { get; set; }

        [JsonProperty("ASIN")]
        [ElasticProperty(Name = "asin", Store = true, Index = FieldIndexOption.NotAnalyzed, Analyzer = "not_analyzed")]
        public string ASIN { get; set; }

        [JsonProperty("Brand")]
        [ElasticProperty(Name = "brand", Store = true, Index = FieldIndexOption.NotAnalyzed, Analyzer = "not_analyzed")]
        public string Brand { get; set; }
                
        [JsonProperty("Manufacturer")]
        [ElasticProperty(Name = "manufacturer", Store = true, Index = FieldIndexOption.NotAnalyzed, Analyzer = "not_analyzed")]
        public string Manufacturer { get; set; }

        [JsonProperty("ManufacturerID")]
        public string ManufacturerID { get; set; }

        [JsonProperty("MPN")]
        public string MPN { get; set; }
                                
        [JsonProperty("Color")]
        public string Color { get; set; }

        [JsonProperty("ColorMap")]
        public string ColorMap { get; set; }

        [JsonProperty("Gender")]
        public string Gender { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("SizeType")]
        public string SizeType { get; set; }

        [JsonProperty("SizeSystem")]
        public string SizeSystem { get; set; }

        // newborn, infant, toddler, kids, adult
        [JsonProperty("AgeGroup")]
        public string AgeGroup { get; set; }

        [JsonProperty("Availability")]
        public string Availability { get; set; }

        [JsonProperty("Author")]
        public string Author { get; set; }

        [JsonProperty("Artist")]
        public string Artist { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Publisher")]
        public string Publisher { get; set; }
        
        [JsonProperty("Gift")]
        public bool Gift { get; set; }
                
        [JsonProperty("Adult")]
        public bool Adult { get; set; }

        [JsonProperty("Weight")]
        public string Weight { get; set; }

        [JsonProperty("Condition")]
        public string Condition { get; set; }

        //////////////////////
        // Milkshake Search //
        //////////////////////
        [JsonIgnore]
        public bool IsMilkshake { get; set; }
        [JsonIgnore]
        public string Price { get; set; }
        [JsonIgnore]
        public string Store { get; set; }

        public Guid StoreId { get; set; }
        // Score held when returning items during a product search
        [JsonIgnore]
        public int Score { get; set; }
                
        //////////////////////////////////
        // Milkshake analytics / rating //
        //////////////////////////////////
        [JsonProperty("Rating")]
        public int Rating { get; set; }

        [JsonProperty("RatingCount")]
        public int RatingCount { get; set; }

        [JsonProperty("Views")]
        public int Views { get; set; }

        [JsonProperty("Saves")]
        public int Saves { get; set; }

                
        public static bool IsValid(Product pn)
        {
            if (pn.Name == null || String.IsNullOrEmpty(pn.Name.Trim()))
                return false;

            if (pn.Image == null || String.IsNullOrEmpty(pn.Image.Trim()))
                return false;
            
            return true;
        }

        public bool IsValid()
        {
            if (this.Name == null || String.IsNullOrEmpty(this.Name.Trim()))
                return false;

            if (this.Image == null || String.IsNullOrEmpty(this.Image.Trim()))
                return false;
                       
            
            return true;
        }

        public void AddOffer(Offer o)
        {
            try
            {
                /*Graph.Instance.Cypher
                    .Match("(p:Product),(off:Offer)")
                    .Where((Product p) => p.Id == this.Id)
                    .AndWhere((Offer off) => off.Id == o.Id)
                    .Create("(p)-[:HAS_OFFER]->(off)")
                    .ExecuteWithoutResults();*/
                Offers.Add(o);
            }
            catch
            {

            }
        }

        public int GetOfferCount()
        {
            try
            {
                /*return (int)Graph.Instance.Cypher
                    .Match("(p:Product)-[:HAS_OFFER]-(o:Offer)")
                    .Where((Product p) => p.Id == this.Id)
                    .Return(o => o.Count())
                    .Results.Single();*/
                return Offers.Count;
            }
            catch
            {
                return 0;
            }
        }

        public List<Offer> GetOffers()
        {
            try
            {
                /*return Graph.Instance.Cypher
                    .Match("(p:Product)-[:HAS_OFFER]->(o:Offer)")
                    .Where((Product p) => p.Id == this.Id)
                    .Return(o => o.As<Offer>())
                    .Results.ToList();*/
                return Offers;
            } 
            catch
            {
                return new List<Offer>();
            }
        }

        public Offer GetLowestOffer()
        {
            try
            {
                //List<Offer> offers = GetOffers();
                //return offers.OrderBy(o => o.Price).First();
                return Offers.OrderBy(o => o.Price).First();
            }
            catch
            {
                return new Offer();
            }
        }

        public Offer GetHighestOffer()
        {
            try
            {
                //List<Offer> offers = GetOffers();
                //return offers.OrderByDescending(o => o.Price).First();
                return Offers.OrderByDescending(o => o.Price).First();
            }
            catch
            {
                return new Offer();
            }
        }
    }
}
