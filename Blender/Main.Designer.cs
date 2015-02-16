namespace Blender
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.listView1 = new System.Windows.Forms.ListView();
            this.siteName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.siteStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.siteLastCrawl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.siteProductCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.siteAddedCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.siteUpdatedCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.siteRemovedCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.crawlSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSiteDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crawlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crawlToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(970, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.siteName,
            this.siteStatus,
            this.siteLastCrawl,
            this.siteProductCount,
            this.siteAddedCount,
            this.siteUpdatedCount,
            this.siteRemovedCount});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(970, 252);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.StateImageList = this.imageList1;
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // siteName
            // 
            this.siteName.Text = "Name";
            this.siteName.Width = 250;
            // 
            // siteStatus
            // 
            this.siteStatus.Text = "Status";
            this.siteStatus.Width = 100;
            // 
            // siteLastCrawl
            // 
            this.siteLastCrawl.Text = "Last Crawl";
            this.siteLastCrawl.Width = 120;
            // 
            // siteProductCount
            // 
            this.siteProductCount.Text = "Product Count";
            this.siteProductCount.Width = 120;
            // 
            // siteAddedCount
            // 
            this.siteAddedCount.Text = "Added (Current)";
            this.siteAddedCount.Width = 120;
            // 
            // siteUpdatedCount
            // 
            this.siteUpdatedCount.Text = "Updated (Current)";
            this.siteUpdatedCount.Width = 130;
            // 
            // siteRemovedCount
            // 
            this.siteRemovedCount.Text = "Removed (Current)";
            this.siteRemovedCount.Width = 130;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crawlSiteToolStripMenuItem,
            this.toolStripSeparator2,
            this.viewSiteDetailsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 54);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(156, 6);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "application-detail.png");
            this.imageList1.Images.SetKeyName(1, "bug.png");
            this.imageList1.Images.SetKeyName(2, "calendar-select.png");
            this.imageList1.Images.SetKeyName(3, "counter.png");
            this.imageList1.Images.SetKeyName(4, "shopping-basket.png");
            this.imageList1.Images.SetKeyName(5, "shopping-basket--arrow.png");
            this.imageList1.Images.SetKeyName(6, "shopping-basket--exclamation.png");
            this.imageList1.Images.SetKeyName(7, "shopping-basket--minus.png");
            this.imageList1.Images.SetKeyName(8, "shopping-basket--pencil.png");
            this.imageList1.Images.SetKeyName(9, "shopping-basket--plus.png");
            this.imageList1.Images.SetKeyName(10, "status.png");
            this.imageList1.Images.SetKeyName(11, "status-away.png");
            this.imageList1.Images.SetKeyName(12, "status-busy.png");
            this.imageList1.Images.SetKeyName(13, "status-offline.png");
            this.imageList1.Images.SetKeyName(14, "store.png");
            this.imageList1.Images.SetKeyName(15, "store--arrow.png");
            this.imageList1.Images.SetKeyName(16, "store--exclamation.png");
            this.imageList1.Images.SetKeyName(17, "store-label.png");
            this.imageList1.Images.SetKeyName(18, "store-market-stall.png");
            this.imageList1.Images.SetKeyName(19, "store-medium.png");
            this.imageList1.Images.SetKeyName(20, "store--minus.png");
            this.imageList1.Images.SetKeyName(21, "store-network.png");
            this.imageList1.Images.SetKeyName(22, "store-open.png");
            this.imageList1.Images.SetKeyName(23, "store--pencil.png");
            this.imageList1.Images.SetKeyName(24, "store--plus.png");
            this.imageList1.Images.SetKeyName(25, "store-share.png");
            this.imageList1.Images.SetKeyName(26, "store-small.png");
            this.imageList1.Images.SetKeyName(27, "toolbox.png");
            this.imageList1.Images.SetKeyName(28, "traffic-light.png");
            this.imageList1.Images.SetKeyName(29, "wooden-box--minus.png");
            this.imageList1.Images.SetKeyName(30, "wooden-box--pencil.png");
            this.imageList1.Images.SetKeyName(31, "wooden-box--plus.png");
            this.imageList1.Images.SetKeyName(32, "blue-folder-network-horizontal.png");
            this.imageList1.Images.SetKeyName(33, "categories.png");
            this.imageList1.Images.SetKeyName(34, "category.png");
            this.imageList1.Images.SetKeyName(35, "category-group.png");
            this.imageList1.Images.SetKeyName(36, "category-group-select.png");
            this.imageList1.Images.SetKeyName(37, "category-item.png");
            this.imageList1.Images.SetKeyName(38, "category-item-select.png");
            this.imageList1.Images.SetKeyName(39, "color.png");
            this.imageList1.Images.SetKeyName(40, "color-swatch.png");
            this.imageList1.Images.SetKeyName(41, "compile.png");
            this.imageList1.Images.SetKeyName(42, "compile-error.png");
            this.imageList1.Images.SetKeyName(43, "compile-warning.png");
            this.imageList1.Images.SetKeyName(44, "computer.png");
            this.imageList1.Images.SetKeyName(45, "computer-cloud.png");
            this.imageList1.Images.SetKeyName(46, "construction.png");
            this.imageList1.Images.SetKeyName(47, "credit-card.png");
            this.imageList1.Images.SetKeyName(48, "credit-cards.png");
            this.imageList1.Images.SetKeyName(49, "dashboard.png");
            this.imageList1.Images.SetKeyName(50, "desktop.png");
            this.imageList1.Images.SetKeyName(51, "document-attribute-x.png");
            this.imageList1.Images.SetKeyName(52, "drill.png");
            this.imageList1.Images.SetKeyName(53, "feed.png");
            this.imageList1.Images.SetKeyName(54, "feed-document.png");
            this.imageList1.Images.SetKeyName(55, "fingerprint.png");
            this.imageList1.Images.SetKeyName(56, "fire.png");
            this.imageList1.Images.SetKeyName(57, "fire-big.png");
            this.imageList1.Images.SetKeyName(58, "users.png");
            this.imageList1.Images.SetKeyName(59, "broom.png");
            this.imageList1.Images.SetKeyName(60, "broom--arrow.png");
            this.imageList1.Images.SetKeyName(61, "broom-code.png");
            this.imageList1.Images.SetKeyName(62, "broom--exclamation.png");
            this.imageList1.Images.SetKeyName(63, "broom--minus.png");
            this.imageList1.Images.SetKeyName(64, "broom--pencil.png");
            this.imageList1.Images.SetKeyName(65, "broom--plus.png");
            this.imageList1.Images.SetKeyName(66, "bug--arrow.png");
            this.imageList1.Images.SetKeyName(67, "bug--exclamation.png");
            this.imageList1.Images.SetKeyName(68, "bug--minus.png");
            this.imageList1.Images.SetKeyName(69, "bug--pencil.png");
            this.imageList1.Images.SetKeyName(70, "bug--plus.png");
            this.imageList1.Images.SetKeyName(71, "building.png");
            this.imageList1.Images.SetKeyName(72, "building-low.png");
            this.imageList1.Images.SetKeyName(73, "building-old.png");
            this.imageList1.Images.SetKeyName(74, "building--plus.png");
            this.imageList1.Images.SetKeyName(75, "cake.png");
            this.imageList1.Images.SetKeyName(76, "sd-memory-card.png");
            this.imageList1.Images.SetKeyName(77, "sealing-wax.png");
            this.imageList1.Images.SetKeyName(78, "servers.png");
            this.imageList1.Images.SetKeyName(79, "service-bell.png");
            this.imageList1.Images.SetKeyName(80, "shopping-basket.png");
            this.imageList1.Images.SetKeyName(81, "shopping-basket--arrow.png");
            this.imageList1.Images.SetKeyName(82, "shopping-basket--exclamation.png");
            this.imageList1.Images.SetKeyName(83, "shopping-basket--minus.png");
            this.imageList1.Images.SetKeyName(84, "shopping-basket--pencil.png");
            this.imageList1.Images.SetKeyName(85, "shopping-basket--plus.png");
            this.imageList1.Images.SetKeyName(86, "spectrum.png");
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBox1.Size = new System.Drawing.Size(970, 227);
            this.listBox1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 27);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox1);
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(970, 505);
            this.splitContainer1.SplitterDistance = 252;
            this.splitContainer1.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 227);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(970, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(46, 17);
            this.statusLabel.Text = "{status}";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage10);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 562);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.menuStrip1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(976, 535);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Crawler";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.ImageIndex = 51;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(976, 535);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "XML Import";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.ImageIndex = 53;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(976, 535);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Feeds";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.ImageIndex = 58;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(976, 535);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Crowdsourced";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.ImageIndex = 27;
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(976, 535);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Data Tools";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.ImageIndex = 14;
            this.tabPage6.Location = new System.Drawing.Point(4, 23);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(976, 535);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Stores";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.listView2);
            this.tabPage7.ImageIndex = 33;
            this.tabPage7.Location = new System.Drawing.Point(4, 23);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(976, 535);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Tokens";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.ImageIndex = 77;
            this.tabPage8.Location = new System.Drawing.Point(4, 23);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(976, 535);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "Brands";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // tabPage9
            // 
            this.tabPage9.ImageIndex = 40;
            this.tabPage9.Location = new System.Drawing.Point(4, 23);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(976, 535);
            this.tabPage9.TabIndex = 8;
            this.tabPage9.Text = "Colors";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // tabPage10
            // 
            this.tabPage10.ImageIndex = 3;
            this.tabPage10.Location = new System.Drawing.Point(4, 23);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(976, 535);
            this.tabPage10.TabIndex = 9;
            this.tabPage10.Text = "Ranking";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product -> Store Associations";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Count:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "N/A";
            // 
            // crawlSiteToolStripMenuItem
            // 
            this.crawlSiteToolStripMenuItem.Image = global::Blender.Properties.Resources.bug__arrow;
            this.crawlSiteToolStripMenuItem.Name = "crawlSiteToolStripMenuItem";
            this.crawlSiteToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.crawlSiteToolStripMenuItem.Text = "Crawl Site";
            this.crawlSiteToolStripMenuItem.Click += new System.EventHandler(this.crawlSiteToolStripMenuItem_Click);
            // 
            // viewSiteDetailsToolStripMenuItem
            // 
            this.viewSiteDetailsToolStripMenuItem.Image = global::Blender.Properties.Resources.table;
            this.viewSiteDetailsToolStripMenuItem.Name = "viewSiteDetailsToolStripMenuItem";
            this.viewSiteDetailsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.viewSiteDetailsToolStripMenuItem.Text = "View Site Details";
            // 
            // crawlToolStripMenuItem
            // 
            this.crawlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.crawlToolStripMenuItem.Image = global::Blender.Properties.Resources.bug;
            this.crawlToolStripMenuItem.Name = "crawlToolStripMenuItem";
            this.crawlToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.crawlToolStripMenuItem.Text = "Crawl";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Image = global::Blender.Properties.Resources.cog_go;
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Image = global::Blender.Properties.Resources.stop;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(95, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Blender.Properties.Resources.door_in;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Image = global::Blender.Properties.Resources.drill;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(6, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(208, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "Repair";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Image = global::Blender.Properties.Resources.magnifier;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(208, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Analyze";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView2.Location = new System.Drawing.Point(3, 3);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(967, 263);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Word";
            this.columnHeader1.Width = 240;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Product Count";
            this.columnHeader2.Width = 120;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Blender";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem crawlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader siteName;
        private System.Windows.Forms.ColumnHeader siteStatus;
        private System.Windows.Forms.ColumnHeader siteLastCrawl;
        private System.Windows.Forms.ColumnHeader siteProductCount;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader siteAddedCount;
        private System.Windows.Forms.ColumnHeader siteUpdatedCount;
        private System.Windows.Forms.ColumnHeader siteRemovedCount;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem crawlSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem viewSiteDetailsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

