using Commen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WeiXinProcessing
{
    public class WeiXinProcess
    {

        ///// <summary>
        ///// 处理具体的微信请求
        ///// </summary>
        ///// <param name="poststr"></param>
        ///// <returns></returns>
        //public static string processRequest(string poststr)
        //{
        //    return "";
        //}




        /// <summary>
        /// 处理微信发来的请求 
        /// </summary>
        /// <param name="xml"></param>
        public  string processRequest(String xml)
        {
            try
            {
                #region 初始化接受到的xml   初始化消息父类
                // xml请求解析  
                Hashtable requestHT = WeixinServer.ParseXml(xml);
                // 发送方帐号（open_id）  
                string fromUserName = (string)requestHT["FromUserName"];
                // 公众帐号  
                string toUserName = (string)requestHT["ToUserName"];
                // 消息类型  
                string msgType = (string)requestHT["MsgType"];

                BaseMessage info = new BaseMessage();
                info.FromUserName = fromUserName;
                info.ToUserName = toUserName;
                info.MsgType = msgType;
                #endregion

                //文字消息
                if (msgType == ReqMsgType.Text)
                {
                    string s = ResponseByText(requestHT["Content"].ToString(), info);

                    return s;

                    #region 描述

                    //用户在点击按钮后的输入以后   session[用户id]!=null
                    //如果session["用户id"]=="自助服务菜单下"  那么用户所有的输入都是按照   自助服务菜单下做回复  用户切换菜单后session[用户id]="新的用户菜单"
                    //主界面的用户输入最好不要和自定义菜单的输入重复; 这样就可以实现不同菜单下用户输入相同的数字 服务器做出不同的响应 
                    #endregion
                }
                //事件推送
                else if (msgType == ReqMsgType.Event)
                {
                    // 事件类型  
                    String eventType = (string)requestHT["Event"];

                    #region 订阅取消订阅部分
                    // 订阅  
                    if (eventType.ToLower() == ReqEventType.Subscribe)
                    {   //qrscene_123123
                        if (requestHT["EventKey"].ToString().Contains("qrscene_"))//用户未关注情况下
                        {
                            #region MyRegion

                          
                            #endregion
                            if (requestHT["EventKey"].ToString() == "qrscene_2")
                            {
                                List<ArticleEntity> lis = new List<ArticleEntity> { new ArticleEntity { Description = "大砍价", Title = "活动详情", PicUrl = "http:/mobile.mj100.com/active/img/price.png", Url = "http://mobile.mj100.com/wechart/login7" } };


                                return GetresponseNews(info, lis);
                            }
                            else
                            {
                                return GetresponseText(info, @"感谢您关注极客美家服务号！极客美家是集设计、装修、建材、家居领域为一体，提供优质家装配套服务的云装修平台。点击屏幕下方功能按钮，您可以预约量房、查看装修工程进度，还可以申请装修贷款。" + requestHT["EventKey"].ToString());
                                 
                            }



                        }
                        else
                        {
                      return   GetresponseText(info, @"感谢您关注极客美家服务号！极客美家是集设计、装修、建材、家居领域为一体，提供优质家装配套服务的云装修平台。点击屏幕下方功能按钮，您可以预约量房、查看装修工程进度，还可以申请装修贷款。
");
                           

                        }

                    }
                    // 取消订阅  
                    else if (eventType.ToLower() == ReqEventType.Unsubscribe)
                    {
                        // TODO 取消订阅后用户再收不到公众号发送的消息，因此不需要回复消息  
                    }
                    #endregion

                    //自定义菜单点击事件  
                    else if (eventType.ToLower() == ReqEventType.CLICK)
                    {
                        #region 描述
                        //这里可能要记录每次用户的点击 用用户的appid做key  EventKey作为值
                        //根据 EventKey　判断是点的哪个按钮　，　然后返回什么样的数据 ，　比如说用户点的是查询装修进度 ，　返回请输入进度用户名 ，　则可返回
                        //多少个key值全列出来 每个值下面做不同的处理 
                        /* {
     "button":[
     {	
         
          "name":"美家计划",
         "sub_button":[
            {
               "type":"click",
               "name":"积分商城",
               "key":"jfsc"
            },
            {
               "type":"click",
               "name":"建材折扣",
               "key":"jczk"
            },
            {
               "type":"click",
               "name":"最新活动",
               "key":"wqhg"
            }
            ]
      },
      {
           "name":"自助服务",
          "sub_button":[
            {
               "type":"click",
               "name":"小美装家",
               "key":"jfcx"
            },
            {
               "type":"click",
               "name":"云装修平台",
               "key":"sgys"
            },
            {
               "type":"click",
               "name":"我的订单",
               "key":"wddd"
            },
            {
               "type":"click",
               "name":"施工管理",
               "key":"sgppp"
            },
            {
               "type":"click",
               "name":"实时验收",
               "key":"sgppp"
            }
            ]
      },
      {
           "name":"关于我们",
           "sub_button":[
            {
               "type":"click",
               "name":"美家易贷",
               "key":"gywm"
            },
            {
               "type":"click",
               "name":"小美管家",
               "key":"fqf"
            },
            {
               "type":"click",
               "name":"极客美家",
               "key":"zxb"
            }
            ]
       }]
 } */
                        #endregion
                        return ResponseByClick((string)requestHT["EventKey"], info);
                        
                    }
                    else if (eventType.ToLower() == "scan")//用户关注过后扫描二维码事件
                    {
                       return  ResponseByClick((string)requestHT["EventKey"], info);
                         
                    }
                    else if (eventType.ToLower() == ReqEventType.view)
                    {

                       return GetresponseText(info, "你要跳转的链接是：" + (string)requestHT["EventKey"]);
                        
                    }
                }
                //位置消息
                else if (msgType.ToLower() == ReqMsgType.Location)
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                throw e;
            }


            return "";
        }

        /// <summary>
        /// 响应文本信息
        /// </summary>
        /// <param name="info">基类</param>
        /// <param name="content">返回的内容</param>
        /// <returns></returns>
        public string GetresponseText(BaseMessage info, string content)
        {

            ResponseText model = new ResponseText(info);
            model.Content = content;

            return model.ToXml();
        }


        /// <summary>
        /// 转接到客服
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GotoKefu(BaseMessage info)
        {
            string to = info.ToUserName;
            string fr = info.FromUserName;

            info.ToUserName = fr;
            info.FromUserName = to;

            info.MsgType = "transfer_customer_service";
           
            return info.ToXml();

        }


        /// <summary>
        /// 响应图文信息
        /// </summary>
        /// <param name="info">基类</param>
        /// <param name="list">返回的图文注意不能超过十个</param>
        /// <returns></returns>
        public string GetresponseNews(BaseMessage info, List<ArticleEntity> list)
        {

            ResponseNews model = new ResponseNews(info);
            model.Articles.AddRange(list);
            return model.ToXml();
        }

        #region 一个根据需求出文本  一个根据需求出articles集合  暂时没用到
        /// <summary>
        /// 返回图文根据不同的信息返回图文
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public List<ArticleEntity> GetArticleEntitys(Hashtable ht)
        {
            return null;
        }

        /// <summary>
        /// 返回图文根据不同的信息返回文本信息
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public string GetresponseText(Hashtable ht)
        {
            return null;
        }
        #endregion


        /// <summary>
        /// 根据不同的EventKey做出不同的响应
        /// </summary>
        /// <param name="EventKey">自定义事件key</param>
        /// <returns></returns>
        public string ResponseByClick(string EventKey, BaseMessage info)
        {

            string res = "";
            switch (EventKey)
            {
        
                #region 按钮点击
                
                case "gj":
                   List<ArticleEntity> lishd = new List<ArticleEntity> { new ArticleEntity { Description = @"工程进度：油工中期
客户名称：X先生
项目地址： 海淀区XXX A-B-XXXX
2015.1.30您家的油工中期完毕了~

1月31日将进行热水器安装，2月1日将进行铝扣板安装。您家新居的漆作阶段将于2月1日完成。如有任何疑问，请语音/文字回复或联系您的小美管家（季建生先生，电话：13146109935）。 
期待已久？小美马上上图~~", Title = "管家体验：》》》》   2015 01 30 您的漆作工程施工节点推送", PicUrl = "http://img.mj100.com/weixin/ueditor/net/upload/image/20150313/6356186523821237508619902.jpg", Url = "http://img.mj100.com/weixin/ueditor/readhtml.aspx?key=6e1066ff-21fc-4f35-83b2-76a597af6d68" } };
                    res = GetresponseNews(info, lishd);
                    break;
                 
                 
                

                 
                

                default:
                    res = GotoKefu(info);
                    break;
                #endregion
            }
            return res;
        }


        /// <summary>
        /// 根据不同的文本消息做出不同的响应
        /// </summary>
        /// <param name="EventKey">自定义事件key</param>
        /// <returns></returns>
        public string ResponseByText(string Text, BaseMessage info)
        {

            //【中铁建小区用户】请回复中铁建＋您的姓名+手机号
            //【参与美家计划698活动】请回复698+您的姓名+手机号
            //设计师会在第一时间与您取得联系


            string res = "";

         



            switch (Text)
            {
                #region MyRegion
                case "2":
                    res = GetresponseText(info, "小美为了鞭策自己减肥，将每日体重做了Excel表格，生成走势图。今天，同事经过小美座位。只见他走过去，又若有所思地倒了回来，悄悄问：“那个，能不能透露一下，你买的哪支股票啊？走势这么好啊……”当时小美泪奔了;");
                    break;
                case "1":
                    res = GetresponseText(info, "家装小妙招：一些门窗由于制作和安装质量不良，缝隙不严，冷天透风，夏天穿热，门窗装上“密封条”，节能又隔音;");
                    break;
                case "3":
                    res = GetresponseText(info, "<a href='http://mobile.mj100.com/demo/demo1/se.htm'>1.看看我有多色</a>\n\n<a href='http://mobile.mj100.com/demo/demo1/0.htm'>2.看看我前世是什么</a>\n\n<a href='http://mobile.mj100.com/demo/demo1/2048.htm'>3.挑战2048</a>");
                    break;
                case "客服":
                    res = GotoKefu(info);
                    break;
                case "体验管家":
                    List<ArticleEntity> lishd = new List<ArticleEntity> { new ArticleEntity { Description = @"工程进度：油工中期
客户名称：X先生
项目地址： 海淀区XXX A-B-XXXX
2015.1.30您家的油工中期完毕了~

1月31日将进行热水器安装，2月1日将进行铝扣板安装。您家新居的漆作阶段将于2月1日完成。如有任何疑问，请语音/文字回复或联系您的小美管家（季建生先生，电话：13146109935）。 
期待已久？小美马上上图~~", Title = "2015 01 30 您的漆作工程施工节点推送", PicUrl = "http://img.mj100.com/weixin/ueditor/net/upload/image/20150313/6356186523821237508619902.jpg", Url = "http://img.mj100.com/weixin/ueditor/readhtml.aspx?key=6e1066ff-21fc-4f35-83b2-76a597af6d68" } };
                    res = GetresponseNews(info, lishd);

                    break;
               
                case "易贷":
                    res = GetresponseText(info, @"Q：什么是美家易贷？
A：美家易贷是极客美家与多家银行强强联手为极客美家用户量身定制的家装消费贷款分期产品。利率低、门槛低、效率高，30分钟内完成审核，无需抵押和担保，通过率远高于银行等金融机构。。    
<a href='http://img.mj100.com/weixin/ueditor/readhtml.aspx?key=ed344ddb-8ac2-4ca4-b657-7004962b3288'>点击查看更多易贷常见问题</a>
没有你想要的问答？回复 客服 二字，即可联系客服解答
 ");
                    break;
                #endregion

                default:
                    res = GotoKefu(info);
                    break;
            }
            return res;
        }

    }
}
