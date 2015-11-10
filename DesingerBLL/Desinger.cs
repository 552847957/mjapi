using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

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

        public bool Yz(string phone)
        {

            object o = SqlHelper.ExecuteScalar("select COUNT(*) from DesignerGrade where Extension5='" + phone + "' or mPhone='" + phone + "';");

            return int.Parse(o.ToSafeString()) == 0;
        }


        /// <summary>
        /// 修改阶段时间
        /// </summary>
        /// <param name="benginday"></param>
        /// <param name="endday"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static bool UpdateJd(string rbname, string benginday, string endday, string projectid)
        {

            string sql = "update XState set rbTime='" + benginday + "' ,reTime='" + endday + "' where extension1='" + projectid + "' and rlx='jd' and rbName='" + rbname + "' ;";


            return SqlHelper.ExecuteNonQuery(sql) > 0;

        }

        /// <summary>
        /// 添加施工日志
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static int AddSgrz(string projectid)
        {

            string sql = @"insert into XState (rbName, rxName, rbTime, reTime, rlx, pics, extensioin, extension1, extension2, createTime)

select   rbName, rxName, rbTime, reTime, rlx, pics, extensioin, '" + projectid + "', extension2, '" + DateTime.Now.ToString("yyyy-MM-dd") + "' from View_sgrz order by extensioin ";

            return SqlHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 得到施工日志中的最大记录
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static string GetMaxNUmber(string projectid)
        {

            return SqlHelper.ExecuteScalar("select MAX( cast( (case extensioin when 'NAN' then '0' else extensioin end) as int)) from XState where  extension1='" + projectid + "';").ToSafeString();
        }

        public static bool Isexit(string projectid)
        {

            return SqlHelper.ExecuteScalar("select MAX(extension1) from XState where  extension1='" + projectid + "';").ToSafeString().IsEmpty();
        }
        /// <summary>
        /// 添加项目  人员  阶段
        /// </summary>
        /// <param name="rname">阶段名</param>
        /// <param name="b">开始时间</param>
        /// <param name="e">结束天</param>
        /// <param name="rlx">类别</param>
        /// <param name="n">数字</param>
        /// <param name="peojectid">项目id</param>
        /// <returns></returns>
        public static string AddXmRyJd(string rname, string b, string e, string rlx, string n, string projectid)
        {

            string sql = @"insert into XState (rbName,rxName,rbTime,reTime,rlx,pics,extensioin,extension1,extension2,createTime)
values('" + rname + "','','" + b + "','" + e + "','" + rlx + "','','" + n + "','" + projectid + "','','" + DateTime.Now.ToString("yyyy-MM-dd") + "');";

            return SqlHelper.ExecuteNonQuery(sql).ToString();
        }


        public static string AddXmRyJdchild(string rname, string rxname, string b, string e, string rlx, string n, string projectid)
        {

            string sql = @"

declare @extension nvarchar(20)

select @extension=extensioin from XState where rbName='" + rname + @"' and rlx='" + rlx + @"' and extension1='" + projectid + @"'

insert into XState (rbName,rxName,rbTime,reTime,rlx,pics,extensioin,extension1,extension2,createTime)
values('" + rname + "','" + rxname + "','" + b + "','" + e + "','" + rlx + "1" + "','',@extension,'" + projectid + "','','" + DateTime.Now.ToString("yyyy-MM-dd") + "');";

            return SqlHelper.ExecuteNonQuery(sql).ToString();
        }

        /// <summary>
        /// 添加 项目  阶段 人员
        /// </summary>
        /// <param name="rname"></param>
        /// <param name="rlx"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static string AddXm(string rname, string rlx, string projectid)
        {

            if (CheckParm(new Dictionary<string, string>() { { "rname", rname }, { "rlx", rlx }, { "projectid", projectid } }))
            {
                return errormsg;
            }


            #region 拿到最大数并插入数据库
            int j = 30;
            string v = GetMaxNUmber(projectid);
            if (v.IsEmpty())
            {
                AddSgrz(projectid);

            }
            else
            {
                j = int.Parse(v) + 1;
            }
            #endregion



            if (rlx == "jd")
            {
                AddXmRyJd(rname, "1", "65", rlx, GetExtension(rname, j).ToString(), projectid);

            }
            else
            {
                AddXmRyJd(rname, "", "", rlx, GetExtension(rname, j).ToString(), projectid);
            }




            return "{\"errorcode\":0,\"msg\":\"添加成功\"}"; ;

        }



        /// <summary>
        /// 添加 项目  阶段 人员
        /// </summary>
        /// <param name="rname"></param>
        /// <param name="rlx"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static string Updatejd(string jdname, string b, string e, string projectid)
        {

            if (CheckParm(new Dictionary<string, string>() { { "jdname", jdname }, { "b", b }, { "e", e }, { "projectid", projectid } }))
            {
                return errormsg;
            }

            if (!Regex.IsMatch(b, "^\\d+$") || !Regex.IsMatch(e, "^\\d+$"))
            {
                return "{\"errorcode\":1,\"msg\":\"参数异常\"}";
            }

            #region 拿到最大数并插入数据库
            int j = 30;
            string v = GetMaxNUmber(projectid);
            if (v.IsEmpty())
            {
                AddSgrz(projectid);

            }
            else
            {
                j = int.Parse(v) + 1;
            }
            #endregion



            UpdateJd(jdname, b, e, projectid);





            return "{\"errorcode\":0,\"msg\":\"修改成功\"}"; ;

        }

        /// <summary>
        /// 添加孩子
        /// </summary>
        /// <param name="rname"></param>
        /// <param name="rxname"></param>
        /// <param name="rlx"></param>
        /// <param name="e"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static string AddChild(string rname, string rxname, string rlx, string e, string projectid)
        {
            if (CheckParm(new Dictionary<string, string>() { { "rname", rname }, { "rxname", rxname }, { "rlx", rlx }, { "e", e }, { "projectid", projectid } }))
            {
                return errormsg;
            }


            #region 拿到最大数并插入数据库
            int j = 29;
            string v = GetMaxNUmber(projectid);
            if (v.IsEmpty())
            {
                AddSgrz(projectid);

            }
            else
            {
                j = int.Parse(v);
            }
            #endregion

            AddXmRyJdchild(rname, rxname, "", e, rlx, j.ToSafeString(), projectid);

            return "{\"errorcode\":0,\"msg\":\"" + rname + "下的" + "子项" + rxname + "添加成功\"}";
        }


        /// <summary>
        /// 删除 项目  人员   阶段
        /// </summary>
        /// <returns></returns>
        public static string Delete(string rname, string rxname, string rlx, string projectid)
        {
            if (CheckParm(new Dictionary<string, string> { { "rname", rname }, { "rlx", rlx }, { "projectid", projectid } }))
            {
                return errormsg;
            }


            if (Isexit(projectid))
            {
                AddSgrz(projectid);

            }
            if (rlx.Contains("1"))
            {
                SqlHelper.ExecuteNonQuery("delete from XState where  rbName='" + rname + "' and  rxName='" + rxname + "' and extension1='" + projectid + "' and rlx='" + rlx + "'");
            }
            else
            {
                SqlHelper.ExecuteNonQuery("delete from XState where rbName='" + rname + "' and extension1='" + projectid + "' and rlx='" + rlx + "'");
            }



            return "{\"errorcode\":0,\"msg\":\"删除成功\"}";

        }


        /// <summary>
        /// 删除子项
        /// </summary>
        /// <returns></returns>
        public static string Delete2(string rname, string rlx, string projectid)
        {
            if (CheckParm(new Dictionary<string, string> { { "rname", rname }, { "rlx", rlx }, { "projectid", projectid } }))
            {
                return errormsg;
            }

            string v = GetMaxNUmber(projectid);
            if (v.IsEmpty())
            {
                AddSgrz(projectid);

            }

            SqlHelper.ExecuteNonQuery("delete from XState where rbName='" + rname + "' and extension1='" + projectid + "' and rlx='" + rlx + "'");

            return "{\"errorcode\":0,\"msg\":\"删除成功\"}";

        }



        public static int GetExtension(string rname, int r)
        {
            int v = r;

            #region MyRegion
            //装前准备 1
            //水电改造 2
            //泥作   3
            //木作   4
            //漆作   5
            //整体安装 6

            //铝扣板   7
            //灯具五金开关 8
            //橱柜定制家具 9
            //瓷砖 10
            //门   11
            //石材  12
            //断桥铝窗 13
            //防盗门   14
            //暖气     15
            //壁纸     16
            //地板     17
            //洁具     18
            //地暖     19
            //中央空调  20
            //新风      21
            //智能家居   22

            //收款日期  23
            //微信推送  24
            //建材 25
            //业主  26
            //设计师 27
            //小美管家 28
            //项目经理 29 
            #endregion

            switch (rname)
            {
                case "装前准备":
                    v = 1;
                    break;
                case "水电改造":
                    v = 2;
                    break;
                case "泥作":
                    v = 3;
                    break;
                case "木作":
                    v = 4;
                    break;
                case "漆作":
                    v = 5;
                    break;
                case "整体安装":
                    v = 6;
                    break;
                case "铝扣板":
                    v = 7;
                    break;
                case "灯具五金开关":
                    v = 8;
                    break;
                case "橱柜定制家具":
                    v = 9;
                    break;
                case "瓷砖":
                    v = 10;
                    break;
                case "门":
                    v = 11;
                    break;
                case "石材":
                    v = 12;
                    break;
                case "断桥铝窗":
                    v = 13;
                    break;
                case "防盗门":
                    v = 14;
                    break;
                case "暖气":
                    v = 15;
                    break;
                case "壁纸":
                    v = 16;
                    break;
                case "地板":
                    v = 17;
                    break;
                case "洁具":
                    v = 18;
                    break;
                case "地暖":
                    v = 19;
                    break;
                case "中央空调":
                    v = 20;
                    break;
                case "新风":
                    v = 21;
                    break;
                case "智能家居":
                    v = 22;
                    break;
                case "收款日期":
                    v = 23;
                    break;
                case "微信推送":
                    v = 24;
                    break;
                case "建材":
                    v = 25;
                    break;
                case "业主":
                    v = 26;
                    break;
                case "设计师":
                    v = 27;
                    break;
                case "小美管家":
                    v = 28;
                    break;
                case "项目经理":
                    v = 29;
                    break;


                default:
                    v = r;
                    break;
            }

            return v;
        }


        /// <summary>
        /// 注册设计
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int Adddesinger(string phone, string city, string yzm, string fromid)
        {
            if (Yz(phone))
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
            return 1;



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

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjectsExt(lis));
            sb.Append("}");

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

            StringBuilder sb = new StringBuilder();



            sb.Append("{");
            sb.Append("\"errorcode\":0,");


            sb.Append("\"data\":[");


            #region 第一天

            sb.Append("{");
            sb.Append("\"msg\":\"第一天\",");
            sb.Append("\"time\":\"" + DateTime.Now.ToString("yy-MM-dd") + "\",");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjects(lis));
            sb.Append("}");
            #endregion


            sb.Append("]");
            sb.Append("}");
            return sb.ToSafeString();



        }


        /// <summary>
        /// 编辑项目人员
        /// </summary>
        public static string EditXmRy(string input)
        {
            //input = "{\"projectid\":\"4266\",\"beigintime\":\"2015-5-30\",\"desingername\":\"设计师名字\",\"rlx\":\"xm\",\"rname\":\"橱柜定制家具\",\"data\":[{\"rxname\":\"进场确认水电位\",\"oper\":\"delete\",\"time\":\"2015-6-9\",\"timeindex\":\"11\"},{\"rxname\":\"测量\",\"oper\":\"update\",\"time\":\"2015-6-20\",\"timeindex\":\"28\"},{\"rxname\":\"自定义一个\",\"oper\":\"add\",\"time\":\"2015-6-29\",\"timeindex\":\"30\"}";


            #region json格式
            //{
            //"projectid": "123",
            //"beigintime": "2015-5-30",
            //"desingername": "设计师名字",
            //"rlx": "xm",
            //"rname": "水电",
            //"oper": [
            //{
            //"rxname": "进场确定方案",
            //"oper": "delete",
            //"time": "2015-6-9",
            //"timeindex": "处在项目的第几天"
            //},
            //{
            //"rxname": "交付",
            //"oper": "update",
            //"time": "2015-6-20",
            //"timeindex": "28"
            //},
            //{
            //"rxname": "自定义一个",
            //"oper": "add",
            //"time": "2015-6-29",
            //"timeindex": "30"
            //}
            //]
            //} 
            #endregion



            object obj = JsonConvert.DeserializeObject(input);

            Newtonsoft.Json.Linq.JObject js = obj as Newtonsoft.Json.Linq.JObject;//把上面的obj转换为 Jobject对象

            string projectid = js["projectid"].ToSafeString();

            string beigintime = js["beigintime"].ToSafeString();

            string desingername = js["desingername"].ToSafeString();

            string rlx = js["rlx"].ToSafeString();

            string rname = js["rname"].ToSafeString();


            #region 查询出项目的extensioin
            string sql = "select extensioin from XState where  rlx='" + rlx + "' and rbName='" + rname + "' and  extension1='" + projectid + "'";
            object oextensioin = SqlHelper.ExecuteScalar(sql);
            if (oextensioin == null)
            {
                AddSgrz(projectid);
                oextensioin = SqlHelper.ExecuteScalar(sql).ToSafeString();
            }

            #endregion



            JArray jarr = JArray.Parse(js["data"].ToSafeString());

            #region 增删改

            StringBuilder sb = new StringBuilder();
            sb.Append(@"declare @error int
               set @error=0
begin tran
 ");
            foreach (var item in jarr)
            {


                string rxname = item["rxname"].ToSafeString().Trim();
                string oper = item["oper"].ToSafeString().Trim();
                string time = item["time"].ToSafeString().Trim();
                string timeindex = item["timeindex"].ToSafeString().Trim();

                #region 增删改
                if (oper == "delete")
                {
                    //删除
                    sb.Append(" delete from XState where rxName='" + rxname + "' and rlx='" + rlx + "1' and extensioin='" + oextensioin + "' and  extension1='" + projectid + "' ");

                }
                else if (oper == "update")
                {

                    //更新

                    string newname = item["newname"].ToSafeString().Trim();
                    if (newname.IsEmpty())
                    {
                        sb.Append(" update XState set reTime='" + timeindex + "'  where rxName='" + rxname + "' and rlx='" + rlx + "1' and extensioin='" + oextensioin + "' and  extension1='" + projectid + "' ");
                    }
                    else
                    {
                        sb.Append(" update XState set reTime='" + timeindex + "',rxName='" + newname + "'   where rxName='" + rxname + "' and rlx='" + rlx + "1' and extensioin='" + oextensioin + "' and  extension1='" + projectid + "' ");
                    }


                }
                else if (oper == "add")
                {



                    //添加
                    sb.Append(" insert into XState (rbName,rxName,reTime,rlx,extensioin,extension1,createTime) values('" + rname + "','" + rxname + "','" + timeindex + "','" + rlx + "1','" + oextensioin + "','" + projectid + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')");

                }
                #endregion

            }




            sb.Append(@"set @error=@@ERROR
 if(@error>0)
 begin
  rollback tran
 end
 else
 begin
commit tran

 end");
            #endregion



            SqlHelper.ExecuteNonQuery(sb.ToSafeString());


            return "{\"errorcode\":0,\"msg\":\"操作成功\"}"; ;
        }

        public static string GetTodaythings7(string desingerid)
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

            StringBuilder sb = new StringBuilder();



            sb.Append("{");
            sb.Append("\"errorcode\":0,");


            sb.Append("\"data\":[");


            #region 第一天

            sb.Append("{");
            sb.Append("\"msg\":\"第一天\",");
            sb.Append("\"time\":\"" + DateTime.Now.ToString("yy-MM-dd") + "\",");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjects(lis));
            sb.Append("},");
            #endregion

            #region 第二天

            sb.Append("{");
            sb.Append("\"msg\":\"第二天\",");
            sb.Append("\"time\":\"" + DateTime.Now.AddDays(1).ToString("yy-MM-dd") + "\",");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            {
                RzId = n.RzId,
                createTime = n.createTime,
                DemandShowroomId = n.DemandShowroomId,
                extensioin = n.extensioin,
                extension1 = n.extension1,
                Extension12 = n.Extension12,
                Extension15 = n.Extension15,
                Extension16 = n.Extension16,
                extension2 = n.extension2,
                pics = n.pics,
                rbName = n.rbName,
                rbTime = n.rbTime,
                reTime = n.reTime,
                rlx = n.rlx,
                rxName = n.rxName,
                index = n.index + 1
            }).ToList()));
            sb.Append("},");
            #endregion

            #region 第三天

            sb.Append("{");
            sb.Append("\"msg\":\"第三天\",");
            sb.Append("\"time\":\"" + DateTime.Now.AddDays(2).ToString("yy-MM-dd") + "\",");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            {
                RzId = n.RzId,
                createTime = n.createTime,
                DemandShowroomId = n.DemandShowroomId,
                extensioin = n.extensioin,
                extension1 = n.extension1,
                Extension12 = n.Extension12,
                Extension15 = n.Extension15,
                Extension16 = n.Extension16,
                extension2 = n.extension2,
                pics = n.pics,
                rbName = n.rbName,
                rbTime = n.rbTime,
                reTime = n.reTime,
                rlx = n.rlx,
                rxName = n.rxName,
                index = n.index + 2
            }).ToList()));
            sb.Append("},");
            #endregion

            #region 第四天

            sb.Append("{");
            sb.Append("\"msg\":\"第四天\",");
            sb.Append("\"time\":\"" + DateTime.Now.AddDays(3).ToString("yy-MM-dd") + "\",");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            {
                RzId = n.RzId,
                createTime = n.createTime,
                DemandShowroomId = n.DemandShowroomId,
                extensioin = n.extensioin,
                extension1 = n.extension1,
                Extension12 = n.Extension12,
                Extension15 = n.Extension15,
                Extension16 = n.Extension16,
                extension2 = n.extension2,
                pics = n.pics,
                rbName = n.rbName,
                rbTime = n.rbTime,
                reTime = n.reTime,
                rlx = n.rlx,
                rxName = n.rxName,
                index = n.index + 3
            }).ToList()));
            sb.Append("},");
            #endregion


            #region 第五天

            sb.Append("{");
            sb.Append("\"msg\":\"第五天\",");
            sb.Append("\"time\":\"" + DateTime.Now.AddDays(4).ToString("yy-MM-dd") + "\",");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            {
                RzId = n.RzId,
                createTime = n.createTime,
                DemandShowroomId = n.DemandShowroomId,
                extensioin = n.extensioin,
                extension1 = n.extension1,
                Extension12 = n.Extension12,
                Extension15 = n.Extension15,
                Extension16 = n.Extension16,
                extension2 = n.extension2,
                pics = n.pics,
                rbName = n.rbName,
                rbTime = n.rbTime,
                reTime = n.reTime,
                rlx = n.rlx,
                rxName = n.rxName,
                index = n.index + 4
            }).ToList()));
            sb.Append("},");
            #endregion


            #region 第六天

            sb.Append("{");
            sb.Append("\"msg\":\"第六天\",");
            sb.Append("\"time\":\"" + DateTime.Now.AddDays(5).ToString("yy-MM-dd") + "\",");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            {
                RzId = n.RzId,
                createTime = n.createTime,
                DemandShowroomId = n.DemandShowroomId,
                extensioin = n.extensioin,
                extension1 = n.extension1,
                Extension12 = n.Extension12,
                Extension15 = n.Extension15,
                Extension16 = n.Extension16,
                extension2 = n.extension2,
                pics = n.pics,
                rbName = n.rbName,
                rbTime = n.rbTime,
                reTime = n.reTime,
                rlx = n.rlx,
                rxName = n.rxName,
                index = n.index + 5
            }).ToList()));
            sb.Append("},");
            #endregion


            #region 第七天

            sb.Append("{");
            sb.Append("\"msg\":\"第七天\",");
            sb.Append("\"time\":\"" + DateTime.Now.AddDays(6).ToString("yy-MM-dd") + "\",");
            sb.Append("\"data\":");
            sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            {
                RzId = n.RzId,
                createTime = n.createTime,
                DemandShowroomId = n.DemandShowroomId,
                extensioin = n.extensioin,
                extension1 = n.extension1,
                Extension12 = n.Extension12,
                Extension15 = n.Extension15,
                Extension16 = n.Extension16,
                extension2 = n.extension2,
                pics = n.pics,
                rbName = n.rbName,
                rbTime = n.rbTime,
                reTime = n.reTime,
                rlx = n.rlx,
                rxName = n.rxName,
                index = n.index + 6
            }).ToList()));
            sb.Append("}");
            #endregion


            sb.Append("]");
            sb.Append("}");
            return sb.ToSafeString();



        }



        public static string GetTodaythings30(string desingerid, string startday = "", string endday = "", string projectid = "")
        {
            if (startday.IsEmpty())
            {
                startday = DateTime.Now.ToString("yyyy-MM-dd");
            }
            DateTime dtstart = DateTime.Now.AddHours(15);

            if (!DateTime.TryParse(startday, out dtstart))
            {
                dtstart = DateTime.Now;
            }
            else
            {


            }


            DateTime dtend = DateTime.Now;

            if (!DateTime.TryParse(endday, out dtend))
            {
                dtend = DateTime.Now.AddDays(30);
            }
            else
            {

                dtend = dtend.AddDays(1);

            }




            TimeSpan span = dtend - dtstart;

            //这里改下时间起始

            int ok = (int)Math.Ceiling(span.TotalDays);

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
            if (!projectid.IsEmpty())
            {
                #region 一个房间的情况
                sql = @"select * from 

(select * from XState where reTime is not null and extension1 in ('" + projectid + @"') )

 a
 
left join 

( 
select DemandShowroomId,Extension12,Extension16,Extension15 
from DemandShowRooms  where 
DemandShowroomId='" + projectid + @"' and Extension12 is not null and Extension16 is not null
) 

b

on a.extension1=b.DemandShowroomId 


where DemandShowroomId is not null order by rzId desc";
                #endregion
            }
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
                    index = Math.Ceiling((dtstart - Convert.ToDateTime(row["Extension16"].ToSafeString())).TotalDays) + 1


                };
                p.latestdays = "," + p.index + "," + (p.index + 1) + "," + (p.index + 2) + "," + (p.index + 3) + "," + (p.index + 4) + "," + (p.index + 5) + "," + (p.index + 6) + "," + (p.index + 7) + ",";

                lis.Add(p);
            }
            #endregion

            StringBuilder sb = new StringBuilder();



            sb.Append("{");
            sb.Append("\"errorcode\":0,");


            sb.Append("\"data\":[");


            #region MyRegion
            //#region 第一天

            //sb.Append("{");
            //sb.Append("\"msg\":\"第一天\",");
            //sb.Append("\"time\":\"" + DateTime.Now.ToString("yy-MM-dd") + "\",");
            //sb.Append("\"data\":");
            //sb.Append(GetOneDayProjects(lis));
            //sb.Append("},");
            //#endregion

            //#region 第二天

            //sb.Append("{");
            //sb.Append("\"msg\":\"第二天\",");
            //sb.Append("\"time\":\"" + DateTime.Now.AddDays(1).ToString("yy-MM-dd") + "\",");
            //sb.Append("\"data\":");
            //sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            //{
            //    RzId = n.RzId,
            //    createTime = n.createTime,
            //    DemandShowroomId = n.DemandShowroomId,
            //    extensioin = n.extensioin,
            //    extension1 = n.extension1,
            //    Extension12 = n.Extension12,
            //    Extension15 = n.Extension15,
            //    Extension16 = n.Extension16,
            //    extension2 = n.extension2,
            //    pics = n.pics,
            //    rbName = n.rbName,
            //    rbTime = n.rbTime,
            //    reTime = n.reTime,
            //    rlx = n.rlx,
            //    rxName = n.rxName,
            //    index = n.index + 1
            //}).ToList()));
            //sb.Append("},");
            //#endregion

            //#region 第三天

            //sb.Append("{");
            //sb.Append("\"msg\":\"第三天\",");
            //sb.Append("\"time\":\"" + DateTime.Now.AddDays(2).ToString("yy-MM-dd") + "\",");
            //sb.Append("\"data\":");
            //sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            //{
            //    RzId = n.RzId,
            //    createTime = n.createTime,
            //    DemandShowroomId = n.DemandShowroomId,
            //    extensioin = n.extensioin,
            //    extension1 = n.extension1,
            //    Extension12 = n.Extension12,
            //    Extension15 = n.Extension15,
            //    Extension16 = n.Extension16,
            //    extension2 = n.extension2,
            //    pics = n.pics,
            //    rbName = n.rbName,
            //    rbTime = n.rbTime,
            //    reTime = n.reTime,
            //    rlx = n.rlx,
            //    rxName = n.rxName,
            //    index = n.index + 2
            //}).ToList()));
            //sb.Append("},");
            //#endregion

            //#region 第四天

            //sb.Append("{");
            //sb.Append("\"msg\":\"第四天\",");
            //sb.Append("\"time\":\"" + DateTime.Now.AddDays(3).ToString("yy-MM-dd") + "\",");
            //sb.Append("\"data\":");
            //sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            //{
            //    RzId = n.RzId,
            //    createTime = n.createTime,
            //    DemandShowroomId = n.DemandShowroomId,
            //    extensioin = n.extensioin,
            //    extension1 = n.extension1,
            //    Extension12 = n.Extension12,
            //    Extension15 = n.Extension15,
            //    Extension16 = n.Extension16,
            //    extension2 = n.extension2,
            //    pics = n.pics,
            //    rbName = n.rbName,
            //    rbTime = n.rbTime,
            //    reTime = n.reTime,
            //    rlx = n.rlx,
            //    rxName = n.rxName,
            //    index = n.index + 3
            //}).ToList()));
            //sb.Append("},");
            //#endregion


            //#region 第五天

            //sb.Append("{");
            //sb.Append("\"msg\":\"第五天\",");
            //sb.Append("\"time\":\"" + DateTime.Now.AddDays(4).ToString("yy-MM-dd") + "\",");
            //sb.Append("\"data\":");
            //sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            //{
            //    RzId = n.RzId,
            //    createTime = n.createTime,
            //    DemandShowroomId = n.DemandShowroomId,
            //    extensioin = n.extensioin,
            //    extension1 = n.extension1,
            //    Extension12 = n.Extension12,
            //    Extension15 = n.Extension15,
            //    Extension16 = n.Extension16,
            //    extension2 = n.extension2,
            //    pics = n.pics,
            //    rbName = n.rbName,
            //    rbTime = n.rbTime,
            //    reTime = n.reTime,
            //    rlx = n.rlx,
            //    rxName = n.rxName,
            //    index = n.index + 4
            //}).ToList()));
            //sb.Append("},");
            //#endregion


            //#region 第六天

            //sb.Append("{");
            //sb.Append("\"msg\":\"第六天\",");
            //sb.Append("\"time\":\"" + DateTime.Now.AddDays(5).ToString("yy-MM-dd") + "\",");
            //sb.Append("\"data\":");
            //sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            //{
            //    RzId = n.RzId,
            //    createTime = n.createTime,
            //    DemandShowroomId = n.DemandShowroomId,
            //    extensioin = n.extensioin,
            //    extension1 = n.extension1,
            //    Extension12 = n.Extension12,
            //    Extension15 = n.Extension15,
            //    Extension16 = n.Extension16,
            //    extension2 = n.extension2,
            //    pics = n.pics,
            //    rbName = n.rbName,
            //    rbTime = n.rbTime,
            //    reTime = n.reTime,
            //    rlx = n.rlx,
            //    rxName = n.rxName,
            //    index = n.index + 5
            //}).ToList()));
            //sb.Append("},");
            //#endregion


            //#region 第七天

            //sb.Append("{");
            //sb.Append("\"msg\":\"第七天\",");
            //sb.Append("\"time\":\"" + DateTime.Now.AddDays(6).ToString("yy-MM-dd") + "\",");
            //sb.Append("\"data\":");
            //sb.Append(GetOneDayProjects(lis.Select(n => new Project()
            //{
            //    RzId = n.RzId,
            //    createTime = n.createTime,
            //    DemandShowroomId = n.DemandShowroomId,
            //    extensioin = n.extensioin,
            //    extension1 = n.extension1,
            //    Extension12 = n.Extension12,
            //    Extension15 = n.Extension15,
            //    Extension16 = n.Extension16,
            //    extension2 = n.extension2,
            //    pics = n.pics,
            //    rbName = n.rbName,
            //    rbTime = n.rbTime,
            //    reTime = n.reTime,
            //    rlx = n.rlx,
            //    rxName = n.rxName,
            //    index = n.index + 6
            //}).ToList()));
            //sb.Append("}");
            //#endregion 
            #endregion

            for (int i = 0; i < ok; i++)
            {
                sb.Append("{");
                sb.Append("\"msg\":\"第" + (i + 1) + "天\",");
                sb.Append("\"time\":\"" + dtstart.AddDays(i).ToString("yy-MM-dd") + "\",");
                sb.Append("\"data\":");
                sb.Append(GetOneDayProjects(lis.Select(n => new Project()
                {
                    RzId = n.RzId,
                    createTime = n.createTime,
                    DemandShowroomId = n.DemandShowroomId,
                    extensioin = n.extensioin,
                    extension1 = n.extension1,
                    Extension12 = n.Extension12,
                    Extension15 = n.Extension15,
                    Extension16 = n.Extension16,
                    extension2 = n.extension2,
                    pics = n.pics,
                    rbName = n.rbName,
                    rbTime = n.rbTime,
                    reTime = n.reTime,
                    rlx = n.rlx,
                    rxName = n.rxName,
                    index = n.index + i
                }).ToList()));
                sb.Append("},");


            }


            sb.Append("]");
            sb.Append("}");
            return sb.ToSafeString().Replace(",]", "]");



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

        private static StringBuilder GetOneDayProjects7(List<Project> lis, int Y)
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

        public static string GetAllProjectsext(string desingerid)
        {
            string sql = "select top  100 Extension1, DemandShowroomId,Extension12 ,Extension16 from DemandShowRooms where Extension15=@desingerid order by DemandShowroomId desc;";

            DataTable dt = SqlHelper.ExecuteDataTable(sql, new SqlParameter("@desingerid", desingerid));

            List<object> lis = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                var project = new { projectid = row["DemandShowroomId"].ToSafeString(), projectname = row["Extension12"].ToSafeString(), begintime = row["Extension16"].ToSafeString(), needdays = row["Extension1"].ToSafeString(), num = 0 };
                lis.Add(project);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"newsnumber\":" + SqlHelper.ExecuteScalar(@"
  select  COUNT(*) as num  from xgrlrz 
 
 
 
  right join ( select Extension12, DemandShowroomId from DemandShowRooms 
 where
 Extension15='" + desingerid + @"' 
 )b  on xgrlrz.extension=b.DemandShowroomId where xgrlrz.isread='1'  
").ToSafeString() + ",");
            sb.Append("\"number1\":5,");
            sb.Append("\"number2\":12,");
            sb.Append("\"desingerid\":\"" + desingerid + "\",");
            sb.Append("\"projects\":");
            sb.Append(JsonConvert.SerializeObject(lis));
            sb.Append("}");
            return sb.ToSafeString();
        }
        /// <summary>
        /// 加载修改状态
        /// </summary>
        /// <param name="peojectid"></param>
        /// <returns></returns>
        public static string LoadState(string projectid)
        {
            string sql = "select  rbName,rbtime,rxName,reTime,rlx,pics ,extensioin from  XState where extension1='" + projectid + "';";

            #region 项目开始时间
            DateTime dt0 = DateTime.Now;
            if (DateTime.TryParse(SqlHelper.ExecuteScalar("select Extension16 from DemandShowRooms where  DemandShowroomId='" + projectid + "'").ToSafeString(), out dt0))
            {

            }
            else
            {
                dt0 = DateTime.Now;
            }
            int end = 65;
            if (int.TryParse(SqlHelper.ExecuteScalar("select Extension1 from DemandShowRooms where  DemandShowroomId='" + projectid + "'").ToSafeString(), out end))
            {

            }


            #endregion

            DataTable dt = SqlHelper.ExecuteDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = GetProjectStr(dt, dt0, end, projectid);

                return sb.ToSafeString().Replace(",]", "]");
            }
            else
            {

                string sql2 = "select  rbName,rbtime,rxName,reTime,rlx,pics ,extensioin  from    View_sgrz";

                DataTable dt2 = SqlHelper.ExecuteDataTable(sql2);

                return GetProjectStr(dt2, dt0, end, projectid).ToSafeString().Replace(",]", "]"); ;
            }



        }

        private static StringBuilder GetProjectStr(DataTable dt, DateTime t, int end, string projecid)
        {
            string projectname = SqlHelper.ExecuteScalar("select   Extension12 from DemandShowRooms  where DemandShowroomId='" + projecid + "' ").ToSafeString();
            DataRow[] jdRow = dt.Select("rlx='jd'"); //阶段
            DataRow[] xmRow = dt.Select("rlx='xm'"); //项目和人员
            DataRow[] ryRow = dt.Select("rlx='ry'"); //项目和人员
            List<object> lisjd = new List<object>();
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"projectname\":\"" + projectname + "\",");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"starttime\":\"" + t.ToString("yyyy-MM-dd") + "\",");
            sb.Append("\"endtime\":\"" + t.AddDays(end - 1).ToString("yyyy-MM-dd") + "\",");
            sb.Append("\"Timelimit\":" + end + ",");
            sb.Append("\"jd\":[");

            foreach (var item in jdRow)
            {
                string d1 = t.AddDays(int.Parse(item["rbTime"].ToSafeString()) - 1).ToString("yyyy-MM-dd"); ;

                string d2 = t.AddDays(int.Parse(item["reTime"].ToSafeString()) - 1).ToString("yyyy-MM-dd"); ;

                sb.Append("{\"name\":\"" + item["rbName"].ToSafeString() + "\",\"beiginday\":\"" + d1 + "\",\"endday\":\"" + d2 + "\",\"b\":" + item["rbTime"].ToSafeString() + ",\"e\":" + item["reTime"].ToSafeString() + "},");

            }

            sb.Append("],");

            StringBuilder sbxm = new StringBuilder();
            sbxm.Append("\"xm\":");
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
                        xm.Append("{\"name\":\"" + item["rxname"].ToSafeString() + "\",\"time\":\"" + t.AddDays(int.Parse(item["reTime"].ToSafeString()) - 1).ToString("yyyy-MM-dd") + "\",\"daysindex\":" + item["retime"].ToSafeString() + "},");
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
            sbry.Append("\"ry\":");
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
                        xm.Append("{\"name\":\"" + item["rxname"].ToSafeString() + "\",\"time\":\"" + t.AddDays(int.Parse(item["reTime"].ToSafeString()) - 1).ToString("yyyy-MM-dd") + "\",\"daysindex\":" + item["retime"].ToSafeString() + "},");
                    }

                }

                xm.Append("]");
                xm.Append("},");
                sbry.Append(xm);
            }
            #endregion

            sbry.Append("]");



            sb.Append(sbry);

            sb.Append(",\"related\":[{\"name\":\"王紫云\",\"desingerid\":\"1\"},{\"name\":\"张天英\",\"desingerid\":\"2\"},{\"name\":\"郑泽禹\",\"desingerid\":\"3\"},{\"name\":\"关东\",\"desingerid\":\"4\"},{\"name\":\"朱丹琼\",\"desingerid\":\"5\"}]");


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

            if (!Regex.IsMatch(Timelimit, @"^\d+$"))
            {
                return "{\"errorcode\":1,\"msg\":\"Timelimit必须输入正整数\"}"; ;
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


            object o = SqlHelper.ExecuteScalar("  select DemandShowroomId from DemandShowRooms where Extension12=@name and Extension14=@phone", new SqlParameter("@name", name), new SqlParameter("@phone", phone));


            return "{\"errorcode\":0,\"msg\":\"项目添加成功\",\"projectid\":\"" + o.ToSafeString() + "\"}";
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

            return "{\"errorcode\":0,\"desingers\":" + JsonConvert.SerializeObject(lis) + ",\"foremans\":[]}";

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
            string sql = "select Extension12, DemandShowroomId, Extension1,Extension16 from DemandShowRooms where  DemandShowRooms.Extension16 is not null and DemandShowroomId=" + projectid;
            DataTable dtproject = SqlHelper.ExecuteDataTable(sql);
            var project = new { projectname = dtproject.Rows[0]["Extension12"].ToSafeString(), projectid = projectid, needdays = Convert.ToInt32(dtproject.Rows[0]["Extension1"]), begintime = Convert.ToDateTime(dtproject.Rows[0]["Extension16"]) };
            #endregion


            //得到项目   项目id   项目所需时间    项目开始时间
            DataTable dt = new DataTable();

            dt = SqlHelper.ExecuteDataTable("select * from XState where extension1='" + projectid + "' and rlx='jd'");

            if (dt == null || dt.Rows.Count < 1)
            {
                dt = SqlHelper.ExecuteDataTable("select * from View_sgrz  where rlx='jd'  ;");
            }

            //rzId	rbName	rxName	rbTime	reTime	rlx	pics	extensioin	extension1	extension2	createTime
            //5718	橱柜定制家具	NULL	NULL	NULL	xm	NULL	9	3017	NULL	2015-04-23


            //List<object> lisjs = new List<object>();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    var row=dt.Rows[i];
            //    var obj = new { rzId = row[""].ToSafeString(), rbName = row[""].ToSafeString(), rxName = row[""].ToSafeString(), rbTime = row[""].ToSafeString(), reTime = row[""].ToSafeString(), rlx = row[""].ToSafeString(), pics = row[""].ToSafeString(), extensioin = row[""].ToSafeString(), extension1 = row[""].ToSafeString(), extension2 = row[""].ToSafeString(), createTime = row[""].ToSafeString() };

            //    lisjs.Add(obj);
            //}

            #region 构造结果字符串

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"errorcode\":0,");
            sb.Append("\"projectname\":\"" + project.projectname + "\",");
            sb.Append("\"data\":");
            sb.Append("[");
            //List<object> lis = new List<object>();
            for (int i = 0; i < project.needdays; i++)
            {
                //第i天      (i+1)天所处阶段    天数
                //select rbName from View_sgrz  where rlx='jd' and 11>=rbTime and 15<=reTime;

                // DataRow[] rows = dt.Select((i + 1) + ">=rbTime and " + (i + 1) + "<=reTime");
                int ttt = i + 1;
                DataRow[] rows = dt.Select(ttt + "<=reTime"); ;
                 
                string s = "";

                foreach (var item in rows)
                {
                    if (item["rbTime"].Todouble()<=ttt)
                    {
                        s += item["rbName"].ToSafeString() + ",";
                    }
                    
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

                        if (arr[j].Contains("http"))
                        {
                            //jk += "\"" + arr[i].Replace("\\", "/") + "\",";
                            string v = arr[j].Replace("\\", "/");

                            string sm = "http://mobile.mj100.com/desingerapi/pic?url=" + v + "&w=260&h=166";

                            string ss1 = "{\"bigimg\":\"" + v + "\",\"smallimg\":\"" + sm + "\"},";

                            jk += ss1;
                        }
                        else
                        {
                            // jk += "\"http://www.mj100.com/SGDaily/UploadFile/500/" + arr[i].Replace("\\", "/") + "\",";

                            string v = "http://www.mj100.com/SGDaily/UploadFile/500/" + arr[j].Replace("\\", "/");

                            string sm = "http://mobile.mj100.com/desingerapi/pic?url=" + v + "&w=260&h=166";

                            string ss1 = "{\"bigimg\":\"" + v + "\",\"smallimg\":\"" + sm + "\"},";
                            jk += ss1;
                        }




                    }
                    jk += "]";

                    pics = jk;
                }




                var obj = new { jd = s.TrimEnd(','), pic = pics, time = project.begintime.AddDays(i).ToString("yyyy-MM-dd"), dayindex = (i + 1) };

                var ss = "{\"jd\":\"" + obj.jd + "\",\"pic\":" + pics.Replace(",]", "]") + ",\"time\":\"" + obj.time + "\",\"dayindex\":\"" + obj.dayindex + "\"},";
                sb.Append(ss);

                //lis.Add(obj);
            }
            sb.Append("]");
            #endregion
            //查询项目记录
            sb.Append("}");

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
        public static string AddModifyRecord(string desingername, string con, string Reason, string projectid, string Delaydays)
        {
            if (Reason.IsEmpty())
            {
                Reason = " ";
            }
            if (CheckParm(new Dictionary<string, string>() { { "desingername", desingername }, { "con", con }, { "Reason", Reason }, { "projectid", projectid }, { "Delaydays", Delaydays } }))
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
            SqlHelper.ExecuteNonQuery(sql, arr);
            return "{\"errorcode\":0,\"msg\":\"添加修改记录成功\"}";
        }

        /// <summary>
        /// 添加照片
        /// </summary>
        /// <returns></returns>
        public static string InsertPic(string projectid, string day, string pic)
        {
            string sql = "select rzPicId from xStatePic where demandId='" + projectid + "' and day='" + day + "' ;";

            object o = SqlHelper.ExecuteScalar(sql);

            if (o == null)
            {
                SqlHelper.ExecuteNonQuery("insert into xStatePic(demandId,day,pic,createTime) values('" + projectid + "','" + day + "','" + pic + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "');");
            }
            else
            {
                SqlHelper.ExecuteNonQuery("update xStatePic set pic=pic+'," + pic + "' where demandId='" + projectid + "' and day='" + day + "' ;");
            }
            return "";
        }


        /// <summary>
        /// 得到新闻列表
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public static string GetNewsList(string desingerid)
        {
            if (CheckParm(new Dictionary<string, string> { { "desingerid", desingerid } }))
            {
                return errormsg;
            }


            #region
            string sql = @" select * from xgrlrz  right join ( select Extension12, DemandShowroomId from DemandShowRooms 
 where
 Extension15='" + desingerid + @"' 
 )b  on xgrlrz.extension=b.DemandShowroomId where xgrlrz.createTime is not null order by xgrlrz.id  desc";
            #endregion


            DataTable dt = SqlHelper.ExecuteDataTable(sql);



            #region 新消息
            List<object> lis = new List<object>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                int iss = 0;
                if (row["isread"].ToSafeString() == "1")
                {
                    iss = 1;
                }

                lis.Add(new { newsid = row["id"].ToSafeString(), name = row["username"].ToSafeString().Replace("%40", "@"), projectname = row["Extension12"].ToSafeString(), time = row["createTime"].ToSafeString(), isread = iss });

            }

            #endregion


            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append("\"errorcode\":0,");


            sb.Append("\"data\":");



            sb.Append(JsonConvert.SerializeObject(lis));

            sb.Append("}");

            return sb.ToSafeString();
        }

        /// <summary>
        /// 新消息详情
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static string GetNewsDetail(string newsid)
        {
            if (CheckParm(new Dictionary<string, string> { { "newsid", newsid } }))
            {
                return errormsg;
            }

            DataTable dt = SqlHelper.ExecuteDataTable(" select * from xgrlrz where id='" + newsid + "' order by id desc;");
            object obj = null;
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                obj = new { errorcode = 0, name = row["userName"].ToSafeString(), modification = Regex.Replace(row["con"].ToSafeString(), "", ""), delaydays = row["extension1"].ToSafeString(), reason = row["xgyy"].ToSafeString(), projectid = row["extension"].ToSafeString() };
            }

            SqlHelper.ExecuteNonQuery("update xgrlrz set isread=0 where id='" + newsid + "'");

            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <returns></returns>
        public static string ChangeRecords(string projectid)
        {
            if (CheckParm(new Dictionary<string, string>() { { "projectid", projectid } }))
            {
                return errormsg;
            }
            List<object> lis = new List<object>();

            DataTable dt = SqlHelper.ExecuteDataTable(" select * from xgrlrz where extension='" + projectid + "' order by id desc;");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var obj = new { name = row["userName"].ToSafeString(), modification = Regex.Replace(row["con"].ToSafeString(), "", ""), delaydays = row["extension1"].ToSafeString(), reason = row["xgyy"].ToSafeString(), projectid = row["extension"].ToSafeString(), time = row["createtime"].ToSafeString() };


                lis.Add(obj);
            }





            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"errorcode\":0,");
            sb.Append("\"data\":");
            sb.Append(JsonConvert.SerializeObject(lis));
            sb.Append("}");
            return sb.ToString();
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


        public static string UpdatePic(string projectid, string dayindex, string picurl)
        {
            //  select pic from xStatePic where demandId='4287' and day='0'


            if (!Regex.IsMatch(projectid.ToSafeString(), @"^\d+$"))
            {
                return "{\"errorcode\":1,\"msg\":\"projectid不合法\"}"; ;
            }
            if (!Regex.IsMatch(dayindex.ToSafeString(), @"^\d+$"))
            {
                return "{\"errorcode\":1,\"msg\":\"dayindex不合法\"}"; ;
            }
            if (picurl.IsEmpty())
            {
                return "{\"errorcode\":1,\"msg\":\"图片路径不合法\"}"; ;
            }


            object o = SqlHelper.ExecuteScalar("select pic from xStatePic where demandId='" + projectid + "' and day='" + dayindex + "'");

            string v = o.ToSafeString().Replace("," + picurl.Trim(), "").Replace(picurl.Trim(), "");


            SqlHelper.ExecuteNonQuery("update xStatePic set pic='" + v + "'  where demandId='" + projectid + "' and day='" + dayindex + "'");

            return "{\"errorcode\":0,\"msg\":\"操作成功\"}"; ;
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
