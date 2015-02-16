using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MilkshakeTester
{
    // Represents a product in the Milkshake database.
    public class RawProduct
    {
        public enum ProductDataSource
        {
            Linkshare,
            CommissionJunction,
            Pepperjam,
            ImpactRadius,
            ShareASale,
            linkconnector,
            Performics,
            AvantLink,
            clixGalore,
            affiliatefuture,
            webgains,
            affiliatewindow,
            Google,
            Amazon,
            Nextag,
            PriceGrabber,
            BingAd,
            BingShopping,
            Become,
            Smarter,
            Pronto,
            TheFind,
            Shopzilla,
            Sears,
            Shopify,
            Magento,
            Prestashop,
            Bigcommerce,
            Tictail,
            Wordpress,
            Blogger,
            Tumblr,
            Facebook,
            Twitter,
            Fancy,
            Fab,
            Shopping,
            eBay,
            Slickdeals,
            woot,
            OpenSky,
            SkyMall,
            Indix,
            Semantics3,
            BestBuy,
            Kohls,
            Etsy,
            Wanelo,
            Pinterest,
            Target,            
            Walgreens,            
            GoodGuide,
            Rakuten,            
            Buzzillions,            
            Walmart,            
            Epinions,
            itemMaster,
            meijer,            
            Supersup,
            StockNGo,
            SortPrice,
            frogit,
            wishpot,
            shopwiki,
            oodlemarketplace,
            ShopMania,
            FindGift,
            Gifts,
            Price,
            OneWayShopping,
            Ebates,
            wineaccess,
            Pricewatch,
            PriceRunner,
            winesearcher,
            WineZap,
            MyCoupons,
            RedLaser,
            shopsavvy,
            TellApart,
            Triggit,
            celebros,
            buysafe,
            MyBuys,
            nextopia,
            SLISystems,
            ClassifiedFlyerAds,
            SearchSpring,
            PowerReviews,
            TestFreaks,
            newegg,
            houzz,
            pricefalls,
            searsmarketplace,
            Adchemy,
            adCore,
            Akami,
            Kenshoo,
            Kneon,
            monetate,
            Polyvore,
            responsys,
            Sailthru,
            shoprunner,
            sociomatic,
            thedressspot,
            bloomreach,
            
            Manual,
            Scraper,
            Feed,
            Crowdsource,
            Other,
            Invalid,
            Unknown
        }

        public RawProduct()
        {            
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
            //StartDate = DateTimeOffset.Now;
            //EndDate = DateTimeOffset.MaxValue;
        }

        public ProductDataSource Source { get; set; }
           
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
        public string ImageLink { get; set; }
        public string Logo { get; set; }

        public string Video { get; set; }

        [JsonProperty("AdditionalImages")]
        public List<String> AdditionalImages { get; set; }
        public List<String> AdditionalImageLink { get; set; }

        [JsonProperty("Store")]
        public string Store { get; set; }

        public string StoreWebsite { get; set; }
        
        [JsonProperty("StoreId")]
        public Guid StoreId { get; set; }

        // Price of Current Sale
        [JsonProperty("SalePrice")]
        public string SalePrice { get; set; }

        [JsonProperty("SaleStartDate")]
        public string SaleStartDate { get; set; }

        [JsonProperty("SaleEndDate")]
        public string SaleEndDate { get; set; }

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

        public string Tax { get; set; }

        public string Shipping { get; set; }

        [JsonProperty("StandardShippingCost")]
        public string StandardShippingCost { get; set; }

        [JsonProperty("ShippingInformation")]
        public string ShippingInformation { get; set; }

        public string ShippingWeight { get; set; }
        public string ShippingLength { get; set; }
        public string ShippingWidth { get; set; }
        public string ShippingHeight { get; set; }
        public string ShipingLabel { get; set; }
        
        // Item Grouping / Variation
        public string ItemGroupId { get; set; }
        public string VariationId { get; set; }
        public int Muiltipack { get; set; }
        public bool IsBundle { get; set; }
        
        // Unique identifiers
        public bool IdentifierExists { get; set; }

        [JsonProperty("UPC")]
        public string UPC { get; set; }

        public string EAN { get; set; }
        public string JAN { get; set; }

        // Global Trade Identification Number (US Only: UPC)
        [JsonProperty("GTIN")]
        public string GTIN { get; set; }

        public string GTIN14 { get; set; }
        public string GTIN13 { get; set; }
        public string GTIN8 { get; set; }

        [JsonProperty("ISBN")]
        public string ISBN { get; set; } // commercial books, International Standard Book Number

        [JsonProperty("SBN")]
        public string SBN { get; set; } // older books, Standard Book Numbering Code

        [JsonProperty("SKU")]
        public string SKU { get; set; } // internal identifier by merchant, Stock Keeping Unit

        public string ISSN { get; set; } // periodicals / magazine subscriptions, International Standard Serial Number

        public string NDC { get; set; } // prescription/non-prescription drugs, National Drug Code

        public string ISMN { get; set; } // printed music, International Standard Music Number

        public string ISRC { get; set; } // sound/music video recordings, International Standard Recording Code

        public string ISIL { get; set; } // identifies libraries/archives/museums, International Standard Identifier for Libraries and Related Organizations

        public string ISAN { get; set; } // audiovisual works and related versions, International Standard Audiovisual Number

        public string ISWC { get; set; } // musical works, International Standard Musical Work Code

        public string ISTC { get; set; } // text-based works, International Standard Text Code

        public string DOI { get; set; } // identify object such as an electronic document, Digital Object Identifier

        public string ISNI { get; set; } // identify public identities of contributors to media content (books, TV programs, newspaper articles), International Standard Name Identifier

        public string GPI { get; set; } // identifies drugs (manufacturer, pill size, etc), Generic Product Identifier

        public string AVIN { get; set; } // AVIN.cc, "A unique code for each wine"

        // Amazon
        public string ASIN { get; set; }

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
        public string Link { get; set; }

        [JsonProperty("MobileUrl")]
        public string MobileUrl { get; set; }
        public string MobileLink { get; set; }

        [JsonProperty("AffiliateUrl")]
        public string AffiliateUrl { get; set; }

        [JsonProperty("ImpressionUrl")]
        public string ImpressionUrl { get; set; }

        [JsonProperty("BuyUrl")]
        public string BuyUrl { get; set; }

        public string Department { get; set; }

        public string GoogleProductCategory { get; set; }

        [JsonProperty("AdvertiserCategory")]
        public string AdvertiserCategory { get; set; }

        public string ThirdPartyId { get; set; }

        [JsonProperty("ThirdPartyCategory")]
        public string ThirdPartyCategory { get; set; }

        public string PrimaryCategory { get; set; }
        public string SecondaryCategory { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("SubCategory")]
        public string SubCategory { get; set; }

        [JsonProperty("Color")]
        public string Color { get; set; }

        [JsonProperty("ColorMap")]
        public string ColorMap { get; set; }

        public string Depth { get; set; }

        public string Pattern { get; set; }

        [JsonProperty("Gender")]
        public string Gender { get; set; }

        public string Model { get; set; }
        public string PowerType { get; set; }
        public string Dimensions { get; set; }

        public string FeaturesAndSpecifications { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("SizeType")]
        public string SizeType { get; set; }

        [JsonProperty("SizeSystem")]
        public string SizeSystem { get; set; }

        public string Theme { get; set; }
        public string Style { get; set; }
        public string Recipient { get; set; }
        public string RAM { get; set; }
        public string ProductType { get; set; }
        public string Region { get; set; }
        public string Processor { get; set; }
        public string Platform { get; set; }
        public string PetType { get; set; }
        public string Occasion { get; set; }
        public string MonitorSize { get; set; }
        public string Modem { get; set; }
        public string Material { get; set; }
        public string Make { get; set; }
        public string HardDrive { get; set; }
        public string Drive { get; set; }
        public string Age { get; set; }
        public string Zoom { get; set; }
        public string Year { get; set; }
        public string WirelessInterface { get; set; }
        public string Width { get; set; }
        public string Weight { get; set; }
        public string Tracks { get; set; }
        public string TechSpecsUrl { get; set; }
        public string Staring { get; set; }
        public string ShoeWidth { get; set; }
        public string ShoeSize { get; set; }
        public string ShippingPrice { get; set; }
        public string ShippingMethod { get; set; }
        public string ScreenSize { get; set; }
        public string Resolution { get; set; }
        public string RecommendedUsage { get; set; }
        public string Rating { get; set; }
        public string QuantityInStock { get; set; }
        public string ProgramId { get; set; }
        public string ProgramCategory { get; set; }
        public string PaymentNotes { get; set; }
        public string PaymentAccepted { get; set; }
        public string Pages { get; set; }
        public string OpticalDrive { get; set; }
        public string OperatingSystem { get; set; }
        public string NetworkCategory { get; set; }
        public string ModelNumber { get; set; }
        public string MemoryType { get; set; }
        public string MemoryCardSlot { get; set; }
        public string MemoryCapacity { get; set; }
        public string Megapixels { get; set; }
        public string MadeIn { get; set; }
        public string Location { get; set; }
        public string LoadType { get; set; }
        public string Length { get; set; }
        public string Installation { get; set; }

        public string ImageThumbUrl { get; set; }

        public string Height { get; set; }
        public string HeelHeight { get; set; }
        public string Functions { get; set; }
        public string FocusType { get; set; }
        public string ExpirationDate { get; set; }
        public string Edition { get; set; }
        public string DisplayType { get; set; }        
        public string ColorOutput { get; set; }
        public string Binding { get; set; }
        public string BatteryLife { get; set; }
        public string AspectRatio { get; set; }
        public string AgeRange { get; set; }

        // newborn, infant, toddler, kids, adult
        [JsonProperty("AgeGroup")]
        public string AgeGroup { get; set; }

        public string Audience { get; set; }

        [JsonProperty("Availability")]
        public string Availability { get; set; }

        public string AvailabilityDate { get; set; }
               
        [JsonProperty("Author")]
        public string Author { get; set; }

        [JsonProperty("Artist")]
        public string Artist { get; set; }

        public string Genre { get; set; }

        public string Format { get; set; }
        public string Album { get; set; }
        public string SongTitle { get; set; }
        public string ReleaseDate { get; set; }

        public string Director { get; set; }
        public string Studio { get; set; }
        public string MovieRating { get; set; }
        public string Actor { get; set; }
        public string Producer { get; set; }
        public string RunningTime { get; set; }
        public string Writer { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Publisher")]
        public string Publisher { get; set; }

        [JsonProperty("PublishDate")]
        public string PublishDate { get; set; }

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Gift")]
        public string Gift { get; set; }

        [JsonProperty("Offline")]
        public string Offline { get; set; }

        [JsonProperty("Online")]
        public string Online { get; set; }

        [JsonProperty("InStock")]
        public string InStock { get; set; }

        public string OutOfStock { get; set; }
        public string InStoreOnly { get; set; }
        public string LimitedAvailability { get; set; }
        public string OnlineOnly { get; set; }
        public string PreOrder { get; set; }
        public string Discontinued { get; set; }

        [JsonProperty("Adult")]
        public string Adult { get; set; }

        [JsonProperty("Warranty")]
        public string Warranty { get; set; }

        [JsonProperty("PromotionalText")]
        public string PromotionalText { get; set; }

        public string Special { get; set; }

        [JsonProperty("Miscellaneous")]
        public string Miscellaneous { get; set; }

        public string Language { get; set; }

        public string Season { get; set; } // Winter, Spring, Summer, Fall
        public string SellingRate { get; set; } // BestSeller, LowSeller
        public string Clearance { get; set; } // Product is clearance
        public string Margin { get; set; } // Product sales margin / profit margin
        public string ReleaseYear { get; set; } // e.g. 1900 to 2100

        public string ProductId { get; set; }

        // Schema.org
        public string IsAccessoryFor { get; set; } // MSID of a product
        public string IsSparePartFor { get; set; } // MSID of a product
        public string IsConsumableFor { get; set; } // MSID of a product
        public List<string> IsRelatedTo { get; set; } // MSID of a product
        public List<string> IsSimilarTo { get; set; } // MSID of a product
        
        // Google Shopping
        public string ExcludedDestination { get; set; }
        public string PromotionId { get; set; }

        // Google AdWords
        public string AdWordsGrouping { get; set; }
        public List<string> AdWordsLabels { get; set; }
        public string AdWordsRedirect { get; set; } // AdWords redirect special tracking URL

        // Lists
        public List<Offer> Offers { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<Review> Reviews { get; set; }

        // Analytics
        // TODO: How do we model sentiment score? sentiment analysis? social sentiment?

        public string CardType { get; set; }
        public string RegularAPR { get; set; }
        public string IntroAPR { get; set; }
        public string Incentive { get; set; }
        public string Benefits { get; set; }
        public string BalanceTransfer { get; set; }
        public string AnnualFee { get; set; }
        public string TermsURL { get; set; }
                
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SalePriceEffectiveDate { get; set; }

        // Linkshare Merchandiser
        public string LinkshareProductId { get; set; }
        public string LinkshareMID { get; set; }
        public string LinkshareM1 { get; set; }
        public LinkshareAttributeClass LinkshareClassId { get; set; }
        
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

            if (this.Store == null || String.IsNullOrEmpty(this.Store.Trim()))
                return false;

            if (this.StoreId == null || this.StoreId == Guid.Empty)
                return false;

            return true;
        } 
    }

    public enum LinkshareAttributeClass
    {
        Books = 10,
        Music = 20,
        Movies = 30,
        ComputerHardware = 40,
        ComputerSoftware = 50,
        ClothingAndAccessories = 60,
        Art = 70,
        Toys = 80,
        Pets = 90,
        Games = 100,
        FoodAndDrink = 110,
        GiftsAndFlowers = 120,
        Auto = 130,
        Electronics = 140,
        CreditCards = 150
    }
}