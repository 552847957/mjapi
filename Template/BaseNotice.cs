using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template
{
   public class BaseNotice
    {

        //   {
        //    "touser":"OPENID",
        //    "template_id":"ngqIpbwh8bUfcSsECmogfXcV14J0tQlEpBO27izEYtY",
        //    "url":"http://weixin.qq.com/download",
        //    "topcolor":"#FF0000",
        //    "data":{
        //            "first": {
        //                "value":"恭喜你购买成功！",-----
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



        public string touser;

       
       

       public string url;

       public string topcolor;


        
       /// <summary>
       /// 模版消息父类构造函数
       /// </summary>
       /// <param name="touser"></param>
       /// <param name="template_id"></param>
       /// <param name="url"></param>
       /// <param name="tpcolor"></param>
       /// <param name="data"></param>
       public BaseNotice(string touser, string url, string tpcolor )
       {
           this.touser = touser;
           
           this.url = url;
           this.topcolor = tpcolor;
          
       }

       public BaseNotice(){}

    }
}
