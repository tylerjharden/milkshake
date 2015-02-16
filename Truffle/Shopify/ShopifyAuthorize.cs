using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopifyAPIAdapterLibrary;
using Truffle.Common;

namespace Truffle.Shopify
{
    public class ShopifyAuthorize : AuthorizeAttribute
    {
        private static readonly string AuthSessionKey = "shopify_auth_state";

        public static void SetAuthorization(System.Web.HttpContextBase httpContext, ShopifyAuthorizationState state)
        {
            httpContext.Session[AuthSessionKey] = state;

            try
            {
                Logger.Log("Setting auth state");

                Graph.Instance.Cypher
                    .Merge("(auth:ShopifyAuth { ShopName:{sn}, AccessToken:{at} })")                    
                    .WithParam("sn", state.ShopName)
                    .WithParam("at", state.AccessToken)
                    .ExecuteWithoutResults();
            }
            catch (Exception ex) 
            {
                Logger.Log(ex.ToString());
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authState = GetAuthorizationState(httpContext);
            if (authState == null || String.IsNullOrWhiteSpace(authState.AccessToken))
                return false;
            return true;
        }

        public static ShopifyAuthorizationState GetAuthorizationState(System.Web.HttpContextBase httpContext)
        {            
            string shopname = httpContext.Request.QueryString.Get("shop");

            if (shopname == null)
                return httpContext.Session[AuthSessionKey] as ShopifyAuthorizationState;

            shopname = shopname.Replace(".myshopify.com", String.Empty);

            try
            {
                Logger.Log("Grabbing auth state from database");

                return Graph.Instance.Cypher
                    .Match("(auth:ShopifyAuth)")
                    .Where((ShopifyAuthorizationState auth) => auth.ShopName == shopname)
                    .Return(auth => auth.As<ShopifyAuthorizationState>())
                    .Results.Single();
            }    
            catch
            {
                return httpContext.Session[AuthSessionKey] as ShopifyAuthorizationState;
            }
        }
    }
}