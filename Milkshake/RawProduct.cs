using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Milkshake
{
    // Represents a product in the Milkshake database.
    public class RawProduct
    {
        public RawProduct()
        {
            Id = Guid.Empty;            
            Store = "";
            StoreId = Guid.Empty;
            UPC = "";

            // Safe, empty, default price/currency
            StandardShippingCost = "";
            Currency = "USD";
            Price = "";
            FromPrice = "";
            RetailPrice = "";
            SalePrice = "";

            // Safe, empty categories
            Category = "";
            SubCategory = "";
            AdvertiserCategory = "";
            ThirdPartyCategory = "";

            // Default offer start/end dates
            StartDate = DateTimeOffset.Now;
            EndDate = DateTimeOffset.MaxValue;
        }
        
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("CreatedOn")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("LastModified")]
        public DateTimeOffset LastModified { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Keywords")]
        public string Keywords { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("ShortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("LongDescription")]
        public string LongDescription { get; set; }

        [JsonProperty("Image")]
        public string Image { get; set; }

        [JsonProperty("AdditionalImages")]
        public List<String> AdditionalImages { get; set; }

        [JsonProperty("Store")]
        public string Store { get; set; }

        public string StoreWebsite { get; set; }
        
        [JsonProperty("StoreId")]
        public Guid StoreId { get; set; }

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

        // new, used, refurbished
        [JsonProperty("Condition")]
        public string Condition { get; set; }

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

        [JsonProperty("AdvertiserCategory")]
        public string AdvertiserCategory { get; set; }

        [JsonProperty("ThirdPartyCategory")]
        public string ThirdPartyCategory { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("SubCategory")]
        public string SubCategory { get; set; }

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

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Gift")]
        public bool Gift { get; set; }

        [JsonProperty("Offline")]
        public bool Offline { get; set; }

        [JsonProperty("Online")]
        public bool Online { get; set; }

        [JsonProperty("InStock")]
        public bool InStock { get; set; }

        [JsonProperty("Adult")]
        public bool Adult { get; set; }

        [JsonProperty("Warranty")]
        public string Warranty { get; set; }

        [JsonProperty("Promotion")]
        public string Promotion { get; set; }

        /////////////////////////
        // Milkshake Analytics //
        /////////////////////////        
        [JsonProperty("StartDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("EndDate")]
        public DateTimeOffset EndDate { get; set; }

        ////////////////
        // Validation //
        ////////////////
        public static bool IsValid(RawProduct p)
        {
            if (p.Name == null || String.IsNullOrEmpty(p.Name.Trim()))
                return false;

            if (p.Image == null || String.IsNullOrEmpty(p.Image.Trim()))
                return false;

            if (p.Url == null || String.IsNullOrEmpty(p.Url.Trim()))
                return false;

            if (p.Price == null || String.IsNullOrEmpty(p.Price.Trim()))
                return false;

            if (p.Store == null || String.IsNullOrEmpty(p.Store.Trim()))
                return false;

            if (p.StoreId == null || p.StoreId == Guid.Empty)
                return false;

            return true;
        }

        public bool IsValid()
        {
            if (this.Name == null || String.IsNullOrEmpty(this.Name.Trim()))
                return false;

            if (this.Image == null || String.IsNullOrEmpty(this.Image.Trim()))
                return false;

            if (this.Url == null || String.IsNullOrEmpty(this.Url.Trim()))
                return false;

            if (this.Price == null || String.IsNullOrEmpty(this.Price.Trim()))
                return false;

            //if (this.Store == null || String.IsNullOrEmpty(this.Store.Trim()))
            //    return false;

            if (this.StoreId == null || this.StoreId == Guid.Empty)
                return false;

            return true;
        } 
    }
}
