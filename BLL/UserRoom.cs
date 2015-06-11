using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Threading;
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

                //SqlHelper.ExecuteNonQuery("delete from UserRoom where extension1 is null and userId=@userid", new SqlParameter("@userid", userid));

                DeleteAll(userid);

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
        /// 添加房间模块，不清空以前的方案
        /// </summary>
        /// <param name="pra"></param>
        /// <returns></returns>
        public string AddRomExt(string pra)
        {
            //pra = "{\"userid\":\"10010\",\"kct\":\"2\",\"sf\":\"4\"}";
            try
            {


                object obj = JsonConvert.DeserializeObject(pra);

                Newtonsoft.Json.Linq.JObject js = obj as Newtonsoft.Json.Linq.JObject;//把上面的obj转换为 Jobject对象

                #region 得到用户id和需求id
                string userid = js["userid"].ToSafeString();

                //SqlHelper.ExecuteNonQuery("delete from UserRoom where extension1 is null and userId=@userid", new SqlParameter("@userid", userid));



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
            string sql = "update UserRoom set Extension1=@FrontCover,Extension3=@Unit,userRoomDes=@RoomDesp,szq=@Extension13,szh=@Extension14,mpt=@Rmpt,pmt=@Rpmt,extension4=@Extension4,roomId=@dide where id=@roomid";


            SqlParameter[] arr = new SqlParameter[] { 
             new SqlParameter("@FrontCover",room.FrontCover),
             new SqlParameter("@Unit",room.Unit),
             new SqlParameter("@RoomDesp",room.RoomDesp),
             new SqlParameter("@Extension13",room.Extension13),
             new SqlParameter("@Extension14",room.Extension14),
             new SqlParameter("@Rmpt",room.Rmpt),
             new SqlParameter("@Rpmt",room.Rpmt),
             new SqlParameter("@Extension4",room.Extension4),
             new SqlParameter("@roomid",roomid),
             new SqlParameter("@dide",did)

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
                        #region 异步执行线程
                        List<string> lis = new List<string> { DemandId, item["products"].ToSafeString() };
                        Thread t2 = new Thread(new ParameterizedThreadStart(UpJc));
                        t2.IsBackground = true;
                        t2.Start(lis);
                        #endregion
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
        /// 异步更新建材
        /// </summary>
        /// <param name="DemandId"></param>
        /// <param name="item"></param>
        private void UpJc(object o)
        {


            List<string> lis = o as List<string>;
            string DemandId = lis[0];
            string item = lis[1];


            JArray products = JArray.Parse(item);
            foreach (var iteme in products)
            {
                string oldpid = iteme["oldproductid"].ToSafeString();//jiu'jian'c
                string newpid = iteme["newproductid"].ToSafeString();//新建材id
                string lx = iteme["tp"].ToSafeString();//墙面  地面  什么的

                UpdateProduct(oldpid, newpid, lx, DemandId);
            }
        }
        public static void TestMethod(object data)
        {
            string datastr = data as string;

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
        /// 删除用户所有方案
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="demandid"></param>
        /// <returns></returns>
        public string DeleteAll(string userid, string DemandId = "")
        {
            try
            {
                #region 获取需求id部分
                DemandId = GetDemandId(userid);
                if (DemandId.IsEmpty())
                {
                    DemandId = AddDemand(userid);
                }
                #endregion

                #region 删除操作
                string sql = @"begin tran
declare @error int
set @error=0
delete from UserRoom where userId=@userid  
set @error=@error+@@ERROR
delete from DemandYppCenter where TypeId=@demandid
set @error=@error+@@ERROR
delete from DemandShowRoomProduct where DemandShowroomId=@demandid
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
               new SqlParameter("@demandid",DemandId)
                
            };

                int v = SqlHelper.ExecuteNonQuery(sql, arr);



                return "{\"success\":\"true\",\"msg\":\"删除成功\"}"; ;
                #endregion
            }
            catch (Exception)
            {
                return "{\"success\":\"false\",\"msg\":\"删除失败\"}";

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

            object o = SqlHelper.ExecuteScalar("select Netprice from Products where PID=@pid", new SqlParameter("@pid", Newpid));
            #region 循环更新
            double dj = Convert.ToDouble(o);//单价

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


        /// <summary>
        /// 单个模版房间详情
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public string GetModelDetail(string did)
        {
            StringBuilder sb = new StringBuilder();
            string s = Commen.DataCache.GetCache(did).ToSafeString();

           // string s = "";
            if (!s.IsEmpty())
            {
                return s.Replace("m&sup2;", "㎡").Replace("平米", "㎡").Replace("dm", "顶面").Replace("ld", "地面").Replace("qm", "墙面").Replace("a顶面in","admin"); ;
            }
            else
            {
                #region 详细查询
                sb.Append("{");
                string room = new BLL.ModelRoom().GetModelRoomSExt(did);
                sb.Append("\"modleroom\":");
                sb.Append(room);
                sb.Append(",");
                sb.Append("\"jiancai\":");
                string zcstr = new BLL.ZC().GetZcMx(did);
                sb.Append(zcstr);
                sb.Append(",");
                sb.Append("\"gongyi\":");
                string gystr = new GY().GetGyMx(did);
                sb.Append(gystr);
                sb.Append("}");
                #endregion


                Commen.DataCache.SetCache(did, sb, DateTime.Now.AddMonths(1), TimeSpan.Zero);

                //    cache.Insert("DD", "滑动过期测试", null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(10));

                return sb.ToSafeString().Replace("m&sup2;", "㎡").Replace("平米", "㎡").Replace("dm", "顶面").Replace("ld", "地面").Replace("qm", "墙面").Replace("a顶面in", "admin");

            }
        }



        /// <summary>
        /// 用户房间图片清单
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetUserRoomQd(string userid)
        {


            string sql = @"select UserRoom.roomName, UserRoom.id, UserRoom.extension1 as frontcover,UserRoom.extension3 as mj,Room.unit as omj ,Room.Extension1 as gjjg

, (select SUM( CAST(Price as float)) from DemandShowRoomProduct where ProjectTypeId=cast(UserRoom.id as  nvarchar) )

 as jcjg from UserRoom left join Room on UserRoom.roomId=Room.did where userId=@userid  and frontCover is not null ";

            DataTable dt = SqlHelper.ExecuteDataTable(sql, new SqlParameter("@userid", userid));


            return dt;
        }


        /// <summary>
        /// 用户房间建材清单
        /// </summary>
        /// <param name="DemandShowroomId"></param>
        /// <returns></returns>
        public DataTable getUserJcmx(string DemandShowroomId)
        {
            string sql = @"select ProductId,SUM(CAST( Num as float) ) as num ,SUM( cast( Price as float)) as price ,max(Pname) as pname ,MAX(Unit) as unit

from DemandShowRoomProduct left join Products on DemandShowRoomProduct.ProductId=Products.PID  where DemandShowroomId=@DemandShowroomId group by ProductId
";
            return SqlHelper.ExecuteDataTable(sql, new SqlParameter("@DemandShowroomId", DemandShowroomId)); ;

        }


        /// <summary>
        /// 用户房间工艺清单
        /// </summary>
        /// <param name="DemandShowroomId"></param>
        /// <returns></returns>
        public DataTable getUseGymx(string DemandShowroomId)
        {
            string sql = @"select ProjectId,MAX(DemandYppCenter.Extension) as pname,SUM( CAST ( DemandYppCenter.Extension1 as float)) as mj ,SUM( CAST ( DemandYppCenter.Extension2 as float)) as price from DemandYppCenter
 left join product   on DemandYppCenter.projectid=product.productid
 where
 DemandYppCenter.TypeId=@DemandShowroomId and ProductAmount<>'杂费' group by ProjectId
";
            return SqlHelper.ExecuteDataTable(sql, new SqlParameter("@DemandShowroomId", DemandShowroomId)); ;

        }


        /// <summary>
        /// 装修清单
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string DecorateList(string userid)
        {

            System.Net.WebClient web = new System.Net.WebClient();
            web.DownloadString("http://www.mj100.com/userDiy/Default.aspx?userId="+userid);


            #region 获取需求id部分
            string DemandId = GetDemandId(userid);
            if (DemandId.IsEmpty())
            {
                DemandId = AddDemand(userid);
            }
            #endregion

            //CC820B
            //EB650E
            //TR2850B
            //衣帽间
            //多功能室
            //EC550E
            //影音室
            //次卧室
            //书房
            //储物间
            //景观阳台
            //CLD580B
            //ES520E
            //客,餐厅
            //主卧室
            //子女房
            //老人房
            //厨房
            //EB650B
            //TB1100E
            //客餐厅
            //CB780B
            //TS980B
            //儿童房
            //CS650B
            //客、餐厅
            //TLD800B
            //ER1950E
            //室外空间
            //TK2150B
            //CK2000E
            //TB1100B
            //CK2000B
            //CC820E
            //卧室
            //卫生间

            //厅 厨  卫  

            int t = 0;

            int c = 0;

            int w = 0;

            int ss = 0;

            //图片清单
            DataTable dtpic = GetUserRoomQd(userid);

            if (dtpic.Rows.Count > 0)
            {
                double jczj = 0;//建材总价

                double gyzj = 0;//工艺总价

                double tzj = 0;//总价

                double zmj = 0;//总面积



                #region 图片清单
                List<object> pics = new List<object>();

                for (int i = 0; i < dtpic.Rows.Count; i++)
                {
                    DataRow row = dtpic.Rows[i];
                    //roomName	id	frontcover	mj	omj 	gjjg	jcjg
                    string gjjg = row["gjjg"].ToSafeString();//工艺价格
                    string jcjg = row["jcjg"].ToSafeString();//建材价格
                    string omj = row["omj"].ToSafeString();//原始面积
                    string mj = row["mj"].ToSafeString();
                    double price = 0;
                    if (!gjjg.IsEmpty() && !jcjg.IsEmpty() && !mj.IsEmpty())
                    {
                        price = ((double.Parse(gjjg) + double.Parse(jcjg)) / double.Parse(omj)) * double.Parse(mj);
                        tzj += price;
                        jczj += ((double.Parse(jcjg)) / double.Parse(omj)) * double.Parse(mj);
                        zmj += double.Parse(mj);
                        gyzj += ((double.Parse(gjjg)) / double.Parse(omj)) * double.Parse(mj);
                    }

                    string url = row["frontcover"].ToSafeString().IsEmpty() ? "" : "http://www.mj100.com/UploadFile/610/" + row["frontcover"].ToSafeString();
                    var opic = new { pic = url, roomNmae = row["roomName"].ToSafeString(), mj = row["mj"].ToSafeString(), price = price.ToSafeString() };

                    if (opic.roomNmae.Contains("厅"))
                    {
                        t += 1;
                    }
                    else if (opic.roomNmae.Contains("厨"))
                    {
                        c += 1;
                    }
                    else if (opic.roomNmae.Contains("卫"))
                    {
                        w += 1;
                    }
                    else
                    {
                        ss += 1;
                    }

                    pics.Add(opic);
                }
                #endregion



                #region 建材清单
                //建材清单
                List<object> lisjc = new List<object>();
                DataTable dtjc = getUserJcmx(DemandId);
                for (int i = 0; i < dtjc.Rows.Count; i++)
                {
                    //ProductId	num	price	pname	unit

                    DataRow row = dtjc.Rows[i];

                    var objjc = new { pname = row["pname"].ToSafeString(), num = row["num"].ToSafeString(), unit = row["unit"].ToSafeString().Replace("m&sup2;", "㎡"), price = row["price"].ToSafeString() };
                    if (objjc.pname.Length > 0)
                    {
                        lisjc.Add(objjc);
                    }

                }
                #endregion



                #region 工艺清单
                //工艺清单
                List<object> listgy = new List<object>();
                DataTable dtgy = getUseGymx(DemandId);
                for (int i = 0; i < dtgy.Rows.Count; i++)
                {
                    //ProjectId	pname	mj	price

                    DataRow row = dtgy.Rows[i];

                    var objgy = new { pname = row["pname"].ToSafeString(), price = row["price"].ToSafeString() };

                    listgy.Add(objgy);

                }
                #endregion


                double avgprice = 0;
                #region 拼接字符串
                if (zmj.ToSafeString().IsEmpty() || zmj == 0)
                {
                    avgprice = 0;
                }
                else
                {
                    avgprice = tzj / zmj;
                }
                string title = "";
                if (ss != 0)
                {
                    title += new BLL.MoneyHelperExt(ss).Convert() + "室";
                }

                if (t != 0)
                {
                    title += new BLL.MoneyHelperExt(t).Convert() + "厅";
                }

                if (c != 0)
                {
                    title += new BLL.MoneyHelperExt(c).Convert() + "厨";
                }

                if (w != 0)
                {
                    title += new BLL.MoneyHelperExt(w).Convert() + "卫";
                }

                title = title.Replace("零室", "").Replace("零厅", "").Replace("零厨", "").Replace("零卫", "");
                StringBuilder sb = new StringBuilder();
                sb.Append("{");

                sb.Append("\"title\":\"" + title + "\",");

                sb.Append("\"totalprice\":\"" + tzj + "\",");

                sb.Append("\"totalarea\":\"" + zmj + "\",");

                sb.Append("\"averageprice\":\"" + avgprice + "\",");

                sb.Append("\"jczj\":\"" + jczj + "\",");

                sb.Append("\"gyzj\":\"" + gyzj + "\",");


                sb.Append("\"rooms\":");
                sb.Append(JsonConvert.SerializeObject(pics));
                sb.Append(",");


                sb.Append("\"jclist\":");
                sb.Append(JsonConvert.SerializeObject(lisjc));
                sb.Append(",");

                sb.Append("\"gylist\":");
                sb.Append(JsonConvert.SerializeObject(listgy));


                sb.Append("}");
                #endregion


                return sb.ToSafeString().Replace("m&sup2;", "㎡").Replace("平米", "㎡");
            }
            else
            {
                return "{\"msg\":\"你尚未配置任何房间\"}";
            }


        }


        /// <summary>
        /// 预约
        /// </summary>
        /// <returns></returns>
        public string MakeAppointment(string name, string phone, string userid, string code)
        {
            // name  mobile  uerid  code

            if (code.IsEmpty() || Commen.DataCache.GetCache("order" + phone).ToSafeString() != code)
            {
                return "{\"success\":\"false\",\"msg\":\"验证码错误\"}";
            }
            var tent = new
            {
                userid = userid,
                Tkey = "0",
                Extension4 = name,
                Extension1 = phone,
                Extension2 = "",
                Extension3 = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd"),
                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")   //㎡
            };

            #region MyRegion
            if (!userid.Contains("-"))
            {
                object o = SqlHelper.ExecuteScalar(" select UserId from Tentent where UserId=@UserId", new SqlParameter("@UserId", userid));

                if (o != null)
                {
                    return "{\"success\":\"true\",\"msg\":\"你已预约，无需再次预约\"}";
                }

                SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@UserId",userid),
            new SqlParameter("@Tkey",tent.Tkey),
            new SqlParameter("@Extension4",tent.Extension4),
            new SqlParameter("@Extension1",tent.Extension1),
            new SqlParameter("@Extension2",tent.Extension2),
            new SqlParameter("@Extension3",tent.Extension3),
            new SqlParameter("@CreateTime",tent.CreateTime)
            };
                SqlHelper.ExecuteNonQuery(@"insert into Tentent(UserId,Tkey,Extension4,Extension1,Extension2,Extension3,CreateTime)

values(@UserId,@Tkey,@Extension4,@Extension1,@Extension2,@Extension3,@CreateTime)", arr);
            }



            #endregion

            return "{\"success\":\"false\",\"msg\":\"预约成功\"}";
        }
    }


}
