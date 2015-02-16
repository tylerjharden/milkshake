using System;

namespace Milkshake
{
    // Contains historic crawl data. Used for analytics. Blender uses this for scheduling automated crawls/updated.
    public class CrawlData
    {
        public Guid Id { get; set; }
        public string Site { get; set; }
        public Guid SiteId { get; set; }
        public DateTimeOffset Time { get; set; }
        public int Added { get; set; }
        public int Updated { get; set; }
        public int Removed { get; set; }
        public int ProductCount { get; set; }
    }
}
