using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DesingerBLL
{
    public class DesignerPlatform
    {
        private static string errormsg = "";

        /// <summary>
        /// 设计师登录
        /// </summary>
        /// <param name="loginname">登录名</param>
        /// <param name="pwd">明文密码</param>
        /// <returns></returns>
        public static string Login(string loginname, string pwd)
        {
            if (CheckParm(new Dictionary<string, string>() { { "loginname", loginname }, { "pwd", pwd } }))
            {
                return errormsg;
            }

            #region 查询用户
            string sql = "select * from DesignerGrade where Extension5=@loginname and Extension6=@pwd";

            DataTable dt = SqlHelper.ExecuteDataTable(sql, new SqlParameter("@loginname", loginname), new SqlParameter("@pwd", pwd.To16Md5()));
            #endregion
            if (dt == null || dt.Rows.Count < 1)
            {
                return JsonConvert.SerializeObject(new { errorcode = 2, msg = "用户名或密码错误" });

            }
            else
            {
                var row = dt.Rows[0];

                var desinger = new { errorcode = 0, desingerid = row["id"].ToSafeString(), name = row["did"].ToSafeString(), email = row["Extension2"].ToSafeString(), phone = row["mphone"].ToSafeString(), headimg = "http://www.mj100.com/GEEKPRO/img/head/" + (row["extension3"].ToSafeString().IsEmpty() ? "" : row["extension3"].ToSafeString()), servicerange = row["Dgrade"].ToSafeString() };

                return JsonConvert.SerializeObject(desinger);



            }
        }




        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static bool CheckParm(Dictionary<string, string> dic)
        {

            foreach (var item in dic)
            {
                if (item.Value.IsEmpty())
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (var ele in dic)
                    {
                        sb.Append(ele.Key + ":" + (ele.Value.IsEmpty() ? "null" : ele.Value).Replace("\"", "'") + ",");
                    }

                    errormsg = ("{\"errorcode\":1,\"msg\":\"有参数为空\",\"parms\":\"" + sb.ToSafeString().TrimEnd(',') + "\"}");
                    return true;

                }
            }

            return false;
        }
    }
}
