using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace JsApi
{
    public class Businesslogic
    {


        /// <summary>
        /// 添加微信用户
        /// </summary>
        /// <param name="openid"></param>
        public static void AddWeChartUser(string openid)
        {
            string sql = @"declare @num int set @num=0;
select @num=count(openid) from WebChartUser where openid=@openid;
if(@num<1)begin  
insert into WebChartUser (openid) values(@openid);
end";

            SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@openid", openid));
        }



        /// <summary>
        /// 提交需求
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="phone"></param>
        /// <param name="time"></param>
        /// <param name="name"></param>
        public static void AddDemand(string phone, string serverId, string address, string openid)
        {


            //先删除需求


            #region 插入需求表

            string sql = "insert into DemandShowRooms (UserId,Extension11,createtime,Extension) values(@UserId,@hxt,@createtime,'未审核');";
            string userid = "";
            object o = SqlHelper.ExecuteScalar("select UserId from Users where LoginName=@phone or UserMPhone=@phone;", new SqlParameter("@phone", phone));
            if (o != null)
            {
                userid = o.ToString();
                SqlHelper.ExecuteNonQuery("delete from DemandShowRooms where UserId='"+userid+"'");//删除需求
            }
            else
            {
                o = SqlHelper.ExecuteScalar("insert into Users(LoginName,UserMPhone)values(@phone,@phone) select @@IDENTITY;", new SqlParameter("@phone", phone));
                userid = o.ToString();
            }
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@UserId",userid),
            new SqlParameter("@hxt",Dowload(serverId,JsApi.JsToken.GetApptoken(),userid)),
            new SqlParameter("@createtime",DateTime.Now.ToString())
        
            };
            SqlHelper.ExecuteNonQuery(sql, arr);
            #endregion



            #region 修改用户微信状态userid
            SqlHelper.ExecuteNonQuery("update WebChartUser set userid=@userid where openid=@openid", new SqlParameter("@userid", userid), new SqlParameter("@openid", openid));
            #endregion

        }

        /// <summary>
        /// 预约量房
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="phone"></param>
        /// <param name="time"></param>
        /// <param name="name"></param>
        public static void MakeAnAppointment(string phone, string name)
        {
            string sql = "insert into Tentent(UserId,Extension1,Extension3,Extension4) values(@UserId,@phone,@time,@name);";
            string userid = "";
            object o = SqlHelper.ExecuteScalar("select UserId from Users where LoginName=@phone or UserMPhone=@phone;", new SqlParameter("@phone", phone));
            if (o != null)
            {
                userid = o.ToString();
            }
            else
            {
                o = SqlHelper.ExecuteScalar("insert into Users(LoginName,UserMPhone)values(@phone,@phone) select @@IDENTITY;", new SqlParameter("@phone", phone));
                userid = o.ToString();
            }
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@UserId",userid),
            new SqlParameter("@phone",phone),
            new SqlParameter("@time",DateTime.Now.ToString("yy-MM-dd")),
            new SqlParameter("@name",name)
            };
            SqlHelper.ExecuteNonQuery(sql, arr);

        }


        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="accestoken"></param>
        /// <returns></returns>
        public static string Dowload(string serverId, string accestoken, string userid)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\HXT\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\"));
            System.Net.WebClient clinet = new System.Net.WebClient();
            clinet.DownloadFile("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + accestoken + "&media_id=" + serverId, AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\HXT\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + userid + ".jpg");

            return "http://mobile.mj100.com/HXT/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + userid + ".jpg";


        }


        /// <summary>
        /// 发送提醒
        /// </summary>
        /// <returns></returns>
        public static string Remind()
        {

            //查询出设计师的信息  id  name    电话
            return Commen.HttpRequest.PostMa("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + JsApi.JsToken.GetApptoken(), new Template.Notice5("o8r91jjmQWUqO8zrq4rxL0QVTEYs", "http://img.mj100.com/admin", "#FF0000", "点击进入服务查询了解最新的服务进度>>", "李连荣", "13800138000", "2015-1-12 15:00", "我们已为您分配好您的专属设计师啦！设计师上门量尺前会与您电话联系，请保持电话畅通！").ToString(), Encoding.UTF8);

        }

        /// <summary>
        /// 能提交返回true
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static bool NoExpire(string openid)
        {
            string sql = " select top 1 CreateTime from DemandShowRooms where UserId in (select userid from WebChartUser where openid='" + openid + "')";


            object o = SqlHelper.ExecuteScalar(sql);

            if (o == null)
            {
                return true;
            }

            DateTime dt = Convert.ToDateTime(o);

            
            TimeSpan ts = DateTime.Now.Subtract(dt);

           
            if (ts.TotalMinutes>60)
            {
                return true;
            }


            return false;
        }

    }
}
