using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Milkshake.Linkshare
{
    public class Linkshare
    {
        private static readonly string ConsumerKey = "jUlHbI_zWRaHIkHw2Ffun5Bi2_Ea";
        private static readonly string ConsumerSecret = "B4w5Pa0CQdtJR0psfxWCgxKdrMQa";
        public static readonly string SiteID = "3140526";
        private static readonly string Username = "wishlu";
        private static readonly string Password = "manatee12";

        private static readonly string WebServicesToken = "32f1f9b47d9c05291f9424b0a21a53fa429e1521509d390d3c6110ace39f0692";
        private static readonly string SecurityToken = "3a50c7b1337491aafb2c3f84d69945cdc8f052e402d53fe2cc9f0492a9e8e7c9";

        private static readonly string TokenRequestAuthorization = "Basic alVsSGJJX3pXUmFISWtIdzJGZnVuNUJpMl9FYTpCNHc1UGEwQ1FkdEpSMHBzZnhXQ2d4S2RyTVFh";

        // API Endpoints
        private static readonly string Endpoint_Base = "https://api.rakutenmarketing.com";
        private static readonly string Endpoint_RequestAccessToken = "/token?grant_type=password&username={0}&password={1}&scope={2}";
        private static readonly string Endpoint_RefreshAccessToken = "/token?grant_type=refresh_token&refresh_token={0}&scope=Production";
        private static readonly string Endpoint_GetAdvertisers = "/advertisersearch/1.0";

        // Instance Variables
        private string BearerToken = "";
        private string RefreshToken = "";
        private int ExpiresIn = 0;

        // Static List
        public static List<LinkshareMerchant> Merchants = new List<LinkshareMerchant>();

        public object GetAccessToken()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Endpoint_Base);

            // JSON Accept Header
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Authorization Header
            client.DefaultRequestHeaders.Add("Authorization", TokenRequestAuthorization);

            // Payload
            HttpContent content = new StringContent("");

            // Post Request & wait for response
            HttpResponseMessage response = client.PostAsync(String.Format(Endpoint_RequestAccessToken, Username, Password, SiteID), content).Result;

            string json = response.Content.ReadAsStringAsync().Result;

            dynamic result = JsonConvert.DeserializeObject(json);

            BearerToken = "Bearer " + result.access_token;
            RefreshToken = result.refresh_token;
            ExpiresIn = result.expires_in;

            return result;
        }

        public object GetRefreshedAccessToken()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Endpoint_Base);

            // JSON Accept Header
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Authorization Header
            client.DefaultRequestHeaders.Add("Authorization", TokenRequestAuthorization);

            // Payload
            HttpContent content = new StringContent("");

            // Post Request & wait for response
            HttpResponseMessage response = client.PostAsync(String.Format(Endpoint_RefreshAccessToken, RefreshToken), content).Result;

            string json = response.Content.ReadAsStringAsync().Result;

            dynamic result = JsonConvert.DeserializeObject(json);

            BearerToken = "Bearer " + result.access_token;
            RefreshToken = result.refresh_token;
            ExpiresIn = result.expires_in;

            return result;
        }

        public dynamic GetAdvertisers()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Endpoint_Base);

            // JSON Accept Header
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            // Authorization Header
            client.DefaultRequestHeaders.Add("Authorization", BearerToken);
                        
            // Post Request & wait for response
            HttpResponseMessage response = client.GetAsync(Endpoint_GetAdvertisers).Result;

            string xml = response.Content.ReadAsStringAsync().Result;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);

            dynamic result = JsonConvert.DeserializeObject(json);
                                    
            return result.result.midlist;
        }

        public void LoadAdvertisers()
        {
            foreach (dynamic merc in GetAdvertisers().merchant)
            {
                Linkshare.Merchants.Add(new LinkshareMerchant {MID = merc.mid, Name = merc.merchantname});
            }
        }
    }
}
