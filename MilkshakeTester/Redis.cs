using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkshakeTester
{
    public static class Redis
    {
        private static ConnectionMultiplexer redis;
        //private static ISubscriber sub;
        private static IDatabase db;

        static Redis()
        {
            redis = ConnectionMultiplexer.Connect("10.0.0.40:6379");
            redis.PreserveAsyncOrder = true;

            //sub = redis.GetSubscriber();
            db = redis.GetDatabase();
        }   
        
        public static void PushJSON(string json)
        {
            db.ListRightPush("milkshake_product_parse_queue", json, When.Always, CommandFlags.FireAndForget);
            //sub.Publish("milkshake_newproduct", "1", CommandFlags.FireAndForget);
        }                
    }
}
