using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commen
{
    /// <summary>
    /// 接受消息类型
    /// </summary>
    public static class ReqMsgType
    {
        //文本消息
        public static string Text = "text";

        //事件消息
        public static string Event = "event";

        //位置消息
        public static string Location = "location";


    }


    /// <summary>
    /// 事件类型
    /// </summary>
    public static class ReqEventType
    {
        //订阅
        public static string Subscribe = "subscribe";

        //取消订阅
        public static string Unsubscribe = "unsubscribe";

        //点击事件
        public static string CLICK = "click";

        public static string view = "view";
    }



}
