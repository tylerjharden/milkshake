using Nest;
using Newtonsoft.Json;
using System;

namespace Milkshake
{
    // Represent's an actual product being sold by a specific site (similar to how Indix, Amazon, Semantics3 categorize single products with multiple offers)
    public class Offer
    {
        public Offer()
        {
            Id = Guid.Empty;            
        }
                                        
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("ProductId")]
        public Guid ProductId { get; set; }

        [JsonProperty("Store")]
        public string Store { get; set; }

        [JsonProperty("StoreId")]
        [ElasticProperty(Name="storeId", Index=FieldIndexOption.NotAnalyzed)]
        public Guid StoreId { get; set; }

        [JsonProperty("CreatedOn")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("LastModified")]
        public DateTimeOffset LastModified { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
                
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Image")]
        public string Image { get; set; }
                
        // Price of Current Sale
        [JsonProperty("SalePrice")]
        public string SalePrice { get; set; }

        // Actual Current Price
        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        // MSRP
        [JsonProperty("RetailPrice")]
        public string RetailPrice { get; set; }

        // Low-end Price if price range (i.e. $10 - $50) FromPrice: $10, Price: $50
        [JsonProperty("FromPrice")]
        public string FromPrice { get; set; }

        [JsonProperty("StandardShippingCost")]
        public string StandardShippingCost { get; set; }

        // new, used, refurbished
        [JsonProperty("Condition")]
        public string Condition { get; set; }

        //[Obsolete("Products should specify a GTIN, of which UPC is a sub-type")]
        [JsonProperty("UPC")]
        public string UPC { get; set; }

        // Global Trade Identification Number (US Only: UPC)
        [JsonProperty("GTIN")]
        public string GTIN { get; set; }

        [JsonProperty("ISBN")]
        public string ISBN { get; set; }

        [JsonProperty("SKU")]
        public string SKU { get; set; }

        [JsonProperty("Brand")]
        public string Brand { get; set; }

        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("ManufacturerID")]
        public string ManufacturerID { get; set; }

        [JsonProperty("MPN")]
        public string MPN { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("MobileUrl")]
        public string MobileUrl { get; set; }

        [JsonProperty("AffiliateUrl")]
        public string AffiliateUrl { get; set; }

        [JsonProperty("ImpressionUrl")]
        public string ImpressionUrl { get; set; }
                
        [JsonProperty("Offline")]
        public bool Offline { get; set; }

        [JsonProperty("Online")]
        public bool Online { get; set; }

        [JsonProperty("InStock")]
        public bool InStock { get; set; }

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Warranty")]
        public string Warranty { get; set; }

        [JsonProperty("Promotion")]
        public string Promotion { get; set; }

        /////////////////////////
        // Milkshake Analytics //
        /////////////////////////
        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("StartDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("EndDate")]
        public DateTimeOffset EndDate { get; set; }
    }    
}
