import json
import redis

def main():
    r = redis.Redis(host='10.0.0.40')
    while True:
        # process queue as FIFO, change 'blpop' to 'brpop' to process as LIFO
        source, data = r.blpop(["nordstrom:items"])
        item = json.loads(data)
        try:
            print u"Processing: %(name)s <%(url)s>" % item
        except KeyError:
            print u"Error processing: %r" % item

if __name__ == '__main__':
    main()