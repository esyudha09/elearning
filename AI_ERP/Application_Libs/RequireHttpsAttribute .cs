using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Net.Http.Handlers;

namespace AI_ERP.Application_Libs
{
    public class RequireHttpsAttribute
    {
    }
    //public class RequireHttpsAttribute : AuthorizationFilterAttribute
    //{
    //    public override void OnAuthorization(HttpActionContext actionContext)
    //    {
    //        if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
    //        {
    //            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
    //            {
    //                ReasonPhrase = "HTTPS Required"
    //            };
    //        }
    //        else
    //        {
    //            base.OnAuthorization(actionContext);
    //        }
    //    }
    //}
}