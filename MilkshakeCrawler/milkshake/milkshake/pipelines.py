# -*- coding: utf-8 -*-

# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: http://doc.scrapy.org/en/latest/topics/item-pipeline.html
from datetime import datetime

class MilkshakePipeline(object):
    def process_item(self, item, spider):        
        if item['price'] and item['name'] and item['image']:            
            item["crawled"] = datetime.utcnow()
            item["spider"] = spider.name
            return item
        else:
            raise DropItem("Missing name, price, or image in %s" % item)
    