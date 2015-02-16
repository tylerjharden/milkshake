# -*- coding: utf-8 -*-

# Define here the models for your scraped items
#
# See documentation in:
# http://doc.scrapy.org/en/latest/topics/items.html
import scrapy

from scrapy.item import Item, Field
from scrapy.contrib.loader import ItemLoader
from scrapy.contrib.loader.processor import MapCompose, TakeFirst, Join

from w3lib.html import remove_tags

class MilkshakeItem(Item):    
    url = Field()
    name = Field()
    description = Field()
    price = Field()
    upc = Field()
    image = Field()

    # crawler data fields
    crawled = Field()
    spider = Field()
    pass

def filter_price(value):
    v = value.strip('$')
    if v.isdigit():
        return v

class MilkshakeLoader(ItemLoader):
    default_item_class = MilkshakeItem
    default_input_processor = MapCompose(remove_tags, lambda s: s.strip())
    default_output_processor = TakeFirst()

    # price input
    #price_in = MapCompose(remove_tags, filter_price)

    # description output
    description_out = Join()