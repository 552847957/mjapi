using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MJAPI.Controllers
{
    public class DesingerController : Controller
    {

     
        //
        // GET: /Desinger/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Register(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "0";
            }
            ViewBag.id = id;


            string timestamp = JsApi.JsToken.getTimestamp();
            string noncestr = JsApi.JsToken.getNoncestr();

            SortedDictionary<string, string> sor = new SortedDictionary<string, string>();
            sor.Add("url", Request.Url.ToString());
            sor.Add("timestamp", timestamp);
            sor.Add("noncestr", noncestr);
            sor.Add("jsapi_ticket", JsApi.JsToken.Getjsapi_ticket());

            ViewBag.jsapi_ticket = JsApi.JsToken.Getjsapi_ticket();
            ViewBag.url = Request.Url.ToString();
            ViewBag.appid = tenpay.WeChartConfigItem.appid;
            ViewBag.timestamp = timestamp;
            ViewBag.noncestr = noncestr;
            ViewBag.signature = JsApi.JsToken.Getsignext(sor);

            ViewBag.IsMoblie = IsMoblie();

            return View();
        }

        //phone: phone, city: city, yzm: yzm, fromid
        public string AddDesinger(string phone, string city, string yzm, string fromid)
        {

            if (Commen.DataCache.GetCache("desinger"+phone)==null)
            {
                return "{\"errorcode\":\"1\",\"msg\":\"验证码错误\"}";
            }
            if (Commen.DataCache.GetCache("desinger"+phone).ToSafeString()!=yzm)
            {
                 return "{\"errorcode\":\"1\",\"msg\":\"验证码错误\"}";
            }

            DesingerBLL.Desinger bll = new DesingerBLL.Desinger();

            bll.Adddesinger( phone, city, yzm,  fromid);

            return "{\"errorcode\":\"0\",\"msg\":\"注册成功\"}";
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendCode(string phone)
        {

            DesingerBLL.Desinger bll = new DesingerBLL.Desinger();

            return bll.SendMsg(phone);


        }

        public bool IsMoblie()
        {

            string agent = (Request.UserAgent + "").ToLower().Trim();
            if (agent == "" ||

            agent.IndexOf("mobile") != -1 ||

            agent.IndexOf("mobi") != -1 ||

            agent.IndexOf("nokia") != -1 ||

            agent.IndexOf("samsung") != -1 ||

            agent.IndexOf("sonyericsson") != -1 ||

            agent.IndexOf("mot") != -1 ||

            agent.IndexOf("blackberry") != -1 ||

            agent.IndexOf("lg") != -1 ||

            agent.IndexOf("htc") != -1 ||

            agent.IndexOf("j2me") != -1 ||

            agent.IndexOf("ucweb") != -1 ||

            agent.IndexOf("opera mini") != -1 ||

            agent.IndexOf("mobi") != -1 ||

            agent.IndexOf("android") != -1 ||

            agent.IndexOf("iphone") != -1)
            {

                //终端可能是手机
                return true;
            }
            return false;

        }




    }
}
