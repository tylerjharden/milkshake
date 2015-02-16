using Nest;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake
{
    public static class Search
    {
        private static ElasticClient _client;

        static Search()
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static void Initialize()
        {
            var node = new Uri("http://10.0.0.40:9200");
            var settings = new ConnectionSettings(node, "milkshake");

            _client = new ElasticClient(settings);

            // Milkshake Product, POCO, Mapping to ElasticSearch
            _client.Map<Product>(m => m.MapFromAttributes());
            /*_client.Map<Product>(m => m
                .Properties(props => props
                    .String(s => s
                        .Name(p => p.Name)
                        .Analyzer("keyword")
                        .Fields(fs => fs
                            .String(f => f
                                .Name(p => p.Name.Suffix("analyzed"))
                                .Analyzer("snowball")
                            )
                        )
                    )
                )
            );*/
            _client.Map<Offer>(m => m.MapFromAttributes());
        }

        public static void Reset()
        {
            _client.DeleteIndex("milkshake");            
        }

        private static bool Index<T>(T @object) where T : class
        {
            var index = _client.Index<T>(@object);

            return index.Created && index.IsValid;
        }

        private static async Task<bool> IndexAsync<T>(T @object) where T : class
        {
            var index = await _client.IndexAsync<T>(@object);

            return index.Created && index.IsValid;
        }

        public static bool Index(Product p)
        {
            return Index<Product>(p);            
        }

        public static bool Index(Offer o)
        {
            return Index<Offer>(o);
        }

        public static bool Index(Store s)
        {
            return Index<Store>(s);
        }

        public static bool Index(Brand b)
        {
            return Index<Brand>(b);
        }

        public static bool Index(Manufacturer m)
        {
            return Index<Manufacturer>(m);
        }

        public static async Task<bool> IndexAsync(Product p)
        {
            return await IndexAsync<Product>(p);
        }

        public static async Task<bool> IndexAsync(Offer o)
        {
            return await IndexAsync<Offer>(o);
        }

        public static async Task<bool> IndexAsync(Store s)
        {
            return await IndexAsync<Store>(s);
        }

        public static async Task<bool> IndexAsync(Brand b)
        {
            return await IndexAsync<Brand>(b);
        }

        public static async Task<bool> IndexAsync(Manufacturer m)
        {
            return await IndexAsync<Manufacturer>(m);
        }

        public static ISearchResponse<Product> ProductName(string query, int page = 0, int pagesize = 50)
        {
            var terms = query.Split(' ');

            var results = _client.Search<Product>(s => s
                .From(page*pagesize)
                .Size(pagesize)                 
                .Query(q => q                    
                    .FunctionScore(fsq => fsq
                        .Query(qse => qse                            
                            .QueryString(qs => qs
                                /*.OnFieldsWithBoost(d => d
                                    .Add(entry => entry.Brand, 1.0)
                                    .Add(entry => entry.Manufacturer, 1.0)
                                    .Add(entry => entry.Store, 2.5)
                                )*/
                                .Query(query)                                
                            )                            
                        )  
                        .ScriptScore(ss => ss
                            .Script("_score + log(1 + 0.1 * doc['views'].value) + log(1 + 0.5 * doc['saves'].value)")
                        )
                        /*.Functions(
                            fv => fv                            
                                .FieldValueFactor(db => db
                                    .Field(p => p.Views)
                                    .Modifier(FieldValueFactorModifier.Log1P)
                                    .Factor(0.1)
                            ),
                            fs => fs
                                .FieldValueFactor(db => db
                                .Field(p => p.Saves)
                                .Modifier(FieldValueFactorModifier.Log1P)
                                .Factor(0.5)
                            )
                        )                       
                        .BoostMode(FunctionBoostMode.Multiply)*/
                    )
                    
                    /*.Boosting(bq => bq
                        .Positive(pq => pq                            
                           .QueryString(qs => qs                               
                                .OnFieldsWithBoost(d => d
                                    .Add(entry => entry.Brand, 1.0)
                                    .Add(entry => entry.Manufacturer, 1.0)
                                    .Add(entry => entry.Store, 2.5)
                                )
                                .Query(query)
                            )
                            //.Script("_score + (doc['views'].value*0.01)")
                            //.Script("_score + (doc['saves'].value*0.1)")                            
                        )
                        .Negative(nq => nq
                            .Filtered(nfq => nfq
                                .Query(qq => qq.MatchAll())
                                .Filter(f => f.Missing(p => p.Brand) || f.Missing(p => p.Manufacturer) || f.Missing(p => p.MPN) )                                
                            )
                        )
                        .NegativeBoost(0.2)                        
                    )*/
                )                
            );

            /*var results = _client.Search<Product>(s => s
                .From(page*pagesize)
                .Size(pagesize)
                //.Scroll("2s")
                .Query(q=> q.QueryString(qs => qs.Query(query)))
                .FacetTerm("Brand", t => t
                    .OnField(f => f.Brand)
                    .Size(20)
                )
                .FacetTerm("Color", t => t
                    .OnField(f => f.Color)
                    .Size(20)
                )
                .FacetTerm("Manufacturer", t => t
                    .OnField(f => f.Manufacturer)
                    .Size(20)
                )
                .SortDescending("_score")
                .SortDescending(p => p.Saves)
                .SortDescending(p => p.Views)                
            );*/

            return results;
        }   
        
        public static Product ProductId(Guid id)
        {
            var results = _client.Get<Product>(id.ToString());
            
            return results.Source;
        }

        public static bool ProductView(Guid id)
        {
            var p = _client.Get<Product>(id.ToString()).Source;

            p.Views = p.Views + 1;

            var result = _client.Update<Product>(u => u.Id(id.ToString()).Doc(p));

            return result.IsValid;
        }

        public static bool ProductSave(Guid id)
        {
            var p = _client.Get<Product>(id.ToString()).Source;

            p.Saves = p.Saves + 1;

            var result = _client.Update<Product>(u => u.Id(id.ToString()).Doc(p));

            return result.IsValid;
        }

        public static ISearchResponse<Product> StoreId(Guid id, int page = 0, int pagesize = 50)
        {
            var results = _client.Search<Product>(s => s
                .From(page * pagesize)
                .Size(pagesize)
                .Df("storeId")
                .Query(q=> q.QueryString(qs => qs.Query("\"" + id.ToString() + "\"")))
            );

            return results;
        }

        public static long StoreIdCount(Guid id)
        {
            var results = _client.Search<Product>(s => s                
                .Df("storeId")
                .SearchType(Elasticsearch.Net.SearchType.Count)
                .Query(q=> q.QueryString(qs => qs.Query("\"" + id.ToString() + "\"")))                
            );

            return results.HitsMetaData.Total;
        }
    }
}
