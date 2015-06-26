using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MJAPI.Controllers
{
    public class AppController : Controller
    {
        //
        // GET: /App/

        public ActionResult Index()
        {
            //            wx.config({
            //    debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            //    appId: '', // 必填，公众号的唯一标识
            //    timestamp: , // 必填，生成签名的时间戳
            //    nonceStr: '', // 必填，生成签名的随机串
            //    signature: '',// 必填，签名，见附录1
            //    jsApiList: [] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            //});

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

            return View();
        }


        public ActionResult Stepone(string code)
        {



            #region 登录成功缓存用户信息
            string post_data = "appid=" + "wx2c2f2e7b5b62daa1" + "&secret=" + "ed815afc669a9201a6070677d1771166" + "&code=" + code + "&grant_type=authorization_code";
            string requestData = tenpay.TenpayUtil.PostXmlToUrl(tenpay.TenpayUtil.getAccess_tokenUrl(), post_data);
            JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            JsApi.WeChartUser auth = js.Deserialize<JsApi.WeChartUser>(requestData);    //将json数据转化为对象类型并赋值给auth
            Session["WeChartUser"] = auth;//标识用户登录 

            JsApi.Businesslogic.AddWeChartUser(auth.openid);


            //是否有需求

            #endregion



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
            ViewBag.openid = auth.openid;

            return View();
        }



        /// <summary>
        /// 接收保存请求
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="phone"></param>
        /// <param name="address"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string Upload(string serverId, string phone, string address, string openid)
        {

            //查询时间------------

            if (!JsApi.Businesslogic.NoExpire(openid))
            {
                 return "{\"errcode\": 1,\"errmsg\": \"你已经成功提交过一个需求了,请耐心等待\"}";
            }


            try
            {

                JsApi.Businesslogic.AddDemand(phone, serverId, address, openid);
                return "{\"errcode\": 0,\"errmsg\": \"ok\"}";


            }
            catch (Exception e)
            {
                return "{\"errcode\":2,\"errmsg\": \""+e.Message+"\"}";
            }
        }



        public string Remind(string id)
        {

            return JsApi.Businesslogic.Remind();
        }
    }
}
