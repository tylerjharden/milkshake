using Schloss.IO.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkshakeTester.Parsers
{
    public class Pepperjam : CSVCatalogParser
    {
        internal Pepperjam()
        {
            _name = "Pepperjam";
            _folder = "pj";
        }

        public override RawProduct Parse(CsvReader reader)
        {
            RawProduct p = new RawProduct();

            p.Source = RawProduct.ProductDataSource.Pepperjam;

            try { p.ProgramId = reader["program_id"]; }
            catch { }
            try { p.Store = reader["program_name"]; }
            catch { }
            try { p.Currency = reader["currency"]; }
            catch { }
            try { p.AgeRange = reader["age_range"]; }
            catch { }
            try { p.Artist = reader["artist"]; }
            catch { }
            try { p.AspectRatio = reader["aspect_ratio"]; }
            catch { }
            try { p.Author = reader["author"]; }
            catch { }
            try { p.BatteryLife = reader["battery_life"]; }
            catch { }
            try { p.Binding = reader["binding"]; }
            catch { }
            try { p.Url = reader["buy_url"]; }
            catch { }
            try { p.BuyUrl = reader["buy_url"]; }
            catch { }
            try { p.Color = reader["color"]; }
            catch { }
            try { p.ColorOutput = reader["color_output"]; }
            catch { }
            try { p.Condition = reader["condition"]; }
            catch { }
            try { p.LongDescription = reader["description_long"]; }
            catch { }
            try { p.Director = reader["director"]; }
            catch { }
            try { p.DisplayType = reader["display_type"]; }
            catch { }
            try { p.Edition = reader["edition"]; }
            catch { }
            try { p.ExpirationDate = reader["expiration_date"]; }
            catch { }
            try { p.FeaturesAndSpecifications = reader["features"]; }
            catch { }
            try { p.FocusType = reader["focus_type"]; }
            catch { }
            try { p.Format = reader["format"]; }
            catch { }
            try { p.Functions = reader["functions"]; }
            catch { }
            try { p.Genre = reader["genre"]; }
            catch { }
            try { p.HeelHeight = reader["heel_height"]; }
            catch { }
            try { p.Height = reader["height"]; }
            catch { }
            try { p.ImageThumbUrl = reader["image_thumb_url"]; }
            catch { }
            try { p.Image = reader["image_url"]; }
            catch { }
            try { p.Installation = reader["installation"]; }
            catch { }
            try { p.ISBN = reader["isbn"]; }
            catch { }
            try { p.Length = reader["length"]; }
            catch { }
            try { p.LoadType = reader["load_type"]; }
            catch { }
            try { p.Location = reader["location"]; }
            catch { }
            try { p.MadeIn = reader["made_in"]; }
            catch { }
            try { p.Manufacturer = reader["manufacturer"]; }
            catch { }
            try { p.Material = reader["material"]; }
            catch { }
            try { p.Megapixels = reader["megapixels"]; }
            catch { }
            try { p.MemoryType = reader["memory_type"]; }
            catch { }
            try { p.MemoryCapacity = reader["memory_capacity"]; }
            catch { }
            try { p.MemoryCardSlot = reader["memory_card_slot"]; }
            catch { }
            try { p.ModelNumber = reader["model_number"]; }
            catch { }
            try { p.MPN = reader["mpn"]; }
            catch { }
            try { p.Name = reader["name"]; }
            catch { }
            try { p.Occasion = reader["occasion"]; }
            catch { }
            try { p.OperatingSystem = reader["operating_system"]; }
            catch { }
            try { p.OpticalDrive = reader["optical_drive"]; }
            catch { }
            try { p.RetailPrice = reader["price_retail"]; }
            catch { }
            try { p.Pages = reader["pages"]; }
            catch { }
            try { p.PaymentAccepted = reader["payment_accepted"]; }
            catch { }
            try { p.PaymentNotes = reader["payment_notes"]; }
            catch { }
            try { p.Platform = reader["platform"]; }
            catch { }
            try { p.SalePrice = reader["price_sale"]; }
            catch { }
            try { p.Processor = reader["processor"]; }
            catch { }
            try { p.Publisher = reader["publisher"]; }
            catch { }
            try { p.QuantityInStock = reader["quantity_in_stock"]; }
            catch { }
            try { p.Rating = reader["rating"]; }
            catch { }
            try { p.RecommendedUsage = reader["recommended_usage"]; }
            catch { }
            try { p.Resolution = reader["resolution"]; }
            catch { }
            try { p.ShoeSize = reader["shoe_size"]; }
            catch { }
            try { p.ScreenSize = reader["screen_size"]; }
            catch { }
            try { p.ShippingMethod = reader["shipping_method"]; }
            catch { }
            try { p.ShippingPrice = reader["price_shipping"]; }
            catch { }
            try { p.ShoeWidth = reader["shoe_width"]; }
            catch { }
            try { p.Size = reader["size"]; }
            catch { }
            try { p.SKU = reader["sku"]; }
            catch { }
            try { p.Staring = reader["staring"]; }
            catch { }
            try { p.Style = reader["style"]; }
            catch { }
            try { p.Tracks = reader["tracks"]; }
            catch { }
            try { p.UPC = reader["upc"]; }
            catch { }
            try { p.Weight = reader["weight"]; }
            catch { }
            try { p.Width = reader["width"]; }
            catch { }
            try { p.WirelessInterface = reader["wireless_interface"]; }
            catch { }
            try { p.Year = reader["year"]; }
            catch { }
            try { p.Zoom = reader["zoom"]; }
            catch { }
            try { p.NetworkCategory = reader["category_network"]; }
            catch { }
            try { p.ProgramCategory = reader["category_program"]; }
            catch { }
            try { p.ShortDescription = reader["description_short"]; }
            catch { }
            try { p.Discontinued = reader["discontinued"]; }
            catch { }
            try { p.InStock = reader["in_stock"]; }
            catch { }
            try { p.TechSpecsUrl = reader["tech_specs_url"]; }
            catch { }
            try { p.Keywords = reader["keywords"]; }
            catch { }
            try { p.Price = reader["price"]; }
            catch { }

            return p;
        }
    }
}
