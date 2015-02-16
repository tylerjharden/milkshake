# -*- coding: utf-8 -*-

# Scrapy settings for MilkshakeCrawler project
#
# For simplicity, this file contains only the most important settings by
# default. All the other settings are documented here:
#
#     http://doc.scrapy.org/en/latest/topics/settings.html
#

BOT_NAME = 'milkshake'

SPIDER_MODULES = ['milkshake.spiders']
NEWSPIDER_MODULE = 'milkshake.spiders'

# Crawl responsibly by identifying yourself (and your website) on the user-agent
#USER_AGENT = 'MilkshakeCrawler (+http://www.yourdomain.com)'

# Scrapy-redis COnfiguration

# store scheduling storing requests queue in redis
SCHEDULER = "scrapy_redis.scheduler.Scheduler"

# don't cleanup redis, allow pause-resume of crawls
SCHEDULER_PERSIST = True 

#priority queue
#SCHEDULER_QUEUE_CLASS = 'scrapy_redis.queue.SpiderPriorityQueue'
# queue (FIFO)
#SCHEDULER_QUEUE_CLASS = 'scrapy_redis.queue.SpiderQueue'
# stack (LIFO)
#SCHEDULER_QUEUE_CLASS = 'scrapy_redis.queue.SpiderStack'

# max idle time to prevent early closing
SCHEDULER_IDLE_BEFORE_CLOSE = 10

# store scraped item in redis for post-processing
ITEM_PIPELINES = {
    'milkshake.pipelines.MilkshakePipeline': 300,
    'scrapy_redis.pipelines.RedisPipeline': 400
}

# Host/Port to connect to Redis
REDIS_HOST = '10.0.0.40'
REDIS_PORT = 6379