using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template
{
    public class Notice2 : BaseNotice
    {
        //{{first.DATA}}
        //{{Content1.DATA}}
        //商品名称：{{Good.DATA}}
        //服务进度：{{contentType.DATA}}
        //{{remark.DATA}}



        public Notice2(string touser, string url, string topcolor, string first, string keyword1, string keyword2, string remark)
            : base(touser, url, topcolor)
        {
            this.first = first;
            this.keyword1 = keyword1;
            this.keyword2 = keyword2;
            this.remark = remark;
        }

        public Notice2()
        {
        }

        public string first;

        public string keyword1;

        public string keyword2;
 
        public string remark;

        private string template_id = "u3AwHVhaX1r-1ZxcppN6t7uaa_OH70-dDhdi1-mLrSE";

//      {{first.DATA}}
//昵称：{{keyword1.DATA}}
//内容：{{keyword2.DATA}}
//{{remark.DATA}}


        //{{first.DATA}}
        //{{Content1.DATA}}
        //商品名称：{{Good.DATA}}
        //服务进度：{{contentType.DATA}}
        //{{remark.DATA}}
        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + this.touser+ "\",");
            sb.Append("\"template_id\":\"" + this.template_id + "\",");
            sb.Append("\"url\":\"" + this.url + "\",");
            sb.Append("\"topcolor\":\"" + this.topcolor + "\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + this.first + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + this.keyword1 + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + this.keyword2 + "\",\"color\":\"#173177\"},");
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
