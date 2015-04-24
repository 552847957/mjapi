using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DbHelper;
namespace BLL
{
    public class Hdbll
    {


        public string Add(string name, string phone, string ip)
        {
            string sql = " insert Hd666(id,phone,name,ip)values(@id,@phone,@name,@ip)";

            if (Convert.ToInt32(SqlHelper.ExecuteScalar("select COUNT(*) from Hd666 where phone=@phone", new SqlParameter("@phone", phone))) < 1)
            {
                SqlParameter[] arr = new SqlParameter[] { 
            
              new SqlParameter("@id",Guid.NewGuid()),
              new SqlParameter("@phone",phone),
              new SqlParameter("@name",name),
              new SqlParameter("@ip",ip),
            };

                SqlHelper.ExecuteNonQuery(sql, arr);


                MakeAnAppointment(phone,name);

                return "预约成功";

            }
            else
            {
                return "你之前已预约无需再次预约";
            }


        }


        public string Add2(string ip, string mark)
        {
            string sql = " insert HdRecord(id,mark,ip)values(@id,@mark,@ip)";


            SqlParameter[] arr = new SqlParameter[] { 
            
            new SqlParameter("@id",Guid.NewGuid()),

             new SqlParameter("@mark",mark),
            
              new SqlParameter("@ip",ip),
            };

            SqlHelper.ExecuteNonQuery(sql, arr);
            return "";
        }



        /// <summary>
        /// 预约量房
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="phone"></param>
        /// <param name="time"></param>
        /// <param name="name"></param>
        public  void MakeAnAppointment(string phone, string name)
        {
            string sql = "insert into Tentent(Tkey,UserId,Extension1,Extension3,Extension4,CreateTime) values(@Tkey,@UserId,@phone,@time,@name,@CreateTime);";
            string userid = "";
            object o = Sqlhelperhd.ExecuteScalar("select UserId from Users where LoginName=@phone or UserMPhone=@phone;", new SqlParameter("@phone", phone));
            if (o != null)
            {
                userid = o.ToString();
            }
            else
            {
                o = Sqlhelperhd.ExecuteScalar("insert into Users(LoginName,UserMPhone)values(@phone,@phone) select @@IDENTITY;", new SqlParameter("@phone", phone));
                userid = o.ToString();
            }
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@UserId",userid),
            new SqlParameter("@phone",phone),
            new SqlParameter("@time",DateTime.Now.ToString("yyyy-MM-dd")),
            new SqlParameter("@name",name),
            new SqlParameter("@Tkey",20),//20  666活动页
            new SqlParameter("@CreateTime",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
            };
            Sqlhelperhd.ExecuteNonQuery(sql, arr);

        }
    }
}
