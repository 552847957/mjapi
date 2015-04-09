using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MJAPI.Controllers
{
    public class CustomExceptionAttribute : FilterAttribute, IExceptionFilter   //HandleErrorAttribute
    {

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled == true)
            {
                HttpException httpExce = filterContext.Exception as HttpException;
                if (httpExce.GetHttpCode() != 500)//为什么要特别强调500 因为MVC处理HttpException的时候，如果为500 则会自动
                //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                {
                    return;
                }
            }
            HttpException httpException = filterContext.Exception as HttpException;
            if (httpException != null)
            {
                filterContext.Controller.ViewBag.UrlRefer = filterContext.HttpContext.Request.UrlReferrer;
                if (httpException.GetHttpCode() == 404)
                {
                    filterContext.HttpContext.Response.Redirect("~/home/index");
                }
                else if (httpException.GetHttpCode() == 500)
                {



                    System.IO.File.AppendAllText(filterContext.HttpContext.Server.MapPath("") + "log.txt", httpException.Message + ":" + httpException.Source +  ":"+(string)filterContext.RouteData.Values["action"]+":" + DateTime.Now.ToSafeString() + "\r\n\r\n");
                    // return "{\"success\":\"false\",\"msg\":\"userid不能为空\"}"; ;
                    filterContext.HttpContext.Response.Write("{\"success\":\"false\",\"msg\":\"异常错误，已捕捉\"}");
                    filterContext.HttpContext.Response.End();
                   
                }
            }
            //写入日志 记录
            filterContext.ExceptionHandled = true;//设置异常已经处理
        }
    }
}