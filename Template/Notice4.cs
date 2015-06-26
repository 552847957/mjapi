using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template
{
    public class Notice4 : BaseNotice
    {
        //{{first.DATA}}
        //{{Content1.DATA}}
        //商品名称：{{Good.DATA}}
        //服务进度：{{contentType.DATA}}
        //{{remark.DATA}}



        public Notice4(string touser, string url, string topcolor, string first, string keyword1, string keyword2, string keyword3, string keyword4, string keyword5, string remark)
            : base(touser, url, topcolor)
        {
            this.first = first;
            this.keyword1 = keyword1;
            this.keyword2 = keyword2;
            this.keyword3 = keyword3;
            this.keyword4 = keyword4;
            this.keyword5 = keyword5;
            this.remark = remark;
        }

        public Notice4()
        {
        }

        public string first;

        public string keyword1;

        public string keyword2;

        public string keyword3;

        public string keyword4;

        public string keyword5;

        public string remark;

        private string template_id = "gNhuyreQxz07KShkxGmvvt8bUCvDqfX8epJgzk8sykg";


//        {{first.DATA}}
//顾客姓名：{{keyword1.DATA}}
//顾客电话：{{keyword2.DATA}}
//预约项目：{{keyword3.DATA}}
//预约时间：{{keyword4.DATA}}
//其他信息：{{keyword5.DATA}}
//{{remark.DATA}}
//在发送时，需要将内容中的参数（{{.DATA}}内为参数）赋值替换为需要的信息
//内容示例
//阿伟，你好，有一个顾客预约了您的服务，请注意安排时间。
//顾客姓名：杨一帆小姐
//顾客电话：1397823****
//预约项目：染发
//预约时间：9月2号（明天）下午3:00
//其他信息：请帮我安排车位
//人数：1人。

 
        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\":\"" + this.touser + "\",");
            sb.Append("\"template_id\":\"" + this.template_id + "\",");
            sb.Append("\"url\":\"" + this.url + "\",");
            sb.Append("\"topcolor\":\"" + this.topcolor + "\",");
            sb.Append("\"data\":{");
            sb.Append("\"first\":{\"value\":\"" + this.first + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword1\":{\"value\":\"" + this.keyword1 + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword2\":{\"value\":\"" + this.keyword2 + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword3\":{\"value\":\"" + this.keyword3 + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword4\":{\"value\":\"" + this.keyword4 + "\",\"color\":\"#173177\"},");
            sb.Append("\"keyword5\":{\"value\":\"" + this.keyword5 + "\",\"color\":\"#173177\"},");
            sb.Append("\"remark\":{\"value\":\"" + this.remark + "\",\"color\":\"#173177\"}");
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();


        }



    }
}
