using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Milkshake;
using System.Windows.Forms;

namespace Blender
{
    public class Scheduler
    {
        private Crawler c;

        private List<Site> toCrawl = new List<Site>();
        private List<Site> stoppedCrawl = new List<Site>();
        private List<Site> completedCrawl = new List<Site>();
        private List<Site> errorCrawl = new List<Site>();

        public Scheduler(Main parent)
        {
            this.parent = parent;
            isRunning = false;
            this.c = new Crawler(this);
        }

        private Main parent;
        public Main Parent { get { return parent; } }

        public bool isRunning = false;
        private bool stopped = false;

        public void Start(Site s)
        {
            isRunning = true;
            stopped = false;
            parent.Log("Scheduler has been started for a targeted run.");

            toCrawl.Add(s);
            parent.SetSiteStatus(s, CrawlStatus.Pending);
            
            while (isRunning)
            {
                parent.SetSiteStatus(s, CrawlStatus.Crawling);
                Store.UpdateLastCrawl(Store.Get(s.Name));
                parent.SetSiteLastCrawl(s, DateTime.Now);

                c.Initialize();

                c.Update(s);

                if (c.Crawl(s))
                {
                    if (stopped)
                        return;

                    parent.SetSiteStatus(s, CrawlStatus.Completed);
                    toCrawl.Remove(s);
                    completedCrawl.Add(s);
                    return;
                }
                else
                {
                    if (stopped)
                        return;

                    parent.SetSiteStatus(s, CrawlStatus.Error);
                    toCrawl.Remove(s);
                    errorCrawl.Add(s);
                    return;
                }
            }
        }

        public void Start()
        {
            isRunning = true;
            stopped = false;
            parent.Log("Scheduler has been started.");

            foreach (Site ss in Site.Sites)
            {
                if (parent.GetSiteStatus(ss) == CrawlStatus.NotCrawled)
                {
                    toCrawl.Add(ss);
                    parent.SetSiteStatus(ss, CrawlStatus.Pending);
                    Application.DoEvents();
                }
            }
            
            while (isRunning)
            {
                try
                {
                    Site s = GetSite();

                    parent.SetSiteStatus(s, CrawlStatus.Crawling);
                    Store.UpdateLastCrawl(Store.Get(s.Name));
                    parent.SetSiteLastCrawl(s, DateTime.Now);

                    c.Initialize();

                    c.Update(s);

                    if (c.Crawl(s))
                    {
                        if (stopped)
                            return;

                        parent.SetSiteStatus(s, CrawlStatus.Completed);
                        toCrawl.Remove(s);
                        completedCrawl.Add(s);
                    }
                    else
                    {
                        if (stopped)
                            return;

                        parent.SetSiteStatus(s, CrawlStatus.Error);
                        toCrawl.Remove(s);
                        errorCrawl.Add(s);
                    }
                }
                catch (Exception e)
                {
                    Program.Log("Scheduler has encountered an error: " + e.ToString());
                }
            }            
        }

        public void Stop()
        {
            isRunning = false;
            stopped = true;
            parent.Log("Scheduler has been stopped.");

            if (c.CurrentSite != null)
            {
                parent.SetSiteStatus(c.CurrentSite, CrawlStatus.Stopped);
                toCrawl.Remove(c.CurrentSite);
                stoppedCrawl.Add(c.CurrentSite);
            }

            c.Abort();
        }

        public Site GetSite()
        {
            Random r = new Random();

            return toCrawl[r.Next(0, toCrawl.Count - 1)];
        }
    }
}
