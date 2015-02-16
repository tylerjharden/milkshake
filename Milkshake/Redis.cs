using Schloss.Data.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake
{
    public static class Redis
    {
        private static ConnectionMultiplexer redis;
        private static ISubscriber sub;
        private static IDatabase db;

        static Redis()
        {
            redis = ConnectionMultiplexer.Connect("10.0.0.40:6379");
            redis.PreserveAsyncOrder = false;

            sub = redis.GetSubscriber();
            db = redis.GetDatabase();
        }   
        
        public static async Task PushJSON(string json)
        {
            await db.ListRightPushAsync("milkshake_product_parse_queue", json, When.Always, CommandFlags.FireAndForget);
            await sub.PublishAsync("milkshake_newproduct", "1", CommandFlags.FireAndForget);
        }

        public static async Task PushLogMessage(string message)
        {
            await sub.PublishAsync("squid_log", message, CommandFlags.FireAndForget);
        }
    }
}
