using Newtonsoft.Json;
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
    public class CJ : XMLCatalogParser
    {
        internal CJ()
        {
            _name = "CommissionJunction";
            _folder = "cj";
        }
                
        public override RawProduct Parse(XElement node)
        {            
            RawProduct p = new RawProduct();

            p.Source = RawProduct.ProductDataSource.CommissionJunction;

            try { p.Store = node.Element("programname").Value; }
            catch { }
            try { p.StoreWebsite = node.Element("programurl").Value; }
            catch { }

            try { p.Name = node.Element("name").Value; }
            catch { }
            try { p.Keywords = node.Element("keywords").Value; }
            catch { }
            try { p.Description = node.Element("description").Value; }
            catch { }

            try { p.SKU = node.Element("sku").Value; }
            catch { }
            try { p.Manufacturer = node.Element("manufacturer").Value; }
            catch { }
            try { p.ManufacturerID = node.Element("manufacturerid").Value; }
            catch { }

            // Unique Identifiers
            try { p.UPC = node.Element("upc").Value; }
            catch { }
            try { p.ISBN = node.Element("isbn").Value; }
            catch { }

            try { p.Currency = node.Element("currency").Value; }
            catch { }
            try { p.Price = node.Element("price").Value; }
            catch { }
            try { p.SalePrice = node.Element("saleprice").Value; }
            catch { }
            try { p.RetailPrice = node.Element("retailprice").Value; }
            catch { }
            try { p.FromPrice = node.Element("fromprice").Value; }
            catch { }
            
            try { p.Url = node.Element("buyurl").Value; }
            catch { }
            try { p.BuyUrl = node.Element("buyurl").Value; }
            catch { }
            try { p.ImpressionUrl = node.Element("impressionurl").Value; }
            catch { }
            try { p.Image = node.Element("imageurl").Value; }
            catch { }

            try { p.AdvertiserCategory = node.Element("advertisercategory").Value; }
            catch { }
            try { p.ThirdPartyId = node.Element("thirdpartyid").Value; }
            catch { }
            try { p.ThirdPartyCategory = node.Element("thirdpartycategory").Value; }
            catch { }

            try { p.Author = node.Element("author").Value; }
            catch { }
            try { p.Artist = node.Element("artist").Value; }
            catch { }
            try { p.Title = node.Element("title").Value; }
            catch { }
            try { p.Publisher = node.Element("publisher").Value; }
            catch { }
            try { p.Label = node.Element("label").Value; }
            catch { }
            try { p.Format = node.Element("format").Value; }
            catch { }

            try { p.Special = node.Element("special").Value; }
            catch { }
            try { p.Gift = node.Element("gift").Value; }
            catch { }
            try { p.PromotionalText = node.Element("promotionaltext").Value; }
            catch { }

            try { p.StartDate = node.Element("startdate").Value; }
            catch { }
            try { p.EndDate = node.Element("enddate").Value; }
            catch { }

            try { p.Offline = node.Element("offline").Value; }
            catch { }
            try { p.Online = node.Element("online").Value; }
            catch { }
            
            try { p.InStock = node.Element("instock").Value; }
            catch { }

            try { p.Condition = node.Element("condition").Value; }
            catch { }

            try { p.Condition = node.Element("condition").Value; }
            catch { }

            try { p.Warranty = node.Element("warranty").Value; }
            catch { }

            try { p.StandardShippingCost = node.Element("standardshippingcost").Value; }
            catch { }
            
            return p;
        }
    }
}
