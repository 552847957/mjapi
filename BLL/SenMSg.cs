using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BLL
{
    public class SenMSg
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string Send(string phone)
        {

            string sql = "select count(*) from users where (LoginName=@LoginName or UserMPhone=@LoginName )";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@LoginName",phone)             
            };

            object o= SqlHelper.ExecuteScalar(sql,arr);

            if (Convert.ToInt32(o)>0)
            {
                return "{\"success\":\"false\",\"msg\":\"此手机号已被注册\"}";
            }
            else
            {
                #region 发送随机短信
                Random r = new Random();

                string s = r.Next(100000, 999999).ToString();

                Commen.DataCache.SetCache(phone, s);

                Commen.SendMsg.FSong(phone, "你好，你的验证码是：" + s);
                #endregion

                return "{\"success\":\"true\",\"msg\":\"验证码发送成功\"}";
            }
            
            
          


           
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendResetPwd(string phone)
        {

            string sql = "select count(*) from users where (LoginName=@LoginName or UserMPhone=@LoginName )";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@LoginName",phone)             
            };

            object o = SqlHelper.ExecuteScalar(sql, arr);

            if (Convert.ToInt32(o) > 0)
            {
                #region 发送随机短信
                Random r = new Random();

                string s = r.Next(100000, 999999).ToString();

                Commen.DataCache.SetCache("pwd" + phone, s);

                Commen.SendMsg.FSong(phone, "你好，你正在找回密码，你的验证码是：" + s);
                #endregion

                return "{\"success\":\"true\",\"msg\":\"找回密码验证码发送成功\"}";
            }
            else
            {
                return "{\"success\":\"false\",\"msg\":\"此手机号尚未被注册\"}";
            }






        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendOrderMsg(string phone)
        {

             
 
                #region 发送随机短信
                Random r = new Random();

                string s = r.Next(100000, 999999).ToString();

                Commen.DataCache.SetCache("order" + phone, s);

                Commen.SendMsg.FSong(phone, "你好，您正在预约量房，您的验证码是：" + s);
                #endregion

                return "{\"success\":\"true\",\"msg\":\"预约量房验证码发送成功\"}";
           






        }
    }
}
