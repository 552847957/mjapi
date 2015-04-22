using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commen;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;//先引入这两个命名空间 
namespace LFBLL
{
    public class LFlogin
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string Login(string loginname, string pwd)
        {

            //if (string.IsNullOrEmpty(loginname) || string.IsNullOrEmpty(pwd))
            //{
            //    return Returnhelper.GetReturnStr("false", "参数不完善", "1");
            //}
            //loginname = loginname.Replace("'", ",");
            //pwd = pwd.Replace("'", ",");
            //#region
            //string sql = "select * from  users where loginpwd='" + Zx.Common.Common.MD5(pwd, true) + "'and loginname='" + loginname + "' and (userphone='4' or userphone='5')";
            //Zx.Query.ExecSqlQuery exs = new Zx.Query.ExecSqlQuery();
            //DataTable dt = exs.DataTableBySql(sql);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    return "{\"success\":\"true\",\"msg\":\"登录成功\",\"errorcode\":\"0\",\"userid\":\"" + dt.Rows[0]["userid"] + "\",\"mobile\":\"" + dt.Rows[0]["loginname"] + "\",\"nickname\":\"" + dt.Rows[0]["extension"] + "\",\"company\":\"" + dt.Rows[0]["Extension4"] + "\",\"type\":\"" + (dt.Rows[0]["UserPhone"].ToString() == "4" ? "家装设计" : "工装设计") + "\"}";
            //}
            //else
            //{
            //    return Returnhelper.GetReturnStr("false", "用户名或密码错误", "0");
            //}
            //#endregion


            #region 查询登录
            string sql = "select * from  users where loginpwd=@loginpwd and loginname=@loginname and (userphone='4' or userphone='5')";

            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@loginname",loginname),
            new SqlParameter("@loginpwd",pwd.To16Md5())
            };

            DataTable dt = SqlHelperMeasure.ExecuteDataTable(sql, arr);
            #endregion
            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                #region 检查头像
                string headimg = row["HeadImage"].ToSafeString();
                if (headimg.Contains("http"))
                {

                }
                else if (headimg.Length == 0)
                {
                    headimg = "http://www.mj100.com/img/defaultHead.png";
                }
                else
                {
                    headimg = "http://www.mj100.com/UploadFile/head/" + headimg;
                }
                #endregion

                var user = new { success = "true", msg = "登录成功", errorcode = "0", userid = row["userid"].ToSafeString(), mobile = row["loginname"].ToSafeString(), nickname = row["extension"].ToSafeString(), company = row["Extension4"].ToSafeString(), type = row["UserPhone"].ToSafeString() == "4" ? "家装设计" : "工装设计", headimg = headimg };
                return JsonConvert.SerializeObject(user);
            }
            else
            {
                return "{\"success\":\"false\",\"msg\":\"用户名或密码错误\",\"errorcode\":\"0\"}";
            }


        }



        /// <summary>
        /// 得到需求列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetDemandlist(string userid)
        {

            string sql = @"select Constructionid, Start_jlxy as projectid,SupportingId as userid 
,phone,Start as demanddesc ,Start_qht as address,ConstructionLxr as CustomerName,ProjectCost as fileurls, Start_kgjc as Thumbnailpic , CreateTime  from dbo.Construction where SupportingId=@userid  order by CreateTime desc";



            return SqlHelper.ExecuteDataTable(sql, new SqlParameter("@userid", userid));
        }

        /// <summary>
        /// 得到一个单独的需求
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Getsingle(string userid, string id)
        {
            string sql = @"select Constructionid  , Start_jlxy as projectid,SupportingId as userid 
,phone,Start as demanddesc ,

Start_qht as address,ConstructionLxr as CustomerName,ProjectCost as fileurls, 
Start_kgjc as Thumbnailpic , CreateTime  from dbo.Construction where SupportingId=@userid and ConstructionId=@id ";

            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@userid",userid),
            new SqlParameter("@id",id)
            };


            return SqlHelperMeasure.ExecuteDataTable(sql, arr);

        }


        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <param name="phone"></param>
        /// <param name="address"></param>
        /// <param name="demanddesc"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string SaveItem(string CustomerName, string phone, string address, string demanddesc, string id)
        {

            #region MyRegion
            string sql = "update Construction set ConstructionLxr=@CustomerName ,Phone=@phone,Start_qht=@address,Start=@demanddesc where ConstructionId=@id";
            #endregion


            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@CustomerName",CustomerName),
             new SqlParameter("@phone",phone),
              new SqlParameter("@address",address),
               new SqlParameter("@demanddesc",demanddesc),
                new SqlParameter("@id",id)
            };

            return SqlHelperMeasure.ExecuteNonQuery(sql, arr).ToSafeString();
        }


        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DelteProject(string id,string userid )
        {
            string sql = "delete from Construction where ConstructionId=@id and SupportingId=@userid";

            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@id",id),
            new SqlParameter("@userid",userid)
            };

            return  SqlHelperMeasure.ExecuteNonQuery(sql,arr);
        }
    }
}
