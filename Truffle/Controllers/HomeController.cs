using ShopifyAPIAdapterLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Truffle.Shopify;

namespace Truffle.Controllers
{
    [ShopifyAuthorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
                
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            object shopResponse = _shopify.Get("/admin/products.json");

            return View();
        }

        ShopifyAPIClient _shopify;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ShopifyAuthorizationState authState = ShopifyAuthorize.GetAuthorizationState(this.HttpContext);
            if (authState != null)
            {
                _shopify = new ShopifyAPIClient(authState, new JsonDataTranslator());
            }
        }

        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult SetupScript()
        {
            // pass the supplied JSON Data Translator
            //var api = new ShopifyAPIClient((ShopifyAuthorizationState)Session["authState"], new JsonDataTranslator());

            //api.Delete("/admin/script_tags.json");

            // The JSON Data Translator will automatically decode the JSON for you
            dynamic st = new {
                script_tag = new
                {
                    @event = "onload",
                    @src = "http://dev.wishlu.com:1337/Scripts/address.js"
                },                
            };
                
            var result = _shopify.Post("/admin/script_tags.json", st);

            return View("Index", result);
        }

        public ActionResult Authorize()
        {
            string shopName = "wishlu-sandbox";// get the shop name from the user (i.e. a web form)
            // you will need to pass a URL that will handle the response from Shopify when it passes you the code parameter
            Uri returnURL = new Uri("http://dev.wishlu.com:1337/doauth");
            var authorizer = new ShopifyAPIAuthorizer(shopName,
                ConfigurationManager.AppSettings["Shopify.ConsumerKey"], // In this case I keep my key and secret in my config file
                ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);

            // get the Authorization URL and redirect the user
            var authUrl = authorizer.GetAuthorizationURL(new string[] { ConfigurationManager.AppSettings["Shopify.Scope"] }, returnURL.ToString());
            return Redirect(authUrl);
        }

        public ActionResult DoAuth(string code = "", string shop = "", string error = "")
        {
            // get the following variables from the Query String of the request
            //string code = "";
            //string shop = "";
            //string error = "";

            // check for an error first
            if (!String.IsNullOrEmpty(error))
            {
                this.TempData["Error"] = error;
                return RedirectToAction("Login");
            }

            // make sure we have the code
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(shop))
                return RedirectToAction("index", "home");

            var shopName = shop.Replace(".myshopify.com", String.Empty);
            var authorizer = new ShopifyAPIAuthorizer(shopName,
                ConfigurationManager.AppSettings["Shopify.ConsumerKey"], // In this case I keep my key and secret in my config file
                ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);

            // get the authorization state
            ShopifyAuthorizationState authState = authorizer.AuthorizeClient(code);

            if (authState != null && authState.AccessToken != null)
            {
                // TODO!
                // store the auth state in the session or DB to be used for all API calls for the specified shop
                Session["authState"] = authState;
            }

            return View("Success");
        }
    }
}
