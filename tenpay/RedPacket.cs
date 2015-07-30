using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tenpay
{
    /// <summary>
    /// 红包
    /// </summary>
    public class RedPacket
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string sign = "";
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string mch_billno = "";
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id = "";
        /// <summary>
        /// wxappid
        /// </summary>
        public string wxappid = "";
        /// <summary>
        /// 付款金额
        /// </summary>
        public string total_amount = "";
        /// <summary>
        /// 最小红包金额
        /// </summary>
        public string min_value = "";

        /// <summary>
        /// 最大红包金额
        /// </summary>
        public string max_value = "";
        /// <summary>
        /// 红包发放总人数
        /// </summary>
        public string total_num = "";
        /// <summary>
        /// 红包祝福语
        /// </summary>
        public string wishing = "";
        /// <summary>
        /// Ip地址
        /// </summary>
        public string client_ip = "";
        /// <summary>
        /// 活动名称
        /// </summary>
        public string act_name = "";
        /// <summary>
        /// act_id
        /// </summary>
        public string act_id = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string remark = "";
        /// <summary>
        /// 商户logo的ur
        /// </summary>
        public string logo_imgurl = "";
        /// <summary>
        /// 分享文案
        /// </summary>
        public string share_content = "";
        /// <summary>
        /// 分享链接
        /// </summary>
        public string share_url = "";
        /// <summary>
        /// 分享的图片
        /// </summary>
        public string share_imgurl = "";
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str = "";

        /// <summary>
        /// 提供方名称
        /// </summary>
        public string nick_name = "";
        /// <summary>
        /// 商户名称
        /// </summary>
        public string send_name = "";

        /// <summary>
        /// 用户openid
        /// </summary>
        public string re_openid = "";
    }
}
