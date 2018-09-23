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
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Store = reader["program_name"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Currency = reader["currency"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.AgeRange = reader["age_range"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Artist = reader["artist"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.AspectRatio = reader["aspect_ratio"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Author = reader["author"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.BatteryLife = reader["battery_life"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Binding = reader["binding"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Url = reader["buy_url"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.BuyUrl = reader["buy_url"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Color = reader["color"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ColorOutput = reader["color_output"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Condition = reader["condition"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.LongDescription = reader["description_long"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Director = reader["director"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.DisplayType = reader["display_type"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Edition = reader["edition"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ExpirationDate = reader["expiration_date"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.FeaturesAndSpecifications = reader["features"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.FocusType = reader["focus_type"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Format = reader["format"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Functions = reader["functions"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Genre = reader["genre"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.HeelHeight = reader["heel_height"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Height = reader["height"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ImageThumbUrl = reader["image_thumb_url"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Image = reader["image_url"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Installation = reader["installation"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ISBN = reader["isbn"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Length = reader["length"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.LoadType = reader["load_type"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Location = reader["location"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.MadeIn = reader["made_in"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Manufacturer = reader["manufacturer"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Material = reader["material"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Megapixels = reader["megapixels"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.MemoryType = reader["memory_type"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.MemoryCapacity = reader["memory_capacity"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.MemoryCardSlot = reader["memory_card_slot"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ModelNumber = reader["model_number"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.MPN = reader["mpn"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Name = reader["name"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Occasion = reader["occasion"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.OperatingSystem = reader["operating_system"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.OpticalDrive = reader["optical_drive"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.RetailPrice = reader["price_retail"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Pages = reader["pages"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.PaymentAccepted = reader["payment_accepted"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.PaymentNotes = reader["payment_notes"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Platform = reader["platform"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.SalePrice = reader["price_sale"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Processor = reader["processor"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Publisher = reader["publisher"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.QuantityInStock = reader["quantity_in_stock"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Rating = reader["rating"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.RecommendedUsage = reader["recommended_usage"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Resolution = reader["resolution"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ShoeSize = reader["shoe_size"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ScreenSize = reader["screen_size"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ShippingMethod = reader["shipping_method"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ShippingPrice = reader["price_shipping"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ShoeWidth = reader["shoe_width"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Size = reader["size"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.SKU = reader["sku"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Staring = reader["staring"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Style = reader["style"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Tracks = reader["tracks"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.UPC = reader["upc"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Weight = reader["weight"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Width = reader["width"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.WirelessInterface = reader["wireless_interface"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Year = reader["year"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Zoom = reader["zoom"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.NetworkCategory = reader["category_network"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ProgramCategory = reader["category_program"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.ShortDescription = reader["description_short"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Discontinued = reader["discontinued"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.InStock = reader["in_stock"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.TechSpecsUrl = reader["tech_specs_url"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Keywords = reader["keywords"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  
            try { p.Price = reader["price"]; }
            catch (Exception e)  
            {  
               Console.WriteLine(Exception e);  
            }  

            return p;
        }
    }
}
