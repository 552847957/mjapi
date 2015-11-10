using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Collections;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web;


namespace Commen
{
    public class WeixinServer
    {
        //   // <add key="appid" value="wx2c2f2e7b5b62daa1"/>
        // <add key="secret" value="ed815afc669a9201a6070677d1771166" />
        /// <summary>
        /// 用户自定义的token  从配置中读取
        /// </summary>
        protected static string token = "token";
        public static string appId = "wxfe7b6eadc5528854"; // "wx2c2f2e7b5b62daa1";//公众号的appIdwx2b7afab991637512
        public static string appSecret = "90469a5b65a4f53b1c3104c0fa2f114f "; //公众号的appSecret  e300f432598cf83d563bbb469a170927
        /// <summary>
        /// 微信授权用户链接
        /// </summary>
        public static string Oauthurl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid="+appId+"&redirect_uri=http%3a%2f%2fmobile.mj100.com%2fauto.aspx&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public static bool CheckSignature(String signature, String timestamp, String nonce)
        {
            String[] arr = new String[] { token, timestamp, nonce };
            // 将token、timestamp、nonce三个参数进行字典序排序  
            Array.Sort<String>(arr);

            StringBuilder content = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                content.Append(arr[i]);
            }
            String tmpStr = SHA1_Encrypt(content.ToString());
            // 将sha1加密后的字符串可与signature对比，标识该请求来源于微信  
            return tmpStr != null ? tmpStr.Equals(signature) : false;
        }


        /// <summary>
        /// 使用缺省密钥给字符串加密 sha1加密
        /// </summary>
        /// <param name="Source_String"></param>
        /// <returns></returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();

            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }


        /// <summary>
        /// 将xml文件转换成Hashtable  解析xml用
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Hashtable ParseXml(String xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            XmlNode bodyNode = xmlDocument.ChildNodes[0];
            Hashtable ht = new Hashtable();
            if (bodyNode.ChildNodes.Count > 0)
            {
                foreach (XmlNode xn in bodyNode.ChildNodes)
                {
                    ht.Add(xn.Name, xn.InnerText);
                }
            }
            return ht;
        }

        /// <summary>
        /// 获取Access_token
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <returns></returns>
        public static string Get_Access_token(string appid, string appsecret)
        {
            WebClient webclient = new WebClient();
            string url = @"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret + "";
            byte[] bytes = webclient.DownloadData(url);//在指定的path上下载
            string result = Encoding.GetEncoding("utf-8").GetString(bytes);//转string
            //{"access_token":"ACCESS_TOKEN","expires_in":7200}    {"errcode":40013,"errmsg":"invalid appid"}

            //{"access_token":"(.+?)","expires_in":\d+}  返回成功的正则代码

            //{"errcode":(\d+),"errmsg":"(.+)"}          返回失败的正则代码

            Regex re = new Regex("{\"access_token\":\"(.+?)\",\"expires_in\":\\d+}");
            string a_token = "";
            Match m = re.Match(result);
            if (m.Success)
            {
                a_token = m.Groups[1].Value;
            }
            else
            {
                //这里写日志
            }

            return a_token;
        }

        /// <summary>
        /// 得到Access_token
        /// </summary>
        /// <returns></returns>
        public static string Get_Access_token()
        {
          
            object o= DataCache.GetCache("Access_token");
            if (o==null)
            {   
            string Access_token = Get_Access_token(appId,appSecret);
            DataCache.SetCache("Access_token", Access_token,DateTime.Now.AddSeconds(7000),TimeSpan.Zero);
            return Access_token;
            }
            else
            {
                return o.ToString();
            }

 
        }
        /// <SUMMARY> 
        /// 上传多媒体文件,返回 MediaId 
        /// </SUMMARY> 
        /// <PARAM name="ACCESS_TOKEN"></PARAM> 
        /// <PARAM name="Type"></PARAM> 
        /// <RETURNS></RETURNS> 
        public string UploadMultimedia(string ACCESS_TOKEN, string Type)
        {
            string result = "";
            string wxurl = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + ACCESS_TOKEN + "&type=" + Type;//向微信发送数据
            string filepath = "";//绝对路径

            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                byte[] responseArray = myWebClient.UploadFile(wxurl, "POST", filepath);
                result = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);

                //解析result  {"type":"TYPE","media_id":"MEDIA_ID","created_at":123456789}

                //{"errcode":40004,"errmsg":"invalid media type"}
                result = "";
            }
            catch (Exception ex)
            {
                result = "Error:" + ex.Message;
            }

            return result;
        }



        //<summary>
        //写日志(用于跟踪)
        //</summary>
        public static void WriteLog(string strMemo)
        {   //E:\Code\WebChart\Chart\
            string filename = AppDomain.CurrentDomain.BaseDirectory + "/logs/log.txt";
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/logs/"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/logs/");
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(filename))
                {
                    sr = File.CreateText(filename);
                }
                else
                {
                    sr = File.AppendText(filename);
                }
                sr.WriteLine(strMemo);
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 将对象序列化为xml
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="toBeIndented"></param>
        /// <returns></returns>
        public static string ObjectToXml(object obj, bool toBeIndented)
        {
            // Check the input arguments.
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            // Serialize the object to XML string.
            UTF8Encoding encoding = new UTF8Encoding(false);
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, encoding);
            writer.Formatting = (toBeIndented ? Formatting.Indented : Formatting.None);
            serializer.Serialize(writer, obj);
            string xml = encoding.GetString(stream.ToArray());
            // Clean up and return.
            writer.Close();
            return xml;
        }


        /// <summary>
        /// 可以发送自定义菜单  发送https  post请求
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="paramData"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static string PostMa(string postUrl, string paramData, Encoding ed)
        {
            string ret = string.Empty;
            byte[] byteArray = ed.GetBytes(paramData);
            HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
            webreq.Method = "POST";
            webreq.ContentType = "application/x-www-form-urlencoded";
            webreq.ContentLength = byteArray.Length;
            Stream newStream = webreq.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)webreq.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            ret = sr.ReadToEnd();
            sr.Close();
            response.Close();
            newStream.Close();
            return ret;
        }
    }
}
