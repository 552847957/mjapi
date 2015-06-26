using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JsApi
{
    public class JsToken
    {
        //{
        //"errcode":0,
        //"errmsg":"ok",
        //"ticket":"bxLdikRXVbTPdHSM05e5u5sUoXNKd8-41ZO3MhKoyN5OfkWITDGgnr2fwJ0m9E8NYzWKVZvdVtaUgWvsdshFKA",
        //"expires_in":7200
        //}




        /// <summary>
        /// 得到Apptoken
        /// </summary>
        /// <returns></returns>
        public static string GetApptoken()
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            return wc.DownloadString("http://img.mj100.com/weixin/GetToken.aspx");
        }

        /// <summary>
        /// 得到jsapi_ticket  2个小时过期策略
        /// </summary>
        /// <returns></returns>
        public static string Getjsapi_ticket()
        {
            object o = Commen.DataCache.GetCache("jstoken");

            if (o != null)
            {
                return o.ToSafeString();
            }
            else
            {
                string ACCESS_TOKEN = GetApptoken();
                System.Net.WebClient wc = new System.Net.WebClient();
                string json = wc.DownloadString("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + ACCESS_TOKEN + "&type=jsapi");

                Regex re = new Regex("\"ticket\":\"([-\\w]+)\"", RegexOptions.Singleline);
                Match m = re.Match(json);
                Commen.DataCache.SetCache("jstoken", m.Groups[1].Value, DateTime.Now.AddMinutes(110), TimeSpan.Zero);
                return m.Groups[1].Value;
            }
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
        /// 随机串
        /// </summary>
        public static string getNoncestr()
        {
            Random random = new Random();
            return HashUtil.GetMD5(random.Next(1000).ToString(), "GBK").ToLower().Replace("s", "S");
        }

        /// <summary>
        /// 获取微信jsapi_ticket签名
        /// </summary>
        /// <param name="sParams"></param>
        /// <returns></returns>
        public static string Getsignext(SortedDictionary<string, string> sParams)
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

            string signkey = sb.ToString().TrimEnd('&');



            sign = HashUtil.SHA1_Hash(signkey);

          


            return sign;

        }
    }
}
