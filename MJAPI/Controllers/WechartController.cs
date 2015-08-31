using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using tenpay;

namespace MJAPI.Controllers
{
    public class WechartController : Controller
    {
        //
        // GET: /Wechart/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 微信登录授权
        /// </summary>
        /// <returns></returns>
        public string Login()
        {
            //http://mobile.mj100.com/Wechart/LoginOk
            //https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2c2f2e7b5b62daa1&redirect_uri=&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect
            //appid	是	公众号的唯一标识
            //redirect_uri	是	授权后重定向的回调链接地址，请使用urlencode对链接进行处理
            //response_type	是	返回类型，请填写code
            //scope	是	应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）
            //state	否	重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值，最多128字节
            //#wechat_redirect	是	无论直接打开还是做页面302重定向时候，必须带此参数



            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2c2f2e7b5b62daa1&redirect_uri=http://mobile.mj100.com/mjpay/index?showwxpaytitle=1&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect");





            return "";
        }
        /// <summary>
        /// 微信登录授权
        /// </summary>
        /// <returns></returns>
        public string Login2()
        {
            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2c2f2e7b5b62daa1&redirect_uri=http://mobile.mj100.com/App/Stepone?response_type=code&scope=snsapi_base&state=STATE#wechat_redirect");
           return "";
        }

        /// <summary>
        /// 微信登录授权
        /// </summary>
        /// <returns></returns>
        public string Login5()
        {
            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2c2f2e7b5b62daa1&redirect_uri=http://mobile.mj100.com/App/index?response_type=code&scope=snsapi_base&state=STATE#wechat_redirect");
            return "";
        }

        public string Login6()//LuckDraw
        {
            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2c2f2e7b5b62daa1&redirect_uri=http://mobile.mj100.com/App/LuckDraw?response_type=code&scope=snsapi_base&state=STATE#wechat_redirect");
            return "";
        }

        public string Login7(string id)
        {
            if (id.IsEmpty())
            {
                id = "dpzc";
            }
            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2c2f2e7b5b62daa1&redirect_uri=http://mobile.mj100.com/App/Bargain?response_type=code&scope=snsapi_base&state=" + id + "#wechat_redirect");
            return "";
        }


        /// <summary>
        /// 微信登录授权
        /// </summary>
        /// <returns></returns>
        public string Login3()
        {
            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2c2f2e7b5b62daa1&redirect_uri=http://mobile.mj100.com/Wechart/Login4?response_type=code&scope=snsapi_base&state=STATE#wechat_redirect");
            return "";
        }

        public string Login4(string code)
        {
            #region 登录成功缓存用户信息
            string post_data = "appid=" + "wx2c2f2e7b5b62daa1" + "&secret=" + "ed815afc669a9201a6070677d1771166" + "&code=" + code + "&grant_type=authorization_code";
            string requestData = tenpay.TenpayUtil.PostXmlToUrl(tenpay.TenpayUtil.getAccess_tokenUrl(), post_data);
            JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            JsApi.WeChartUser auth = js.Deserialize<JsApi.WeChartUser>(requestData);    //将json数据转化为对象类型并赋值给auth


            #endregion


            return auth.openid;
        }
        /// <summary>
        /// 授权回调
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOk(string code, string state)
        {
            string post_data = "appid=" + "wx2c2f2e7b5b62daa1" + "&secret=" + "ed815afc669a9201a6070677d1771166" + "&code=" + code + "&grant_type=authorization_code";
            string requestData = tenpay.TenpayUtil.PostXmlToUrl(tenpay.TenpayUtil.getAccess_tokenUrl(), post_data);
            JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            authorization auth = js.Deserialize<authorization>(requestData);    //将json数据转化为对象类型并赋值给auth

            Session["auth"] = auth;//标识用户登录



            UnifiedOrder order = new UnifiedOrder();
            order.appid = "wx2c2f2e7b5b62daa1";
            order.attach = "vinson";
            order.body =  "100拍币";
            order.device_info = "";
            order.mch_id = "1246407101";
            order.nonce_str = TenpayUtil.getNoncestr();
            order.notify_url = "http://mobile.mj100.com";//回调网址
            order.openid = auth.openid;
            order.out_trade_no = "2015666697854232";//订单号
            order.trade_type = "JSAPI";
            order.spbill_create_ip = Request.UserHostAddress;
            order.total_fee =  100;
            //order.total_fee = 1;

            string prepay_id = tenpay.TenpayUtil.getPrepay_id(order, "43804496F28A4F0FBF1195AA0F1Abcde");

            string timeStamp = TenpayUtil.getTimestamp();


            string  nonceStr = TenpayUtil.getNoncestr();

            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appId", "wx2c2f2e7b5b62daa1");
            sParams.Add("timeStamp", timeStamp);
            sParams.Add("nonceStr", nonceStr);
            sParams.Add("package", "prepay_id=" + prepay_id);
            sParams.Add("signType", "MD5");


            string paySign = TenpayUtil.getsign(sParams, "43804496F28A4F0FBF1195AA0F1Abcde");


            //var appId = "<%=appId %>";
            //var timeStamp = "<%=timeStamp %>";
            //var nonceStr = "<%=nonceStr %>";
            //var prepay_id = "<%=prepay_id %>";
            //var paySign = "<%=paySign %>";

            ViewBag.appId = "wx2c2f2e7b5b62daa1";
            ViewBag.timeStamp = timeStamp;
            ViewBag.nonceStr = nonceStr;

            ViewBag.prepay_id = prepay_id;

            ViewBag.paySign = paySign;



            ViewBag.openid = paySign+"我是paySign";



            return View();
        }
        /// <summary>
        /// 微信用户实体
        /// </summary>
        public class authorization
        {
            public string access_token { get; set; }  //属性的名字，必须与json格式字符串中的"key"值一样。
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
        }



    }
}
         