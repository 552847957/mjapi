using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template
{
    public class Notice1:BaseNotice
    {
        //{{first.DATA}}
        //{{Content1.DATA}}
        //商品名称：{{Good.DATA}}
        //服务进度：{{contentType.DATA}}
        //{{remark.DATA}}


     
        public Notice1(string touser, string url, string topcolor, string first, string Content1, string Good, string contentType, string remark)
            : base(touser,url,topcolor)
        {
            this.first = first;
            this.Content1 = Content1;
            this.Good = Good;
            this.contentType = contentType;
            this.remark = remark;
        }

        public Notice1()
        { 
        }

        public string first;

        public string Content1;

        public string Good;

        public string contentType;

        public string remark;

        private string template_id = "nkLd6D0zQ1OKywLqjk8hnKUhKjo2MpUlqTBKi_n_l2M";

        //{{first.DATA}}
        //{{Content1.DATA}}
        //商品名称：{{Good.DATA}}
        //服务进度：{{contentType.DATA}}
        //{{remark.DATA}}
        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + base.touser + "\",");
            sb.Append("\"template_id\":\"" + this.template_id + "\",");
            sb.Append("\"url\":\"" + this.url + "\",");
            sb.Append("\"topcolor\":\"" + this.topcolor + "\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\""+this.first+"\",\"color\":\"#173177\"},");
            sb.Append("\"Content1\":{\"value\":\"" + this.Content1 + "\",\"color\":\"#173177\"},");
            sb.Append("\"Good\":{\"value\":\"" + this.Good + "\",\"color\":\"#173177\"},");
            sb.Append("\"contentType\":{\"value\":\"" + this.contentType + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + this.remark + "\",\"color\":\"#173177\"}");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();


        }


        //   {
        //    "touser":"OPENID",
        //    "template_id":"ngqIpbwh8bUfcSsECmogfXcV14J0tQlEpBO27izEYtY",
        //    "url":"http://weixin.qq.com/download",
        //    "topcolor":"#FF0000",
        //    "data":{
        //            "first": {
        //                "value":"恭喜你购买成功！",
        //                "color":"#173177"
        //            },
        //            "keynote1":{
        //                "value":"巧克力",
        //                "color":"#173177"
        //            },
        //            "keynote2": {
        //                "value":"39.8元",
        //                "color":"#173177"
        //            },
        //            "keynote3": {
        //                "value":"2014年9月16日",
        //                "color":"#173177"
        //            },
        //            "remark":{
        //                "value":"欢迎再次购买！",
        //                "color":"#173177"
        //            }
        //    }
        //}
    }
}
