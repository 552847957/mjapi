using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsApi
{
    public class LuckDrawUser
    {
        //        {
        //    "subscribe": 1, 
        //    "openid": "o6_bmjrPTlm6_2sgVt7hMZOPfL2M", 
        //    "nickname": "Band", 
        //    "sex": 1, 
        //    "language": "zh_CN", 
        //    "city": "广州", 
        //    "province": "广东", 
        //    "country": "中国", 
        //    "headimgurl":    "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0", 
        //   "subscribe_time": 1382694957,
        //   "unionid": " o6_bmasdasdsad6_2sgVt7hMZOPfL"
        //   "remark": "",
        //   "groupid": 0
        //}

        public int subscribe;

        public string openid = "";
        public string nickname = "";
        public int sex;

        public string language = "";

        public string city = "";

        public string province = "";

        public string country = "";

        public string headimgurl = "";

        public string subscribe_time = "";

        public string unionid = "";


        public string remark = "";

        public int groupid;
    }
}
