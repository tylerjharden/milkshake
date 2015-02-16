using ShopifyAPIAdapterLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Truffle.Models;

namespace Truffle.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string shopName = model.ShopName.Replace(".myshopify.com", String.Empty);

                Uri requestUrl = this.Url.RequestContext.HttpContext.Request.Url;
                Uri returnURL = new Uri(string.Format("{0}://{1}{2}",
                                                        requestUrl.Scheme,
                                                        requestUrl.Authority,
                                                        this.Url.Action("ShopifyAuthCallback", "Account")));

                var authorizer = new ShopifyAPIAuthorizer(shopName, ConfigurationManager.AppSettings["Shopify.ConsumerKey"], ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);
                var authUrl = authorizer.GetAuthorizationURL(new string[] { ConfigurationManager.AppSettings["Shopify.Scope"] }, returnURL.ToString());
                return Redirect(authUrl);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ShopifyAuthCallback(string code, string shop, string error)
        {
            if (!String.IsNullOrEmpty(error))
            {
                this.TempData["Error"] = error;
                return RedirectToAction("Login");
            }
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(shop))
                return RedirectToAction("Index", "Home");

            var shopName = shop.Replace(".myshopify.com", String.Empty);
            var authorizer = new ShopifyAPIAuthorizer(shopName, ConfigurationManager.AppSettings["Shopify.ConsumerKey"], ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);

            ShopifyAuthorizationState authState = authorizer.AuthorizeClient(code);
            if (authState != null && authState.AccessToken != null)
            {
                Shopify.ShopifyAuthorize.SetAuthorization(this.HttpContext, authState);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
