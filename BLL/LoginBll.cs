﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commen;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;//先引入这两个命名空间 
namespace BLL
{
    public class LoginBll
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="loginpwd"></param>
        /// <returns></returns>
        public string Login(string loginname, string loginpwd)
        {
            #region 登录
            string sql = "select * from users where (LoginName=@LoginName or UserMPhone=@LoginName ) and LoginPwd=@LoginPwd";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@LoginName",loginname),
            new SqlParameter("@LoginPwd",loginpwd.To16Md5())
            };
            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);
            #endregion
            if (dt.Rows.Count < 1)
            {
                return "{\"success\":\"false\",\"msg\":\"用户名或密码错误\"}";
            }
            else
            {
                DataRow row = dt.Rows[0];
                #region 各种字段
                //private int _userid;
                //private string _loginname;
                //private string _loginpwd;
                //private string _userphone;
                //private string _usermphone;
                //private string _email;
                //private string _address;
                //private string _headimage;
                //private string _logintime;
                //private string _extension;
                //private string _extension1;
                //private string _extension2;
                //private string _extension3;
                //private string _extension4;
                //private string _extension5;
                //private string _extension6;
                //private int? _extension7;
                //private string _extension8;
                //private string _createtime;
                var person = new { success = "true", userid = row["userid"].ToSafeString(), loginname = row["loginname"].ToSafeString(), usermphone = row["usermphone"].ToSafeString(), name = row["Extension"].ToSafeString(), gender = row["Extension4"].ToSafeString(), address = row["address"].ToSafeString() };
                #endregion


                return JsonConvert.SerializeObject(person);
            }

        }

        /// <summary>
        /// 注册帐号
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public string Regist(string phone, string pwd, string name, string gender)
        {
            string sql = @"insert into Users(LoginName,LoginPwd,UserMPhone,HeadImage,Extension,Extension4)

values (@LoginName,@LoginPwd,@UserMPhone,'img/defaultHead.png',@Extension,@Extension4) select @@IDENTITY;";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@LoginName",phone),
            new SqlParameter("@LoginPwd",pwd.To16Md5()),
            new SqlParameter("@UserMPhone",phone),
            new SqlParameter("@Extension",name),
            new SqlParameter("@Extension4",gender)
            };
            object o = SqlHelper.ExecuteScalar(sql, arr);
            return "{\"success\":\"true\",\"msg\":\"注册成功\",\"userid\":\"" + o.ToSafeString() + "\"}"; ;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <returns></returns>
        public string UpdateUser(string userid, string name, string gender, string address)
        {
            string setstr = "";
            //  string sql = "update Users set Extension='',Extension4='',Address='' where UserId='@UserId';";

            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@UserId", userid));
            if (!name.IsEmpty())
            {
                setstr += ",Extension=@name";
                lis.Add(new SqlParameter("@name", name));
            }
            if (!gender.IsEmpty())
            {
                setstr += ",Extension4=@gender";
                lis.Add(new SqlParameter("@gender", gender));
            }
            if (!address.IsEmpty())
            {
                setstr += ",Address=@address";
                lis.Add(new SqlParameter("@address", address));
            }
            string sql = "update Users set " + setstr + " where UserId=@UserId;";
            sql = sql.Replace("set ,", "set ");
            if (lis.Count == 1)
            {
                return "{\"success\":\"true\",\"msg\":\"没有更新任何字段\"}"; ;
            }
            else
            {
                SqlHelper.ExecuteScalar(sql, lis.ToArray());
            }


            return "{\"success\":\"true\",\"msg\":\"更新成功\"}"; ;
        }


        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        public string ResetPWd(string phone, string newpwd)
        {

            #region 执行sql语句
            string sql = "update Users set LoginPwd=@LoginPwd where LoginName=@LoginName or UserMPhone=@UserMPhone";

            SqlParameter[] arr = new SqlParameter[]{
            new SqlParameter("@LoginPwd", newpwd.To16Md5()),
             new SqlParameter("@LoginName", phone),
             new SqlParameter("@UserMPhone", phone)
            };
            object o = SqlHelper.ExecuteNonQuery(sql, arr); 
            #endregion

            if (Convert.ToInt32(o)>0)
            {
                return "{\"success\":\"true\",\"msg\":\"更新成功\"}";
            }
            else
            {
                return "{\"success\":\"false\",\"msg\":\"异常情况\"}";
            }
            
        }
    }
}
