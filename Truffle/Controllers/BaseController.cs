using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Truffle.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string signature = this.Request.QueryString.Get("signature");

            if (signature != null)
            {
                string shop = Request.QueryString.Get("shop");
                string path_prefix = Request.QueryString.Get("path_prefix");
                string timestamp = Request.QueryString.Get("timestamp");

                string join = "";
                if (path_prefix == null)
                {
                    join = shop + timestamp;
                }
                else
                {
                    join = path_prefix + shop + timestamp;
                }

                HMAC md5 = HMAC.Create("hmacsha256");
                
                md5.Key = Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);
                string computed_sig = Encoding.Unicode.GetString(md5.ComputeHash(Encoding.Unicode.GetBytes(join)));

                if (signature != computed_sig)
                {
                    Common.Logger.Log("INVALID SIGNATURE!!!!");
                }
                else
                {
                    Common.Logger.Log("Request is a valid, Shopify signed request.");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
