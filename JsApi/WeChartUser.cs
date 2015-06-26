using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsApi
{
    public class WeChartUser
    {
        public string access_token { get; set; }  //属性的名字，必须与json格式字符串中的"key"值一样。
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }

    }
}
