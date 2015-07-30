using MJAPI.Controllers.filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            return Myjson(DesingerBLL.Desinger.GetAllProjects(desingerid));
        }


        /// <summary>
        /// 最近七天要做的事
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public JsonResult TheThingsToDoInTheNearFuture(string desingerid)
        {

            return Myjson(DesingerBLL.Desinger.GetDesingerProjects(desingerid));

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
                return "{\"errcode\":1,\"msg\":\"projectid        不能为空\"}"; ;
            }
            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength < 10)
            {
                return "{\"errcode\":1,\"msg\":\"上传图片过小\"}"; ;
            }

            string fileext = Path.GetExtension(file.FileName);

            if (fileext.ToLower() != ".jpg" && fileext.ToLower() != ".png" && fileext.ToLower() != ".gif")
            {
                return "{\"errcode\":1,\"msg\":\"请上传图片\"}"; ;
            }
            string temppath = "~/sg/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

            string path = System.Web.HttpContext.Current.Request.MapPath(temppath);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string filename = Guid.NewGuid().ToString().Substring(0, 6) + projectid + fileext;

            file.SaveAs(path + filename);


            string url = "http://mobile.mj100.com" + temppath.Replace("~", "") + filename;


            DesingerBLL.Desinger.InsertPic(projectid, dayindex, url);


            return "{\"errcode\":0,\"msg\":\"上传成功\",\"url\":\"http://mobile.mj100.com" + temppath.Replace("~", "") + filename + "\"}";
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
        public  string AddModifyRecord(string desingername, string con, string Reason, string projectid, string Delaydays)
        {
            return DesingerBLL.Desinger.AddModifyRecord(desingername,con,Reason,projectid,Delaydays);
        }




        public string Test(string projectid)
        {

            return DesingerBLL.Desinger.ProjectSchedule(projectid);
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
