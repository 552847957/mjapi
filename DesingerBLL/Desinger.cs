using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace DesingerBLL
{
    public class Desinger
    {
        protected static string errormsg = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendMsg(string phone)
        {


            object o = SqlHelper.ExecuteScalar("select COUNT(*) from DesignerGrade where Extension5='" + phone + "' or mPhone='" + phone + "';");
            if (Convert.ToInt32(o) < 1)
            {
                #region 发送随机短信
                Random r = new Random();

                string s = r.Next(100000, 999999).ToString();

                Commen.DataCache.SetCache("desinger" + phone, s);

                Commen.SendMsg.FSong(phone, "你好，您正在注册极客美家设计师，您的验证码是：" + s + ",此验证码也是你的初始登录密码，请勿丢失。");
                #endregion

                return "{\"errorcode\":\"0\",\"msg\":\"验证码发送成功\"}";
            }
            else
            {
                return "{\"errorcode\":\"1\",\"msg\":\"此手机号已注册无需再次注册\"}";
            }









        }


        /// <summary>
        /// 注册设计
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int Adddesinger(string phone, string city, string yzm, string fromid)
        {


            string sql = "insert into DesignerGrade(Extension5,mPhone,Extension6,Dgrade,cTime,Extension7)values(@phone,@phone,@yzm,@city,'" + DateTime.Now.ToString("yyyy-MM-dd") + "',@fromid);";

            SqlParameter[] psarms = new SqlParameter[] { 
            new SqlParameter("@phone",phone),
             new SqlParameter("@yzm",yzm.To16Md5()),
            new SqlParameter("@city",city),
               new SqlParameter("@fromid",fromid)
            };
            return SqlHelper.ExecuteNonQuery(sql, psarms);





        }


        /// <summary>
        /// 设计师登录
        /// </summary>
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

                var desinger = new { errorcode = 0, desingerid = row["id"].ToSafeString(), name = row["did"].ToSafeString(), email = row["Extension2"].ToSafeString(), phone = row["mphone"].ToSafeString(), headimg = "http://www.mj100.com/GEEKPRO/img/head/" + (row["extension3"].ToSafeString().IsEmpty() ? "" : row["extension3"].ToSafeString()), role = "0" };

                return JsonConvert.SerializeObject(desinger);



            }
        }
        /// <summary>
        /// 近期要做的事
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public static string GetDesingerProjects(string desingerid)
        {
            #region 设计师所有的项目

            #region sql语句
            string sql = @" select * from 

(select * from XState where reTime is not null and extension1 in (select DemandShowroomId from DemandShowRooms where Extension15='" + desingerid + @"') )

 a
 
left join 

( 
select DemandShowroomId,Extension12,Extension16,Extension15 
from DemandShowRooms  where 
Extension15='" + desingerid + @"' and Extension12 is not null and Extension16 is not null
) 

b

on a.extension1=b.DemandShowroomId 


where DemandShowroomId is not null order by rzId desc";
            #endregion

            DataTable dt = SqlHelper.ExecuteDataTable(sql, new SqlParameter("@desingerid", desingerid));



            // rzId	rbName	rxName	rbTime	reTime	rlx	pics	extensioin	extension1	extension2	createTime	DemandShowroomId	Extension12	Extension16	Extension15


            #endregion

            #region 填充实体对象
            List<Project> lis = new List<Project>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                Project p = new Project()
                {
                    RzId = row["RzId"].ToSafeString(),
                    createTime = row["createTime"].ToSafeString(),
                    DemandShowroomId = row["DemandShowroomId"].ToSafeString(),
                    extensioin = row["extensioin"].ToSafeString(),
                    extension1 = row["extension1"].ToSafeString(),
                    Extension12 = row["Extension12"].ToSafeString(),
                    Extension15 = row["Extension15"].ToSafeString(),
                    Extension16 = row["Extension16"].ToSafeString(),
                    extension2 = row["extension2"].ToSafeString(),
                    pics = row["pics"].ToSafeString(),
                    rbName = row["rbName"].ToSafeString(),
                    rbTime = row["rbTime"].ToSafeString().IsEmpty() ? row["reTime"].ToSafeString() : row["rbTime"].ToSafeString(),
                    reTime = row["reTime"].ToSafeString(),
                    rlx = row["rlx"].ToSafeString(),
                    rxName = row["rxName"].ToSafeString(),
                    index = Math.Ceiling((DateTime.Now - Convert.ToDateTime(row["Extension16"].ToSafeString())).TotalDays)


                };
                p.latestdays = "," + p.index + "," + (p.index + 1) + "," + (p.index + 2) + "," + (p.index + 3) + "," + (p.index + 4) + "," + (p.index + 5) + "," + (p.index + 6) + "," + (p.index + 7) + ",";

                lis.Add(p);
            }
            #endregion

            StringBuilder sb = GetOneDayProjectsExt(lis);


            return sb.ToSafeString();



        }

        /// <summary>
        /// 今天要做的事
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public static string GetTodaythings(string desingerid)
        {
            #region 设计师所有的项目

            #region sql语句
            string sql = @" select * from 

(select * from XState where reTime is not null and extension1 in (select DemandShowroomId from DemandShowRooms where Extension15='" + desingerid + @"') )

 a
 
left join 

( 
select DemandShowroomId,Extension12,Extension16,Extension15 
from DemandShowRooms  where 
Extension15='" + desingerid + @"' and Extension12 is not null and Extension16 is not null
) 

b

on a.extension1=b.DemandShowroomId 


where DemandShowroomId is not null order by rzId desc";
            #endregion

            DataTable dt = SqlHelper.ExecuteDataTable(sql, new SqlParameter("@desingerid", desingerid));



            // rzId	rbName	rxName	rbTime	reTime	rlx	pics	extensioin	extension1	extension2	createTime	DemandShowroomId	Extension12	Extension16	Extension15


            #endregion

            #region 填充实体对象
            List<Project> lis = new List<Project>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                Project p = new Project()
                {
                    RzId = row["RzId"].ToSafeString(),
                    createTime = row["createTime"].ToSafeString(),
                    DemandShowroomId = row["DemandShowroomId"].ToSafeString(),
                    extensioin = row["extensioin"].ToSafeString(),
                    extension1 = row["extension1"].ToSafeString(),
                    Extension12 = row["Extension12"].ToSafeString(),
                    Extension15 = row["Extension15"].ToSafeString(),
                    Extension16 = row["Extension16"].ToSafeString(),
                    extension2 = row["extension2"].ToSafeString(),
                    pics = row["pics"].ToSafeString(),
                    rbName = row["rbName"].ToSafeString(),
                    rbTime = row["rbTime"].ToSafeString().IsEmpty() ? row["reTime"].ToSafeString() : row["rbTime"].ToSafeString(),
                    reTime = row["reTime"].ToSafeString(),
                    rlx = row["rlx"].ToSafeString(),
                    rxName = row["rxName"].ToSafeString(),
                    index = Math.Ceiling((DateTime.Now - Convert.ToDateTime(row["Extension16"].ToSafeString())).TotalDays)


                };
                p.latestdays = "," + p.index + "," + (p.index + 1) + "," + (p.index + 2) + "," + (p.index + 3) + "," + (p.index + 4) + "," + (p.index + 5) + "," + (p.index + 6) + "," + (p.index + 7) + ",";

                lis.Add(p);
            }
            #endregion

            StringBuilder sb = GetOneDayProjects(lis);


            return sb.ToSafeString();



        }

        /// <summary>
        /// 得到今天的数据
        /// </summary>
        /// <param name="lis"></param>
        /// <returns></returns>
        private static StringBuilder GetOneDayProjects(List<Project> lis)
        {
            var lis2 = lis.Where(n => { return n.rlx == "jd" && n.index <= Convert.ToInt32(n.reTime) && n.index >= Convert.ToInt32(n.rbTime); });

            #region 分组
            Dictionary<string, Project> dic = new Dictionary<string, Project>();
            foreach (var item in lis2)
            {
                if (!dic.ContainsKey(item.extension1))
                {
                    dic.Add(item.extension1, item);
                }
            }
            #endregion


            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            foreach (var item in dic)
            {
                sb.Append("{");
                sb.Append("\"projectid\":\"" + item.Value.extension1 + "\",");
                sb.Append("\"projectname\":\"" + item.Value.Extension12 + "\",");
                sb.Append("\"projectbegintime\":\"" + item.Value.Extension16 + "\",");
                var projects = lis.Where(n => { return n.rlx == "jd" && n.index <= Convert.ToInt32(n.reTime) && n.index >= Convert.ToInt32(n.rbTime) && n.extension1 == item.Value.extension1; });//今天的阶段

                StringBuilder sb2 = new StringBuilder();
                sb2.Append("[");



                string name = "";
                foreach (var d in projects)
                {
                    name += d.rbName + ",";
                }

                sb2.Append("{\"stagename\":\"" + name.TrimEnd(',') + "\",");
                var items = lis.Where(n => { return (n.rlx == "xm1" || n.rlx == "ry1") && n.index == Convert.ToInt32(n.reTime) && n.extension1 == item.Value.extension1; });
                sb2.Append("\"items\":[");

                foreach (var dd in items)
                {
                    sb2.Append("{\"name\":\"" + dd.rbName + dd.rxName + "\",\"time\":\"" + Convert.ToDateTime(item.Value.Extension16).AddDays(double.Parse(dd.reTime) - 1).ToString("yyyy-MM-dd") + "\"},");
                }

                sb2.Append("]},");


                sb2.Append("]");
                sb.Append("\"projectstage\":" + sb2.ToSafeString().Replace(",]", "]"));


                sb.Append("},");

            }




            sb.Append("]").Replace(",]", "]");
            return sb;
        }



        /// <summary>
        /// 得到最近七天的数据
        /// </summary>
        /// <param name="lis"></param>
        /// <returns></returns>
        private static StringBuilder GetOneDayProjectsExt(List<Project> lis)
        {
            var lis2 = lis.Where(n => { return n.rlx == "jd" && n.index <= Convert.ToInt32(n.reTime) && n.index >= Convert.ToInt32(n.rbTime); });

            #region 分组
            Dictionary<string, Project> dic = new Dictionary<string, Project>();
            foreach (var item in lis2)
            {
                if (!dic.ContainsKey(item.extension1))
                {
                    dic.Add(item.extension1, item);
                }
            }
            #endregion


            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            foreach (var item in dic)
            {
                sb.Append("{");
                sb.Append("\"projectid\":\"" + item.Value.extension1 + "\",");
                sb.Append("\"projectname\":\"" + item.Value.Extension12 + "\",");
                sb.Append("\"projectbegintime\":\"" + item.Value.Extension16 + "\",");
                var projects = lis.Where(n => { return n.rlx == "jd" && n.index <= Convert.ToInt32(n.reTime) && n.index >= Convert.ToInt32(n.rbTime) && n.extension1 == item.Value.extension1; });

                StringBuilder sb2 = new StringBuilder();
                sb2.Append("[");



                string name = "";
                foreach (var d in projects)
                {
                    name += d.rbName + ",";
                }

                sb2.Append("{\"stagename\":\"" + name.TrimEnd(',') + "\",");
                var items = lis.Where(n => { return (n.rlx == "xm1" || n.rlx == "ry1") && n.latestdays.Contains("," + n.reTime.Trim(' ') + ",") && n.extension1 == item.Value.extension1; }).OrderBy(n => int.Parse(n.reTime)); ;//今天的阶段;
                sb2.Append("\"items\":[");

                foreach (var dd in items)
                {
                    sb2.Append("{\"name\":\"" + dd.rbName + dd.rxName + "\",\"time\":\"" + Convert.ToDateTime(item.Value.Extension16).AddDays(double.Parse(dd.reTime) - 1).ToString("yyyy-MM-dd") + "\"},");
                }

                sb2.Append("]},");


                sb2.Append("]");
                sb.Append("\"projectstage\":" + sb2.ToSafeString().Replace(",]", "]"));


                sb.Append("},");

            }




            sb.Append("]").Replace(",]", "]");
            return sb;
        }


        /// <summary>
        /// 得到设计师所有的项目
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public static string GetAllProjects(string desingerid)
        {
            string sql = "select top  100 Extension1, DemandShowroomId,Extension12 ,Extension16 from DemandShowRooms where Extension15=@desingerid order by DemandShowroomId desc;";

            DataTable dt = SqlHelper.ExecuteDataTable(sql, new SqlParameter("@desingerid", desingerid));

            List<object> lis = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                var project = new { projectid = row["DemandShowroomId"].ToSafeString(), projectname = row["Extension12"].ToSafeString(), begintime = row["Extension16"].ToSafeString(), needdays = row["Extension1"].ToSafeString() };

                lis.Add(project);
            }

            return JsonConvert.SerializeObject(lis);
        }


        /// <summary>
        /// 加载修改状态
        /// </summary>
        /// <param name="peojectid"></param>
        /// <returns></returns>
        public static string LoadState(string projectid)
        {
            string sql = "select  rbName,rbtime,rxName,reTime,rlx,pics ,extensioin from  XState where extension1='" + projectid + "';";

            DataTable dt = SqlHelper.ExecuteDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = GetProjectStr(dt);

                return sb.ToSafeString().Replace(",]", "]");
            }
            else
            {

                string sql2 = "select  rbName,rbtime,rxName,reTime,rlx,pics ,extensioin  from    View_sgrz";

                DataTable dt2 = SqlHelper.ExecuteDataTable(sql2);

                return GetProjectStr(dt2).ToSafeString().Replace(",]", "]"); ;
            }



        }

        private static StringBuilder GetProjectStr(DataTable dt)
        {
            DataRow[] jdRow = dt.Select("rlx='jd'"); //阶段
            DataRow[] xmRow = dt.Select("rlx='xm'"); //项目和人员
            DataRow[] ryRow = dt.Select("rlx='ry'"); //项目和人员
            List<object> lisjd = new List<object>();
            StringBuilder sb = new StringBuilder();
            sb.Append("{");



            sb.Append("\"jd\":[");

            foreach (var item in jdRow)
            {
                sb.Append("{\"name\":\"" + item["rbName"].ToSafeString() + "\",\"beiginday\":\"" + item["rbtime"].ToSafeString() + "\",\"endday\":\"" + item["reTime"].ToSafeString() + "\"},");
            }

            sb.Append("],");





            StringBuilder sbxm = new StringBuilder();
            sbxm.Append("xm:");
            sbxm.Append("[");
            #region 项目
            //项目
            for (int i = 0; i < xmRow.Length; i++)
            {
                //{"":[{"":""}]}

                StringBuilder xm = new StringBuilder();
                xm.Append("{");
                DataRow row = xmRow[i];//小项
                xm.Append("\"name\":\"" + row["rbName"].ToSafeString() + "\",");
                xm.Append("\"items\":[");
                //子项

                #region MyRegion
                DataRow[] xiaoxiang = dt.Select("extensioin='" + row["extensioin"].ToSafeString() + "'");

                foreach (var item in xiaoxiang)
                {
                    //循环每一个小项

                    if (item["retime"].ToSafeString().Length != 0)
                    {
                        xm.Append("{\"name\":\"" + item["rxname"].ToSafeString() + "\",\"days\":\"" + item["retime"].ToSafeString() + "\"},");
                    }


                }
                #endregion

                xm.Append("]");
                xm.Append("},");

                sbxm.Append(xm);
            }
            #endregion

            sbxm.Append("],");

            sb.Append(sbxm);



            StringBuilder sbry = new StringBuilder();
            sbry.Append("ry:");
            sbry.Append("[");
            #region 人员
            for (int i = 0; i < ryRow.Length; i++)
            {
                //{"":[{"":""}]}

                StringBuilder xm = new StringBuilder();
                xm.Append("{");
                DataRow row = ryRow[i];//小项
                xm.Append("\"name\":\"" + row["rbName"].ToSafeString() + "\",");
                xm.Append("\"items\":[");
                //子项

                DataRow[] xiaoxiang = dt.Select("extensioin='" + row["extensioin"].ToSafeString() + "'");

                foreach (var item in xiaoxiang)
                {
                    //循环每一个小项
                    if (item["retime"].ToSafeString().Length != 0)
                    {
                        xm.Append("{\"name\":\"" + item["rxname"].ToSafeString() + "\",\"days\":\"" + item["retime"].ToSafeString() + "\"},");
                    }

                }

                xm.Append("]");
                xm.Append("},");
                sbry.Append(xm);
            }
            #endregion

            sbry.Append("]");
            sb.Append(sbry);



            sb.Append("}");
            return sb;
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="housetype"></param>
        /// <param name="area"></param>
        /// <param name="jcprice"></param>
        /// <param name="sgprice"></param>
        /// <param name="address"></param>
        /// <param name="begintime"></param>
        /// <param name="Timelimit"></param>
        /// <returns></returns>
        public static string AddProject(string desingerid, string name, string phone, string housetype, string area, string jcprice, string sgprice, string address, string begintime, string Timelimit, string relative)
        {

            #region 检查参数
            if (CheckParm(new Dictionary<string, string>() { { "name", name }, { "phone", phone }, { "housetype", housetype }, { "area", area }, { "jcprice", jcprice }, { "sgprice", sgprice }, { "address", address }, { "begintime", begintime }, { "Timelimit", Timelimit } }))
            {
                return errormsg;
            }
            #endregion

            string userid = GetUserid(phone);
            if (string.IsNullOrEmpty(userid))
            {
                //添加需求
                userid = InsertUser(phone, phone, address);
                AddDemand(desingerid, userid, name, phone, housetype, area, jcprice, sgprice, address, begintime, Timelimit);

            }
            else
            {
                //检查需求


                string demandid = Getdemandid(userid);

                if (string.IsNullOrEmpty(demandid))
                {
                    AddDemand(desingerid, userid, name, phone, housetype, area, jcprice, sgprice, address, begintime, Timelimit);
                }
                else
                {
                    //更新需求
                    UpdateDemand(demandid, name, phone, housetype, area, jcprice, sgprice, address, begintime, Timelimit);
                }

            }




            return "{\"errorcode\":0,\"msg\":\"项目添加成功\"}";
        }
        /// <summary>
        /// 添加用户返回userid
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="UserMPhone"></param>
        /// <param name="Address"></param>
        /// <returns></returns>
        public static string InsertUser(string LoginName, string UserMPhone, string Address)
        {



            string sql = " insert into Users(LoginName,LoginPwd,UserPhone,UserMPhone,Address,HeadImage,CreateTime)values(@LoginName,@LoginPwd,@UserPhone,@UserMPhone,@Address,@HeadImage,@CreateTime);select @@IDENTITY";


            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@LoginName",LoginName),
             new SqlParameter("@LoginPwd","49ba59abbe56e057"),
              new SqlParameter("@UserPhone","1"),
               new SqlParameter("@UserMPhone",UserMPhone),
                new SqlParameter("@Address",Address),
                 new SqlParameter("@HeadImage","img/defaultHead.png"),
                  new SqlParameter("@CreateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            return SqlHelper.ExecuteScalar(sql, arr).ToSafeString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUserid(string UserMPhone)
        {
            object o = SqlHelper.ExecuteScalar("select UserId from Users where LoginName=@phone or UserMPhone=@phone;", new SqlParameter("@phone", UserMPhone));

            if (o.ToSafeString() != "")
            {
                return o.ToSafeString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 得到需求id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string Getdemandid(string userid)
        {
            object o = SqlHelper.ExecuteScalar("select DemandShowroomId from DemandShowRooms where UserId='" + userid + "';");
            return o.ToSafeString();
        }

        /// <summary>
        /// 更新需求
        /// </summary>
        /// <param name="DemandShowroomId"></param>
        /// <param name="userid"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="housetype"></param>
        /// <param name="area"></param>
        /// <param name="jcprice"></param>
        /// <param name="sgprice"></param>
        /// <param name="address"></param>
        /// <param name="begintime"></param>
        /// <param name="Timelimit"></param>
        /// <returns></returns>
        public static int UpdateDemand(string DemandShowroomId, string name, string phone, string housetype, string area, string jcprice, string sgprice, string address, string begintime, string Timelimit)
        {

            string sql = @"update DemandShowRooms set Extension12=@name,Extension14=@phone,Extension13=@housetype,UnitId=@area,ProjectId=@jcprice,Extension2=@address,Extension16=@begintime,Extension1=@Timelimit
                
                where DemandShowroomId=@DemandShowroomId";

            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@name",name),
              new SqlParameter("@phone",phone),
                new SqlParameter("@housetype",housetype),
                  new SqlParameter("@area",area),
                   new SqlParameter("@jcprice",jcprice),
                    new SqlParameter("@address",address),
                      new SqlParameter("@begintime",begintime),
                      new SqlParameter("@Timelimit",Timelimit),
                      
                       new SqlParameter("@DemandShowroomId",DemandShowroomId)
            };
            return SqlHelper.ExecuteNonQuery(sql, arr); ;

        }
        /// <summary>
        /// 添加需求
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="housetype"></param>
        /// <param name="area"></param>
        /// <param name="jcprice"></param>
        /// <param name="sgprice"></param>
        /// <param name="address"></param>
        /// <param name="begintime"></param>
        /// <param name="Timelimit"></param>
        /// <returns></returns>
        public static int AddDemand(string desingerid, string userid, string name, string phone, string housetype, string area, string jcprice, string sgprice, string address, string begintime, string Timelimit)
        {
            string sql = "insert into DemandShowRooms(userid,Extension12,Extension14,Extension13,UnitId,ProjectId,Extension2,Extension16,Extension1,Extension15,CreateTime) values                                                                                                                 (@userid,@name, @phone,@housetype, @area,@jcprice,@address,@begintime,@Timelimit,@desingerid,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            SqlParameter[] arr = new SqlParameter[] { 
                  new SqlParameter("@userid", userid),
                  new SqlParameter("@name",name),
                  new SqlParameter("@phone",phone),
                  new SqlParameter("@housetype",housetype),
                  new SqlParameter("@area",area),
                   new SqlParameter("@jcprice",jcprice),
                    new SqlParameter("@address",address),
                      new SqlParameter("@begintime",begintime),
                      new SqlParameter("@Timelimit",Timelimit),
                       new SqlParameter("@desingerid",desingerid)
            };


            return SqlHelper.ExecuteNonQuery(sql, arr);
        }


        /// <summary>
        /// 查询设计师
        /// </summary>
        /// <returns></returns>
        public static string DesignerSearch(string searchvalue)
        {



            string sql = "select top 100 * from DesignerGrade where examine='通过'";

            if (!string.IsNullOrEmpty(searchvalue))
            {
                sql = "select * from DesignerGrade where examine='通过' and DID+Extension2+mPhone like '%" + searchvalue + "%'";
            }

            DataTable dt = SqlHelper.ExecuteDataTable(sql);
            List<object> lis = new List<object>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];

                    var obj = new { desingerid = row["id"].ToSafeString(), name = row["did"].ToSafeString() };
                    lis.Add(obj);
                }
            }

            return "{\"desingers\":" + JsonConvert.SerializeObject(lis) + ",\"foremans\":[]}";

        }


        /// <summary>
        /// 得到项目进度
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static string ProjectSchedule(string projectid)
        {


            DataTable dtpic = SqlHelper.ExecuteDataTable("select * from xStatePic where demandId='" + projectid + "';");

            if (CheckParm(new Dictionary<string, string>() { { "projectid", projectid } }))
            {
                return errormsg;
            }

            #region 项目详情
            string sql = "select DemandShowroomId, Extension1,Extension16 from DemandShowRooms where  DemandShowRooms.Extension16 is not null and DemandShowroomId=" + projectid;
            DataTable dtproject = SqlHelper.ExecuteDataTable(sql);
            var project = new { projectid = projectid, needdays = Convert.ToInt32(dtproject.Rows[0]["Extension1"]), begintime = Convert.ToDateTime(dtproject.Rows[0]["Extension16"]) };
            #endregion


            //得到项目   项目id   项目所需时间    项目开始时间
            DataTable dt = new DataTable();

            dt = SqlHelper.ExecuteDataTable("select * from XState where extension1='" + projectid + "' and rlx='jd'");

            if (dt == null || dt.Rows.Count < 1)
            {
                dt = SqlHelper.ExecuteDataTable("select * from View_sgrz  where rlx='jd'  ;");
            }

            #region 构造结果字符串

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            //List<object> lis = new List<object>();
            for (int i = 0; i < project.needdays; i++)
            {
                //第i天      (i+1)天所处阶段    天数
                //select rbName from View_sgrz  where rlx='jd' and 11>=rbTime and 15<=reTime;

                DataRow[] rows = dt.Select((i + 1) + ">=rbTime and " + (i + 1) + "<=reTime");

                string s = "";

                foreach (var item in rows)
                {
                    s += item["rbName"].ToSafeString() + ",";
                }

                DataRow[] rowpics = dtpic.Select("day='" + (i + 1) + "'");

                string pics = "[]";

                if (rowpics.Count() > 0)
                {
                    pics = rowpics[0]["pic"].ToSafeString();


                    var arr = pics.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    string jk = "[";
                    for (int j = 0; j < arr.Length; j++)
                    {
                        if (arr[i].Contains("http"))
                        {
                            jk += "\"" + arr[i].Replace("\\", "/") + "\",";
                        }
                        else
                        {
                            jk += "\"http://www.mj100.com/SGDaily/UploadFile/500/" + arr[i].Replace("\\", "/") + "\",";
                        }

                    }
                    jk += "]";

                    pics = jk;
                }




                var obj = new { jd = s, pic = pics, time = project.begintime.AddDays(i).ToString("yyyy-MM-dd"), dayindex = (i + 1) };

                var ss = "{\"jd\":\"" + obj.jd + "\",\"pic\":" + pics.Replace(",]", "]") + ",\"time\":\"" + obj.time + "\",\"dayindex\":\"" + obj.dayindex + "\"},";
                sb.Append(ss);

                //lis.Add(obj);
            }
            sb.Append("]");
            #endregion
            //查询项目记录

            return sb.ToSafeString().Replace(",]", "]"); ;
        }

        /// <summary>
        /// 添加修改记录
        /// </summary>
        /// <param name="desingername"></param>
        /// <param name="con"></param>
        /// <param name="Reason"></param>
        /// <param name="projectid"></param>
        /// <param name="Delaydays"></param>
        /// <returns></returns>
        public static string AddModifyRecord(string desingername, string con, string Reason,string projectid,string Delaydays)
        {
            if (Reason.IsEmpty())
            {
                Reason = " ";
            }
            if (CheckParm(new Dictionary<string, string>() { {"desingername",desingername},{"con",con},{"Reason",Reason},{"projectid",projectid},{"Delaydays",Delaydays}}))
            {
                return errormsg;
            }

            string sql = "insert into xgrlrz(userName,con,xgyy,xgTime,extension,extension1,createTime) values(@desingername,@con,@Reason,@xgTime,@projectid,@Delaydays,@createtime)";
            SqlParameter[] arr = new SqlParameter[] { 
            new SqlParameter("@desingername",desingername),
            new SqlParameter("@con",con),
            new SqlParameter("@Reason",Reason),
            new SqlParameter("@xgTime",Reason),
            new SqlParameter("@projectid",projectid),
            new SqlParameter("@Delaydays",Delaydays),
            new SqlParameter("@createtime",DateTime.Now.ToString("yyyy-MM-dd"))
            };
            SqlHelper.ExecuteNonQuery(sql,arr);
            return "{\"errorcode\":0,\"msg\":\"添加修改记录成功\"}";
        }

        /// <summary>
        /// 添加照片
        /// </summary>
        /// <returns></returns>
        public static string InsertPic(string projectid,string day,string pic)
        {
            string sql = "select rzPicId from xStatePic where demandId='"+projectid+"' and day='"+day+"' ;";

            object o = SqlHelper.ExecuteScalar(sql);

            if (o==null)
            {
                SqlHelper.ExecuteNonQuery("insert into xStatePic(demandId,day,pic,createTime) values('"+projectid+"','"+day+"','"+pic+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
            }
            else
            {
                SqlHelper.ExecuteNonQuery("update xStatePic set pic=pic+'' where demandId='" + projectid + "' and day='" + day + "' ;");
            }
            return "";
        }


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


    /// <summary>
    /// 项目类
    /// </summary>
    public class Project
    {

        // rzId	rbName	rxName	rbTime	reTime	rlx	pics	extensioin	extension1	extension2	createTime	DemandShowroomId	Extension12	Extension16	Extension15

        public string RzId;

        public string rbName;


        public string rxName;

        public string rbTime;

        public string reTime;

        public string rlx;


        public string pics;


        public string extensioin;

        public string extension1;

        public string extension2;

        public string createTime;

        public string DemandShowroomId;

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Extension12;

        /// <summary>
        /// 项目开始时间
        /// </summary>
        public string Extension16;

        public string Extension15;


        public double index = 0;

        public string tody = "";

        public string latestdays = "";

    }
}
