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


        public ActionResult StoreUser(string code)
        {
            #region 登录成功缓存用户信息
            string post_data = "appid=" + "wx2c2f2e7b5b62daa1" + "&secret=" + "ed815afc669a9201a6070677d1771166" + "&code=" + code + "&grant_type=authorization_code";
            string requestData = tenpay.TenpayUtil.PostXmlToUrl(tenpay.TenpayUtil.getAccess_tokenUrl(), post_data);
            JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            JsApi.WeChartUser auth = js.Deserialize<JsApi.WeChartUser>(requestData);    //将json数据转化为对象类型并赋值给auth
            Session["WeChartUser"] = auth;//标识用户登录 

            JsApi.Businesslogic.AddWeChartUser(auth.openid);



            return RedirectToAction("Stepone");


            //是否有需求

            #endregion


        }

        public ActionResult Stepone(string code, string id)
        {

            JsApi.WeChartUser auth = new JsApi.WeChartUser();

            if (!string.IsNullOrEmpty(code))
            {
                #region 登录成功缓存用户信息
                string post_data = "appid=" + "wx2c2f2e7b5b62daa1" + "&secret=" + "ed815afc669a9201a6070677d1771166" + "&code=" + code + "&grant_type=authorization_code";
                string requestData = tenpay.TenpayUtil.PostXmlToUrl(tenpay.TenpayUtil.getAccess_tokenUrl(), post_data);
                JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
                auth = js.Deserialize<JsApi.WeChartUser>(requestData);    //将json数据转化为对象类型并赋值给auth
                Session["WeChartUser"] = auth;//标识用户登录 

                JsApi.Businesslogic.AddWeChartUser(auth.openid);


                //是否有需求

                #endregion
            }
            else
            {
                if (Session["WeChartUser"] != null)
                {
                    auth = Session["WeChartUser"] as JsApi.WeChartUser;
                }
                else
                {
                    Response.Redirect("http://mobile.mj100.com/wechart/login2");
                }
            }

            //得到需求



            JsApi.DemandShowRooms drs = JsApi.Businesslogic.GetDemandShowRoomsext(auth.openid);
            ViewBag.drs = drs;


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
            ViewBag.id = id;
            return View();
        }

        public ActionResult Steptwo(string id)
        {
            JsApi.WeChartUser auth = new JsApi.WeChartUser();
            if (Session["WeChartUser"] != null)
            {
                auth = Session["WeChartUser"] as JsApi.WeChartUser;
            }
            else
            {
                return RedirectToAction("Stepone");
            }

            ViewBag.openid = auth.openid;
            ViewBag.url = JsApi.Businesslogic.GetHxtUrl(auth.openid);
            return View();
        }

        /// <summary>
        /// 推送的设计师页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Stepthree(string id)
        {

            //设计师  户型图    用户id   查出用户户型图  用户是否预约


            JsApi.DemandShowRooms drs = JsApi.Businesslogic.GetDemandShowRooms(id);
            ViewBag.drs = drs;
         
            JsApi.DesignerGrade desinger = JsApi.Businesslogic.GetDesingerGrade(drs.Extension9);

            ViewBag.desinger = desinger;

           
        
            //用户id
            return View();
        }


        /// <summary>
        /// 设计师页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult zDesigner(string id,string userid)
        {
            //设计师id
            //设计师作品
            //个人简介
            //能力评估
            ViewBag.userid = userid;

            JsApi.DesignerGrade desinger = JsApi.Businesslogic.GetDesingerGrade(id);

            ViewBag.desinger = desinger;

            IList<JsApi.DesignWorks> list = JsApi.Businesslogic.Getdesingerworks(id);

            ViewBag.list = list;


            return View();
        }


        /// <summary>
        /// 设计师案例页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Case(string id)
        {
            //设计师id
            //设计师作品
            //个人简介
            //能力评估
            IList<JsApi.DesignerWorksRoom> list = JsApi.Businesslogic.GetDesignerWorksRoom(id);

            ViewBag.list = list;

            JsApi.DesignWorks work = JsApi.Businesslogic.Getdesingerworksext(id)[0];

            ViewBag.work = work;

            JsApi.DesignerGrade desinger = JsApi.Businesslogic.GetDesingerGrade(work.SjsId.ToSafeString());

            ViewBag.desinger = desinger;

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
        public string Upload(string serverId, string phone, string address, string openid, string Description)
        {

            //查询时间------------

            if (!JsApi.Businesslogic.NoExpire(openid))
            {
                return "{\"errcode\": 1,\"errmsg\": \"你已经成功提交过一个需求了,请耐心等待\"}";
            }


            try
            {

                JsApi.Businesslogic.AddDemand(phone, serverId, address, openid, Description);
                return "{\"errcode\": 0,\"errmsg\": \"ok\"}";


            }
            catch (Exception e)
            {
                return "{\"errcode\":2,\"errmsg\": \"" + e.Message + "\"}";
            }
        }


        /// <summary>
        /// 给用户发送提醒
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Remind(string id, string openid)
        {
            //id是userid  


            openid = JsApi.Businesslogic.GetAppid(id);

            if (!string.IsNullOrEmpty(openid))
            {
                //发送提醒
                return JsApi.Businesslogic.Remind(id, openid);
            }

            return "";
          
        }

        /// <summary>
        /// 浮层页
        /// </summary>
        /// <returns></returns>
        public ActionResult Mask(string id)
        {
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

            ViewBag.id = id;
            //ViewBag.openid = auth.openid;
            return View();
        }

        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public string In(JsApi.Ustage u)
        {
            if (Session["u"]==null)
            {
                u.serverId = Request["serverId"];
                u.img_dp = Request["img_dp"];
                if (Request["Description"] != null)
                {
                    u.Description = Request["Description"];
                }
                Session["u"] = u;
            }
            else
            {
                u = Session["u"] as JsApi.Ustage;
                u.serverId = Request["serverId"];
                u.img_dp = Request["img_dp"];
                if (Request["Description"]!=null)
                {
                    u.Description=Request["Description"];
                }
                Session["u"] = u;

                
            }
           

            return "";
        }


        public string Test(JsApi.Ustage u)
        {
            u = Session["u"] as JsApi.Ustage;

            return u.Description;
        }
    }
}
