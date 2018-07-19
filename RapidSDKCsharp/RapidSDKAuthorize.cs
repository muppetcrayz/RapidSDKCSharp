using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RapidSDKCSharp
{
   
    public sealed class RapidSDKAuthorize : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {

          if(  (HttpContext.Current.Session["Used_id"] != null && HttpContext.Current.Session["session_id"] != null))
            {
                return;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "CRUD" }, { "action", "Login" } });
            }
        }
    }
}