using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
                Response.Redirect("http://mobile.mj100.com/wechart/login");
            }


            #region 发送预支付单
            UnifiedOrder order = new UnifiedOrder();
            order.appid = tenpay.WeChartConfigItem.appid;
            order.attach = "vinson1";
            order.body = "极客美家支付正式测试";//订单描述
            order.device_info = "";
            order.mch_id = tenpay.WeChartConfigItem.mch_id;
            order.nonce_str = TenpayUtil.getNoncestr();//随机字符串
            order.notify_url = "http://mobile.mj100.com/test/h?id=100";//回调网址
            order.openid = auth.openid;
            order.out_trade_no = "20156666978542323" + DateTime.Now.Day + DateTime.Now.Minute + DateTime.Now.Second;//订单号
            order.trade_type = "JSAPI";
            order.spbill_create_ip = Request.UserHostAddress;
            order.total_fee = 1;

            string prepay_id = tenpay.TenpayUtil.getPrepay_id(order, tenpay.WeChartConfigItem.key);//商户key
            #endregion



            #region 得到paySign
            string timeStamp = TenpayUtil.getTimestamp();
            string nonceStr = TenpayUtil.getNoncestr().ToUpper();
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appId", tenpay.WeChartConfigItem.appid);

            sParams.Add("timeStamp", timeStamp);

            sParams.Add("nonceStr", nonceStr);

            sParams.Add("package", "prepay_id=" + prepay_id);

            sParams.Add("signType", "MD5");

            string paySign = TenpayUtil.getsign(sParams, tenpay.WeChartConfigItem.key);

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
        /// NATIVE第二种扫码支付
        /// </summary>
        /// <returns></returns>
        public string Code()
        {

            //根据什么生成订单号   价格  商品名称  附加信息

            UnifiedOrder order = new UnifiedOrder();
            order.appid = tenpay.WeChartConfigItem.appid;
            order.attach = "vinson1";
            order.body = "极客美家NATIVE支付正式测试";//订单描述
            order.device_info = "";
            order.mch_id = tenpay.WeChartConfigItem.mch_id;
            order.nonce_str = TenpayUtil.getNoncestr();//随机字符串
            order.notify_url = "http://mobile.mj100.com/test/h?id=100";//回调网址

            order.out_trade_no = "20156666978542323" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();//订单号
            order.trade_type = "NATIVE";
            order.spbill_create_ip = Request.UserHostAddress;
            order.total_fee = 1;
            //order.total_fee = 1;
            string Code_url = tenpay.TenpayUtil.getCode_url(order, tenpay.WeChartConfigItem.key);//商户key

            return Code_url;

        }



        /// <summary>
        /// native第一种扫码支付
        /// </summary>
        /// <returns></returns>
        public string Code2()
        {
            try
            {


                #region 获取到请求的值
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                string postStr = Encoding.UTF8.GetString(b);
                #endregion




                SortedDictionary<string, string> dic = TenpayUtil.GetInfoFromXml(postStr);

                string osign = dic["sign"];//微信sign


                string sign = TenpayUtil.getsign(dic, tenpay.WeChartConfigItem.key);//自己加密后的sign



                #region 取到的各种值
                string appid = dic["appid"];
                string openid = dic["openid"];
                string mach_id = dic["mch_id"];
                string is_subscribe = dic["is_subscribe"];
                string nonce_str = dic["nonce_str"];
                string product_id = dic["product_id"];//产品id或者订单号
                #endregion

                //    System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "native.txt", product_id + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");

                #region 统一下单
                UnifiedOrder order = new UnifiedOrder();
                order.appid = tenpay.WeChartConfigItem.appid;
                order.attach = "vinson1";
                order.body = "微信扫码回调测试product_id:" + product_id;//订单描述
                order.device_info = "";
                order.mch_id = tenpay.WeChartConfigItem.mch_id;
                order.nonce_str = TenpayUtil.getNoncestr();//随机字符串
                order.notify_url = "http://mobile.mj100.com/test/h?id=100";//回调网址
                order.openid = openid;
                order.out_trade_no = "20156666978542323" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();//订单号
                order.trade_type = "NATIVE";
                order.spbill_create_ip = Request.UserHostAddress;
                order.total_fee = 1;
                //order.total_fee = 1;
                string prepay_id = tenpay.TenpayUtil.getPrepay_id(order, tenpay.WeChartConfigItem.key);//商户key 
                #endregion

                //    System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "native.txt", prepay_id + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");


                #region 响应请求
                SortedDictionary<string, string> pdic = new SortedDictionary<string, string>();
                pdic.Add("return_code", "SUCCESS");
                pdic.Add("return_msg", "");
                pdic.Add("appid", appid);
                pdic.Add("mch_id", mach_id);
                pdic.Add("nonce_str", nonce_str);
                pdic.Add("prepay_id", prepay_id);
                pdic.Add("result_code", "SUCCESS");
                pdic.Add("err_code_des", "");
                string nesign = TenpayUtil.getsign(pdic, tenpay.WeChartConfigItem.key);
                pdic.Add("sign", nesign);






                StringBuilder sbPay = new StringBuilder();
                foreach (KeyValuePair<string, string> k in pdic)
                {
                    if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                    {
                        sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                    }
                    else
                    {
                        sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                    }
                }
                string return_string = string.Format("<xml>{0}</xml>", sbPay.ToString().TrimEnd(','));
                #endregion




                return return_string;

            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "native.txt", e.Message + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");
                return "";

            }
        }



        /// <summary>
        /// native扫码支付
        /// </summary>
        /// <returns></returns>
        public string Code3()
        {


            string postStr = @"<xml><appid><![CDATA[wx2c2f2e7b5b62daa1]]></appid>
<openid><![CDATA[o8r91jjmQWUqO8zrq4rxL0QVTEYs]]></openid>
<mch_id><![CDATA[1246407101]]></mch_id>
<is_subscribe><![CDATA[Y]]></is_subscribe>
<nonce_str><![CDATA[x20MklmXIxrD7cGE]]></nonce_str>
<product_id><![CDATA[888654444]]></product_id>
<sign><![CDATA[77B6BAE35570D78DE1BA99B99CD7803B]]></sign>
</xml>
";

            System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "native.txt", postStr + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");

            SortedDictionary<string, string> dic = TenpayUtil.GetInfoFromXml(postStr);

            string osign = dic["sign"];//微信sign


            string sign = TenpayUtil.getsign(dic, "43804496F28A4F0FBF1195AA0F1Abcde");//自己加密后的sign


            #region 取到的各种值
            string appid = dic["appid"];
            string openid = dic["openid"];
            string mach_id = dic["mch_id"];
            string is_subscribe = dic["is_subscribe"];
            string nonce_str = dic["nonce_str"];
            string product_id = dic["product_id"];
            #endregion

            System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "native.txt", product_id + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");

            #region 统一下单
            UnifiedOrder order = new UnifiedOrder();
            order.appid = "wx2c2f2e7b5b62daa1";
            order.attach = "vinson1";
            order.body = "微信扫码回调测试product_id:" + product_id;//订单描述
            order.device_info = "";
            order.mch_id = "1246407101";
            order.nonce_str = TenpayUtil.getNoncestr();//随机字符串
            order.notify_url = "http://mobile.mj100.com/test/h?id=100";//回调网址
            order.openid = openid;
            order.out_trade_no = "20156666978542323" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();//订单号
            order.trade_type = "NATIVE";
            order.spbill_create_ip = Request.UserHostAddress;
            order.total_fee = 1;
            //order.total_fee = 1;
            string prepay_id = tenpay.TenpayUtil.getPrepay_id(order, "43804496F28A4F0FBF1195AA0F1Abcde");//商户key 
            #endregion

            System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "native.txt", prepay_id + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");
            #region 响应请求
            SortedDictionary<string, string> pdic = new SortedDictionary<string, string>();

            pdic.Add("return_code", "SUCCESS");
            pdic.Add("return_msg", "");
            pdic.Add("appid", appid);
            pdic.Add("mch_id", mach_id);
            pdic.Add("nonce_str", nonce_str);
            pdic.Add("prepay_id", prepay_id);
            pdic.Add("result_code", "SUCCESS");
            pdic.Add("err_code_des", "");
            string nesign = TenpayUtil.getsign(pdic, "43804496F28A4F0FBF1195AA0F1Abcde");
            pdic.Add("sign", nesign);






            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in pdic)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            string return_string = string.Format("<xml>{0}</xml>", sbPay.ToString().TrimEnd(','));
            #endregion


            System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "native.txt", return_string + ":" + DateTime.Now.ToSafeString() + "\r\n\r\n");

            return return_string;
        }


        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {


            //   weixin://wxpay/bizpayurl?sign=XXXXX&appid=XXXXX&mch_id=XXXXX&product_id=XXXXXX&time_stamp=XXXXXX&nonce_str=XXXXX



            string appid = tenpay.WeChartConfigItem.appid;

            string mch_id = tenpay.WeChartConfigItem.mch_id;

            string product_id = "888654444";

            string time_stamp = TenpayUtil.getTimestamp();

            string nonce_str = TenpayUtil.getNoncestr();


            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            dic.Add("appid", appid);
            dic.Add("mch_id", mch_id);
            dic.Add("product_id", product_id);
            dic.Add("time_stamp", time_stamp);
            dic.Add("nonce_str", nonce_str);
            string sign = TenpayUtil.getsign(dic, "43804496F28A4F0FBF1195AA0F1Abcde");
            return "weixin://wxpay/bizpayurl?sign=" + sign + "&appid=" + appid + "&mch_id=" + mch_id + "&product_id=" + product_id + "&time_stamp=" + time_stamp + "&nonce_str=" + nonce_str;
        }


        /// <summary>
        /// 发红包啊
        /// </summary>
        /// <returns></returns>
        public string FHB()
        {

            tenpay.RedPacket p = new RedPacket()
            {
                nonce_str = TenpayUtil.getNoncestr(),
                mch_billno = "201411111234567" + DateTime.Now.Hour.ToSafeString() + DateTime.Now.Minute.ToSafeString() + DateTime.Now.Second.ToSafeString(),
                mch_id = tenpay.WeChartConfigItem.mch_id,
                wxappid = tenpay.WeChartConfigItem.appid,
                nick_name = "极客美家",
                send_name = "极客美家",
                re_openid = "o8r91jjmQWUqO8zrq4rxL0QVTEYs",
                total_amount = "100",
                min_value = "100",
                max_value = "100",
                total_num = "1",
                wishing = "1",
                client_ip = "118.144.76.61",
                act_name = "1",
                remark = "1"

            };
            string s = tenpay.TenpayUtil.Fhb(p, tenpay.WeChartConfigItem.key);


            System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "红包.txt", s + DateTime.Now.ToSafeString() + "\r\n\r\n");


            return s;


            #region MyRegion
            //            return TenpayUtil.PostXmlToUrl("https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack", @"<xml>
            ////            <sign></sign>
            ////            <mch_billno></mch_billno>
            ////            <mch_id></mch_id>
            ////            <wxappid></wxappid>
            ////            <nick_name></nick_name>
            ////            <send_name></send_name>
            ////            <re_openid></re_openid>
            ////            <total_amount></total_amount>
            ////            <min_value></min_value>
            ////            <max_value></max_value>
            ////            <total_num></total_num>
            ////            <wishing></wishing>
            ////            <client_ip></client_ip>
            ////            <act_name></act_name>
            ////            <act_id></act_id>
            ////            <remark></remark>
            ////            <logo_imgurl></logo_imgurl>
            ////            <share_content></share_content>
            ////            <share_url></share_url>
            ////            <share_imgurl></share_imgurl>
            ////            <nonce_str></nonce_str>
            ////        </xml>"); ;

            //            return ProcessRequest(@"<xml>
            //            <sign></sign>
            //            <mch_billno></mch_billno>
            //            <mch_id></mch_id>
            //            <wxappid></wxappid>
            //            <nick_name></nick_name>
            //            <send_name></send_name>
            //            <re_openid></re_openid>
            //            <total_amount></total_amount>
            //            <min_value></min_value>
            //            <max_value></max_value>
            //            <total_num></total_num>
            //            <wishing></wishing>
            //            <client_ip></client_ip>
            //            <act_name></act_name>
            //            <act_id></act_id>
            //            <remark></remark>
            //            <logo_imgurl></logo_imgurl>
            //            <share_content></share_content>
            //            <share_url></share_url>
            //            <share_imgurl></share_imgurl>
            //            <nonce_str></nonce_str>
            //        </xml>"); 
            #endregion

        }


      
    }
}
