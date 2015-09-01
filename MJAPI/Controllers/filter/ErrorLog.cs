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
            string code = filterContext.HttpContext.Response.Status;

            if (filterContext.ExceptionHandled == true)
            {
                HttpException httpExce = filterContext.Exception as HttpException;
                if (httpExce.GetHttpCode() != 500)//为什么要特别强调500 因为MVC处理HttpException的时候，如果为500 则会自动
                //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                {
                    return;
                }
            }

            System.IO.File.AppendAllText(filterContext.HttpContext.Server.MapPath("") + "log.txt", filterContext.Exception + ":" + filterContext.Controller + ":" + (string)filterContext.RouteData.Values["action"] + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");
            // return "{\"success\":\"false\",\"msg\":\"userid不能为空\"}"; ;
            filterContext.HttpContext.Response.Write("{\"success\":\"false\",\"msg\":\"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\"}");
            filterContext.HttpContext.Response.End();

            //写入日志 记录
            filterContext.ExceptionHandled = true;//设置异常已经处理
        }




    }

    public class DesingerExceptionAttribute : FilterAttribute, IExceptionFilter   //HandleErrorAttribute
    {

        public void OnException(ExceptionContext filterContext)
        {
            string code = filterContext.HttpContext.Response.Status;

            if (filterContext.ExceptionHandled == true)
            {
                HttpException httpExce = filterContext.Exception as HttpException;
                if (httpExce.GetHttpCode() != 500)//为什么要特别强调500 因为MVC处理HttpException的时候，如果为500 则会自动
                //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                {
                    return;
                }
            }

            //System.IO.File.AppendAllText(filterContext.HttpContext.Server.MapPath("desinger") + "log.txt", filterContext.Exception + ":" + filterContext.Controller + ":" + (string)filterContext.RouteData.Values["action"] + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");
            // return "{\"success\":\"false\",\"msg\":\"userid不能为空\"}"; ;
            filterContext.HttpContext.Response.Write("{\"errcode\":1,\"msg\":\"" + filterContext.Exception.Message + "\"}");
            filterContext.HttpContext.Response.End();

            //写入日志 记录
            filterContext.ExceptionHandled = true;//设置异常已经处理
        }




    }



    public class DesingerPlatformExceptionAttribute : FilterAttribute, IExceptionFilter   //HandleErrorAttribute
    {

        public void OnException(ExceptionContext filterContext)
        {
            string code = filterContext.HttpContext.Response.Status;

            if (filterContext.ExceptionHandled == true)
            {
                HttpException httpExce = filterContext.Exception as HttpException;
                if (httpExce.GetHttpCode() != 500)//为什么要特别强调500 因为MVC处理HttpException的时候，如果为500 则会自动
                //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                {
                    return;
                }
            }

            //System.IO.File.AppendAllText(filterContext.HttpContext.Server.MapPath("desinger") + "log.txt", filterContext.Exception + ":" + filterContext.Controller + ":" + (string)filterContext.RouteData.Values["action"] + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");
            // return "{\"success\":\"false\",\"msg\":\"userid不能为空\"}"; ;
            filterContext.HttpContext.Response.Write("{\"errcode\":1,\"msg\":\"" + filterContext.Exception.Message + "\"}");
            filterContext.HttpContext.Response.End();

            //写入日志 记录
            filterContext.ExceptionHandled = true;//设置异常已经处理
        }




    }



    public class YzAttribute : ActionFilterAttribute
    {
      


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //No Parameters, will return directly.
            
                string o = filterContext.HttpContext.Request["token"].ToSafeString();

                if (o.IsEmpty())
                {
                    filterContext.HttpContext.Response.Write("Are You Kidding me ?");

                    filterContext.HttpContext.Response.End();
                }else if(o!="3eb582a646500738")
                {
                    filterContext.HttpContext.Response.Write("Are You Kidding me ?");

                    filterContext.HttpContext.Response.End();
                }
            
               

             


        }
    }
}
