using Schloss.Data.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake
{
    public class Crawler
    {
        private ConnectionMultiplexer redis;

        public Crawler()
        {
            redis = ConnectionMultiplexer.Connect("10.0.0.40:6379");
        }

        public void PublishLink(string link)
        {
            ISubscriber sub = redis.GetSubscriber();

            sub.Publish("milkshake_parsequeue", link);
        }
    }
}
