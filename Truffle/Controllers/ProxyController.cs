using ShopifyAPIAdapterLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Truffle.Common;

namespace Truffle.Controllers
{
    public class ProxyController : Controller
    {
        public ActionResult Give(string id)
        {
            //string referrer = Request.UrlReferrer.ToString();
            string qs = Request.QueryString.ToString();

            // pass the supplied JSON Data Translator
            //var api = new ShopifyAPIClient((ShopifyAuthorizationState)Session["authState"], new JsonDataTranslator());

            ShopifyAuthorizationState authState = Truffle.Shopify.ShopifyAuthorize.GetAuthorizationState(this.HttpContext);
            var api = new ShopifyAPIClient(authState, new JsonDataTranslator());

            var model = api.Get("/admin/products/" + id + ".json");
                        
            return View("Give", model);
            //return View("Give", referrer);
            //return View("Give", new { product = new { @id = id }, qs = qs });
        }

        public ActionResult Buy(Guid id, string pid)
        {
            ShopifyAuthorizationState authState = Truffle.Shopify.ShopifyAuthorize.GetAuthorizationState(this.HttpContext);
            var api = new ShopifyAPIClient(authState, new JsonDataTranslator());

            WishLuUser user = Graph.GetUserById(id);
            dynamic prod = api.Get("/admin/products/" + pid + ".json");

            dynamic order = new
            {
                order = new {
                    line_items = new dynamic[1]{ new { @variant_id = prod.product.variants[0].id, @quantity = 1 } },                    
                    customer = new { first_name = "Tyler", last_name = "Harden", email = "tyler@wishlu.com" },
                    billing_address = new
                    {
                        first_name = "Tyler",
                        last_name = "Harden",
                        address1 = "1497 Butterfield Circle",
                        phone = "330-766-0277",
                        city = "Niles",
                        province = "Ohio",
                        country = "United States",
                        zip = "44446"
                    },
                    shipping_address = new
                    {
                        first_name = user.FirstName,
                        last_name = user.LastName,
                        address1 = user.ShipAddress1,
                        phone = "N/A",
                        city = user.ShipCity,
                        province = user.ShipStateOrProvince,
                        country = "United States",
                        zip = user.ShipZipOrPostalCode
                    },
                    email = "tyler@wishlu.com"
                }                
            };
            
            object res = null;
            try
            {
                res = api.Post("/admin/orders.json", order);
            }
            catch (WebException ex)
            {
                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                                
                Logger.Log(sr.ReadToEnd());
            }

            return View("Buy", res);
        }
    }
}
