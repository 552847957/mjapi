using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template
{
    public class Notice5 : BaseNotice
    {

         //{{first.DATA}}
        //{{Content1.DATA}}
        //商品名称：{{Good.DATA}}
        //服务进度：{{contentType.DATA}}
        //{{remark.DATA}}



        public Notice5(string touser, string url, string topcolor, string first, string keyword1, string keyword2, string keyword3, string remark)
            : base(touser, url, topcolor)
        {
            this.first = first;
            this.keyword1 = keyword1;
            this.keyword2 = keyword2;
            this.keyword3 = keyword3;
            this.remark = remark;
        }

        public Notice5()
        {
        }

        public string first;

        public string keyword1;

        public string keyword2;
        public string keyword3;
        public string remark;

        private string template_id = "1DDPKu-kxMI3UwECBs-vojsX9pcixefJmBH6qw-zvRg";

//      {{first.DATA}}
//昵称：{{keyword1.DATA}}
//内容：{{keyword2.DATA}}
//{{remark.DATA}}


//        {{first.DATA}}
//设计师：{{keyword1.DATA}}
//联系电话：{{keyword2.DATA}}
//计划量尺时间：{{keyword3.DATA}}
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
            sb.Append("\"keyword2\":{\"value\":\"" + this.keyword3 + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + this.remark + "\",\"color\":\"#173177\"}");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();


        }



    }
}
