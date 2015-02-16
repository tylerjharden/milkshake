using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MilkshakeTester.Parsers
{
    public class Linkshare : XMLCatalogParser
    {        
        internal Linkshare()
        {
            _name = "Linkshare";
            _folder = "linkshare";
        }

        public override RawProduct Parse(XElement node)
        {
            RawProduct p = new RawProduct();

            p.Source = RawProduct.ProductDataSource.Linkshare;

            try { p.Store = _storeName; }
            catch { }
            try { p.StoreWebsite = ""; }
            catch { }
            try { p.LinkshareMID = _storeId; }
            catch { }
                        
            try { p.LinkshareProductId = node.Attribute("product_id").Value; }
            catch { }
            try { p.Name = node.Attribute("name").Value; }
            catch { }
            try { p.SKU = node.Attribute("sku_number").Value; }
            catch { }
            try { p.Manufacturer = node.Attribute("manufacturer_name").Value; }
            catch { }
            try { p.MPN = node.Attribute("part_number").Value; }
            catch { }

            try { p.PrimaryCategory = node.Element("category").Element("primary").Value; }
            catch { }
            try { p.SecondaryCategory = node.Element("category").Element("secondary").Value; }
            catch { }

            try { p.Url = node.Element("URL").Element("product").Value; }
            catch { }
            try { p.Image = node.Element("URL").Element("productImage").Value; }
            catch { }
            try { p.BuyUrl = node.Element("URL").Element("buyLink").Value; }
            catch { }

            try { p.ShortDescription = node.Element("description").Element("short").Value; }
            catch { }
            try { p.LongDescription = node.Element("description").Element("long").Value; }
            catch { }

            try { p.Currency = node.Element("price").Attribute("currency").Value; }
            catch { }
            try { p.SalePrice = node.Element("price").Element("sale").Value; }
            catch { }
            try { p.SaleStartDate = node.Element("price").Element("sale").Attribute("begin_date").Value; }
            catch { }
            try { p.SaleEndDate = node.Element("price").Element("sale").Attribute("end_date").Value; }
            catch { }
            try { p.RetailPrice = node.Element("price").Element("retail").Value; }
            catch { }
                        
            try { p.Brand = node.Element("brand").Value; }
            catch { }

            try { p.StandardShippingCost = node.Element("shipping").Element("cost").Element("amount").Value; }
            catch { }
            try { p.ShippingInformation = node.Element("shipping").Element("information").Value;  }
            catch { }
            try { p.Availability = node.Element("shipping").Element("availability").Value; }
            catch { }

            try { p.Keywords = node.Element("keywords").Value; }
            catch { }

            try { p.UPC = node.Element("upc").Value; }
            catch { }

            try { p.LinkshareM1 = node.Element("m1").Value;  }
            catch { }

            try { p.ImpressionUrl = node.Element("pixel").Value; }
            catch { }

            try { p.LinkshareClassId = (LinkshareAttributeClass)Enum.Parse(typeof(LinkshareAttributeClass), node.Element("attributes").Attribute("class_id").Value, true); }
            catch { }

            // NO attributes, simply return product
            if (!Enum.IsDefined(typeof(LinkshareAttributeClass), p.LinkshareClassId))
                return p;

            // Linkshare Attribute Miscellaneous is there regardless of class
            try { p.Miscellaneous = node.Element("attributes").Element("Miscellaneous").Value; }
            catch { }

            // Linkshare Attributes
            switch (p.LinkshareClassId)
            {
                case LinkshareAttributeClass.Books:
                    try { p.Title = node.Element("attributes").Element("Title").Value; }
                    catch { }
                    try { p.Author = node.Element("attributes").Element("Author").Value; }
                    catch { }
                    try { p.ISBN = node.Element("attributes").Element("ISBN").Value; }
                    catch { }
                    try { p.Publisher = node.Element("attributes").Element("Publisher").Value; }
                    catch { }
                    try { p.PublishDate = node.Element("attributes").Element("Publish_Date").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.Music:
                    try { p.Genre = node.Element("attributes").Element("Genre").Value; }
                    catch { }
                    try { p.Artist = node.Element("attributes").Element("Artist").Value; }
                    catch { }
                    try { p.Format = node.Element("attributes").Element("Format").Value; }
                    catch { }
                    try { p.Album = node.Element("attributes").Element("Album").Value; }
                    catch { }
                    try { p.SongTitle = node.Element("attributes").Element("SongTitle").Value; }
                    catch { }
                    try { p.Label = node.Element("attributes").Element("Label").Value; }
                    catch { }
                    try { p.ReleaseDate = node.Element("attributes").Element("ReleaseDate").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.Movies:
                    try { p.Genre = node.Element("attributes").Element("Genre").Value; }
                    catch { }
                    try { p.Title = node.Element("attributes").Element("Title").Value; }
                    catch { }
                    try { p.Format = node.Element("attributes").Element("Format").Value; }
                    catch { }
                    try { p.Director = node.Element("attributes").Element("Director").Value; }
                    catch { }
                    try { p.Actor = node.Element("attributes").Element("Actor").Value; }
                    catch { }
                    try { p.MovieRating = node.Element("attributes").Element("Rating").Value; }
                    catch { }
                    try { p.Studio = node.Element("attributes").Element("Studio").Value; }
                    catch { }
                    try { p.ReleaseDate = node.Element("attributes").Element("Release_Date").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.ComputerHardware:
                    try { p.Platform = node.Element("attributes").Element("Platform").Value; }
                    catch { }
                    try { p.RAM = node.Element("attributes").Element("Ram").Value; }
                    catch { }
                    try { p.HardDrive = node.Element("attributes").Element("Hard_Drive").Value; }
                    catch { }
                    try { p.Processor = node.Element("attributes").Element("Processor").Value; }
                    catch { }
                    try { p.MonitorSize = node.Element("attributes").Element("Monitor_Size").Value; }
                    catch { }
                    try { p.Modem = node.Element("attributes").Element("Modem").Value; }
                    catch { }
                    try { p.Drive = node.Element("attributes").Element("Drive").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.ComputerSoftware:
                    try { p.Platform = node.Element("attributes").Element("Platform").Value; }
                    catch { }
                    try { p.Category = node.Element("attributes").Element("Category").Value; }
                    catch { }
                    try { p.Age = node.Element("attributes").Element("Age").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.ClothingAndAccessories:
                    try { p.ProductType = node.Element("attributes").Element("Product_Type").Value; }
                    catch { }
                    try { p.Size = node.Element("attributes").Element("Size").Value; }
                    catch { }
                    try { p.Material = node.Element("attributes").Element("Material").Value; }
                    catch { }
                    try { p.Color = node.Element("attributes").Element("Color").Value; }
                    catch { }
                    try { p.Gender = node.Element("attributes").Element("Gender").Value; }
                    catch { }
                    try { p.Style = node.Element("attributes").Element("Style").Value; }
                    catch { }
                    try { p.Age = node.Element("attributes").Element("Age").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.Art:
                    try { p.ProductType = node.Element("attributes").Element("Product_Type").Value; }
                    catch { }
                    try { p.Artist = node.Element("attributes").Element("Artist").Value; }
                    catch { }
                    try { p.Title = node.Element("attributes").Element("Title").Value; }
                    catch { }
                    try { p.Dimensions = node.Element("attributes").Element("Dimensions").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.Toys:
                    try { p.Age = node.Element("attributes").Element("Age").Value; }
                    catch { }
                    try { p.Gender = node.Element("attributes").Element("Gender").Value; }
                    catch { }
                    try { p.Theme = node.Element("attributes").Element("Theme").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.Pets:
                    try { p.PetType = node.Element("attributes").Element("Pet_Type").Value; }
                    catch { }
                    try { p.ProductType = node.Element("attributes").Element("Product_type").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.Games:
                    try { p.Format = node.Element("attributes").Element("Format").Value; }
                    catch { }
                    try { p.Title = node.Element("attributes").Element("Title").Value; }
                    catch { }
                    try { p.Publisher = node.Element("attributes").Element("Publisher").Value; }
                    catch { }
                    try { p.Age = node.Element("attributes").Element("Age").Value; }
                    catch { }
                    try { p.ReleaseDate = node.Element("attributes").Element("Release_date").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.FoodAndDrink:
                    try { p.ProductType = node.Element("attributes").Element("Product_Type").Value; }
                    catch { }
                    try { p.Region = node.Element("attributes").Element("Region").Value; }
                    catch { }
                    try { p.Size = node.Element("attributes").Element("Size").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.GiftsAndFlowers:
                    try { p.Occasion = node.Element("attributes").Element("Occasion").Value; }
                    catch { }
                    try { p.Recipient = node.Element("attributes").Element("Recipient").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.Auto:
                    try { p.Make = node.Element("attributes").Element("Make").Value; }
                    catch { }
                    try { p.Model = node.Element("attributes").Element("Genre").Value; }
                    catch { }
                    try { p.MPN = node.Element("attributes").Element("Part_Number").Value; }
                    catch { }
                    try { p.SubCategory = node.Element("attributes").Element("Category").Value; }
                    catch { }
                    try { p.Color = node.Element("attributes").Element("Color").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.Electronics:
                    try { p.Category = node.Element("attributes").Element("Category").Value; }
                    catch { }
                    try { p.Model = node.Element("attributes").Element("Model").Value; }
                    catch { }
                    try { p.FeaturesAndSpecifications = node.Element("attributes").Element("Genre").Value; }
                    catch { }
                    try { p.Color = node.Element("attributes").Element("Color").Value; }
                    catch { }
                    try { p.Dimensions = node.Element("attributes").Element("Dimensions").Value; }
                    catch { }
                    try { p.PowerType = node.Element("attributes").Element("Power_Type").Value; }
                    catch { }
                    try { p.Warranty = node.Element("attributes").Element("Warranty").Value; }
                    catch { }
                    break;

                case LinkshareAttributeClass.CreditCards:
                    try { p.CardType = node.Element("attributes").Element("Card_Type").Value; }
                    catch { }
                    try { p.IntroAPR = node.Element("attributes").Element("Intro_APR").Value; }
                    catch { }
                    try { p.RegularAPR = node.Element("attributes").Element("Regular_APR").Value; }
                    catch { }
                    try { p.AnnualFee = node.Element("attributes").Element("Annual_Fee").Value; }
                    catch { }
                    try { p.Incentive = node.Element("attributes").Element("Incentive").Value; }
                    catch { }
                    try { p.BalanceTransfer = node.Element("attributes").Element("Balance_Transfer").Value; }
                    catch { }
                    try { p.Benefits = node.Element("attributes").Element("Benefits").Value; }
                    catch { }
                    try { p.TermsURL = node.Element("attributes").Element("TermsURL").Value; }
                    catch { }
                    break;
            }
                        
            return p;
        }               

        public override void ParseFile(string filename)
        {
            // Create a reader to grab the header
            using (XmlSanitizingStream clean = new XmlSanitizingStream(File.OpenRead(filename)))
            {
                using (XmlReader reader = XmlReader.Create(clean, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, }))
                {
                    _storeName = reader.Header().Single().Element("merchantName").Value;
                    _storeId = reader.Header().Single().Element("merchantId").Value;
                }
            }

            base.ParseFile(filename);
        }                
    }
}