using Schloss.Data.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake
{
    public class Parser
    {
        private ConnectionMultiplexer redis;

        public Parser()
        {
            redis = ConnectionMultiplexer.Connect("10.0.0.40:6379");
            redis.PreserveAsyncOrder = false;

            ISubscriber sub = redis.GetSubscriber();
            sub.Subscribe("milkshake_products", (channel, message) =>
            {
                this.ParseLink((string)message);
            });
        }

        public void ParseLink(string link)
        {
            Console.WriteLine("(Parsing) " + link);
        }
    }
}
