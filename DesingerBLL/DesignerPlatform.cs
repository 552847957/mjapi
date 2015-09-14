using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

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

                #region 标签
                string tags = "";
                Regex re = new Regex("标签：(.+)$",RegexOptions.Singleline);
                Match m = re.Match(row["gyzj"].ToSafeString());

                if (m.Success)
                {
                    tags = m.Groups[1].Value;
                } 
                #endregion


                var desinger = new { errorcode = 0, desingerid = row["id"].ToSafeString(), name = row["did"].ToSafeString(), email = row["Extension2"].ToSafeString(), phone = row["mphone"].ToSafeString(), headimg = "http://www.mj100.com/GEEKPRO/img/head/" + (row["extension3"].ToSafeString().IsEmpty() ? "" : row["extension3"].ToSafeString()), servicerange = row["Dgrade"].ToSafeString().Replace(" ","").Replace("\n",""), mjrz = row["mjRztz"].ToSafeString().IsEmpty() ? 0 : 1, smrz = row["examine"].ToSafeString() == "通过" ? 1 : 0,ordernumber=3,programnumber=8,collectionnumber=3,tags=tags };


               

                return JsonConvert.SerializeObject(desinger);



            }
        }

        /// <summary>
        /// 订单管理页数据
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public static string OrderManagementData(string desingerid)
        {
            if (CheckParm(new Dictionary<string, string>() {{"desingerid",desingerid} }))
            {
                return errormsg;
            }

            #region 构造sql
            string sql = @"    select a.Extension5, a.DemandShowroomId, a.UserId, a.Extension2, a.CreateTime,WebChartUser.functionrooms,WebChartUser.area,WebChartUser.themes,WebChartUser.budget,
  WebChartUser.sendid from (select top 100 * from DemandShowRooms where Extension15='" +desingerid+@"'  order by
 
  DemandShowroomId desc) a left join WebChartUser on a.UserId=WebChartUser.userid"; 
            #endregion


            DataTable dt = SqlHelper.ExecuteDataTable(sql);

           


            List<object> lis = new List<object>();
            

            var obj = new { errorcode = 0, all = 41, newest = 2, orders = 1, complete = 38, data = lis };



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row=dt.Rows[i];



                var dic = GetPhoneAndYytime(row["UserId"].ToSafeString());
                #region 订单实体项
                var item = new
                {
                    urserid = row["UserId"].ToSafeString(),
                    demandid = Convert.ToDateTime(row["CreateTime"].ToSafeString().IsEmpty() ? DateTime.Now.AddDays(-1).ToString() : row["CreateTime"].ToSafeString()).ToString("MM月dd日 HH:mm"),
                    createtime = row["DemandShowroomId"].ToSafeString(),
                    address = row["extension2"].ToSafeString(),
                    area = row["area"].ToSafeString(),
                    theme = row["themes"].ToSafeString(),
                    budget = row["budget"].ToSafeString(),
                    phone = dic["phone"],
                    functionrooms = row["functionrooms"].ToSafeString(),
                    issend = row["sendid"].ToSafeString() == row["UserId"].ToSafeString()?1:0,
                    orderid = row["DemandShowroomId"].ToSafeString(),
                    layoutpic = row["extension5"].ToSafeString().IsEmpty() ? "http://mobile.mj100.com/HMobile/images/eg.png" : row["extension5"].ToSafeString(),
                    timeofappointment = dic["timeofappointment"],
                    shoupic=row["themes"].ToSafeString().IsEmpty()?0:1,
                    isorder= dic["isorder"]
                };
                #endregion

                lis.Add(item);
            }




            return JsonConvert.SerializeObject(obj);
        }


        public static Dictionary<string, string> GetPhoneAndYytime(string userid)
        {
            DataTable t1 = SqlHelper.ExecuteDataTable("select * from Users where  UserId='"+userid+"';");

            DataTable t2 = SqlHelper.ExecuteDataTable("select * from Tentent   where UserId='"+userid+"' ");


            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (t1.Rows.Count>0)
            {
                dic.Add("phone", t1.Rows[0]["UserMPhone"].ToSafeString());

            }
            else
            {
                dic.Add("phone", "");
            }

            if (t2.Rows.Count > 0)
            {
                dic.Add("timeofappointment", t2.Rows[0]["Extension3"].ToSafeString());

                dic.Add("isorder", "1");
            }
            else {
                dic.Add("timeofappointment", "");
                dic.Add("isorder", "0");
            }

           


            return dic;
        }

        /// <summary>
        /// 主题套装列表
        /// </summary>
        /// <param name="roomname">房间</param>
        /// <param name="theme">主题</param>
        /// <param name="housetype">户型</param>
        /// <param name="area">面积</param>
        /// <returns></returns>
        public static string Topiclist(string roomname, string theme, string housetype, string area)
        {

            #region 主题套装
            string sql = " select * from  dbo.Showroom where Extension='已审核'";

            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            #endregion

            #region 构造主题套装集合
            List<object> lis = new List<object>();
            string htp = "http://www.mj100.com/UploadFile/610/";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                var obj = new { name = row["Extension2"].ToSafeString(), pic = htp + row["Extension5"].ToSafeString(), area = row["Extension11"].ToSafeString(), totalprice = "8888", roomids = row["Extension3"].ToSafeString().TrimStart(',').TrimEnd(','), issuit = 1 };

                lis.Add(obj);
            }
            #endregion


            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append("\"errorcode\":0,");

            sb.Append("\"data\":" + JsonConvert.SerializeObject(lis) + "");

            sb.Append("}");


            return sb.ToSafeString();
        }

        /// <summary>
        /// 套装详情
        /// </summary>
        /// <param name="roomids"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Packagedetails(string roomids, string name)
        {

            if (CheckParm(new Dictionary<string, string>() { { "roomids", roomids }, { "name", name } }))
            {
                return errormsg;
            }

            if (!Regex.IsMatch(roomids, @"^(\d+,)+\d+$|^\d+$"))
            {
                return "{\"errcode\": 1,\"msg\": \"roomids格式不合法\"}";
            }


            #region 构造sql
            string sql = @"select  roomId, did,Extension5 as roomtype, roomName,frontCover,Extension13,Extension14, Room.roomDesp,
 round((CAST(Extension1 as float)+CAST(Extension2 as float))/CAST(unit as float),0) as price,unit as mj from Room where budget=0 and Extension4='已审核'  
 and Extension6 is null  and did in (" + roomids + ")";

            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            #endregion



            List<object> lis = new List<object>();


            #region MyRegion
            for (int d = 0; d < dt.Rows.Count; d++)
            {
                DataRow row = dt.Rows[d];

                string htp = "http://www.mj100.com/UploadFile/610/";
                string arrstr = row["Extension14"].ToSafeString() + row["Extension13"].ToSafeString();

                List<string> lispic = new List<string>();
                string[] arr = arrstr.Split(new char[] { ',', '’' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arr.Length; i++)
                {
                    lispic.Add(htp + arr[i]);
                }
                var room = new { did = row["did"].ToSafeString(), roomtype = row["roomtype"].ToSafeString(), roomName = row["roomName"].ToSafeString(), price = S(row["price"].ToSafeString()), mj = row["mj"].ToSafeString(), frontCover = htp + row["frontCover"].ToSafeString(), pics = lispic, des = row["roomDesp"].ToSafeString(), zcmx = GetZcMx(row["did"].ToSafeString()), gymx = GetGyMx(row["did"].ToSafeString()) };

                lis.Add(room);

            }
            #endregion





            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append("\"errorcode\":0,");
            sb.Append("\"name\":\"" + name + "\",");
            sb.Append("\"jczj\":\"" + 8000 + "\",");
            sb.Append("\"zmj\":\"" + 250 + "\",");
            sb.Append("\"des\":\"" + "注意看这里是房间描述" + "\",");
            sb.Append("\"gyzj\":\"" + 18000 + "\",");
            sb.Append("\"data\":" + JsonConvert.SerializeObject(lis) + "");


            sb.Append("}");

            return sb.ToSafeString();
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <returns></returns>
        public string AddallRoom(string pra)
        {


            //{"desingerid":"设计师id","pic":"","name":"","totlearea":"总面积","totleprice":"总价格","data":[{"room":"kct","did":"4","area":"100"},{"room":"kct","did":"4"}]}

            // string id = AddRoom(userid, DemandId, EXNumber(item.Key), EX(item.Key));


            object obj = JsonConvert.DeserializeObject(pra);

            Newtonsoft.Json.Linq.JObject js = obj as Newtonsoft.Json.Linq.JObject;//把上面的obj转换为 Jobject对象


            #region 得到用户id和需求id

            string pic = js["pic"].ToSafeString();
            string name = js["name"].ToSafeString();
            string totlearea = js["totlearea"].ToSafeString();
            string totleprice = js["totleprice"].ToSafeString();

            string desingerid = js["desingerid"].ToSafeString();
            string userid = "";
            string DemandId = "";
            if (DemandId.IsEmpty())
            {
                #region 新增用户后立即删除
                string sql = "insert into Users (LoginName) values(@id) select @@IDENTITY";

                //插入另外一张表    立即删除此条记录

                userid = SqlHelper.ExecuteScalar(sql, new SqlParameter("@id", "")).ToSafeString();


                SqlHelper.ExecuteNonQuery("insert into TempZj  (usernumber,userphone) values('" + userid + "','d" + desingerid + "');delete from Users where UserId='" + userid + "' ");
                #endregion


                DemandId = AddDemand(userid);


                AddCol(desingerid, DemandId, pic, name, totlearea, totleprice, "1", "0", "");
            }







            #endregion


            JArray jarr = JArray.Parse(js["data"].ToSafeString());

            foreach (var item in jarr)
            {
                string room = item["room"].ToSafeString();
                string did = item["did"].ToSafeString();
                string area = item["area"].ToSafeString();
                string id = AddRoom(userid, DemandId, EXNumber(room), EX(room));
                UpdateUserRoom(id, area, did);
                UseModelRoom(id, DemandId, did);
            }
            return "{\"errcode\":0,\"msg\":\"使用成功\"}";
        }

        /// <summary>
        /// 得到模版房间列表
        /// </summary>
        /// <returns></returns>
        public static string GetModelRoomS2(string roomtype, string price)
        {
            //select roomId from UserRoom where id=@userroomid;
            #region 参数判断
            string desc = "asc";
            if (!string.IsNullOrEmpty(price))
            {
                desc = "desc";
            }
            string whereand = "";
            if (!string.IsNullOrEmpty(roomtype))
            {
                switch (roomtype.ToLower())
                {
                    case "kct":
                        whereand = "and roomid=34";
                        break;
                    case "cf":
                        whereand = "and roomid=30";
                        break;
                    case "wsj":
                        whereand = "and roomid=35";
                        break;
                    case "znf":
                        whereand = "and roomid=14";
                        break;
                    case "ws":
                        whereand = "and roomid=75";
                        break;
                    case "sf":
                        whereand = "and roomid=5";
                        break;
                    default:
                        break;
                }
            }
            #endregion


            #region 查询
            string sql = @"select  roomId, did,Extension5 as roomtype, roomName,frontCover,Extension13,Extension14,
 round((CAST(Extension1 as float)+CAST(Extension2 as float))/CAST(unit as float),0) as price,unit as mj from Room where budget=0 and Extension4='已审核'  
 and Extension6 is null  " + whereand + " order by price  " + desc + ";";


            SqlParameter[] parr = new SqlParameter[] { 
            
            };
            DataTable dt = SqlHelper.ExecuteDataTable(sql);

            List<object> lis = new List<object>();
            string flagroomid = "0";



            #region MyRegion
            for (int d = 0; d < dt.Rows.Count; d++)
            {
                DataRow row = dt.Rows[d];

                string htp = "http://www.mj100.com/UploadFile/610/";
                string arrstr = row["Extension14"].ToSafeString() + row["Extension13"].ToSafeString();

                List<string> lispic = new List<string>();
                string[] arr = arrstr.Split(new char[] { ',', '’' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arr.Length; i++)
                {
                    lispic.Add(htp + arr[i]);
                }
                var room = new { did = row["did"].ToSafeString(), roomtype = row["roomtype"].ToSafeString(), roomName = row["roomName"].ToSafeString(), price = S(row["price"].ToSafeString()), mj = row["mj"].ToSafeString(), frontCover = htp + row["frontCover"].ToSafeString(), pics = lispic, beused = "0" };

                if (room.did != flagroomid)
                {
                    lis.Add(room);

                }

            }
            #endregion

            #endregion

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"data\":" + JsonConvert.SerializeObject(lis) + "");
            sb.Append("}");


            return sb.ToSafeString();
        }


        /// <summary>
        /// 备选房间方案详情
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public static string GetModelDetail(string did)
        {
            StringBuilder sb = new StringBuilder();
            string s = Commen.DataCache.GetCache(did).ToSafeString();

            // string s = "";
            if (!s.IsEmpty())
            {
                return s.Replace("m&sup2;", "㎡").Replace("平米", "㎡").Replace("dm", "顶面").Replace("ld", "地面").Replace("qm", "墙面").Replace("a顶面in", "admin"); ;
            }
            else
            {
                #region 详细查询
                sb.Append("{");
                sb.Append("\"errorcode\":0,");
                string room = new BLL.ModelRoom().GetModelRoomSExt(did);
                sb.Append("\"modleroom\":");
                sb.Append(room);
                sb.Append(",");
                sb.Append("\"jiancai\":");
                string zcstr = new BLL.ZC().GetZcMx(did);
                sb.Append(zcstr);
                sb.Append(",");
                sb.Append("\"gongyi\":");
                string gystr = new BLL.GY().GetGyMx(did);
                sb.Append(gystr);
                sb.Append("}");
                #endregion


                Commen.DataCache.SetCache(did, sb, DateTime.Now.AddMonths(1), TimeSpan.Zero);

                //    cache.Insert("DD", "滑动过期测试", null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(10));

                return sb.ToSafeString().Replace("m&sup2;", "㎡").Replace("平米", "㎡").Replace("dm", "顶面").Replace("ld", "地面").Replace("qm", "墙面").Replace("a顶面in", "admin");

            }
        }


        /// <summary>
        /// 我的搭配/设计师自己的搭配
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public static string Myplan(string desingerid)
        {

            DataTable dt = SqlHelper.ExecuteDataTable("select * from Collectionproject where desingerid='" + desingerid + "' and issave=1 and isCollection=0;");
            List<object> lis = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                //   id	desingerid	demandid	pic	name	area	totleprice	issave	isCollection	collectionids	createtime

                var obj = new {id=row["id"].ToSafeString(), name = row["name"].ToSafeString(), pic = row["pic"].ToSafeString(), area = row["area"].ToSafeString(), totleprice = row["totleprice"].ToSafeString(), demandid = row["demandid"].ToSafeString() };

                lis.Add(obj);

            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"count\":" + dt.Rows.Count + ",");

            sb.Append("\"data\":" + JsonConvert.SerializeObject(lis) + ",");
            sb.Append("}");

            return sb.ToSafeString();
        }


        /// <summary>
        /// 我的收藏/设计师自己的收藏
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public static string Mycollection(string desingerid)
        {

            DataTable dt = SqlHelper.ExecuteDataTable("select * from Collectionproject where desingerid='" + desingerid + "'   and isCollection=1;");
            List<object> lis = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                //   id	desingerid	demandid	pic	name	area	totleprice	issave	isCollection	collectionids	createtime

                var obj = new {id=row["id"].ToSafeString(), name = row["name"].ToSafeString(), pic = row["pic"].ToSafeString(), area = row["area"].ToSafeString(), totleprice = row["totleprice"].ToSafeString(), roomids = row["collectionids"].ToSafeString() };

                lis.Add(obj);

            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"count\":" + dt.Rows.Count + ",");

            sb.Append("\"data\":" + JsonConvert.SerializeObject(lis) + ",");
            sb.Append("}");

            return sb.ToSafeString();
        }

        /// <summary>
        /// 推荐主材
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public static string RecommendZc(string productid)
        {

            string sql = @" select top 6 a.BID , PID,Unit,Netprice,Pname,Pmodel,  gg, 

 smallpic ,Brand.Bname from
(select top 6 BID , PID,Unit,Netprice,Pname,Pmodel,extension1 as gg, 

extension as smallpic from Products where MOID in(
select MOID from Products where PID=@productid
) and pid<>@productid) as a left join Brand on a.BID=Brand.BID";

            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@productid",productid)
            };
            List<object> lis = new List<object>();

            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                var zxobj = new { productid = row["pid"].ToSafeString(), unit = row["unit"].ToSafeString(), netprice = row["netprice"].ToSafeString(), pname = row["pname"].ToSafeString(), bnmae = row["bname"].ToSafeString(), pmodel = row["pmodel"].ToSafeString(), gg = row["gg"].ToSafeString(), smallpic = "http://www.mj100.com/admin/UploadFile/550/" + GetPic(row["smallpic"].ToSafeString()).Replace("\\", "/") };



                lis.Add(zxobj);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"data\":" + JsonConvert.SerializeObject(lis) + "");
            sb.Append("}");
            return sb.ToSafeString();
        }

        /// <summary>
        /// 得到设计师模块列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserModule(string tempid)
        {

            #region 执行sql
            string sql = @"select  CAST(extension3 as float)*b.dj as totleprice, * from UserRoom left join (
select did, roomName xname,round(  (CAST( Extension1 as  float)+CAST( Extension2 as float))/CAST( unit as float) ,0)  as dj from Room 
)b on UserRoom.roomId=b.did where demandId='" + tempid + "' ";


            #endregion

            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            string htp = "http://www.mj100.com/UploadFile/610/";
            List<object> lis = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow row = dt.Rows[i];

                var model = new { userroomid = row["id"].ToSafeString(), roomName = row["roomName"].ToSafeString(), demandId = row["demandId"].ToSafeString(), FrontCover = row["extension1"].ToSafeString().IsEmpty() ? "" : htp + row["extension1"].ToSafeString(), mj = row["extension3"].ToSafeString(), totleprice = row["totleprice"].ToSafeString(), theme = row["xname"].ToSafeString() };


                lis.Add(model);

            }


            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"data\":" + JsonConvert.SerializeObject(lis) + "");

            sb.Append("}");



            return sb.ToSafeString();
        }


        /// <summary>
        /// 用户实名认证
        /// </summary>
        /// <param name="desingerid">设计师id</param>
        /// <param name="name">实名</param>
        /// <param name="idnumber">身份证号</param>
        /// <param name="idz">身份证正面</param>
        /// <param name="idf">身份证反面</param>
        /// <returns></returns>
        public static string UpdateDesinger(string desingerid, string name, string idnumber, string idz, string idf)
        {
            if (CheckParm(new Dictionary<string, string>() {{"desingerid",desingerid},{"name",name},{"idnumber",idnumber} }))
            {
                return errormsg;
            }


            #region 更新代码
            string sql = "update DesignerGrade set DID=@DID ,IdNumber=@IdNumber, IdNumberZ=@IdNumberZ, IdNumberF=@IdNumberF, examine='审核中' where ID=@ID";

            SqlParameter[] arr = new SqlParameter[]{
            
            new SqlParameter("@DID",name),
             new SqlParameter("@IdNumber",idnumber),
              new SqlParameter("@IdNumberZ",idz),
               new SqlParameter("@IdNumberF",idf),
                new SqlParameter("@ID",desingerid)
            };
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"msg\":\"操作成功\"");

            sb.Append("}"); 
            #endregion



            return sb.ToSafeString();
          
        }


        /// <summary>
        /// 更改设计师标签
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public static string UpdateTags(string desingerid,string tags)
        {
            if (CheckParm(new Dictionary<string, string>() {{"desingerid",desingerid},{"tags",tags} }))
            {
                return errormsg;
            }
            object o = SqlHelper.ExecuteScalar("select GyZj from  DesignerGrade where  ID='"+desingerid+"'");

            string v = Regex.Replace(o.ToSafeString(), "标签：(.+)$", "");

            v += " 标签：" + tags;


           SqlHelper.ExecuteNonQuery("update DesignerGrade set GyZj='"+v+"' where ID='"+desingerid+"'");
           StringBuilder sb = new StringBuilder();

           sb.Append("{");
           sb.Append("\"errorcode\":0,");
           sb.Append("\"msg\":\"操作成功\"");

           sb.Append("}"); 
            
            return sb.ToSafeString();
        }


        #region 添加和使用备选方案
        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="pra"></param>
        /// <returns></returns>
        public string AddRom(string pra)
        {
            //pra = "{\"desingerid\":\"10010\","tempid":"55555",\"kct\":\"2\",\"sf\":\"4\"}";
            try
            {


                object obj = JsonConvert.DeserializeObject(pra);

                Newtonsoft.Json.Linq.JObject js = obj as Newtonsoft.Json.Linq.JObject;//把上面的obj转换为 Jobject对象

                #region 得到用户id和需求id

                string desingerid = js["desingerid"].ToSafeString();
                string userid = "";
                string DemandId = js["tempid"].ToSafeString();
                if (DemandId.IsEmpty())
                {
                    #region 新增用户后立即删除
                    string sql = "insert into Users (LoginName) values(@id) select @@IDENTITY";

                    //插入另外一张表    立即删除此条记录

                    userid = SqlHelper.ExecuteScalar(sql, new SqlParameter("@id", "")).ToSafeString();


                    SqlHelper.ExecuteNonQuery("insert into TempZj  (usernumber,userphone) values('" + userid + "','d" + desingerid + "');delete from Users where UserId='" + userid + "' ");
                    #endregion


                    DemandId = AddDemand(userid);


                    AddCol(desingerid, DemandId, "", "", "", "", "0", "0", "");
                }







                #endregion

                List<object> lis = new List<object>();
                #region 循环添加
                foreach (var item in js)
                {
                    if (item.Key != "desingerid" && item.Key != "tempid")
                    {
                        int count = Convert.ToInt32(item.Value.ToSafeString());
                        for (int i = 0; i < count; i++)
                        {
                            string id = AddRoom(userid, DemandId, EXNumber(item.Key), EX(item.Key));
                            var or = new { userroomid = id, roomName = EX(item.Key), demandId = DemandId, FrontCover = "", mj = "" };
                            lis.Add(or);
                        }
                    }
                }
                #endregion


                return "{\"errcode\":0,\"msg\":\"添加模块成功\",\"tempid\":\"" + DemandId + "\",\"data\":" + JsonConvert.SerializeObject(lis) + "}";

            }
            catch (Exception)
            {

                throw;
            }
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
        /// 添加收藏
        /// </summary>
        /// <param name="desingerid"></param>
        /// <param name="demandid"></param>
        /// <param name="pic"></param>
        /// <param name="name"></param>
        /// <param name="area"></param>
        /// <param name="totleprice"></param>
        /// <param name="issave"></param>
        /// <param name="isCollection"></param>
        /// <param name="collectionids"></param>
        /// <returns></returns>
        public string AddCol(string desingerid, string demandid, string pic, string name, string area, string totleprice, string issave, string isCollection, string collectionids)
        {

            string sql = @"insert into Collectionproject ( desingerid,  demandid,  pic,  name,  area,  totleprice,  issave,  isCollection,  collectionids) values
                              ( @desingerid, @demandid, @pic, @name, @area, @totleprice, @issave, @isCollection, @collectionids)";


            SqlParameter[] arr = new SqlParameter[] { 
            
            new  SqlParameter("@desingerid",desingerid),
             new  SqlParameter("@demandid",demandid),
              new  SqlParameter("@pic",pic),
               new  SqlParameter("@name",name),
                new  SqlParameter("@area",area),
                 new  SqlParameter("@totleprice",totleprice),
                  new  SqlParameter("@issave",issave),
                   new  SqlParameter("@isCollection",isCollection),
                    new  SqlParameter("@collectionids",collectionids)
            };

            return SqlHelper.ExecuteNonQuery(sql, arr).ToSafeString();
        }
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
        /// 使用房间
        /// </summary>
        /// <returns></returns>
        public string SyFj(string pra)
        {


            // { "tempid":"10010","data":[ {"userroomid":"1", "did":"2", "mj":"100", "products":[] }]}
            try
            {


                #region 获取userid部分




                object obj = JsonConvert.DeserializeObject(pra);

                Newtonsoft.Json.Linq.JObject js = obj as Newtonsoft.Json.Linq.JObject;//把上面的obj转换为 Jobject对象

                string DemandId = js["tempid"].ToSafeString();

                #endregion

                #region 获取需求id部分



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



                return "{\"errorcode\":0,\"msg\":\"使用成功\"}"; ;
            }
            catch (Exception e)
            {

                throw;

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
        /// 删除某个模块
        /// </summary>
        /// <param name="userromid"></param>
        /// <returns></returns>
        public static string DeleteSingleModule(string userromid)
        {

            try
            {
                #region 删除操作
                string sql = @"begin tran
declare @error int
set @error=0
delete from UserRoom where id=@userroomid
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
              
               new SqlParameter("@userroomid",userromid)
                
            };

                int v = SqlHelper.ExecuteNonQuery(sql, arr);



                return "{\"errorcode\":0,\"msg\":\"删除" + userromid + "成功\"}"; ;
                #endregion
            }
            catch (Exception)
            {
                throw;
            }



        }
        #endregion


        /// <summary>
        /// 删除收藏或者方案
        /// </summary>
        /// <param name="desingerid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteColOrPlan(string desingerid,string id)
        {

             string sql = "delete from Collectionproject where id='"+id+"'";

             SqlHelper.ExecuteNonQuery(sql);

             return "{\"errcode\":0,\"msg\":\"操作成功\"}";
        }




        /// <summary>
        /// copy方案  将
        /// </summary>
        /// <param name="nuserid">userid</param>
        /// <param name="newdemandid">新需求id</param>
        /// <param name="olddemandid">旧需求id</param>
        /// <returns></returns>
        public static string CopyDemand(string nuserid, string newdemandid, string olddemandid)
        {
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Pro_copydemand", new SqlParameter("@nuserid", ""), new SqlParameter("@newdemandid", ""), new SqlParameter("@olddemandid", "")).ToString();


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

        public static string S(string aa)
        {
            int a = int.Parse(aa);
            int y = a % 10;

            if (y <= 5)
            {
                a = a - y;
            }
            else
            {
                a = a + (10 - y);
            }

            return a.ToSafeString();
        }


        /// <summary>
        /// 得到主材明细
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public static List<object> GetZcMx(string did)
        {
            #region sql语句
            string sql = @"    
     select * from (
select  MAX( view_sp.projectTypeName) as name, sum(cast(num as float))  as Num ,sum(cast(Price as float)) as price,ShowroomId,Bname,Pmodel,unit,
netPrice,view_sp.extension,view_sp.gg ,Marketprice,pname,cName,ProductId,projectTypeId,pc.SmallPic,
tp from view_sp left join Pic as pc on pc.TypeId=ProductId and pc.PicId=(select MAX(PicId) 
from pic where Pic.TypeId=view_sp.ProductId) where showRoomid=@did and  
view_sp.Extension1='房间' group by ShowroomId,Bname,Pmodel,unit
,netPrice,view_sp.extension,view_sp.gg,Marketprice,pname,cName,ProductId,projectTypeId,SmallPic,tp) as b";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@did",did)
            };

            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);

            #endregion
            //     Num	price	ShowroomId	Bname	Pmodel	unit	netPrice	extension	Marketprice	pname	cName	ProductId	projectTypeId	SmallPic	tp


            #region 按组序列化



            List<object> lis = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];


                #region 实体
                var zxobj = new { productid = row["productid"].ToSafeString(), num = row["num"].ToSafeString(), price = row["price"].ToSafeString(), unit = row["unit"].ToSafeString(), tp = ExChange(row["tp"].ToSafeString()), netprice = row["netprice"].ToSafeString(), pname = row["pname"].ToSafeString(), bnmae = row["bname"].ToSafeString(), pmodel = row["pmodel"].ToSafeString(), gg = row["gg"].ToSafeString(), smallpic = "http://www.mj100.com/admin/UploadFile/550/" + GetPic(row["extension"].ToSafeString()).Replace("\\", "/") };
                #endregion
                lis.Add(zxobj);

            }
            #endregion

            string name = "";
            if (dt.Rows.Count > 0)
            {
                name = dt.Rows[0]["name"].ToSafeString();
            }
            StringBuilder sb = new StringBuilder();


            return lis;

        }



        /// <summary>
        /// 得到图片
        /// </summary>
        /// <param name="picarr"></param>
        /// <returns></returns>
        private static string GetPic(string picarr)
        {
            if (picarr.IsEmpty())
            {
                return "";
            }
            else
            {
                return picarr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

        }


        public static string ExChange(string input)
        {
            string returnstr = input;

            switch (input)
            {
                case "qm":
                    // returnstr = "墙面";//TheWall
                    returnstr = "TheWall";//TheWall
                    break;
                case "dm":
                    //  returnstr = "顶面";//TheTop
                    returnstr = "TheTop";//TheTop
                    break;
                case "ld":
                    //  returnstr = "地面";//TheGround
                    returnstr = "TheGround";//TheGround
                    break;
                case "jj":
                    //  returnstr = "洁具";//TheSanitary 
                    returnstr = "TheSanitary";//TheSanitary 
                    break;
                case "ju":
                    //  returnstr = "家具";//TheFurniture
                    returnstr = "TheFurniture";//TheFurniture
                    break;
                case "cj":
                    //  returnstr = "厨具";//ThekitchenWare
                    returnstr = "ThekitchenWare";//ThekitchenWare
                    break;
                default:
                    break;
            }

            return returnstr;
        }


        public static List<object> GetGyMx(string did)
        {
            #region sql语句
            string sql = @"select  types,YPPCenter.extension,YPPCenter.extension1 as univalent,unit,YPPCenter.extension2 as price,ProductAmount  from YPPCenter      left join product   on yppcenter.projectid=product.productid
 where YPPCenter.TypeId=@did ";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@did",did)
            };

            DataTable dt = SqlHelper.ExecuteDataTable(sql, arr);

            #endregion



            #region 按组序列化
            List<object> lis = new List<object>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                //types	extension	univalent	unit	price	ProductAmount

                var zxobj = new { tp = ExChange(row["types"].ToSafeString()), extension = row["extension"].ToSafeString(), univalent = row["univalent"].ToSafeString(), unit = row["unit"].ToSafeString(), price = row["price"].ToSafeString(), ProductAmount = row["ProductAmount"].ToSafeString(), };

                lis.Add(zxobj);
            }
            #endregion


            return lis;

        }
    }
}
