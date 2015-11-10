using Commen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MJAPI.Controllers
{
    public class wxController : Controller
    {
        //
        // GET: /wx/
        /// <summary>
        /// 新微信公众号处理请求
        /// </summary>
        /// <returns></returns>
        public string Index()
        {
            #region   处理请求
            if (Request.HttpMethod.ToUpper() == "GET")//GET
            {

                // 微信加密签名  
                string signature = Request.QueryString["signature"];
                // 时间戳  
                string timestamp = Request.QueryString["timestamp"];
                // 随机数  
                string nonce = Request.QueryString["nonce"];
                // 随机字符串  
                string echostr = Request.QueryString["echostr"];
                if (WeixinServer.CheckSignature(signature, timestamp, nonce))
                {
                    return echostr;
                }


            }
            else if (Request.HttpMethod.ToUpper() == "POST")
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                string postStr = Encoding.UTF8.GetString(b);

                //System.IO.File.AppendAllText(Context.Server.MapPath("img") + "log4.txt", postStr + "\r\n\r\n");

                return new  WeiXinProcessing.WeiXinProcess().processRequest(postStr);

            }
            #endregion
            return "";
        }

        /// <summary>
        /// 得到新微信号的Accesstoken
        /// </summary>
        /// <returns></returns>
        public string GetAccesstoken()
        {

            return Commen.WeixinServer.Get_Access_token();
        
        }



    }
}
