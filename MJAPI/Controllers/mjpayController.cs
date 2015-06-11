using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using tenpay;

namespace MJAPI.Controllers
{
    public class mjpayController : Controller
    {
        //
        // GET: /mjpay/

        public ActionResult Index(string code)
        {
            #region 登录成功缓存用户信息
            string post_data = "appid=" + "wx2c2f2e7b5b62daa1" + "&secret=" + "ed815afc669a9201a6070677d1771166" + "&code=" + code + "&grant_type=authorization_code";
            string requestData = tenpay.TenpayUtil.PostXmlToUrl(tenpay.TenpayUtil.getAccess_tokenUrl(), post_data);
            JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            MJAPI.Controllers.WechartController.authorization auth = js.Deserialize<MJAPI.Controllers.WechartController.authorization>(requestData);    //将json数据转化为对象类型并赋值给auth
            Session["auth"] = auth;//标识用户登录 
            #endregion
            return View();
        }



        /// <summary>
        /// 实际支付页
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Xd(string code, string state)
        {

            //   Session["auth"] = auth;//标识用户登录 
            MJAPI.Controllers.WechartController.authorization auth = null;

            if (Session["auth"] != null)
            {
                auth = Session["auth"] as MJAPI.Controllers.WechartController.authorization;
            }
            else
            {
                Response.Redirect("http://www.mj100.com/wechart/login");
            }


            #region 发送预支付单
            UnifiedOrder order = new UnifiedOrder();
            order.appid = "wx2c2f2e7b5b62daa1";
            order.attach = "vinson1";
            order.body = "极客美家支付正式测试";//订单描述
            order.device_info = "";
            order.mch_id = "1246407101";
            order.nonce_str = TenpayUtil.getNoncestr();//随机字符串
            order.notify_url = "http://mobile.mj100.com/test/h?id=100";//回调网址
            order.openid = auth.openid;
            order.out_trade_no = "20156666978542323" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();//订单号
            order.trade_type = "JSAPI";
            order.spbill_create_ip = Request.UserHostAddress;
            order.total_fee = 1;
            //order.total_fee = 1;
            string prepay_id = tenpay.TenpayUtil.getPrepay_id(order, "43804496F28A4F0FBF1195AA0F1Abcde");//商户key
            #endregion



            #region 得到paySign
            string timeStamp = TenpayUtil.getTimestamp();
            string nonceStr = TenpayUtil.getNoncestr().ToUpper();
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appId", "wx2c2f2e7b5b62daa1");

            sParams.Add("timeStamp", timeStamp);

            sParams.Add("nonceStr", nonceStr);

            sParams.Add("package", "prepay_id=" + prepay_id);

            sParams.Add("signType", "MD5");

            string paySign = TenpayUtil.getsign(sParams, "43804496F28A4F0FBF1195AA0F1Abcde");

            #endregion





            ViewBag.appId = "wx2c2f2e7b5b62daa1";
            ViewBag.timeStamp = timeStamp;
            ViewBag.nonceStr = nonceStr;
            ViewBag.prepay_id = prepay_id;
            ViewBag.paySign = paySign;
            ViewBag.openid = paySign + "我是paySign";
            return View();
        }


        /// <summary>
        /// NATIVE支付
        /// </summary>
        /// <returns></returns>
        public string Code()
        {

            //根据什么生成订单号   价格  商品名称  附加信息

            UnifiedOrder order = new UnifiedOrder();
            order.appid = "wx2c2f2e7b5b62daa1";
            order.attach = "vinson1";
            order.body = "极客美家NATIVE支付正式测试";//订单描述
            order.device_info = "";
            order.mch_id = "1246407101";
            order.nonce_str = TenpayUtil.getNoncestr();//随机字符串
            order.notify_url = "http://mobile.mj100.com/test/h?id=100";//回调网址

            order.out_trade_no = "20156666978542323" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();//订单号
            order.trade_type = "NATIVE";
            order.spbill_create_ip = Request.UserHostAddress;
            order.total_fee = 1;
            //order.total_fee = 1;
            string Code_url = tenpay.TenpayUtil.getCode_url(order, "43804496F28A4F0FBF1195AA0F1Abcde");//商户key

            return Code_url;

        }


    }
}
