using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string SendCode(string phone, string tcode)
        {
            if (Session["checkCode"]==null||Session["checkCode"].ToSafeString().Trim().ToLower()!=tcode.ToSafeString().Trim().ToLower())
            {
                   return "{\"errorcode\":\"3\",\"msg\":\"图文验证码错误\"}";
            }
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




        private string RndNum()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }


            return checkCode;
        }
        public ActionResult CreateCheckCodeImage()
        {
            string checkCode = RndNum();

            Session["checkCode"] = checkCode;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                 

                return File(ms.ToArray(), "image/Gif", "1.gif");
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }



    }
}
