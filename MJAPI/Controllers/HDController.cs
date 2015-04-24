using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MJAPI
{
    public class HDController : Controller
    {
        //
        // GET: /HD/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendMsg(string phone)
        {
            Random r = new Random();

            string s = r.Next(100000, 999999).ToString();

            Commen.DataCache.SetCache(phone, s);

            Commen.SendMsg.FSong(phone, "你好，你的验证码是：" + s);

            return "1";
        }


        public string Add(string name, string phone, string code)
        {
            // name: name, phone: phone, code: code

            //添加操作记录

            if (name.IsEmpty() || phone.IsEmpty() || code.IsEmpty())
            {
                return "请完善输入";
            }
            else
            {
                object o = Commen.DataCache.GetCache(phone);
                if (o.ToSafeString() != code)
                {
                    return "验证码错误";
                }
                else
                {
                    BLL.Hdbll bll = new BLL.Hdbll();


                    GetResponseString("http://img.mj100.com/weixin/Senmdg2.aspx?phone=" + phone + "&name=" + name);

                    return bll.Add(name, phone, Request.UserHostAddress);
                }

            }

        }

        public string Add2(string name, string phone, string code)
        {
            // name: name, phone: phone, code: code

            //添加操作记录

            return new BLL.Hdbll().Add2(Request.UserHostAddress, "点击按钮");

        }


        public static string GetResponseString(string url)
        {
            string _StrResponse = "";
            HttpWebRequest _WebRequest = (HttpWebRequest)WebRequest.Create(url);
            _WebRequest.UserAgent = "MOZILLA/4.0 (COMPATIBLE; MSIE 7.0; WINDOWS NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            _WebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;//自动解压
            _WebRequest.Method = "GET";
            WebResponse _WebResponse = _WebRequest.GetResponse();
            StreamReader _ResponseStream = new StreamReader(_WebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("GBK"));
            _StrResponse = _ResponseStream.ReadToEnd();
            _WebResponse.Close();
            _ResponseStream.Close();
            return _StrResponse;
        }


        public string TEST()
        {

            new BLL.Hdbll().MakeAnAppointment("15136134321","大鹏测试");
            return "";
        }
    }
}
