using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
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
    }
}
