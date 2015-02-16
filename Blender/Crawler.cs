using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abot.Crawler;
using Abot.Poco;
using System.Net;
using HtmlAgilityPack;
using Milkshake;
using System.IO;

namespace Blender
{
    class Crawler
    {
        private PoliteWebCrawler crawler;
        private bool shouldAbort;
        private Site m_currentSite;
        public Site CurrentSite { get { return m_currentSite; } }
        private Scheduler m_Scheduler;
        private Milkshake.Crawler crawlerhub;

        public Crawler(Scheduler s)
        {
            Initialize();
            m_Scheduler = s;
            shouldAbort = false;            
        }

        public void Abort()
        {
            shouldAbort = true;
        }

        public void Initialize()
        {
            shouldAbort = false;

            crawler = new PoliteWebCrawler();

            crawlerhub = new Milkshake.Crawler();
            
            crawler.PageCrawlStarting += crawler_PageCrawlStarting;
            crawler.PageCrawlCompleted += crawler_PageCrawlCompleted;
            crawler.PageLinksCrawlDisallowed += crawler_PageLinksCrawlDisallowed;
            crawler.PageCrawlDisallowed += crawler_PageCrawlDisallowed;

            crawler.ShouldCrawlPage((pageToCrawl, crawlContext) =>
            {
                if (shouldAbort)
                    return new CrawlDecision { Allow = false, ShouldHardStopCrawl = true, ShouldStopCrawl = true, Reason = "Scheduler aborted crawl" };

                if (pageToCrawl.Uri.Authority == "google.com")
                    return new CrawlDecision { Allow = false, Reason = "Dont want to crawl google pages" };

                if (pageToCrawl.Uri.Authority == "facebook.com")
                    return new CrawlDecision { Allow = false, Reason = "Dont want to crawl facebook pages" };

                if (pageToCrawl.Uri.Authority == "twitter.com")
                    return new CrawlDecision { Allow = false, Reason = "Dont want to crawl twitter pages" };

                if (pageToCrawl.Uri.Authority == "youtube.com")
                    return new CrawlDecision { Allow = false, Reason = "Dont want to crawl youtube pages" };

                if (IgnoreList.Urls.Contains(pageToCrawl.Uri.AbsoluteUri.ToString()))
                    return new CrawlDecision { Allow = false, Reason = "Page has been crawled" };

                if (pageToCrawl.Uri.Authority != crawlContext.RootUri.Authority)
                    return new CrawlDecision { Allow = false, Reason = "Not the same site" };

                return new CrawlDecision { Allow = true, Reason = "Do it." };
            });

            crawler.ShouldDownloadPageContent((crawledPage, crawlContext) =>
            {
                if (shouldAbort)
                    return new CrawlDecision { Allow = false, ShouldHardStopCrawl = true, ShouldStopCrawl = true, Reason = "Scheduler aborted crawl" };

                //if (!crawledPage.Uri.AbsoluteUri.Contains(".com"))
                //    return new CrawlDecision { Allow = false, Reason = "Only download raw page content for .com tlds" };

                return new CrawlDecision { Allow = true, Reason = "Do it." };
            });

            crawler.ShouldCrawlPageLinks((crawledPage, crawlContext) =>
            {
                if (shouldAbort)
                    return new CrawlDecision { Allow = false, ShouldHardStopCrawl = true, ShouldStopCrawl = true, Reason = "Scheduler aborted crawl" };

                //if (crawledPage.Content.Bytes.Length < 100)
                //    return new CrawlDecision { Allow = false, Reason = "Just crawl links in pages that have at least 100 bytes" };

                return new CrawlDecision { Allow = true, Reason = "Do it." };
            });
        }

        public bool Update(Site s)
        {
            m_currentSite = s;
            shouldAbort = false;
            Program.Log("Beginning update of " + s.Name + " existing products.");

            List<string> urls = new List<string>();

            try
            {
                urls = Graph.Instance.Cypher
                    .Match("p=(a:Product)")
                    .Where((Product a) => a.Store == s.Name)
                    .Return<string>("extract(n IN nodes(p)| n.Url) AS urls")
                    .Results.ToList();

                Program.Log(urls.Count.ToString() + " existing products to update for " + s.Name);

                int counter = 1;
                foreach (string url in urls)
                {                    
                    Program.Status(String.Format("Updating product {0} of {1} for {2}", counter, urls.Count, s.Name));
                    Program.Progress(counter, urls.Count);
                    counter++;

                    try
                    {
                        string uri = url.Replace("[", "").Replace("]", "").Replace("\"", "").Trim();

                        WebClient client = new WebClient();

                        Stream webhtml = null;
                        try
                        {
                            webhtml = client.OpenRead(uri);
                        }
                        catch (WebException e)
                        {
                            if (e.Status == WebExceptionStatus.ProtocolError)
                            {
                                var response = e.Response as HttpWebResponse;
                                if (response != null)
                                {
                                    // 404 Not Found
                                    if (response.StatusCode == HttpStatusCode.NotFound)
                                    {
                                        Program.Log("HTTP 404: Product no longer exists on site, removing.");
                                        ProductManager.RemoveProduct(ProductManager.GetProduct(uri));
                                        m_Scheduler.Parent.SiteRemoveProduct(m_currentSite, 1);
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                                                
                        HtmlDocument doc = new HtmlDocument();
                        doc.Load(webhtml);

                        doUpdateProduct(doc, uri);
                    }
                    catch (Exception e)
                    { Program.Log("(Error) Updater crawl failed: " + e.ToString()); }
                }
                                
                return true;
            }
            catch (Exception e)
            {
                Program.Log("Failed update.");
                Program.Log(e.ToString());
                return false;
            }
        }

        public bool Crawl(Site s)
        {
            m_currentSite = s;
            shouldAbort = false;
            Program.Log("Beginning crawl of " + s.Name);
            Program.Status("Crawling " + s.Name);

            try
            {
                CrawlResult result = crawler.Crawl(new Uri(s.Uri));

                if (result.ErrorOccurred)
                {
                    Program.Log(String.Format("Crawl of {0} completed with error: {1}", result.RootUri.AbsoluteUri, result.ErrorException.Message));
                    return false;
                }
                else
                {
                    Program.Log(String.Format("Crawl of {0} completed without error.", result.RootUri.AbsoluteUri));
                }

                Program.Log(s.Name + " has been crawled and added to Milkshake successfully.");
                return true;
            }
            catch (Exception e)
            {
                Program.Log(e.ToString());
                return false;
            }            
        }

        void crawler_PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                Program.Log(String.Format("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri));
                return;
            }

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
            {
                Program.Log(String.Format("Page had no content {0}", crawledPage.Uri.AbsoluteUri));
                return;
            }

            crawlerhub.PublishLink(e.CrawledPage.Uri.AbsoluteUri);

            /*string storesite = crawledPage.Uri.Authority;

            if (storesite.StartsWith("www."))
                storesite = storesite.Remove(0, 4);

            
            else
            {
                Program.Log(String.Format("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri));

                try
                {
                    if (Site.SiteExists(crawledPage.Uri))
                    {
                        Site s = Site.GetSite(crawledPage.Uri);

                        if (s.IsProductPage(crawledPage.Uri.AbsoluteUri))
                        {
                            Product pn = s.Parse(crawledPage.HtmlDocument, crawledPage.Uri);

                            if (pn != null)
                            {
                                int count = ProductManager.AddProduct(pn);
                                m_Scheduler.Parent.SiteAddProduct(s, count);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Log("(Error) " + ex.ToString());
                }
                finally
                {
                    IgnoreList.Add(crawledPage.Uri.AbsoluteUri);
                }
            }*/                        
        }

        void crawler_PageCrawlStarting(object sender, PageCrawlStartingArgs e)
        { }

        void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        { }

        void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        { }          
      
        // Updater
        private void doUpdateProduct(HtmlDocument page, string url)
        {
            //CrawledPage crawledPage = e.CrawledPage;

            //string storesite = crawledPage.Uri.Authority;

           // if (storesite.StartsWith("www."))
           //     storesite = storesite.Remove(0, 4);

            //if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
            //{
            //    Program.Log(String.Format("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri));
            //
            //    Program.Log("Removing dead product from database.");

            //    return;
            //}
            //else
            //{
                Program.Log(String.Format("Crawl of page succeeded {0}", url));

                try
                {
                    //if (Site.SiteExists(crawledPage.Uri))
                    //{
                        //Site s = Site.GetSite(crawledPage.Uri);

                        if (m_currentSite.IsProductPage(url))
                        {
                            Product oldpn = ProductManager.GetProduct(url);

                            Product pn = m_currentSite.Parse(page, new Uri(url));
                            
                            bool updated = false;

                            if (pn != null)
                            {
                                // Page is pointing to a new product
                                if (oldpn.Name != pn.Name)
                                {
                                    //Program.Log("Found updated product name.");
                                    //Product.AddProduct(pn);
                                    IgnoreList.Add(url);
                                    return;
                                }

                                if (oldpn.Price != pn.Price)
                                    Program.Log("Found updated product price."); updated = true;
                                                                
                                if (oldpn.Image != pn.Image)
                                    Program.Log("Found updated product image."); updated = true;

                                if (oldpn.UPC != pn.UPC)
                                    Program.Log("Found updated product UPC."); updated = true;

                                if (oldpn.Description != pn.Description)
                                    Program.Log("Found updated product description."); updated = true;

                                if (updated)
                                {
                                    ProductManager.UpdateProduct(pn, oldpn);
                                    IgnoreList.Add(url);
                                    m_Scheduler.Parent.SiteUpdateProduct(m_currentSite, 1);
                                }
                            }
                        }
                        else
                        {
                            Program.Log("Removing dead product from database.");
                        }
                    //}
                }
                catch (Exception ex)
                {
                    Program.Log("(Error) " + ex.ToString());
                }
                finally
                {
                    IgnoreList.Add(url);
                }
            //}

            //if (string.IsNullOrEmpty(crawledPage.Content.Text))
            //    Program.Log(String.Format("Page had no content {0}", crawledPage.Uri.AbsoluteUri));
        }
    }
}
