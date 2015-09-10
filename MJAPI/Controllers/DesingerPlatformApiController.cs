using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MJAPI.Controllers
{
    [DesingerPlatformException]
    
    public class DesingerPlatformApiController : Controller
    {
        //
        // GET: /DesingerPlatformApi/


        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 设计师登录
        /// </summary>
        /// <param name="loginname">登录名</param>
        /// <param name="pwd">明文密码</param>
        /// <returns></returns>
        [Yz]
        public string Login(string loginname, string pwd)
        {
            return DesingerBLL.DesignerPlatform.Login(loginname,pwd); ;
        }


        /// <summary>
        /// 订单管理页数据
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public string OrderManagementData(string desingerid)
        {
            return DesingerBLL.DesignerPlatform.OrderManagementData(desingerid);
        }


        /// <summary>
        /// 主题套装列表
        /// </summary>
        /// <param name="roomname">房间</param>
        /// <param name="theme">主题</param>
        /// <param name="housetype">户型</param>
        /// <param name="area">面积</param>
        /// <returns></returns>
        public  string Topiclist(string roomname, string theme, string housetype, string area)
        {

            return DesingerBLL.DesignerPlatform.Topiclist(roomname,theme,housetype,area);
        }


        /// <summary>
        /// 主题套装详情
        /// </summary>
        /// <param name="roomids"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public   string Packagedetails(string roomids, string name)
        {
            return DesingerBLL.DesignerPlatform.Packagedetails(roomids,name);
        }

        /// <summary>
        /// 备选房间方案列表
        /// </summary>
        /// <param name="roomtype"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public string GetModelRoomList(string roomtype, string price)
        {
            return  DesingerBLL.DesignerPlatform.GetModelRoomS2(roomtype,price);
        }



        /// <summary>
        /// 备选房间方案详情
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public string GetModelDetail(string did)
        {
            return DesingerBLL.DesignerPlatform.GetModelDetail(did);
        
        }

        /// <summary>
        /// 得到推荐主材
        /// </summary>
        /// <returns></returns>
        public string TjZc(string productid)
        {
            if (productid.IsEmpty())
            {
                return  "{\"errorcode\":1,\"msg\":\"参数有空值\"}"; ;
            }
            return DesingerBLL.DesignerPlatform.RecommendZc(productid);
        }



        /// <summary>
        /// 添加设计师自己的模块
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public string AddDesingerModular(string par)
        {
            DesingerBLL.DesignerPlatform bll = new DesingerBLL.DesignerPlatform();


            return bll.AddRom(par); ;
        }




        /// <summary>
        /// 得到设计师自己的模块
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public string GetDesingerModular(string tempid)
        {
            DesingerBLL.DesignerPlatform bll = new DesingerBLL.DesignerPlatform();

            return bll.GetUserModule(tempid); ;
        }
             

        /// <summary>
        /// 使用备选方案
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public string Usageplan(string par)
        {
            DesingerBLL.DesignerPlatform bll = new DesingerBLL.DesignerPlatform();

            return bll.SyFj(par);
        
        }



        /// <summary>
        /// 设计师的搭配
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public string Myplan(string desingerid)
        {


            return DesingerBLL.DesignerPlatform.Myplan(desingerid);

        }

        /// <summary>
        /// 删除设计师定义的一个模块
        /// </summary>
        /// <param name="userroomid"></param>
        /// <returns></returns>
        public string DeleteSingleModule(string userroomid)
        {
            return DesingerBLL.DesignerPlatform.DeleteSingleModule(userroomid);
        }


        /// <summary>
        /// 保存方案
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public string Savescheme(string par)
        {
            //{"desingerid":"设计师id","pic":"","name":"","totlearea":"总面积","totleprice":"总价格","data":[{"room":"kct","did":"4","area":"100"},{"room":"kct","did":"4"}]}

            //{"desingerid":"10086","pic":"http://www.mj100.com/img/index/l_2bg.png","name":"新古风","totlearea":"100","totleprice":"3000","data":[{"room":"kct","did":"3582","area":"100"}，{"room":"kct","did":"3582","area":"500"}]}
            DesingerBLL.DesignerPlatform bll = new DesingerBLL.DesignerPlatform();

            return bll.AddallRoom(par);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="userdemnadid">用户需求id</param>
        /// <param name="olddemandid">将要被复制的需求id</param>
        /// <returns></returns>
        public string Schemereplication(string userid ,string userdemnadid,string olddemandid)
        {
            if (userdemnadid==olddemandid)
            {
                return "";
            }


            return DesingerBLL.DesignerPlatform.CopyDemand(userid, userdemnadid, olddemandid);

           // return DesingerBLL.DesignerPlatform.CopyDemand("8849","6292","6213");
        }

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <returns></returns>
        public string Identityauthentication(string desingerid, string name ,string idnumber)
        {


            #region 正面
            HttpPostedFileBase file = Request.Files["z"];
            if (file.ContentLength < 10)
            {
                return "{\"errorcode\":1,\"msg\":\"上传图片过小\"}"; ;
            }
            string fileext = Path.GetExtension(file.FileName);
            if (fileext.ToLower() != ".jpg" && fileext.ToLower() != ".png" && fileext.ToLower() != ".gif")
            {
                return "{\"errorcode\":1,\"msg\":\"请上传图片\"}"; ;
            }
            string temppath = "~/xerox/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            string path = System.Web.HttpContext.Current.Request.MapPath(temppath);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string filename = Guid.NewGuid().ToString().Substring(0, 6) + idnumber + fileext;
            file.SaveAs(path + filename);
            string url = "http://mobile.mj100.com" + temppath.Replace("~", "") + filename; //正面 
            #endregion



            #region 反面
            HttpPostedFileBase file1 = Request.Files["f"];
            if (file1.ContentLength < 10)
            {
                return "{\"errorcode\":1,\"msg\":\"上传图片过小\"}"; ;
            }
            string fileext1 = Path.GetExtension(file1.FileName);
            if (fileext1.ToLower() != ".jpg" && fileext1.ToLower() != ".png" && fileext1.ToLower() != ".gif")
            {
                return "{\"errorcode\":1,\"msg\":\"请上传图片\"}"; ;
            }
            string temppath1 = "~/xerox/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            string path1 = System.Web.HttpContext.Current.Request.MapPath(temppath1);
            Directory.CreateDirectory(Path.GetDirectoryName(path1));
            string filename1 = Guid.NewGuid().ToString().Substring(0, 6) + idnumber + fileext1;
            file1.SaveAs(path1 + filename1);
            string url1 = "http://mobile.mj100.com" + temppath1.Replace("~", "") + filename1; //反面
            #endregion


            return DesingerBLL.DesignerPlatform.UpdateDesinger(desingerid,name,idnumber,url,url1);
        }


        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="desingerid"></param>
        /// <param name="pic"></param>
        /// <param name="name"></param>
        /// <param name="mj"></param>
        /// <param name="totleprice"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string AddCollection(string desingerid,string pic,string name,string mj,string totleprice,string ids)
        { 
        DesingerBLL.DesignerPlatform bll=new DesingerBLL.DesignerPlatform ();
        return bll.AddCol(desingerid, "", pic, name, mj, totleprice, "0", "1", ids);
        }


        

        /// <summary>
        /// 我的收藏/设计师自己的收藏
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public   string Mycollection(string desingerid)
        {



            return DesingerBLL.DesignerPlatform.Mycollection(desingerid);
        }


        /// <summary>
        /// 更新设计师标签
        /// </summary>
        /// <param name="desingerid"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string UpdateTags(string desingerid, string tags)
        {
            return DesingerBLL.DesignerPlatform.UpdateTags(desingerid,tags);
        }
        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public JsonResult Feedback(string content)
        {
            return Json(new {errorcode=0,msg="提交意见成功" });
        }

    }
}
