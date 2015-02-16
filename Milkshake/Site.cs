using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Milkshake
{
    public class Site
    {
        public static List<Site> Sites = new List<Site>();

        static Site()
        {
            try
            {
                Initialize();
            }
            catch
            { }
        }

        public Site()
        {
            Sites.Add(this);
        }

        public static bool SiteExists(Uri uri)
        {
            return Sites.Where(site => (site.Uri == uri.AbsoluteUri || site.Uri == uri.Authority || site.Uri.Contains(uri.AbsoluteUri) || uri.AbsoluteUri.Contains(site.Uri))).Count() > 0;
        }

        public static bool SiteExists(string name)
        {
            return Sites.Where(site => (site.Name == name)).Count() > 0;
        }

        public static Site GetSite(Uri uri)
        {
            return Sites.Where(site => (site.Uri == uri.AbsoluteUri || site.Uri == uri.Authority || site.Uri.Contains(uri.AbsoluteUri) || uri.AbsoluteUri.Contains(site.Uri))).First();
        }

        public static Site GetSite(string name)
        {
            return Sites.Where(site => (site.Name == name)).First();
        }

        public string Name;
        public string Uri;
        public List<String> Alternatives;

        public string NameXPath;
        public string PriceXPath;
        public string ImageXPath;
        public string DescriptionXPath;
        public string CanonicalXPath;

        public string AdditionalOGNamespace = "og";
        public string OGType = "og:type";
        public string OGTitle = "og:title";
        public string OGUrl = "og:url";
        public string OGImage = "og:image";
        public string OGDescription = "og:description";
        public string OGPrice = "og:price:amount";
        public string OGSiteName = "og:site_name";
        public string OGUPC = "og:upc";

        public bool OpenGraph = false;
        public bool TwitterCard = false;

        public bool OEmbed = false;

        public bool JSON = false;
        public string JSONEndpoint = "";
        public bool JSONEndpointUsesUrl = false;

        public string ProductPageFilterStart;

        public bool ParseProductAttributesMeta = false;

        public bool RootOnly = false;

        public bool HasAPI = false;
        public bool Crawlable = true;

        public bool IsMobile()
        {
            if (Uri.StartsWith("http://m.") || Uri.StartsWith("http://mobi.") || Uri.Contains(".mobi"))
                return true;

            return false;
        }

        public bool IsDesktop()
        {
            return !IsMobile();
        }

        public bool IsProductPage(string uri)
        {
            if (ProductPageFilterStart == null)
                return true;
            else
                return uri.ToLower().StartsWith(ProductPageFilterStart.ToLower());
        }

        public RawProduct Parse(HtmlDocument page, Uri uri)
        {
            ///////////////
            // Variables //
            ///////////////
            string title = "";
            string description = "";
            string url = "";
            string image = "";
            string site_name = "";
            string price = "";
            string upc = "";

            /////////////////////
            // Cleanup the DOM //
            /////////////////////

            // Remove comments
            page.DocumentNode.Descendants()
             .Where(n => n.NodeType == HtmlAgilityPack.HtmlNodeType.Comment)
             .ToList()
             .ForEach(n => n.Remove());

            // Remove scripts, styles, noscripts, iframes, objects, embeds, form inputs, headers, and footers
            page.DocumentNode.Descendants()
             .Where(n => n.Name == "script" || n.Name == "style" || n.Name == "noscript" || n.Name == "iframe" || n.Name == "object" || n.Name == "embed")
             .ToList()
             .ForEach(n => n.Remove());

            ///////////////////
            // Parse the DOM //
            ///////////////////

            // Process the page using OpenGraph
            if (OpenGraph)
            {
                HtmlNodeCollection metanodes = page.DocumentNode.SelectNodes("//meta");

                if (metanodes != null)
                {
                    foreach (HtmlNode meta in metanodes)
                    {
                        if (meta.HasAttributes && meta.Attributes["property"] != null && (meta.Attributes["property"].Value.StartsWith("og") || meta.Attributes["property"].Value.StartsWith(AdditionalOGNamespace)))
                        {
                            if (meta.Attributes["property"].Value == OGTitle && title == "")
                                title = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == OGDescription && description == "")
                                description = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == OGUrl && url == "")
                                url = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == OGImage && image == "")
                                image = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == OGSiteName && site_name == "")
                                site_name = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == OGPrice && price == "")
                                price = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == OGUPC && upc == "")
                                upc = meta.Attributes["content"].Value;

                            //Logger.Log("Found an OG meta tag: " + meta.Attributes["property"].Value + ":" + meta.Attributes["content"].Value);
                        }
                    }
                }
            }

            // ParseProductAttributesMeta
            if (ParseProductAttributesMeta)
            {
                HtmlNode attr = page.DocumentNode.SelectSingleNode("//meta[@name='productattributes']");

                if (attr != null)
                {
                    if (attr.HasAttributes && attr.Attributes["value"] != null)
                    {
                        price = attr.Attributes["value"].Value.ToString().Split('|').Last().Split(':').Last().ToString();
                    }
                }
            }

            // Process the page using Twitter Card
            if (TwitterCard)
            {
                HtmlNodeCollection metanodes = page.DocumentNode.SelectNodes("//meta");

                if (metanodes != null)
                {
                    foreach (HtmlNode meta in metanodes)
                    {
                        // Check for 4 "standard" variants of Twitter Card implementations, since no one does it the same way...anywhere.

                        // name,content
                        if (meta.HasAttributes && meta.Attributes["name"] != null && meta.Attributes["name"].Value.StartsWith("twitter"))
                        {
                            if (meta.Attributes["name"].Value == "twitter:description" && meta.Attributes["content"] != null && description == "")
                                description = meta.Attributes["content"].Value;
                            else if (meta.Attributes["name"].Value == "twitter:title" && meta.Attributes["content"] != null && title == "")
                                title = meta.Attributes["content"].Value;
                            else if (meta.Attributes["name"].Value == "twitter:url" && meta.Attributes["content"] != null && url == "")
                                url = meta.Attributes["content"].Value;
                            else if (meta.Attributes["name"].Value == "twitter:image" && meta.Attributes["content"] != null && image == "")
                                image = meta.Attributes["content"].Value;
                        }
                        // property, content
                        if (meta.HasAttributes && meta.Attributes["property"] != null && meta.Attributes["property"].Value.StartsWith("twitter"))
                        {
                            if (meta.Attributes["property"].Value == "twitter:description" && meta.Attributes["content"] != null && description == "")
                                description = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == "twitter:title" && meta.Attributes["content"] != null && title == "")
                                title = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == "twitter:url" && meta.Attributes["content"] != null && url == "")
                                url = meta.Attributes["content"].Value;
                            else if (meta.Attributes["property"].Value == "twitter:image" && meta.Attributes["content"] != null && image == "")
                                image = meta.Attributes["content"].Value;
                        }
                        //name,value
                        if (meta.HasAttributes && meta.Attributes["name"] != null && meta.Attributes["name"].Value.StartsWith("twitter"))
                        {
                            if (meta.Attributes["name"].Value == "twitter:description" && meta.Attributes["value"] != null && description == "")
                                description = meta.Attributes["value"].Value;
                            else if (meta.Attributes["name"].Value == "twitter:title" && meta.Attributes["value"] != null && title == "")
                                title = meta.Attributes["value"].Value;
                            else if (meta.Attributes["name"].Value == "twitter:url" && meta.Attributes["value"] != null && url == "")
                                url = meta.Attributes["value"].Value;
                            else if (meta.Attributes["name"].Value == "twitter:image" && meta.Attributes["value"] != null && image == "")
                                image = meta.Attributes["value"].Value;
                        }
                        //property,value
                        if (meta.HasAttributes && meta.Attributes["property"] != null && meta.Attributes["property"].Value.StartsWith("twitter"))
                        {
                            if (meta.Attributes["property"].Value == "twitter:description" && meta.Attributes["value"] != null && description == "")
                                description = meta.Attributes["value"].Value;
                            else if (meta.Attributes["property"].Value == "twitter:title" && meta.Attributes["value"] != null && title == "")
                                title = meta.Attributes["value"].Value;
                            else if (meta.Attributes["property"].Value == "twitter:url" && meta.Attributes["value"] != null && url == "")
                                url = meta.Attributes["value"].Value;
                            else if (meta.Attributes["property"].Value == "twitter:image" && meta.Attributes["value"] != null && image == "")
                                image = meta.Attributes["value"].Value;
                        }
                    }
                }
            }

            // OEmbed Endpoint
            if (OEmbed)
            {
                HtmlNode linknode = page.DocumentNode.SelectSingleNode("//link[@rel='alternate' and @type='application/json+oembed']");

                if (linknode == null)
                    return null;

                string jsonuri = linknode.Attributes["href"].Value;

                HttpWebRequest request = WebRequest.Create(jsonuri) as HttpWebRequest;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        JsonTextReader json = new JsonTextReader(new StreamReader(response.GetResponseStream()));
                        while (json.Read())
                        {
                            if (json.TokenType == JsonToken.PropertyName)
                            {
                                if (json.Value.ToString() == "title")
                                {
                                    json.Read();
                                    title = json.Value.ToString();
                                }
                                if (json.Value.ToString() == "description")
                                {
                                    json.Read();
                                    description = json.Value.ToString();
                                }
                                if (json.Value.ToString() == "price")
                                {
                                    json.Read();
                                    price = json.Value.ToString();
                                }
                                if (json.Value.ToString() == "url")
                                {
                                    json.Read();
                                    url = json.Value.ToString();
                                }
                                if (json.Value.ToString() == "thumbnail_url")
                                {
                                    json.Read();
                                    image = json.Value.ToString();
                                }
                            }
                        }
                    }
                }

                // Burke Decor Hack
                if (title == "Default")
                    return null;
            }

            // JSON Endpoint
            if (JSON)
            {
                string endpointkey = "";

                if (JSONEndpointUsesUrl)
                {
                    endpointkey = uri.AbsoluteUri.Remove(uri.AbsoluteUri.IndexOf(ProductPageFilterStart), ProductPageFilterStart.Length);
                }

                HttpWebRequest request = WebRequest.Create(JSONEndpoint + endpointkey) as HttpWebRequest;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        JsonTextReader json = new JsonTextReader(new StreamReader(response.GetResponseStream()));
                        while (json.Read())
                        {
                            if (json.TokenType == JsonToken.PropertyName)
                            {
                                if (json.Value.ToString() == "price")
                                {
                                    json.Read();
                                    price = json.Value.ToString();
                                }
                            }
                        }
                    }
                }
            }

            // Process Using XPath

            // IMAGE
            if (ImageXPath != null)
            {
                HtmlNode img = page.DocumentNode.SelectSingleNode(ImageXPath);
                if (img != null)
                {
                    if (img.HasAttributes && img.Attributes["src"] != null) // img src
                        image = img.Attributes["src"].Value;
                    else if (img.HasAttributes && img.Attributes["href"] != null) // a href
                        image = img.Attributes["href"].Value;
                    else
                        image = img.InnerText;

                    // Artspace Meta
                    if (img.HasAttributes && img.Attributes["content"] != null)
                        image = img.Attributes["content"].Value;

                    // HACK: Amazon Image Hack (image source stored in custom attribute)
                    if (img.HasAttributes && img.Attributes["data-old-hires"] != null)
                        image = img.Attributes["data-old-hires"].Value;

                    // HACK: Domino Hack (image stored as background-image)
                    if (img.HasAttributes && img.Attributes["style"] != null && img.Attributes["style"].Value.Contains("background-image"))
                    {
                        string css = img.Attributes["style"].Value;
                        int bgindex = css.IndexOf("background-image");
                        css = css.Substring(0, bgindex);

                        int scindex = css.IndexOf(";");
                        css = css.Substring(0, scindex);
                    }
                }
            }

            if (image.StartsWith("//"))
            {
                image = uri.Scheme + ":" + image;
            }
            else if (image.StartsWith("/"))
            {
                image = uri.Scheme + "://" + uri.Host + image;
            }
            
            // PRODUCT NAME
            HtmlNode tit;
            if (NameXPath != null)
            {
                tit = page.DocumentNode.SelectSingleNode(NameXPath);
                if (tit != null)
                {
                    if (RootOnly && tit.HasChildNodes)
                        tit.RemoveAllChildren();

                    title = tit.InnerText;
                }
            }

            // Fallback if no other title parsing has succeeded
            if (title == "")
            {
                tit = page.DocumentNode.SelectSingleNode("//title");

                if (tit != null)
                    title = tit.InnerText;
            }

            // PRICE
            if (PriceXPath != null)
            {
                HtmlNode pri = page.DocumentNode.SelectSingleNode(PriceXPath);
                if (pri != null)
                {
                    if (RootOnly && pri.HasChildNodes)
                        pri.RemoveAllChildren();

                    price = pri.InnerText;

                    if (pri.HasAttributes && pri.Attributes["content"] != null)
                        price = pri.Attributes["content"].Value;
                }
            }

            // URL
            if (CanonicalXPath != null)
            {
                HtmlNode can = page.DocumentNode.SelectSingleNode(CanonicalXPath);
                if (can != null)
                {
                    url = can.InnerText;

                    if (can.HasAttributes && can.Attributes["href"] != null)
                        url = can.Attributes["href"].Value;

                    // HACK: Samsung Hack for purely og:url meta tag
                    if (can.Name.Contains("meta") && can.HasAttributes && can.Attributes["property"] != null && can.Attributes["property"].Value.StartsWith("og"))
                        url = can.Attributes["value"].Value;
                }
            }

            // Fallback if no other url parsing logic has succeeded
            if (url == "")
            {
                url = uri.AbsoluteUri;
            }

            // DESCRIPTION
            if (DescriptionXPath != null)
            {
                HtmlNode des = page.DocumentNode.SelectSingleNode(DescriptionXPath);
                if (des != null)
                {
                    if (RootOnly && des.HasChildNodes)
                        des.RemoveAllChildren();

                    if (des.Name.Contains("meta"))
                    {
                        description = des.Attributes["content"].Value;
                    }
                    else
                        description = des.InnerText;
                }
            }

            // Clean-up
            title = title.Trim().Replace("\n", " ").Replace("\t", " ").Replace("&nbsp;", " ");
            price = price.Trim().Replace("\n", "").Replace("\t", " ").Replace("&nbsp;", " ");
            description = description.Trim().Replace("&nbsp;", ""); //.Replace("\n", "").Replace("\t", "");

            if (price.Length > 0)
                title = title.Replace(price, "").Trim(); // Remove duplicate price from title/name (Originally hack for H&M)

            price = Regex.Replace(price, "[^0-9$.]", "");

            RawProduct pn = new RawProduct();
            pn.Name = title;
            pn.Description = description;
            pn.Image = image;
            pn.Url = url;
            pn.UPC = upc;
            pn.Store = Name; //site_name;
            pn.Price = price;

            pn.Id = Guid.NewGuid();
            pn.CreatedOn = DateTimeOffset.Now;
            pn.LastModified = DateTimeOffset.Now;

            return pn;
        }

        public static void Initialize()
        {
            new Site
            {
                Name = "Amazon",
                Uri = "http://www.amazon.com",
                NameXPath = "//*[@id='productTitle']",
                PriceXPath = "//*[@id='priceblock_ourprice']",
                ImageXPath = "//*[@id='landingImage']",
                CanonicalXPath = "/html/head/link[6]",
                DescriptionXPath = "//*[@id='productDescription']/p",
                ProductPageFilterStart = "http://www.amazon.com/gp/product/",
                HasAPI = true
            };

            new Site
            {
                Name = "Target",
                Uri = "http://www.target.com",
                OpenGraph = true,
                PriceXPath = "//*[@id='price_main']/div/p/span[2]",
                ProductPageFilterStart = "http://www.target.com/p/"
            };

            new Site
            {
                Name = "Etsy",
                Uri = "https://www.etsy.com",
                OpenGraph = true,
                AdditionalOGNamespace = "etsymarketplace",
                OGPrice = "etsymarketplace:price",
                ProductPageFilterStart = "https://www.etsy.com/listing/",
                HasAPI = true
            };

            new Site
            {
                Name = "Zappos",
                Uri = "http://www.zappos.com",
                OpenGraph = true,
                PriceXPath = "//*[@id='priceSlot']/span",
                DescriptionXPath = "//*[@id='productDescription']/div/div",
                ProductPageFilterStart = "http://www.zappos.com/"
            };

            new Site
            {
                Name = "Shoptiques",
                Uri = "http://www.shoptiques.com",
                OpenGraph = true,
                JSON = true,
                JSONEndpoint = "http://www.shoptiques.com/api/product/inventory/",
                JSONEndpointUsesUrl = true,
                ProductPageFilterStart = "http://www.shoptiques.com/products/"
            };

            new Site
            {
                Name = "Shopbop",
                Uri = "http://www.shopbop.com",
                NameXPath = "//div[@id='product-information']/h1/span",
                PriceXPath = "//div[@id='productPrices']/meta[1]",
                DescriptionXPath = "//div[@itemprop='description']",
                ImageXPath = "//img[@id='productImage']",
                CanonicalXPath = "/html/head/link[2]",
                ProductPageFilterStart = "http://www.shopbop.com/"
            };

            // Domino is not done!
            // Lack of www. is fucking up URI parsing
            new Site
            {
                Name = "domino",
                Uri = "http://domino.com",
                OpenGraph = true,
                NameXPath = "//h1",
                PriceXPath = "//div[@class='price']",
                DescriptionXPath = "/html/body/div[2]/div[2]/div/section[1]/div[3]/div[3]/div[1]/div[2]/div/div[2]",
                ImageXPath = "/html/body/div[6]/div[2]/div",
                //CanonicalXPath = "/html/head/link[1]",
                ProductPageFilterStart = "http://domino.com/",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "drugstore.com",
                Uri = "http://www.drugstore.com",
                OpenGraph = true,
                PriceXPath = "//div[@id='productprice']/span",
                ProductPageFilterStart = "http://www.drugstore.com/"
            };

            new Site
            {
                Name = "Net-A-Porter",
                Uri = "http://www.net-a-porter.com",
                OpenGraph = true,
                PriceXPath = "//div[@id='product-details']/div/span",
                ProductPageFilterStart = "http://www.net-a-porter.com/product/"
            };

            // Wayfair: Add support for og:brand, their og:upc IS NOT a UPC.
            new Site
            {
                Name = "Wayfair",
                Uri = "http://www.wayfair.com",
                OpenGraph = true,
                ProductPageFilterStart = "http://www.wayfair.com/"
            };

            // Update Anthropologie to use the API/JSON
            new Site
            {
                Name = "Anthropologie",
                Uri = "http://m.anthropologie.com",
                NameXPath = "//*[@id='productdetail-info']/h1/span",
                PriceXPath = "//*[@id='productdetail-price']",
                ImageXPath = "//*[@id='productdetail-images']/ol/li[1]/a/img",
                CanonicalXPath = "//link[@rel='canonical']",
                DescriptionXPath = "//meta[@name='description']",
                ProductPageFilterStart = "http://m.anthropologie.com/mobile/catalog/productdetail"
            };

            new Site
            {
                Name = "Sephora",
                Uri = "http://www.sephora.com",
                OpenGraph = true,
                PriceXPath = "//div[@id='primarySkuInfo_price']/div[1]/span/span[1]",
                ProductPageFilterStart = "http://www.sephora.com/"
            };

            // Has redirect loop to kill bots!!!
            new Site
            {
                Name = "Topshop",
                Uri = "http://us.topshop.com",
                OpenGraph = true,
                PriceXPath = "//div[@id='product_tab_1']/ul/li[1]/span",
                ProductPageFilterStart = "http://us.topshop.com/en/tsus/product/",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "bloomingdale's",
                Uri = "http://www1.bloomingdales.com",
                OpenGraph = true,
                PriceXPath = "//div[@id='PriceDisplay']/div/div/div/span[2]",
                DescriptionXPath = "//div[@id='pdp_tabs_body_left']",
                ProductPageFilterStart = "http://www1.bloomingdales.com/shop/product/"
            };

            new Site
            {
                Name = "Nordstrom",
                Uri = "http://m.nordstrom.com",
                OpenGraph = false,
                TwitterCard = false,
                NameXPath = "//h2[@class='productName']",
                PriceXPath = "//h2[@class='currentPrice']/span",
                ImageXPath = "//*[@id='mainImage']",
                CanonicalXPath = "/html/head/link[1]",
                DescriptionXPath = "//p[@class='Detail']",
                ProductPageFilterStart = "http://m.nordstrom.com/Product/Details"
            };

            new Site
            {
                Name = "HATCH Collection",
                Uri = "http://hatchcollection.com",
                OpenGraph = true,
                DescriptionXPath = "//div[@id='product-accordion']/div[1]/div",
                PriceXPath = "//div[@id='price']",
                ProductPageFilterStart = "http://hatchcollection.com/shop/product/"
            };

            // babyccinokids would go here, but it is like 100 different domains

            new Site
            {
                Name = "T.J.Maxx",
                Uri = "http://tjmaxx.tjx.com",
                OpenGraph = true,
                DescriptionXPath = "//ul[@class='description-list nice-list']",
                PriceXPath = "//span[@class='product-price']",
                ImageXPath = "//img[@class='main-image']",
                ProductPageFilterStart = "http://tjmaxx.tjx.com/store/jump/product/"
            };

            // Requires a custom parser to handle JSON call too product_details api
            new Site
            {
                Name = "J.Crew",
                Uri = "https://www.jcrew.com",
                OpenGraph = true,
                PriceXPath = "//section[@id='description0']/div[2]/span",
                ProductPageFilterStart = "https://www.jcrew.com/",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Mr Porter",
                Uri = "http://www.mrporter.com",
                OpenGraph = true,
                PriceXPath = "//div[@id='product-details']/span/span",
                ProductPageFilterStart = "http://www.mrporter.com/"
            };

            new Site
            {
                Name = "Warby Parker",
                Uri = "https://www.warbyparker.com",
                OpenGraph = true,
                DescriptionXPath = "//p[@class='js-product-description']",
                ProductPageFilterStart = "https://www.warbyparker.com/"
            };

            new Site
            {
                Name = "Starbucks",
                Uri = "http://store.starbucks.com",
                OpenGraph = true,
                ProductPageFilterStart = "http://store.starbucks.com/"
            };

            new Site
            {
                Name = "Dean & Deluca",
                Uri = "http://www.deandeluca.com",
                NameXPath = "//div[@id='product-display']/div/div[1]/h1",
                ImageXPath = "//img[@id='hero-image']",
                DescriptionXPath = "//div[@id='product-description-text']",
                PriceXPath = "//div[@id='ContentPlaceHolder1_ProductDetails']/div[1]/h1",
                CanonicalXPath = "//head/link[5]",
                ProductPageFilterStart = "http://www.deandeluca.com/"
            };

            new Site
            {
                Name = "Williams-Sonoma",
                Uri = "http://www.williams-sonoma.com",
                OpenGraph = true,
                DescriptionXPath = "//dd[@id='tab0']/div/p",
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.williams-sonoma.com/products/"
            };

            new Site
            {
                Name = "General Store",
                Uri = "http://shop-generalstore.com",
                OpenGraph = true,
                NameXPath = "//div[@id='product-description']/h2",
                DescriptionXPath = "//div[@id='product-description']",
                PriceXPath = "//div[@class='current-price']",
                CanonicalXPath = "//link[@rel='canonical']",
                ProductPageFilterStart = "http://shop-generalstore.com/"
            };

            new Site
            {
                Name = "Crate&Barrel",
                Uri = "http://www.crateandbarrel.com",
                OpenGraph = true,
                ProductPageFilterStart = "http://www.crateandbarrel.com/"
            };

            // Finish this later!
            new Site
            {
                Name = "Benjamin Moore",
                Uri = "http://store.benjaminmoore.com",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Papyrus",
                Uri = "http://www.papyrusonline.com",
                NameXPath = "/html/head/title",
                DescriptionXPath = "//div[@class='sales-copy']/div[@class='std']",
                PriceXPath = "//div[@class='price-box']/span[@class='regular-price']/span[@class='price']",
                ImageXPath = "//img[@id='image']",
                CanonicalXPath = "//link[@rel='canonical']",
                ProductPageFilterStart = "http://www.papyrusonline.com/"
            };

            new Site
            {
                Name = "IKEA",
                Uri = "http://www.ikea.com/us/en",
                OpenGraph = true,
                PriceXPath = "//meta[@name='price']",
                ProductPageFilterStart = "http://www.ikea.com/us/en/catalog/products/"
            };

            // No Homegoods

            // TODO: HomeDepot OR API

            // TODO: BestBuy OR API

            // Adidas requires CEF
            new Site
            {
                Name = "adidas",
                Uri = "http://www.adidas.com",
                OpenGraph = true,
                PriceXPath = "//span[@id='currentPrice']",
                ProductPageFilterStart = "http://www.adidas.com/us/",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Shinola",
                Uri = "http://www.shinola.com",
                NameXPath = "//h1[@itemprop='name']",
                PriceXPath = "//span[@itemprop='price']",
                DescriptionXPath = "//meta[@name='description']",
                ImageXPath = "//img[@itemprop='image']",
                ProductPageFilterStart = "http://www.shinola.com/shop/"
            };

            // minted.com is too configurable to grab product data, requires advanced custom milkshake parser

            new Site
            {
                Name = "Barnes & Noble",
                Uri = "http://www.barnesandnoble.com",
                OpenGraph = true,
                DescriptionXPath = "//meta[@name='description']",
                PriceXPath = "//div[@itemprop='price']",
                ProductPageFilterStart = "http://www.barnesandnoble.com"
            };

            // Petsmart should be crawled using their affiliate feed

            new Site
            {
                Name = "FAO Schwarz",
                Uri = "http://www.fao.com",
                NameXPath = "//div[@id='product-detail']/h1",
                DescriptionXPath = "//div[@class='description']",
                PriceXPath = "//span[@id='ours']/strong/span",
                ImageXPath = "//img[@id='mainProductImage']",
                CanonicalXPath = "//link[@rel='canonical']"
            };

            new Site
            {
                Name = "Samsung",
                Uri = "http://www.samsung.com",
                TwitterCard = true,
                NameXPath = "//h1[@class='product-title']",
                PriceXPath = "//p[@class='price']/span[@class='amount']",
                ImageXPath = "//div[@id='product-carousel']/div/div[1]/p/img",
                CanonicalXPath = "//meta[@name='og:url']",
                ProductPageFilterStart = "http://www.samsung.com/us/"
            };

            new Site
            {
                Name = "Burke Decor",
                Uri = "http://www.burkedecor.com",
                OEmbed = true,
                ProductPageFilterStart = "http://www.burkedecor.com/products/"
            };

            // Broadway.com is much more complicated

            // Pages have no content when grabbed by crawler
            new Site
            {
                Name = "Moma Store",
                Uri = "http://www.momastore.org",
                NameXPath = "//h2[@id='moMAItemTitle']",
                DescriptionXPath = "//div[@id='product-description']/p",
                PriceXPath = "//div[@id='moMAItemListPrice']",
                ImageXPath = "//img[@id='momaItemImagePath']",
                CanonicalXPath = "//link[@rel='canonical']",
                ProductPageFilterStart = "http://www.momastore.org",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Artspace",
                Uri = "http://www.artspace.com",
                OpenGraph = true,
                PriceXPath = "//meta[@name='price']",
                ImageXPath = "//meta[@name='sailthru.image.full']",
                ProductPageFilterStart = "http://www.artspace.com"
            };

            new Site
            {
                Name = "Flower Muse",
                Uri = "http://www.flowermuse.com",
                NameXPath = "//h1[@itemprop='name']",
                DescriptionXPath = "//div[@itemprop='description']",
                PriceXPath = "//span[@class='price']",
                ImageXPath = "//a[@id='photo']",
                ProductPageFilterStart = "http://www.flowermuse.com"
            };

            // Victoria's Secret requires a custom parser

            // Martha Stewart American Made is sold on eBay, other Martha Stweart brands are sold at JCPenney, Home Depot, etc.

            new Site
            {
                Name = "Trove General Store",
                Uri = "http://trovegeneral.com",
                OpenGraph = true,
                NameXPath = "//span[@id='lblTitle']",
                DescriptionXPath = "//span[@id='lblDescription']",
                PriceXPath = "//span[@id='lblPrice']",
                ProductPageFilterStart = "http://trovegeneral.com"
            };

            new Site
            {
                Name = "Lily Charleston",
                Uri = "http://www.lilycharleston.com",
                OpenGraph = true,
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.lilycharleston.com"
            };

            // Old World/Road Mercantile - Website is under construction

            new Site
            {
                Name = "Joe Fresh",
                Uri = "https://www.joefresh.com",
                OpenGraph = true,
                PriceXPath = "//span[@class='price']",
                ProductPageFilterStart = "https://www.joefresh.com"
            };

            // REQUIRES CEF
            new Site
            {
                Name = "Piperlime",
                Uri = "http://piperlime.gap.com",
                NameXPath = "//span[@class='productName']",
                PriceXPath = "//span[@id='priceText']",
                DescriptionXPath = "//div[@id='tabWindow']",
                ImageXPath = "//img[@id='zoomImg']",
                CanonicalXPath = "//link[@rel='canonical']",
                ProductPageFilterStart = "http://piperlime.gap.com",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Dillard's",
                Uri = "http://www.dillards.com",
                OpenGraph = true,
                NameXPath = "//h1[@itemprop='name']",
                DescriptionXPath = "//div[@id='description']",
                PriceXPath = "//div[@id='price']",
                ProductPageFilterStart = "http://www.dillards.com/product/"
            };

            new Site
            {
                Name = "Neiman Marcus",
                Uri = "http://www.neimanmarcus.com",
                OpenGraph = true,
                DescriptionXPath = "//div[@itemprop='description']",
                PriceXPath = "//span[@itemprop='price']",
                CanonicalXPath = "//link[@rel='canonical']",
                ProductPageFilterStart = "http://www.neimanmarcus.com"
            };

            // REQUIRES CEF
            new Site
            {
                Name = "Banana Republic",
                Uri = "http://bananarepublic.gap.com",
                NameXPath = "//span[@class='productName']",
                PriceXPath = "//span[@id='priceText']",
                DescriptionXPath = "//div[@id='tabWindow']",
                ImageXPath = "//img[@id='zoomImg']",
                CanonicalXPath = "//link[@rel='canonical']",
                ProductPageFilterStart = "http://bananarepublic.gap.com",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Macy's",
                Uri = "http://www1.macys.com",
                OpenGraph = true,
                DescriptionXPath = "//div[@itemprop='description']",
                PriceXPath = "//meta[@itemprop='price']",
                ProductPageFilterStart = "http://www1.macys.com/shop/product"
            };

            new Site
            {
                Name = "Saks Fifth Avenue",
                Uri = "http://www.saksfifthavenue.com",
                OpenGraph = true,
                DescriptionXPath = "//span[@id='api_prod_copy1']",
                PriceXPath = "//span[@class='product-price']",
                ProductPageFilterStart = "http://www.saksfifthavenue.com"
            };

            new Site
            {
                Name = "Walmart",
                Uri = "http://www.walmart.com",
                OpenGraph = true,
                TwitterCard = true,
                PriceXPath = "//div[@id='WM_PRICE']",
                ProductPageFilterStart = "http://www.walmart.com/ip/"
            };

            new Site
            {
                Name = "The Limited",
                Uri = "http://www.thelimited.com",
                NameXPath = "//h1[@itemprop='name']",
                PriceXPath = "//div[@class='price']",
                DescriptionXPath = "//div[@itemprop='description']",
                ImageXPath = "//img[@itemprop='image']",
                CanonicalXPath = "//link[@rel='canonical']",
                ProductPageFilterStart = "http://www.thelimited.com/product/"
            };

            new Site
            {
                Name = "Express",
                Uri = "http://www.express.com",
                OpenGraph = true,
                DescriptionXPath = "//div[@itemprop='description']",
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.express.com"
            };

            // Requires custom parser
            new Site
            {
                Name = "Bath & Body Works",
                Uri = "http://www.bathandbodyworks.com",
                NameXPath = "//div[@id='product-detail']",
                DescriptionXPath = "//div[@id='product-overview']",
                ImageXPath = "//img[@id='main-product-image']",
                PriceXPath = "//span[@class='value price']",
                ProductPageFilterStart = "http://www.bathandbodyworks.com/product/",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Forever 21",
                Uri = "http://www.forever21.com",
                OpenGraph = true,
                DescriptionXPath = "//span[@id='product_overview']",
                PriceXPath = "//p[@class='product-price']",
                ProductPageFilterStart = "http://www.forever21.com/Product/"
            };

            new Site
            {
                Name = "Bed Bath & Beyond",
                Uri = "http://www.bedbathandbeyond.com",
                OpenGraph = true,
                CanonicalXPath = "//link[@rel='canonical']",
                PriceXPath = "//div[@itemprop='price']",
                ProductPageFilterStart = "http://www.bedbathandbeyond.com/store/product"
            };

            new Site
            {
                Name = "La Perla",
                Uri = "http://www.laperla.com",
                OpenGraph = true,
                NameXPath = "//h1[@itemprop='name']",
                DescriptionXPath = "//div[@itemprop='description']",
                PriceXPath = "//span[@class='price']",
                ProductPageFilterStart = "http://www.laperla.com/us/"
            };

            new Site
            {
                Name = "H&M",
                Uri = "http://www.hm.com/us",
                OpenGraph = true,
                NameXPath = "//*[@id='product']/h1",
                PriceXPath = "//span[@id='text-price']/span",
                CanonicalXPath = "//link[@rel='canonical']",
                ImageXPath = "//img[@id='product-image']",
                ProductPageFilterStart = "http://www.hm.com/us/product/",
                RootOnly = true
            };

            // Gap requires custom parser

            new Site
            {
                Name = "World Market",
                Uri = "http://www.worldmarket.com",
                OpenGraph = true,
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.worldmarket.com/product/"
            };

            new Site
            {
                Name = "Land's End",
                Uri = "http://www.landsend.com",
                OpenGraph = true,
                NameXPath = "//h1[contains(@id, 'productName_')]",
                PriceXPath = "//p[contains(@id, 'productPrice_')]",
                DescriptionXPath = "//div[contains(@id, 'productDesc_')]/p",
                CanonicalXPath = "//link[@rel='canonical']",
                ProductPageFilterStart = "http://www.landsend.com/products/"
            };

            new Site
            {
                Name = "L.L.Bean",
                Uri = "http://www.llbean.com",
                OpenGraph = true,
                NameXPath = "//h1[@itemprop='name']",
                PriceXPath = "//h2[@itemprop='price']",
                ProductPageFilterStart = "http://www.llbean.com/llb/shop/"
            };

            // Old Navy requires custom parser

            new Site
            {
                Name = "Coach",
                Uri = "http://www.coach.com",
                OpenGraph = true,
                PriceXPath = "//meta[@itemprop='price']",
                ProductPageFilterStart = "http://www.coach.com"
            };

            new Site
            {
                Name = "lululemon",
                Uri = "http://shop.lululemon.com",
                OpenGraph = true,
                OGPrice = "og:product:price:amount",
                ProductPageFilterStart = "http://shop.lululemon.com/products/"
            };

            new Site
            {
                Name = "overstock",
                Uri = "http://www.overstock.com",
                OpenGraph = true,
                NameXPath = "//div[@itemprop='name']/h1",
                DescriptionXPath = "//ul[@id='details_descFull']",
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.overstock.com/"
            };

            new Site
            {
                Name = "ULTA",
                Uri = "http://www.ulta.com",
                OpenGraph = true,
                PriceXPath = "//meta[@property='product:price:amount']",
                ProductPageFilterStart = "http://www.ulta.com/ulta/browse/"
            };

            new Site
            {
                Name = "Kohl's",
                Uri = "http://www.kohls.com",
                OpenGraph = true,
                OGPrice = "og:product:price:amount",
                ProductPageFilterStart = "http://www.kohls.com/product/"
            };

            // Sears requires custom parser

            // UPdate JCPenney for BETA to grab sale price instead of MSRP price as it does currently (TODO)
            new Site
            {
                Name = "JCPenney",
                Uri = "http://www.jcpenney.com",
                OpenGraph = true,
                DescriptionXPath = "//div[@itemprop='description']",
                CanonicalXPath = "//link[@rel='canonical']",
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.jcpenney.com"
            };

            new Site
            {
                Name = "Bergdorf Goodman",
                Uri = "http://www.bergdorfgoodman.com",
                OpenGraph = true,
                DescriptionXPath = "//div[@itemprop='description']",
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.bergdorfgoodman.com/"
            };

            // American Eagle requires custom parser (for price)

            // Hollister requires custom parser (for price)

            // Abercrombie & Fitch requires custom parser (for price)

            new Site
            {
                Name = "Pottery Barn",
                Uri = "http://www.potterybarn.com",
                OpenGraph = true,
                TwitterCard = true,
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.potterybarn.com/products/"
            };

            new Site
            {
                Name = "west elm",
                Uri = "http://www.westelm.com",
                OpenGraph = true,
                TwitterCard = true,
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.westelm.com/products/"
            };

            new Site
            {
                Name = "Mark and Graham",
                Uri = "http://www.markandgraham.com",
                OpenGraph = true,
                TwitterCard = true,
                PriceXPath = "//span[@itemprop='price']",
                ProductPageFilterStart = "http://www.markandgraham.com/products/"
            };


            // Requires CEF for price information (loaded via JavaScript)
            new Site
            {
                Name = "NewEgg",
                Uri = "http://www.newegg.com",
                NameXPath = "//span[@itemprop='name']",
                DescriptionXPath = "//div[@id='Overview_Content']",
                PriceXPath = "//*[@itemprop='price']",
                ImageXPath = "//img[contains(@id, 'mainSlide')]",
                ProductPageFilterStart = "http://www.newegg.com/Product/",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Hobby Lobby",
                Uri = "http://shop.hobbylobby.com",
                OpenGraph = true,
                NameXPath = "//h1[@id='hdngItemName']",
                PriceXPath = "//span[@id='CT_ItemRight_0_lblPrice']",
                ProductPageFilterStart = "http://shop.hobbylobby.com/products/"
            };

            new Site
            {
                Name = "Michaels",
                Uri = "http://www.michaels.com",
                NameXPath = "//h2[@itemprop='name']",
                DescriptionXPath = "//div[@itemprop='description']",
                ImageXPath = "//img[@itemprop='image']",
                CanonicalXPath = "//span[@itemprop='url']",
                PriceXPath = "//span[@class='price-standard']",
                ProductPageFilterStart = "http://www.michaels.com"
            };

            // Zara requires a custom parser

            new Site
            {
                Name = "Toys R Us",
                Uri = "http://www.toysrus.com",
                OpenGraph = true,                
                ParseProductAttributesMeta = true,
                ProductPageFilterStart = "http://www.toysrus.com/product/"
            };
                        
            new Site
            {
                Name = "Dick's Sporting Goods",
                Uri = "http://www.dickssportinggoods.com",
                OpenGraph = true,
                ParseProductAttributesMeta = true,
                //PriceXPath = "//div[@class='price']/span[@itemprop='price']",
                ProductPageFilterStart = "http://www.dickssportinggoods.com/product/"
            };

            new Site
            {
                Name = "Gander Mountain",
                Uri = "http://www.gandermountain.com",
                OpenGraph = true,
                PriceXPath = "//div[@class='price']/span",
                ProductPageFilterStart = "http://www.gandermountain.com/modperl/product/"
            };

            new Site
            {
                Name = "Arhaus",
                Uri = "http://www.arhaus.com",
                OpenGraph = true,
                NameXPath = "//h1[@itemprop='name']",
                DescriptionXPath = "//div[@itemprop='description']",
                PriceXPath = "//div[@itemprop='price']",
                CanonicalXPath = "//link[@ref='canonical']",
                ProductPageFilterStart = "http://www.arhaus.com"
            };

            // Joss & Main can't be crawled as of yet, requires a registered user account and login.

            new Site
            {
                Name = "GameStop",
                Uri = "http://www.gamestop.com",
                OpenGraph = true,
                PriceXPath = "//div[@class='buy1']/h3",
                ProductPageFilterStart = "http://www.gamestop.com"
            };

            // Lowes requires custom parser

            new Site
            {
                Name = "Barneys",
                Uri = "http://www.barneys.com",
                OpenGraph = true,
                NameXPath = "//div[@id='detail-column']/h2",
                DescriptionXPath = "//div[@id='accordion']",
                PriceXPath = "//div[@id='detail-column']/div[1]/div/div",
                ProductPageFilterStart = "http://www.barneys.com"
            };

            new Site
            {
                Name = "Ssense",
                Uri = "https://www.ssense.com",
                NameXPath = "//*[@itemprop='name']",
                DescriptionXPath = "//*[@itemprop='description']",
                PriceXPath = "//*[@itemprop='price']",
                ImageXPath = "//img[@itemprop='image']",
                CanonicalXPath = "//link[@rel='canonical']"
            };

            new Site
            {
                Name = "Club Monaco",
                Uri = "http://www.clubmonaco.com",
                OpenGraph = true,
                DescriptionXPath = "//div[@id='tab-details']",
                PriceXPath = "//div[@id='product-information']/div[@class='money']/span",
                ProductPageFilterStart = "http://www.clubmonaco.com/product/"
            };

            new Site
            {
                Name = "Tiffany & Co",
                Uri = "http://www.tiffany.com",
                OpenGraph = true,
                //NameXPath = "//div[@id='itemTitleAndText']/h1",
                PriceXPath = "//div[@id='divItemTotalAndButton']/div[@class='t8']",
                ProductPageFilterStart = "http://www.tiffany.com/Shopping/Item.aspx"
            };

            // Children's Place requires custom parser

            new Site
            {
                Name = "Nike",
                Uri = "http://store.nike.com",
                OpenGraph = true,
                PriceXPath = "//meta[@property='product:price:amount']",
                DescriptionXPath = "//p[@class='oeContent']",
                ProductPageFilterStart = "http://store.nike.com/us/en_us/pd/"
            };

            new Site
            {
                Name = "abc carpet & home",
                Uri = "http://www.abchome.com",
                OpenGraph = true,
                PriceXPath = "//span[contains(@id, 'product-price')]",
                ProductPageFilterStart = "http://www.abchome.com/shop/"
            };

            new Site
            {
                Name = "True Value",
                Uri = "http://www.truevalue.com",
                NameXPath = "//h1[@class='product-name']",
                PriceXPath = "//div[@id='priceContainer']/div/text()",
                DescriptionXPath = "//div[@id='productDetailTabs']//div[@class='tab_content']/div/p/text()",
                ImageXPath = "//div[@id='mainProductImg']/a/img",
                RootOnly = true,
                ProductPageFilterStart = "http://www.truevalue.com/product/"
            };

            new Site
            {
                Name = "Staples",
                Uri = "http://www.staples.com",
                OpenGraph = true,
                NameXPath = "//h1",
                DescriptionXPath = "//div[@id='subdesc_content']",
                PriceXPath = "//*[@class='finalPrice']"
            };

            new Site
            {
                Name = "BCBG",
                Uri = "http://www.bcbg.com",
                OpenGraph = true,
                PriceXPath = "//div[@class='product-price']/span[1]",
                DescriptionXPath = "//div[@itemprop='description']"
            };

            new Site
            {
                Name = "katespade",
                Uri = "http://www.katespade.com",
                OpenGraph = true
            };

            new Site
            {
                Name = "Brooks Brothers",
                Uri = "http://www.brooksbrothers.com",
                OpenGraph = true
            };

            new Site
            {
                Name = "Jo-Ann Fabric",
                Uri = "http://www.joann.com",
                NameXPath = "//h1[@itemprop='name']",
                DescriptionXPath = "//div[@id='description']",
                PriceXPath = "//meta[@itemprop='price']",
                ImageXPath = "//img[@itemprop='image']",
                CanonicalXPath = "//span[@itemprop='url']"
            };

            // Pier1 Requires custom parser for description

            new Site
            {
                Name = "Chico's",
                Uri = "http://www.chicos.com",
                OpenGraph = true,
                PriceXPath = "//meta[@name='product:price:amount']",
                ProductPageFilterStart = "http://www.chicos.com/store/browse/product.jsp"
            };

            // Aldo requires custom parser

            new Site
            {
                Name = "GNC",
                Uri = "http://www.gnc.com",
                OpenGraph = true,
                PriceXPath = "//p[@class='now']"
            };

            // Lane Bryant custom parser

            new Site
            {
                Name = "Frontgate",
                Uri = "http://www.frontgate.com",
                OpenGraph = true,
                NameXPath = "//div[@class='gwt-product-title-panel']/h3",
                DescriptionXPath = "//meta[@name='description']",
                PriceXPath = "//meta[@itemprop='price']",
                CanonicalXPath = "//meta[@itemprop='url']"
            };

            new Site
            {
                Name = "AllPosters",
                Uri = "http://www.allposters.com",
                OpenGraph = true,
                NameXPath = "//h1[@itemprop='name']",
                PriceXPath = "//div[@itemprop='price']",
                DescriptionXPath = "//div[@class='productTypeDetails']"
            };

            new Site
            {
                Name = "Tatcha",
                Uri = "http://www.tatcha.com",
                OpenGraph = true,
                PriceXPath = "//span[@class='price']"
            };

            // jo malone requires custom parser

            new Site
            {
                Name = "sunglass hut",
                Uri = "http://www.sunglasshut.com",
                OpenGraph = true,
                NameXPath = "//h1[@itemprop='name']",
                PriceXPath = "//span[@itemprop='price']"
            };


            // NEEDS CUSTOM PARSER FOR IMAGE
            new Site
            {
                Name = "Sharper Image",
                Uri = "http://www.sharperimage.com",
                NameXPath = "//div[contains(@class, 'pdp-info-summary')]/h3",
                DescriptionXPath = "//div[contains(@class, 'pdp-info-summary')]/p",
                PriceXPath = "//span[@id='catalog_price']",
                ImageXPath = "//img[@onmouseover='showEnlarge']",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "QVC",
                Uri = "http://www.qvc.com",
                OpenGraph = true,
                PriceXPath = "//p[@id='parProductDetailPrice']"
            };

            // bonpoint custom parser

            new Site
            {
                Name = "Aeropostale",
                Uri = "http://www.aeropostale.com",
                OpenGraph = true,
                PriceXPath = "//li[@class='now']"
            };

            // ANTI CRAWLING, SERVES BLANK PAGE (NOTE: This hasn't occured in blender)
            new Site
            {
                Name = "Cartier",
                Uri = "http://www.cartier.us",
                OpenGraph = true,
                PriceXPath = "//div[@itemprop='price']"
            };

            new Site
            {
                Name = "Summit Racing",
                Uri = "http://www.summitracing.com",
                OpenGraph = true,
                PriceXPath = "//p[@class='price']",
                DescriptionXPath = "//p[@class='overview-description']"
            };

            // Needs custom parser for image
            new Site
            {
                Name = "Bass Pro Shops",
                Uri = "http://www.basspro.com",
                NameXPath = "//h1[@itemprop='name']",
                PriceXPath = "//span[@itemprop='minPrice']",
                DescriptionXPath = "//div[@id='description']",
                ImageXPath = "",
                Crawlable = false // TODO
            };

            new Site
            {
                Name = "Hermes",
                Uri = "http://usa.hermes.com",
                OpenGraph = true,
                PriceXPath = "//span[@id='product-price']/span[@class='price']"
            };

            new Site
            {
                Name = "AllModern",
                Uri = "http://www.allmodern.com",
                OpenGraph = true,
                ProductPageFilterStart = "http://www.wayfair.com/"
            };
        }
    }
}
