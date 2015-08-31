using MJAPI.Controllers.filter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MJAPI.Controllers
{
    [DesingerExceptionAttribute]
    public class DesingerApiController : Controller
    {


        //
        // GET: /DesingerApi/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public JsonResult Login(string loginname, string pwd)
        {

            return Myjson(DesingerBLL.Desinger.Login(loginname, pwd));
        }




        /// <summary>
        /// 得到用户所有的项目
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public JsonResult GetProjects(string desingerid)
        {

            return Myjson(DesingerBLL.Desinger.GetAllProjectsext(desingerid));
        }


        /// <summary>
        /// 最近七天要做的事
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public JsonResult TheThingsToDoInTheNearFuture(string desingerid)
        {

            return Myjson(DesingerBLL.Desinger.GetTodaythings7(desingerid));

        }


        /// <summary>
        /// 最近一个月要做的事
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public JsonResult TheThingsToDoInTheNearFuture30(string desingerid)
        {

            return Myjson(DesingerBLL.Desinger.GetTodaythings30(desingerid));

        }

        /// <summary>
        /// 今天要做的事
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public JsonResult GetTodaythings(string desingerid)
        {
            return Myjson(DesingerBLL.Desinger.GetTodaythings(desingerid));
        }


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="desingerid">设计师id</param>
        /// <param name="name">姓名</param>
        /// <param name="phone">电话</param>
        /// <param name="housetype">户型</param>
        /// <param name="area">面积</param>
        /// <param name="jcprice">建材总价</param>
        /// <param name="sgprice">施工总价</param>
        /// <param name="address">地址</param>
        /// <param name="begintime">开始时间</param>
        /// <param name="Timelimit">需要天数</param>
        /// <param name="relative">相关人员,请以逗号拼接id</param>
        /// <returns></returns>
        public string AddProject(string desingerid, string name, string phone, string housetype, string area, string jcprice, string sgprice, string address, string begintime, string Timelimit, string relative)
        {
            //   AddDemand(string desingerid, string userid,string name, string phone, string housetype, string area, string jcprice, string sgprice, string address, string begintime, string Timelimit)
            //desingerid = "0"; name = "大鹏"; phone = "15133333333"; housetype = "15136134321"; area = "200"; jcprice = "10"; sgprice = "10"; address = "15136134321"; begintime = "2015-9-17";

            //desingerid=0&name=大鹏&phone=15133333333&housetype=15136134321&area=200&jcprice=10&sgprice=10&address=15136134321&begintime=2015-9-17
            //Timelimit = "88";



            return DesingerBLL.Desinger.AddProject(desingerid, name, phone, housetype, area, jcprice, sgprice, address, begintime, Timelimit, relative);
        }



        /// <summary>
        /// 查询设计师
        /// </summary>
        /// <param name="searchvalue"></param>
        /// <returns></returns>
        public string DesignerSearch(string searchvalue)
        {

            return DesingerBLL.Desinger.DesignerSearch(searchvalue);

        }



        /// <summary>
        /// 加载项目各阶段详情
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public JsonResult LoadProject(string projectid)
        {


            return Myjson(DesingerBLL.Desinger.LoadState(projectid));

        }


        /// <summary>
        /// 得到项目进度
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public string GetProjectSchedule(string projectid)
        {
            return DesingerBLL.Desinger.ProjectSchedule(projectid);
        }


        /// <summary>
        /// 上传进度图
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="dayindex"></param>
        /// <returns></returns>
        public string UploadScheduleImg(string projectid, string dayindex)
        {
            if (projectid.IsEmpty() || dayindex.IsEmpty())
            {
                return "{\"errorcode\":1,\"msg\":\"projectid        不能为空\"}"; ;
            }
            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength < 10)
            {
                return "{\"errorcode\":1,\"msg\":\"上传图片过小\"}"; ;
            }

            string fileext = Path.GetExtension(file.FileName);

            if (fileext.ToLower() != ".jpg" && fileext.ToLower() != ".png" && fileext.ToLower() != ".gif")
            {
                return "{\"errorcode\":1,\"msg\":\"请上传图片\"}"; ;
            }
            string temppath = "~/sg/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

            string path = System.Web.HttpContext.Current.Request.MapPath(temppath);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string filename = Guid.NewGuid().ToString().Substring(0, 6) + projectid + fileext;

            MakeSmallImg(file.InputStream,3,5);

            file.SaveAs(path + filename);


            string url = "http://mobile.mj100.com" + temppath.Replace("~", "") + filename;


            DesingerBLL.Desinger.InsertPic(projectid, dayindex, url);


            return "{\"errorcode\":0,\"msg\":\"上传成功\",\"url\":\"http://mobile.mj100.com" + temppath.Replace("~", "") + filename + "\",\"smallimg\":\"http://mobile.mj100.com/desingerapi/pic?url=" + url + "&w=260&h=166\"}";
        }





        public ActionResult Pic(string url ,int w,int h)
        {
            //url = "http://mobile.mj100.com/sg/2015/8/7fbb754287.png";
            //w = 50;
            //h = 50;
            var stram = new System.Net.WebClient().OpenRead(url);


            byte[] bytes = MakeSmallImg(stram, w, h);


            return File(bytes, "image/jpeg", "small.jpg");
        }


        /// <summary>
        /// 压缩图片  15-5-4 by倩
        /// </summary>
        /// <param name="fromFileStream"></param>
        /// <param name="templateWidth"></param>
        /// <param name="templateHeight"></param>
        /// <returns></returns>
        public byte[] MakeSmallImg(System.IO.Stream fromFileStream, System.Double templateWidth, System.Double templateHeight)
        {
            //从文件取得图片对象，并使用流中嵌入的颜色管理信息 
            System.Drawing.Image myImage = System.Drawing.Image.FromStream(fromFileStream, true);
            //缩略图宽、高 
            System.Double newWidth = myImage.Width, newHeight = myImage.Height;
            //宽大于模版的横图 
            if (myImage.Width > myImage.Height || myImage.Width == myImage.Height)
            {
                if (myImage.Width > templateWidth)
                {
                    //宽按模版，高按比例缩放 
                    newWidth = templateWidth;
                    newHeight = myImage.Height * (newWidth / myImage.Width);
                }
            }
            //高大于模版的竖图 
            else
            {
                if (myImage.Height > templateHeight)
                {
                    //高按模版，宽按比例缩放 
                    newHeight = templateHeight;
                    newWidth = myImage.Width * (newHeight / myImage.Height);
                }
            }
            //取得图片大小 
            System.Drawing.Size mySize = new Size((int)newWidth, (int)newHeight);
            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(mySize.Width, mySize.Height);
            //新建一个画板 
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空一下画布 
            g.Clear(Color.White);
            //在指定位置画图 
            g.DrawImage(myImage, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
            new System.Drawing.Rectangle(0, 0, myImage.Width, myImage.Height),
            System.Drawing.GraphicsUnit.Pixel);


           
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                g.Dispose();
                myImage.Dispose();
                bitmap.Dispose();

                //输出字节流
                return stream.ToArray();

            }

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
        public string AddModifyRecord(string desingername, string con, string Reason, string projectid, string Delaydays,string desingerids)
        {
            return DesingerBLL.Desinger.AddModifyRecord(desingername, con, Reason, projectid, Delaydays);
        }

        /// <summary>
        /// 事情数量
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public JsonResult GetNumber(string desingerid)
        {

            return Json(new { errorcode = 0, number1 = 5, number2 = 12, desingerid = desingerid }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 添加的各个小项
        /// </summary>
        /// <returns></returns>
        public JsonResult GetConfiglist()
        {
            //装前准备
            //水电改造
            //泥作
            //木作
            //漆作
            //整体安装

            #region MyRegion
            var jd = new
              {
                  errorcode = 0,
                  jd = new string[] { "装前准备", "水电改造", "泥作", "木作", "漆作", "整体安装" },
                  xm = new string[] {
                "铝扣板",
           "灯具五金开关",
            "橱柜定制家具",
            "瓷砖",
            "门",
            "石材",
            "断桥铝窗",
            "防盗门",
           "暖气",
            "壁纸",
            "地板",
            "洁具",
            "地暖",
            "中央空调",
            "新风",
            "智能家居"
                },
                  ry = new string[] { "收款日期", "微信推送", "建材", "业主", "设计师", "小美管家", "项目经理" }
              };

            //铝扣板
            //灯具五金开关
            //橱柜定制家具
            //瓷砖
            //门
            //石材
            //断桥铝窗
            //防盗门
            //暖气
            //壁纸
            //地板
            //洁具
            //地暖
            //中央空调
            //新风
            //智能家居


            //收款日期
            //微信推送
            //建材
            //业主
            //设计师
            //小美管家
            //项目经理 
            #endregion


            return Json(jd,JsonRequestBehavior.AllowGet); ;
        }
        /// <summary>
        /// 消息列表
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public string GetNewsList(string desingerid)
        {
            return DesingerBLL.Desinger.GetNewsList(desingerid);
        }

        /// <summary>
        /// 消息详情
        /// </summary>
        /// <param name="newsid"></param>
        /// <returns></returns>
        public string GetNewsDetail(string newsid)
        {
            return DesingerBLL.Desinger.GetNewsDetail(newsid);
        }

        /// <summary>
        /// 拿到项目的修改记录
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public string GetChangeRecords(string projectid)
        {
            return DesingerBLL.Desinger.ChangeRecords(projectid);
        }


        /// <summary>
        /// 删除安排
        /// </summary>
        /// <returns></returns>
        public string Deletearrangement(string rname,string rxname, string rlx, string projectid)
        {
            return DesingerBLL.Desinger.Delete(rname,rxname,rlx,projectid);
        }


        /// <summary>
        /// 编辑项目人员
        /// </summary>
        /// <returns></returns>
        public string EditXmRyChild()
        {
          
            #region 获取到请求的值
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            string postStr = Encoding.UTF8.GetString(b);
            #endregion
            System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "Edit.txt", postStr + "     :" + DateTime.Now.ToSafeString() + "\r\n\r\n");
            return DesingerBLL.Desinger.EditXmRy(postStr);
          
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="rname"></param>
        /// <param name="rlx"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public string Test(string rname, string rlx, string projectid)
        {

            

            return DesingerBLL.Desinger.EditXmRy("");
        }


        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="dayindex"></param>
        /// <param name="picurl"></param>
        /// <returns></returns>
        public   string UpdatePic(string projectid, string dayindex, string picurl)
        {
            return DesingerBLL.Desinger.UpdatePic( projectid,  dayindex,  picurl);
        
        }


        /// <summary>
        /// 重写 Json方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected JsonResult Myjson(string data)
        {
            return new MyjsonResault() { ContentEncoding = System.Text.Encoding.UTF8, ContentType = "application/json", Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


       

        /// <summary>
        /// 添加阶段安排 项目安排  人员安排 大类
        /// </summary>
        /// <returns></returns>
        public string AddXmJdRy(string rname,string rlx,string projectid)
        {

            return DesingerBLL.Desinger.AddXm(rname, rlx, projectid);
        }

        /// <summary>
        /// 添加项目人员安排子类
        /// </summary>
        /// <returns></returns>
        public string AddXmRyChild(string rname,string rxname,string rlx,string index,string projectid)
        {
            return DesingerBLL.Desinger.AddChild(rname, rxname, rlx, index, projectid);
        
        }


        /// <summary>
        /// 更新阶段时间
        /// </summary>
        /// <param name="jdname"></param>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public  string Updatejd(string jdname, string b, string e, string projectid)
        {
          //  ?jdname=装前准备&b=2&e=4&projectid=4267
           
            return DesingerBLL.Desinger.Updatejd(jdname, b, e, projectid);
        }


        public string test2()
        {

            return DesingerBLL.Desinger.AddChild("welcome", "welcome12", "xm", "12", "4246");
        }


        public string test3()
        {

            return DesingerBLL.Desinger.GetTodaythings30("2");
        }


    }

    /// <summary>
    /// 重写jsonresault
    /// </summary>
    public class MyjsonResault : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(Data);
        }

    }
}
