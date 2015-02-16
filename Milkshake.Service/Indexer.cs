using System;
using Milkshake.Service.Documents;
using Nest;

namespace Milkshake.Service
{
    internal class Indexer
    {
        private readonly ElasticClient _esClient;
        private string _index = "milkshake";

        public Indexer()
        {
            var settings = new ConnectionSettings(new Uri("http://10.0.0.40:9200"));
            settings.SetDefaultIndex(_index);
            _esClient = new ElasticClient(settings);
        }

        public TDocument Get<TDocument>(Guid id) where TDocument : class
        {
            return _esClient.Get<TDocument>(id.ToString()).Source;
        }

        public void Index<TDocument>(TDocument document) where TDocument : class
        {
            _esClient.Index(document, y => y.Index(_index));
        }

        public void Init()
        {
            _esClient.CreateIndex(_index, y => y
                .AddMapping<Product>(m => m.MapFromAttributes()));
        }
    }
}
