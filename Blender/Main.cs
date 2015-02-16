using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using Milkshake;

namespace Blender
{
    public enum CrawlStatus
    {
        NotCrawled = 0,
        Pending = 1,
        Crawling = 2,
        Completed = 3,
        Error = 4,
        Stopped = 5
    }

    public partial class Main : Form
    {
        public Scheduler scheduler;
        private delegate void LogDelegate(string msg);
        private delegate void StatusDelegate(string msg);
        private delegate void ProgressDelegate(int count, int max);
        private delegate void SetSiteDelegate(Site s, CrawlStatus cs);
        private delegate CrawlStatus GetSiteDelegate(Site s);
        private delegate void SetSiteLast(Site s, DateTime time);
        private delegate void SiteAddPro(Site s, int count);
        private delegate void SiteRemovePro(Site s, int count);
        private delegate void SiteUpdatePro(Site s, int count);
        private delegate void AddSiteDelegate(Site s, int count, DateTimeOffset lastcrawl);
        private delegate void AddTokenDelegate(string token, int count);

        public Main()
        {
            InitializeComponent();
        }

        public void Log(String msg)
        {
            this.Invoke(new LogDelegate(doLog), msg);
        }

        public void Status(String msg)
        {
            this.Invoke(new StatusDelegate(doStatus), msg);
        }

        public void Progress(int count, int max)
        {
            this.Invoke(new ProgressDelegate(doProgress), count, max);
        }

        private void doLog(string msg)
        {
            listBox1.Items.Add(msg);

            if (listBox1.Items.Count > 5000)
                listBox1.Items.RemoveAt(0);
            
            listBox1.TopIndex = listBox1.Items.Count - 1;
                        
            //Application.DoEvents();
        }

        private void doStatus(string msg)
        {
            this.statusLabel.Text = msg;
        }

        private void doProgress(int count, int max)
        {
            toolStripProgressBar1.Maximum = max;
            toolStripProgressBar1.Value = count;

            if (count == max)
                doStatus("Complete");
        }

        public void SetSiteStatus(Site s, CrawlStatus cs)
        {
            this.Invoke(new SetSiteDelegate(doSetSiteStatus), s, cs);
        }

        public CrawlStatus GetSiteStatus(Site s)
        {
            return (CrawlStatus)this.Invoke(new GetSiteDelegate(doGetSiteStatus), s);                        
        }

        private CrawlStatus doGetSiteStatus(Site s)
        {
            return (CrawlStatus)Enum.Parse(typeof(CrawlStatus), listView1.Items.Find(s.Name, true).First().SubItems[1].Text.Replace(" ",""));
        }

        public void AddSite(Site s, int count, DateTimeOffset lastcrawl)
        {
            this.Invoke(new AddSiteDelegate(doAddSite), s, count, lastcrawl);
        }

        public void AddToken(string word, int count)
        {
            this.Invoke(new AddTokenDelegate(doAddToken), word, count);
        }

        private void doAddSite(Site s, int count, DateTimeOffset lastcrawl)
        {
            ListViewItem item = listView1.Items.Add(new ListViewItem(s.Name));
            item.Name = s.Name;
            item.UseItemStyleForSubItems = false;
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "Not Crawled", System.Drawing.Color.White, System.Drawing.Color.LightGray, this.Font));

            if (lastcrawl != null && lastcrawl != DateTimeOffset.MinValue)
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, lastcrawl.ToString()));
            else
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "N/A"));

            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, count.ToString()));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "0"));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "0"));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "0"));
        }

        private void doAddToken(string word, int count)
        {
            ListViewItem item = listView2.Items.Add(new ListViewItem(word));
            item.Name = word;
            
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, count.ToString()));            
        }

        public void SetSiteLastCrawl(Site s, DateTime time)
        {
            this.Invoke(new SetSiteLast(doSetSiteLastCrawl), s, time);
        }
        
        public void SiteAddProduct(Site s, int count)
        {
            this.Invoke(new SiteAddPro(doSiteAddProduct), s, count);
        }

        private void doSiteAddProduct(Site s, int count)
        {
            int prod = int.Parse(listView1.Items.Find(s.Name, true).First().SubItems[3].Text);
            prod += count;
            listView1.Items.Find(s.Name, true).First().SubItems[3].Text = prod.ToString();

            int prodcur = int.Parse(listView1.Items.Find(s.Name, true).First().SubItems[4].Text);
            prodcur += count;
            listView1.Items.Find(s.Name, true).First().SubItems[4].Text = prodcur.ToString();
        }

        public void SiteUpdateProduct(Site s, int count)
        {
            this.Invoke(new SiteUpdatePro(doSiteUpdateProduct), s, count);
        }

        private void doSiteUpdateProduct(Site s, int count)
        {
            int prod = int.Parse(listView1.Items.Find(s.Name, true).First().SubItems[5].Text);
            prod += count;
            listView1.Items.Find(s.Name, true).First().SubItems[5].Text = prod.ToString();                        
        }

        public void SiteRemoveProduct(Site s, int count)
        {
            this.Invoke(new SiteRemovePro(doSiteRemoveProduct), s, count);
        }

        private void doSiteRemoveProduct(Site s, int count)
        {
            int prod = int.Parse(listView1.Items.Find(s.Name, true).First().SubItems[6].Text);
            prod += count;
            listView1.Items.Find(s.Name, true).First().SubItems[6].Text = prod.ToString();

            int prod2 = int.Parse(listView1.Items.Find(s.Name, true).First().SubItems[3].Text);
            prod2 -= count;
            listView1.Items.Find(s.Name, true).First().SubItems[3].Text = prod2.ToString();
        }

        private void doSetSiteLastCrawl(Site s, DateTime time)
        {
            listView1.Items.Find(s.Name, true).First().SubItems[2].Text = time.ToString();
        }

        private void doSetSiteStatus(Site s, CrawlStatus cs)
        {
            switch (cs)
            {
                case CrawlStatus.Completed:
                    listView1.Items.Find(s.Name, true).First().SubItems[1].Text = "Completed";
                    listView1.Items.Find(s.Name, true).First().SubItems[1].BackColor = System.Drawing.Color.Green;
                    break;

                case CrawlStatus.Crawling:
                    listView1.Items.Find(s.Name, true).First().SubItems[1].Text = "Crawling";
                    listView1.Items.Find(s.Name, true).First().SubItems[1].BackColor = System.Drawing.Color.Blue;
                    break;

                case CrawlStatus.Error:
                    listView1.Items.Find(s.Name, true).First().SubItems[1].Text = "Error!";
                    listView1.Items.Find(s.Name, true).First().SubItems[1].BackColor = System.Drawing.Color.Red;
                    break;

                case CrawlStatus.NotCrawled:
                    listView1.Items.Find(s.Name, true).First().SubItems[1].Text = "Not Crawled";
                    listView1.Items.Find(s.Name, true).First().SubItems[1].BackColor = System.Drawing.Color.LightGray;
                    break;

                case CrawlStatus.Pending:
                    listView1.Items.Find(s.Name, true).First().SubItems[1].Text = "Pending";
                    listView1.Items.Find(s.Name, true).First().SubItems[1].BackColor = System.Drawing.Color.Gold;
                    break;

                case CrawlStatus.Stopped:
                    listView1.Items.Find(s.Name, true).First().SubItems[1].Text = "Stopped";
                    listView1.Items.Find(s.Name, true).First().SubItems[1].BackColor = System.Drawing.Color.DarkRed;
                    break;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Graph.Initialize();
            Store.Initialize();
            IgnoreList.Initialize();

            Logger.LogExtension = new LogPrint(Program.Log);

            scheduler = new Scheduler(this);

            Thread t = new Thread(() =>
            {
                int sites = Milkshake.Site.Sites.Count;
                int cur = 0;

                foreach (Milkshake.Site s in Milkshake.Site.Sites)
                {
                    if (s.HasAPI || !s.Crawlable)
                        continue;

                    int count = 0;
                    DateTimeOffset lc = DateTimeOffset.MinValue;
                    try
                    {
                        //StoreNode node = Graph.Instance.Cypher
                        //.Match("(sn:Store)")
                        //.Where((StoreNode sn) => sn.Name == s.Name)
                        //.ReturnDistinct(sn => sn.As<StoreNode>())
                        //.Results.Single();
                        
                        count = Store.Get(s.Name).ProductCount;
                        lc = Store.Get(s.Name).LastCrawl;

                        // NOTE: This intensive, slower process, will poll the actual count of products (necessary if statistics are not properly kept up to date)
                        /*count = (int)Graph.Instance.Cypher
                            .Match("(p:Product)")
                            .Where((ProductNode p) => p.Store == s.Name)
                            .ReturnDistinct(p => p.CountDistinct())
                            .Results.First();

                        Graph.Instance.Cypher
                           .Match("(sn:Store)")
                           .Where((StoreNode sn) => sn.Name == s.Name)
                           .Set("sn.ProductCount = {pc}")
                           .WithParam("pc", count)
                           .ExecuteWithoutResults();*/
                    }
                    catch
                    {
                        count = 0;
                    }

                    AddSite(s, count, lc);
                                        
                    cur++;

                    Program.Status("Fetching site data from database (" + cur + " of " + sites + ")");
                    Program.Progress(cur, sites);
                    //MessageBox.Show("Added site/store: " + s.Name);
                }

                Program.Progress(sites, sites);
            });

            t.Start();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;

            var t = new Thread(scheduler.Start);
            t.IsBackground = false;
            t.Start();            
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
            scheduler.Stop();
        }

        private void crawlSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    string name = item.Name;
                    Site s = Milkshake.Site.GetSite(name);

                    var t = new Thread(() => { scheduler.Start(s); });
                    t.IsBackground = false;
                    t.Start();
                }
            }
        }
                
        ////////////////
        // Data Tools //
        ////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            var t = new Thread(() => { DataTools.GetProductStoreErrors(); });
            t.IsBackground = false;
            t.Start();
        }

        public void PSEComplete(int count)
        {
            this.Invoke(new onPSECompleteDel(onPSEComplete), count);
        }

        private delegate void onPSECompleteDel(int count);
        private void onPSEComplete(int count)
        {
            button2.Enabled = true;            
            label2.Text = count.ToString();
        }
    }
}
