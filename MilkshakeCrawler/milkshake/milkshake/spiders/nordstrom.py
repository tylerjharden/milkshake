from scrapy.selector import Selector
from scrapy.contrib.linkextractors import LinkExtractor
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.contrib.spiders import CrawlSpider, Rule
from milkshake.items import MilkshakeLoader
from scrapy_redis.spiders import RedisSpider
from scrapy_redis.spiders import RedisMixin

class NordstromSpider(RedisMixin, CrawlSpider): #RedisSpider
    name = 'nordstrom'
    redis_key = 'nordstrom:start_urls'
    #allowed_domains = ['nordstrom.com']
    #start_urls = ['http://www.nordstrom.com']

    rules = (
        Rule(SgmlLinkExtractor(), callback='parse_page', follow=True),
        #Rule(LinkExtractor(allow=('/c/', ))),       
        Rule(LinkExtractor(allow='/s/'),'parse_product'),
        Rule(LinkExtractor(allow='/p/'),'parse_product')
    )

    def set_crawler(self, crawler):
        CrawlSpider.set_crawler(self, crawler)
        RedisMixin.setup_redis(self)

    def parse_page(self, response):
        pass
        #ml = MilkshakeLoader(response=response)
        #ml.add_xpath('name', '//title[1]/text()')
        #ml.add_value('url', response.url)
        #return ml.load_item()
    
    def parse_product(self, response):
        ml = MilkshakeLoader(response=response)
        ml.add_xpath('url','//meta[@property="og:url"]/@content')
        ml.add_xpath('name','//meta[@property="og:title"]/@content')
        ml.add_xpath('description','//meta[@property="og:description"]/@content')
        ml.add_xpath('image','//div[@id="wanelo-save"]/a/@data-image')
        ml.add_xpath('price','//div[@id="wanelo-save"]/a/@data-price')
        ml.add_xpath('upc','//meta[@property="og:upc"]/@content')
        return ml.load_item()