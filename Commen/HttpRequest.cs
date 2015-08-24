using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Commen
{
    public static class HttpRequest
    {

        /// <summary>
        /// post数据到指定接口并返回数据
        /// </summary>
        public static string Post(string url, string postData)
        {
            string returnmsg = "";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                returnmsg = wc.UploadString(url, "POST", postData);
            }
            return returnmsg;
        }

        /// <summary>
        /// 售房网下载数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponseString(string url)
        {
            string _StrResponse = "";
            HttpWebRequest _WebRequest = (HttpWebRequest)WebRequest.Create(url);
            _WebRequest.UserAgent = "MOZILLA/4.0 (COMPATIBLE; MSIE 7.0; WINDOWS NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";

            _WebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            _WebRequest.Method = "GET";
            WebResponse _WebResponse = _WebRequest.GetResponse();
            StreamReader _ResponseStream = new StreamReader(_WebResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            _StrResponse = _ResponseStream.ReadToEnd();
            _WebResponse.Close();
            _ResponseStream.Close();
            return _StrResponse;
        }


        /// <summary>
        /// 可以发 发送https  post请求
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
