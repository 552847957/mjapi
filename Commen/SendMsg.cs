using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Commen
{
    public class SendMsg
    {

        /// <summary>
        /// 发送短信
        /// </summary>
        public static string FSong(string sj, string content)
        {
            Encoding myEncoding = Encoding.GetEncoding("UTF-8");
            // string url = "http://sdk.kuai-xin.com:8888/sms.aspx?action=send&userid=4970&account=ycs&password=ycs123456&mobile=" + sj + "&content=" + content;
            string url = "http://125.208.3.91:8888/sms.aspx?action=send&userid=5182&account=xpt10106&password=yy7788&mobile=" + sj + "&content=" + content;
            byte[] postBytes = Encoding.ASCII.GetBytes(url);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            myRequest.ContentLength = postBytes.Length;

            using (Stream reqStream = myRequest.GetRequestStream())
            {
                reqStream.Write(postBytes, 0, postBytes.Length);
            }
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            using (WebResponse wr = myRequest.GetResponse())
            {
                StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.UTF8);
                System.IO.StreamReader xmlStreamReader = sr;
                xmlDoc.Load(xmlStreamReader);
            }
            if (xmlDoc == null)
            {
                return "no";
            }
            else
            {
                String returnstatus = xmlDoc.GetElementsByTagName("returnstatus").Item(0).InnerText.ToString();
                String message = xmlDoc.GetElementsByTagName("message").Item(0).InnerText.ToString();
                String remainpoint = xmlDoc.GetElementsByTagName("remainpoint").Item(0).InnerText.ToString();
                String taskID = xmlDoc.GetElementsByTagName("taskID").Item(0).InnerText.ToString();
                String successCounts = xmlDoc.GetElementsByTagName("successCounts").Item(0).InnerText.ToString();
                return message;

            }
        }
    }
}
