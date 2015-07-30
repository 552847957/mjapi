using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace tenpay
{
    public class TenpayUtil
    {
        /// <summary>
        /// 统一支付接口
        /// </summary>
        const string UnifiedPayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 网页授权接口
        /// </summary>
        const string access_tokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token";

        /// <summary>
        /// 微信订单查询接口
        /// </summary>
        const string OrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        /// <summary>
        /// 随机串
        /// </summary>
        public static string getNoncestr()
        {
            Random random = new Random();
            return MD5Util.GetMD5(random.Next(1000).ToString(), "GBK").ToLower().Replace("s", "S");
        }

        /// <summary>
        /// 时间截，自1970年以来的秒数
        /// </summary>
        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 网页授权接口
        /// </summary>
        public static string getAccess_tokenUrl()
        {
            return access_tokenUrl;
        }

        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="sParams"></param>
        /// <returns></returns>
        public static string getsign(SortedDictionary<string, string> sParams, string key)
        {
            int i = 0;
            string sign = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sParams)
            {
                if (temp.Value == "" || temp.Value == null || temp.Key.ToLower() == "sign")
                {
                    continue;
                }
                i++;
                sb.Append(temp.Key.Trim() + "=" + temp.Value.Trim() + "&");
            }
            sb.Append("key=" + key.Trim() + "");
            string signkey = sb.ToString();
            sign = MD5Util.GetMD5(signkey, "utf-8");


            return sign;
        }
        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="sParams"></param>
        /// <returns></returns>
        public static string getsignext(SortedDictionary<string, string> sParams)
        {
            int i = 0;
            string sign = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sParams)
            {
                if (temp.Value == "" || temp.Value == null || temp.Key.ToLower() == "sign")
                {
                    continue;
                }
                i++;
                sb.Append(temp.Key.Trim() + "=" + temp.Value.Trim() + "&");
            }

            string signkey = sb.ToString();
            sign = MD5Util.GetMD5(signkey, "utf-8");


            return sign;
        }
        /// <summary>
        /// post数据到指定接口并返回数据
        /// </summary>
        public static string PostXmlToUrl(string url, string postData)
        {
            string returnmsg = "";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                
                returnmsg = wc.UploadString(url, "POST", postData);
            }
            return returnmsg;
        }

        /// <summary>
        /// post数据到指定接口并返回数据
        /// </summary>
        public static string PostXmlToUrl1(string url, string postData)
        {
            string returnmsg = "";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;//定义对象语言

                Byte[] pageData = wc.UploadData(url, "POST", Encoding.UTF8.GetBytes(postData));


                string rr = Encoding.GetEncoding("utf-8").GetString(pageData);
                returnmsg = rr;
            }
            return returnmsg;
        }
        /// <summary>
        /// 带证书的post
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        protected static string PostPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...  
            try
            {
                //CerPath证书路径
                string certPath = @"D:\cert\apiclient_cert.p12";
                //证书密码
                string password = "1246407101";
                X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet);

                // 设置参数  
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                request.ClientCertificates.Add(cert);
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                string err = string.Empty;


                return content;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取prepay_id
        /// </summary>
        public static string getPrepay_id(UnifiedOrder order, string key)
        {
            string prepay_id = "";
            string post_data = getUnifiedOrderXml(order, key);
            string request_data = PostXmlToUrl(UnifiedPayUrl, post_data);




            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                if (k.Key == "prepay_id")
                {
                    prepay_id = k.Value;
                    break;
                }
            }
            return prepay_id;
        }



        /// <summary>
        /// 获取code_url  扫二维码支付使用
        /// </summary>
        public static string getCode_url(UnifiedOrder order, string key)
        {
            string code_url = "";
            string post_data = getUnifiedOrderXml(order, key);
            string request_data = PostXmlToUrl(UnifiedPayUrl, post_data);




            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                if (k.Key == "code_url")
                {
                    code_url = k.Value;
                    break;
                }
            }
            return code_url;
        }

        /// <summary>
        /// 获取微信订单明细
        /// </summary>
        public OrderDetail getOrderDetail(QueryOrder queryorder, string key)
        {
            string post_data = getQueryOrderXml(queryorder, key);
            string request_data = PostXmlToUrl(OrderQueryUrl, post_data);
            OrderDetail orderdetail = new OrderDetail();
            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                switch (k.Key)
                {
                    case "retuen_code":
                        orderdetail.result_code = k.Value;
                        break;
                    case "return_msg":
                        orderdetail.return_msg = k.Value;
                        break;
                    case "appid":
                        orderdetail.appid = k.Value;
                        break;
                    case "mch_id":
                        orderdetail.mch_id = k.Value;
                        break;
                    case "nonce_str":
                        orderdetail.nonce_str = k.Value;
                        break;
                    case "sign":
                        orderdetail.sign = k.Value;
                        break;
                    case "result_code":
                        orderdetail.result_code = k.Value;
                        break;
                    case "err_code":
                        orderdetail.err_code = k.Value;
                        break;
                    case "err_code_des":
                        orderdetail.err_code_des = k.Value;
                        break;
                    case "trade_state":
                        orderdetail.trade_state = k.Value;
                        break;
                    case "device_info":
                        orderdetail.device_info = k.Value;
                        break;
                    case "openid":
                        orderdetail.openid = k.Value;
                        break;
                    case "is_subscribe":
                        orderdetail.is_subscribe = k.Value;
                        break;
                    case "trade_type":
                        orderdetail.trade_type = k.Value;
                        break;
                    case "bank_type":
                        orderdetail.bank_type = k.Value;
                        break;
                    case "total_fee":
                        orderdetail.total_fee = k.Value;
                        break;
                    case "coupon_fee":
                        orderdetail.coupon_fee = k.Value;
                        break;
                    case "fee_type":
                        orderdetail.fee_type = k.Value;
                        break;
                    case "transaction_id":
                        orderdetail.transaction_id = k.Value;
                        break;
                    case "out_trade_no":
                        orderdetail.out_trade_no = k.Value;
                        break;
                    case "attach":
                        orderdetail.attach = k.Value;
                        break;
                    case "time_end":
                        orderdetail.time_end = k.Value;
                        break;
                    default:
                        break;
                }
            }
            return orderdetail;
        }

        /// <summary>
        /// 把XML数据转换为SortedDictionary<string, string>集合
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetInfoFromXml(string xmlstring)
        {
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstring);
                XmlElement root = doc.DocumentElement;
                int len = root.ChildNodes.Count;
                for (int i = 0; i < len; i++)
                {
                    string name = root.ChildNodes[i].Name;
                    if (!sParams.ContainsKey(name))
                    {
                        sParams.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                    }
                }
            }
            catch { }
            return sParams;
        }

        /// <summary>
        /// 微信统一下单接口xml参数整理
        /// </summary>
        /// <param name="order">微信支付参数实例</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        protected static string getUnifiedOrderXml(UnifiedOrder order, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", order.appid);
            sParams.Add("attach", order.attach);
            sParams.Add("body", order.body);
            sParams.Add("device_info", order.device_info);
            sParams.Add("mch_id", order.mch_id);
            sParams.Add("nonce_str", order.nonce_str);
            sParams.Add("notify_url", order.notify_url);
            sParams.Add("openid", order.openid);
            sParams.Add("out_trade_no", order.out_trade_no);
            sParams.Add("spbill_create_ip", order.spbill_create_ip);
            sParams.Add("total_fee", order.total_fee.ToString());
            sParams.Add("trade_type", order.trade_type);
            order.sign = getsign(sParams, key);
            sParams.Add("sign", order.sign);


            //    <xml>
            //    <sign></sign>
            //    <mch_billno></mch_billno>
            //    <mch_id></mch_id>
            //    <wxappid></wxappid>
            //    <nick_name></nick_name>
            //    <send_name></send_name>
            //    <re_openid></re_openid>
            //    <total_amount></total_amount>
            //    <min_value></min_value>
            //    <max_value></max_value>
            //    <total_num></total_num>
            //    <wishing></wishing>
            //    <client_ip></client_ip>
            //    <act_name></act_name>
            //    <act_id></act_id>
            //    <remark></remark>
            //    <logo_imgurl></logo_imgurl>
            //    <share_content></share_content>       
            //</xml>

            //拼接成XML请求数据
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
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
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString());
            byte[] byteArray = Encoding.UTF8.GetBytes(return_string);
            return_string = Encoding.GetEncoding("GBK").GetString(byteArray);
            return return_string;

        }

        /// <summary>
        /// 发红包xml字符串整理
        /// </summary>
        /// <param name="redpacket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected static string getRedPacketXml(RedPacket redpacket, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("nonce_str", redpacket.nonce_str);
            sParams.Add("mch_billno", redpacket.mch_billno);
            sParams.Add("mch_id", redpacket.mch_id);
            sParams.Add("wxappid", redpacket.wxappid);
            sParams.Add("nick_name", redpacket.nick_name);
            sParams.Add("send_name", redpacket.send_name);
            sParams.Add("re_openid", redpacket.re_openid);
            sParams.Add("total_amount", redpacket.total_amount);
            sParams.Add("min_value", redpacket.min_value);
            sParams.Add("max_value", redpacket.max_value);
            sParams.Add("total_num", redpacket.total_num);
            sParams.Add("wishing", redpacket.wishing);
            sParams.Add("client_ip", redpacket.client_ip);
            sParams.Add("act_name", redpacket.act_name);
            sParams.Add("remark", redpacket.remark);
            sParams.Add("logo_imgurl", redpacket.logo_imgurl);
            sParams.Add("share_content", redpacket.share_content);
            sParams.Add("share_url", redpacket.share_url);
            sParams.Add("share_imgurl", redpacket.share_imgurl);


            redpacket.sign = getsign(sParams, key);
            sParams.Add("sign", redpacket.sign);

            #region 准备xml
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
            {
                if (k.Value == "" || k.Value == null)
                {
                    continue;
                }

                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sbPay.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString());
            //byte[] byteArray = Encoding.UTF8.GetBytes(return_string);
            //return_string = Encoding.GetEncoding("GBK").GetString(byteArray);
            #endregion
            return return_string;

        }


        /// <summary>
        /// 发红包
        /// </summary>
        /// <returns></returns>
        public static string Fhb(RedPacket redpacket, string key)
        {

            string returnstr = PostPage("https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack", getRedPacketXml(redpacket, key));
            return returnstr;
        }

        /// <summary>
        /// 微信订单查询接口XML参数整理
        /// </summary>
        /// <param name="queryorder">微信订单查询参数实例</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        protected string getQueryOrderXml(QueryOrder queryorder, string key)
        {
            string return_string = string.Empty;
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", queryorder.appid);
            sParams.Add("mch_id", queryorder.mch_id);
            sParams.Add("transaction_id", queryorder.transaction_id);
            sParams.Add("out_trade_no", queryorder.out_trade_no);
            sParams.Add("nonce_str", queryorder.nonce_str);
            queryorder.sign = getsign(sParams, key);
            sParams.Add("sign", queryorder.sign);

            //拼接成XML请求数据
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
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
            return_string = string.Format("<xml>{0}</xml>", sbPay.ToString().TrimEnd(','));
            return return_string;
        }
    }
}