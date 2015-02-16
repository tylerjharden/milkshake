import scrapy

from scrapy.contrib.spiders.crawl import CrawlSpider, Rule
from scrapy.contrib.linkextractors import LinkExtractor
from scrapy_redis.spiders import RedisSpider
from milkshake.items import MilkshakeItem
import redis

class MilkshakeSpider(RedisSpider):
    #r = redis.StrictRedis(host='10.0.0.40')
    name = 'milkshake_spider'
    redis_key = 'milkshake_spider:start_urls'
    #allowed_domains = ['target.com']    
    #start_urls = ['http://www.nordstrom.com']
    #rules = (
       # Rule(LinkExtractor(allow=('/c/', ))),
        #Rule(LinkExtractor(allow=('/s/', ))),
        #Rule(LinkExtractor(allow='/p/'),'parse_product')        
    #)

    def parse(self, response):
        ml = MilkshakeLoader(response=response)
        return ml.load_item()
            
    #def parse_product(self, response):
        #prod = MilkshakeItem()
        #prod['url'] = response.xpath("//meta[@property='og:url']/@content").extract()
        #prod['name'] = response.xpath("//meta[@property='og:title']/@content").extract()
        #prod['description'] = response.xpath("//meta[@property='og:description']/@content").extract()
        #prod['image'] = response.xpath("//div[@id='wanelo-save']/a/@data-image").extract()
        #prod['price'] = response.xpath("//div[@id='wanelo-save']/a/@data-price").extract()
        #prod['upc'] = response.xpath("//meta[@property='og:upc']/@content").extract()
                
        #self.r.publish("milkshake_products", prod)

        #if not prod['name'].strip():
        #    return
        #if not prod['price'].strip():
        #    return
         
        #return prod