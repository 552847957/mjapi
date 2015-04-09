using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
namespace BLL
{
    public class UserRoom
    {


        /// <summary>
        /// 添加需求
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string AddDemand(string UserId)
        {
            string sql = @" insert into DemandShowRooms (UserId,ProductId,ProjectId,Extension,CreateTime) 
                            values
                           (@UserId,@ProductId,@ProjectId,@Extension,@CreateTime) select @@IDENTITY;";

            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@UserId",UserId),
            new SqlParameter("@ProductId","0"),
            new SqlParameter("@ProjectId","0"),
            new SqlParameter("@Extension","未审核"),
             new SqlParameter("@CreateTime",DateTime.Now.ToString())
            };


            object o = SqlHelper.ExecuteScalar(sql, arr);
            return o.ToSafeString(); ;
        }

        /// <summary>
        /// 得到需求id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string GetDemandId(string UserId)
        {
            string sql = "select DemandShowroomId from DemandShowRooms where UserId=@UserId";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@UserId",UserId)
           
            };

            object o = SqlHelper.ExecuteScalar(sql, arr);
            return o.ToSafeString(); ;
        }


        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="pra"></param>
        /// <returns></returns>
        public string AddRom(string pra)
        {
            //pra = "{\"userid\":\"10010\",\"kct\":\"2\",\"sf\":\"4\"}";
            try
            {


                object obj = JsonConvert.DeserializeObject(pra);

                Newtonsoft.Json.Linq.JObject js = obj as Newtonsoft.Json.Linq.JObject;//把上面的obj转换为 Jobject对象

                #region 得到用户id和需求id
                string userid = js["userid"].ToSafeString();
                if (userid.IsEmpty() || userid == "0")
                {
                    return "{\"success\":\"false\",\"msg\":\"用户id不能为空\"}";
                }

                string DemandId = GetDemandId(userid);
                if (DemandId.IsEmpty())
                {
                    DemandId = AddDemand(userid);
                }
                #endregion

                List<object> lis = new List<object>();
                #region 循环添加
                foreach (var item in js)
                {
                    if (item.Key != "userid")
                    {
                        int count = Convert.ToInt32(item.Value.ToSafeString());
                        for (int i = 0; i < count; i++)
                        {
                            //加到数据库
                            string id = AddRoom(userid, DemandId, EXNumber(item.Key), EX(item.Key));

                            //"userroomid": "18998",
                            //"roomName": "客餐厅",
                            //"demandId": "2891",
                            //"FrontCover": "",
                            //"mj": ""

                            var or = new { userroomid = id, roomName = EX(item.Key), demandId = DemandId, FrontCover = "", mj = "" };
                            lis.Add(or);
                        }
                    }
                }
                #endregion


                return "{\"success\":\"true\",\"msg\":\"添加用户module成功\",\"data\":" + JsonConvert.SerializeObject(lis) + "}";

            }
            catch (Exception)
            {

                return "{\"success\":\"false\",\"msg\":\"添加用户module失败\"}";
            }
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="roomtype"></param>
        /// <returns></returns>
        public string EX(string roomtype)
        {
            string whereand = "";
            switch (roomtype.ToLower())
            {
                case "kct":
                    whereand = "客餐厅";
                    break;
                case "cf":
                    whereand = "厨房";
                    break;
                case "wsj":
                    whereand = "卫生间";
                    break;
                case "znf":
                    whereand = "子女房";
                    break;
                case "ws":
                    whereand = "卧室";
                    break;
                case "sf":
                    whereand = "书房";
                    break;
                default:
                    break;
            }

            return whereand;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="roomtype"></param>
        /// <returns></returns>
        public string EXNumber(string roomtype)
        {
            string whereand = "";
            switch (roomtype.ToLower())
            {
                case "kct":
                    whereand = "34";
                    break;
                case "cf":
                    whereand = "30";
                    break;
                case "wsj":
                    whereand = "35";
                    break;
                case "znf":
                    whereand = "14";
                    break;
                case "ws":
                    whereand = "75";
                    break;
                case "sf":
                    whereand = "5";
                    break;
                default:
                    break;
            }

            return whereand;
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="demandId"></param>
        /// <param name="roomTpId"></param>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public string AddRoom(string userId, string demandId, string roomTpId, string roomName)
        {
            string sql = @"insert into UserRoom(userId,demandId,roomTpId,roomName,createTime,RoomId)values(@userId,@demandId,@roomTpId,@roomName,@createTime,'0') select @@IDENTITY;";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@userId",userId),
            new SqlParameter("@demandId",demandId),
            new SqlParameter("@roomTpId",roomTpId),
            new SqlParameter("@roomName",roomName),
             new SqlParameter("@CreateTime",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
            };

            return SqlHelper.ExecuteScalar(sql, arr).ToSafeString();
        }





        /// <summary>
        /// 更新房间
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="mj"></param>
        /// <param name="did"></param>
        /// <returns></returns>
        public string UpdateUserRoom(string roomid, string mj, string did)
        {





            string sql1 = " select * from Room where did=@did";

            DataTable dt = SqlHelper.ExecuteDataTable(sql1, new SqlParameter("@did", did));


            DataRow row = dt.Rows[0];



            var room = new { FrontCover = row["FrontCover"], Unit = mj, RoomDesp = row["RoomDesp"], Extension13 = row["Extension13"], Extension14 = row["Extension14"], Rmpt = row["Rmpt"], Rpmt = row["Rpmt"], Extension4 = "s" };



            #region 更新房间
            string sql = "update UserRoom set Extension1=@FrontCover,Extension3=@Unit,userRoomDes=@RoomDesp,szq=@Extension13,szh=@Extension14,mpt=@Rmpt,pmt=@Rpmt,extension4=@Extension4 where id=@roomid";


            SqlParameter[] arr = new SqlParameter[] { 
             new SqlParameter("@FrontCover",room.FrontCover),
             new SqlParameter("@Unit",room.Unit),
             new SqlParameter("@RoomDesp",room.RoomDesp),
             new SqlParameter("@Extension13",room.Extension13),
             new SqlParameter("@Extension14",room.Extension14),
             new SqlParameter("@Rmpt",room.Rmpt),
             new SqlParameter("@Rpmt",room.Rpmt),
             new SqlParameter("@Extension4",room.Extension4),
              new SqlParameter("@roomid",roomid)

            };


            SqlHelper.ExecuteNonQuery(sql, arr);
            #endregion


            return "";


        }

        /// <summary>
        /// 使用房间
        /// </summary>
        /// <param name="userroomid">用户房间id</param>
        /// <param name="demandid">用户需求id</param>
        /// <param name="modelroomid">用户选择的房间id</param>
        public void UseModelRoom(string userroomid, string demandid, string did)
        {
            // @userroomid nvarchar(50),@demandid nvarchar(50),@modelroomid nvarchar(50)

            SqlParameter[] arr = new SqlParameter[] { 
            
            new SqlParameter("@userroomid",userroomid),
            new SqlParameter("@demandid",demandid),
            new SqlParameter("@modelroomid",did)
            };

            SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "proc_apisyfj", arr);
        }


        /// <summary>
        /// 使用房间
        /// </summary>
        /// <returns></returns>
        public string SyFj(string pra)
        {
            // { "userid":"10010","data":[ {"userroomid":"1", "did":"2", "mj":"100", "products":[] }]}
            try
            {


                #region 获取userid部分
                string userid = "";



                object obj = JsonConvert.DeserializeObject(pra);

                Newtonsoft.Json.Linq.JObject js = obj as Newtonsoft.Json.Linq.JObject;//把上面的obj转换为 Jobject对象

                userid = js["userid"].ToSafeString();
                #endregion

                #region 获取需求id部分
                string DemandId = GetDemandId(userid);

                if (DemandId.IsEmpty())
                {
                    DemandId = AddDemand(userid);
                }
                #endregion




                JArray jarr = JArray.Parse(js["data"].ToSafeString());

                foreach (var item in jarr)
                {


                    string userroomid = item["userroomid"].ToSafeString();
                    string did = item["did"].ToSafeString();
                    string mj = item["mj"].ToSafeString();

                    UpdateUserRoom(userroomid, mj, did);

                    UseModelRoom(userroomid, DemandId, did);

                    #region 更新建材部分
                    if (!item["products"].ToSafeString().IsEmpty())
                    {
                        JArray products = JArray.Parse(item["products"].ToSafeString());
                        foreach (var iteme in products)
                        {
                            string oldpid = item["oldproductid"].ToSafeString();//jiu'jian'c
                            string newpid = item["newproductid"].ToSafeString();//新建材id
                            string lx = item["tp"].ToSafeString();//墙面  地面  什么的

                            UpdateProduct(oldpid, newpid, lx, DemandId);
                        }
                    }
                    #endregion



                }



                return "{\"success\":\"true\",\"msg\":\"使用成功\"}"; ;
            }
            catch (Exception e)
            {
                return "{\"success\":\"false\",\"msg\":\"失败,出现异常" + e.Message + "\"}"; ;

            }

        }


        /// <summary>
        /// 得到用户模块列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserModule(string userid)
        {

            #region 执行sql
            string sql = "select id as userroomid ,roomName,demandId,extension1,extension3 from UserRoom where userId=@userid";

            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@userid",userid)
            };
            #endregion

            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);
            string htp = "http://www.mj100.com/UploadFile/610/";
            List<object> lis = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //select id as userroomid ,roomName,demandId,extension1,extension3 from UserRoom where userId=701
                DataRow row = dt.Rows[i];

                var model = new { userroomid = row["userroomid"].ToSafeString(), roomName = row["roomName"].ToSafeString(), demandId = row["demandId"].ToSafeString(), FrontCover = row["extension1"].ToSafeString().IsEmpty() ? "" : htp + row["extension1"].ToSafeString(), mj = row["extension3"].ToSafeString() };


                lis.Add(model);

            }





            return JsonConvert.SerializeObject(lis);
        }


        /// <summary>
        /// 删除某一个用户的单独模块
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="userromid"></param>
        /// <returns></returns>
        public string DeleteSingleModule(string userid, string userromid)
        {

            try
            {
                #region 删除操作
                string sql = @"begin tran
declare @error int
set @error=0
delete from UserRoom where userId=@userid and id=@userroomid
set @error=@error+@@ERROR
delete from DemandYppCenter where projectTypeId=@userroomid
set @error=@error+@@ERROR
delete from DemandShowRoomProduct where ProjectTypeId=@userroomid
set @error=@error+@@ERROR
if @error>0
begin
rollback tran
end
else
begin
commit tran
end";


                SqlParameter[] arr = new SqlParameter[]
            {
               new SqlParameter("@userid",userid),
               new SqlParameter("@userroomid",userromid)
                
            };

                int v = SqlHelper.ExecuteNonQuery(sql, arr);



                return "{\"success\":\"true\",\"msg\":\"删除" + userromid + "成功\"}"; ;
                #endregion
            }
            catch (Exception)
            {
                return "{\"success\":\"false\",\"msg\":\"删除" + userromid + "失败\"}";

            }



        }


        /// <summary>
        /// 更新建材
        /// </summary>
        /// <param name="Pid"></param>
        /// <param name="Newpid"></param>
        /// <param name="lx"></param>
        /// <param name="DemandShowroomId"></param>
        /// <returns></returns>
        public string UpdateProduct(string Pid, string Newpid, string lx, string DemandShowroomId)
        {


            #region 得到老建材
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@ProductId",Pid),
            new SqlParameter("@lx",lx),
            new SqlParameter("@DemandShowroomId",DemandShowroomId)
            };
            string sql = "select * from DemandShowRoomProduct where ProductId=@ProductId   and DemandShowroomId=@DemandShowroomId";
            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);
            #endregion


            #region 循环更新
            double dj = Convert.ToInt32(SqlHelper.ExecuteScalar("select Netprice from Products where PID=@pid", new SqlParameter("@pid", Newpid)));//单价

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                string SroomPId = row["SroomPId"].ToSafeString();//用户房间建材id

                string num = row["num"].ToSafeString();//总量

                double zj = double.Parse(num) * dj;//总价

                string sqlupdate = "update [DemandShowRoomProduct] set ProductId=@ProductId,Price=@Price where SroomPId=@SroomPId";

                SqlHelper.ExecuteNonQuery(sqlupdate, new SqlParameter("@ProductId", Newpid), new SqlParameter("@Price", zj), new SqlParameter("@SroomPId", SroomPId));

            }
            #endregion


            return "";

        }

    }


}
