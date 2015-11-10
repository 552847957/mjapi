using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Commen
{
    /// 
    /// 基础消息内容
    /// 
    [XmlRoot(ElementName = "xml")]
    public class BaseMessage
    {
        /// 
        /// 初始化一些内容，如创建时间为整形，
        /// 
        public BaseMessage()
        {
            this.CreateTime = Commen.WeixinServer.ConvertDateTimeInt(DateTime.Now);
        }

        /// 
        /// 开发者微信号
        /// 
        public string ToUserName { get; set; }

        /// 
        /// 发送方帐号（一个OpenID）
        /// 
        public string FromUserName { get; set; }

        /// 
        /// 消息创建时间 （整型）
        /// 
        public int CreateTime { get; set; }

        /// 
        /// 消息类型
        /// 
        public string MsgType { get; set; }

        public virtual string ToXml()
        {
            this.CreateTime = Commen.WeixinServer.ConvertDateTimeInt(DateTime.Now);//重新更新

            return Commen.WeixinServer.ObjectToXml(this, true).Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "").Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"","");


        }

    }


    /// 
    /// 回复文本消息
    /// 
    [System.Xml.Serialization.XmlRoot(ElementName = "xml")]
    public class ResponseText : BaseMessage
    {
        public ResponseText()
        {
            this.MsgType = "text";
        }

        public ResponseText(BaseMessage info)
            : this()
        {
            this.FromUserName = info.ToUserName;
            this.ToUserName = info.FromUserName;
        }

        /// 
        /// 内容
        ///         
        public string Content { get; set; }
    }


    /// 
    /// 回复图文消息
    /// 
    [System.Xml.Serialization.XmlRoot(ElementName = "xml")]
    public class ResponseNews : BaseMessage
    {
        public ResponseNews()
        {
            this.MsgType = "news";

            this.Articles = new List<ArticleEntity>();
        }
        public ResponseNews(BaseMessage info)
            : this()
        {
            this.FromUserName = info.ToUserName;
            this.ToUserName = info.FromUserName;
        }

        /// 
        /// 图文消息个数，限制为10条以内
        /// 
        public int ArticleCount
        {
            get
            {
                return this.Articles.Count;
            }
            set
            {
                ;//增加这个步骤才出来XML内容
            }
        }

        /// 
        /// 图文列表。
        /// 多条图文消息信息，默认第一个item为大图,注意，如果图文数超过10，则将会无响应
        /// 
        [System.Xml.Serialization.XmlArrayItem("item")]
        public List<ArticleEntity> Articles { get; set; }

    }


    /// <summary>
    /// 图文消息item
    /// </summary>
    public class ArticleEntity
    {
        //图文消息标题
        public string Title;

        //图文消息描述
        public string Description;

        // 图片链接
        public string PicUrl;

        //图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        public string Url;

    }

}


