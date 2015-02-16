from scrapy.selector import Selector
from scrapy.contrib.linkextractors import LinkExtractor
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.contrib.spiders import CrawlSpider, Rule
from milkshake.items import MilkshakeLoader
from scrapy_redis.spiders import RedisSpider
from scrapy_redis.spiders import RedisMixin
from milkshake.items import MilkshakeLoader

class MilkshakeCrawler(RedisMixin, CrawlSpider):
    name = 'milkshake_crawler'
    redis_key = 'milkshake_crawler:start_urls'

    rules = (
        Rule(LinkExtractor(allow='/s/'),'parse_product'),
        Rule(SgmlLinkExtractor(), callback='parse_page', follow=True),
        #Rule(LinkExtractor(allow=('/c/', ))),               
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

        # lowest level fields
        ml.add_xpath('name','//title[1]/text()')
        ml.add_value('url', response.url)
        ml.add_xpath('description','//meta[@property="description"]/@content')

        # OpenGraph
        ml.replace_xpath('url','//meta[@property="og:url"]/@content')
        ml.replace_xpath('name','//meta[@property="og:title"]/@content')
        ml.replace_xpath('image','//meta[@property="og:image"]/@content')
        ml.replace_xpath('description','//meta[@property="og:description"]/@content')
        ml.add_xpath('upc','//meta[@property="og:upc"]/@content')
        ml.replace_xpath('upc','//meta[@property="product:upc"]/@content')
        ml.add_xpath('price','//meta[@property="product:price:amount"]/@content')

        # Microdata
        # TODO

        # XPath selectors
        # TODO: Compile list of common selectors for sites that don't use proper meta data

        # wanelo button data
        ml.replace_xpath('url','//div[@id="wanelo-save"]/a/@data-url')
        ml.replace_xpath('name','//div[@id="wanelo-save"]/a/@data-title')
        ml.replace_xpath('image','//div[@id="wanelo-save"]/a/@data-image')
        ml.replace_xpath('price','//div[@id="wanelo-save"]/a/@data-price')
        
        return ml.load_item()