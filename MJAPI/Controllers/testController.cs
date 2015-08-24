﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using tenpay;

namespace MJAPI.Controllers
{
    public class testController : Controller
    {
        //
        // GET: /test/

        public ActionResult Index(string code, string state)
        {
            #region 登录成功缓存用户信息
            string post_data = "appid=" + "wx2c2f2e7b5b62daa1" + "&secret=" + "ed815afc669a9201a6070677d1771166" + "&code=" + code + "&grant_type=authorization_code";
            string requestData = tenpay.TenpayUtil.PostXmlToUrl(tenpay.TenpayUtil.getAccess_tokenUrl(), post_data);
            JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            MJAPI.Controllers.WechartController.authorization auth = js.Deserialize<MJAPI.Controllers.WechartController.authorization>(requestData);    //将json数据转化为对象类型并赋值给auth
            Session["auth"] = auth;//标识用户登录 
            #endregion



            #region MyRegion
            //#region 发送预支付单
            //UnifiedOrder order = new UnifiedOrder();
            //order.appid = "wx2c2f2e7b5b62daa1";
            //order.attach = "vinson1";
            //order.body = "极客美家支付测试";//订单描述
            //order.device_info = "";
            //order.mch_id = "1246407101";
            //order.nonce_str = TenpayUtil.getNoncestr();//随机字符串
            //order.notify_url = "http://mobile.mj100.com/test/h?id=100";//回调网址
            //order.openid = auth.openid;
            //order.out_trade_no = "20156666978542323" + DateTime.Now.Minute.ToString();//订单号
            //order.trade_type = "JSAPI";
            //order.spbill_create_ip = Request.UserHostAddress;
            //order.total_fee = 1;
            ////order.total_fee = 1;
            //string prepay_id = tenpay.TenpayUtil.getPrepay_id(order, "43804496F28A4F0FBF1195AA0F1Abcde");//商户key
            //#endregion



            //#region 得到paySign
            //string timeStamp = TenpayUtil.getTimestamp();
            //string nonceStr = TenpayUtil.getNoncestr().ToUpper();
            //SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            //sParams.Add("appId", "wx2c2f2e7b5b62daa1");

            //sParams.Add("timeStamp", timeStamp);

            //sParams.Add("nonceStr", nonceStr);

            //sParams.Add("package", "prepay_id=" + prepay_id);

            //sParams.Add("signType", "MD5");

            //string paySign = TenpayUtil.getsign(sParams, "43804496F28A4F0FBF1195AA0F1Abcde");

            //#endregion





            //ViewBag.appId = "wx2c2f2e7b5b62daa1";
            //ViewBag.timeStamp = timeStamp;
            //ViewBag.nonceStr = nonceStr;
            //ViewBag.prepay_id = prepay_id;
            //ViewBag.paySign = paySign;
            //ViewBag.openid = paySign + "我是paySign"; 
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
            order.body = "极客美家支付测试";//订单描述
            order.device_info = "";
            order.mch_id = "1246407101";
            order.nonce_str = TenpayUtil.getNoncestr();//随机字符串
            order.notify_url = "http://mobile.mj100.com/test/h?id=100";//回调网址
            order.openid = auth.openid;
            order.out_trade_no = "20156666978542323" + DateTime.Now.Minute.ToString();//订单号
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
        /// 回调链接
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string H(string code, string state)
        {

            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            string postStr = Encoding.UTF8.GetString(b);
            System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "zifu.txt", postStr + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");

            return @"<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>";


        }



        public ActionResult Indexs(string code, string state)
        {


            ViewBag.appId = "1111111111111111111111";
            return View();


        }


        public string KK()
        {

            //string   return_string = string.Format("{0}", "极客美家");
            //byte[] byteArray = Encoding.UTF8.GetBytes(return_string);
            //return_string = Encoding.GetEncoding("GBK").GetString(byteArray);
            return AppDomain.CurrentDomain.SetupInformation.ApplicationBase; ; ;
        }


        public string OO()
        {


            return Commen.HttpRequest.GetResponseString("https://api.weixin.qq.com/cgi-bin/user/info?access_token=iF4UhkMfgQHp6cKD0iWfGT7hMw0A189LzNz3T6klXdXYiqRvziknHx-3FnyebIhUO3u_0VFVlhbGIie8NYgryH480XVE35WSVh0lzS5-GGs&openid=o8r91jtiU-jL0DAQoCgMexSNuNXU&lang=zh_CN");
        
        }

    }
}
