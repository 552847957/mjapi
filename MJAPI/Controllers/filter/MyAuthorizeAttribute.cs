using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MJAPI.Controllers.filter
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            //控制权限
            //return base.AuthorizeCore(httpContext);
            return DateTime.Now.Minute % 2 == 0;
        }

        /// <summary>
        /// 没有权限操作
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            filterContext.HttpContext.Response.ContentType = "application/json";
            filterContext.HttpContext.Response.Write("{\"errorcode\":500,\"msg\":\"Insufficient authority\"}");
            filterContext.HttpContext.Response.End();
        }
    }
}