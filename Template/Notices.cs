using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template
{
    public class Notice3 : BaseNotice
    {
        //{{first.DATA}}
        //{{Content1.DATA}}
        //商品名称：{{Good.DATA}}
        //服务进度：{{contentType.DATA}}
        //{{remark.DATA}}



        public Notice3(string touser, string url, string topcolor, string first, string keyword1, string keyword2, string keyword3, string keyword4, string remark)
            : base(touser, url, topcolor)
        {
            this.first = first;
            this.keyword1 = keyword1;
            this.keyword2 = keyword2;
            this.keyword3 = keyword3;
            this.keyword4 = keyword4;
            this.remark = remark;
        }

        public Notice3()
        {
        }

        public string first;

        public string keyword1;

        public string keyword2;

        public string keyword3;

        public string keyword4;

        public string remark;

        private string template_id = "-OocH4JumuL-WSmT__Hh50Jt7l_5gSCQxF3x3emdETQ";

         



//        {{first.DATA}}
//服务编号：{{keyword1.DATA}}
//服务类型：{{keyword2.DATA}}
//服务内容：{{keyword3.DATA}}
//进度情况：{{keyword4.DATA}}
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
            sb.Append("\"keyword3\":{\"value\":\"" + this.keyword3 + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword4\":{\"value\":\"" + this.keyword4 + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + this.remark + "\",\"color\":\"#173177\"}");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();


        }


        
    }
}
