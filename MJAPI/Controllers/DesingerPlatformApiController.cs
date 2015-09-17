using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            return DesingerBLL.DesignerPlatform.Login(loginname, pwd); ;
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
        public string Topiclist(string roomname, string theme, string housetype, string area)
        {

            return DesingerBLL.DesignerPlatform.Topiclist(roomname, theme, housetype, area);
        }


        /// <summary>
        /// 主题套装详情
        /// </summary>
        /// <param name="roomids"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Packagedetails(string roomids, string name)
        {
            return DesingerBLL.DesignerPlatform.Packagedetails(roomids, name);
        }

        /// <summary>
        /// 备选房间方案列表
        /// </summary>
        /// <param name="roomtype"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public string GetModelRoomList(string roomtype, string price)
        {
            return DesingerBLL.DesignerPlatform.GetModelRoomS2(roomtype, price);
        }



        /// <summary>
        /// 备选房间方案详情
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public string GetModelDetail(string did, string desingerid="")
        {
            return DesingerBLL.DesignerPlatform.GetModelDetail(did, desingerid);

        }

        /// <summary>
        /// 得到推荐主材
        /// </summary>
        /// <returns></returns>
        public string TjZc(string productid)
        {
            if (productid.IsEmpty())
            {
                return "{\"errorcode\":1,\"msg\":\"参数有空值\"}"; ;
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
        /// 给用户发送提醒
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Remind(string id, string openid)
        {
            //id是userid  


            openid = JsApi.Businesslogic.GetAppid(id);

            if (!string.IsNullOrEmpty(openid))
            {
                //发送提醒
                return JsApi.Businesslogic.Remind(id, openid);
            }

            return "{\"eroorcode\":0,\"msg\":\"操作成功\"}";

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
        public string Schemereplication(string userid, string userdemnadid, string olddemandid)
        {
            if (userdemnadid == olddemandid)
            {
                return "";
            }


            return DesingerBLL.DesignerPlatform.CopyDemand( userdemnadid, olddemandid);

            // return DesingerBLL.DesignerPlatform.CopyDemand("8849","6292","6213");
        }

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <returns></returns>
        public string Identityauthentication(string desingerid, string name, string idnumber)
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


            return DesingerBLL.DesignerPlatform.UpdateDesinger(desingerid, name, idnumber, url, url1);
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
        public string AddCollection(string desingerid, string pic, string name, string mj, string totleprice, string ids)
        {
            DesingerBLL.DesignerPlatform bll = new DesingerBLL.DesignerPlatform();
            return bll.AddCol(desingerid, "", pic, name, mj, totleprice, "0", "1", ids,0);
        }




        /// <summary>
        /// 我的收藏/设计师自己的收藏
        /// </summary>
        /// <param name="desingerid"></param>
        /// <returns></returns>
        public string Mycollection(string desingerid)
        {



            return DesingerBLL.DesignerPlatform.Mycollection(desingerid);
        }



        /// <summary>
        /// 删除方案和收藏
        /// </summary>
        /// <param name="desingerid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteColOrPlan(string desingerid, string id)
        {
            return DesingerBLL.DesignerPlatform.DeleteColOrPlan(desingerid, id);
        }

        /// <summary>
        /// 装修清单json版
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string DecorateList(string userid)
        {
            return new DesingerBLL.DesignerPlatform().DecorateList(userid);

        }


        /// <summary>
        /// 更新设计师标签
        /// </summary>
        /// <param name="desingerid"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string UpdateTags(string desingerid, string tags)
        {
            return DesingerBLL.DesignerPlatform.UpdateTags(desingerid, tags);
        }
        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public JsonResult Feedback(string content)
        {
            return Json(new { errorcode = 0, msg = "提交意见成功" });
        }

        /// <summary>
        /// 发送验证码  手机或者邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string SendVerificationCode(string input)
        {

            return DesingerBLL.DesignerPlatform.SendMsg(input);

        }

        /// <summary>
        /// 设计师注册
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string Register(string loginname, string pwd,string yzm)
        {
            bool shpuji = Regex.IsMatch(loginname, @"^\d{11}$");
            bool youxiang = Regex.IsMatch(loginname, @"^.+@.+$");

            #region 看验证码是否正确
            if (Commen.DataCache.GetCache("desinger" + loginname) == null)
            {
                return "{\"errorcode\":\"1\",\"msg\":\"验证码错误\"}";
            }
            if (Commen.DataCache.GetCache("desinger" + loginname).ToSafeString() != yzm)
            {
                return "{\"errorcode\":\"1\",\"msg\":\"验证码错误\"}";
            } 
            #endregion

            if (shpuji)
            {
                return DesingerBLL.DesignerPlatform.Adddesinger(loginname, "", pwd, "", "", loginname);
            }
            else if (youxiang)
            {
                return DesingerBLL.DesignerPlatform.Adddesinger(loginname, "", pwd, "", loginname, "");
            }

            return "";
        }


        /// <summary>
        /// 发送找回密码验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string RetrievepasswordSMS(string input)
        {

            return DesingerBLL.DesignerPlatform.SendMsgGetPwd(input);
        }

        /// <summary>
        /// 验证找回密码验证码是否正确
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string VerifyUserCode(string input,string code)
        {
            bool shpuji = Regex.IsMatch(input, @"^\d{11}$");
            bool youxiang = Regex.IsMatch(input, @"^.+@.+$");


            if (Commen.DataCache.GetCache("desingerpwd" + input) == null)
            {
                return "{\"errorcode\":1,\"msg\":\"验证码错误\"}";
            }
            if (Commen.DataCache.GetCache("desingerpwd" + input).ToSafeString() != code)
            {
                return "{\"errorcode\":1,\"msg\":\"验证码错误\"}";
            }


            return "{\"errorcode\":0,\"msg\":\"验证码正确\"}";
        
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="loginname">登录名/手机号/邮箱</param>
        /// <param name="newpwd">新密码</param>
        /// <param name="code">收到的验证码</param>
        /// <returns></returns>
        public string UpdatePwd(string loginname, string newpwd, string code)
        {
            return DesingerBLL.DesignerPlatform.UpdatePwd(loginname,newpwd,code);
        
        }



        /// <summary>
        /// 生成装修清单
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string Generatinglist(string demandid)
        {
            return DesingerBLL.DesignerPlatform.Generatinglist(demandid);
        }

        /// <summary>
        /// 替换建材
        /// </summary>
        /// <param name="desingerid"></param>
        /// <param name="did"></param>
        /// <param name="originalid"></param>
        /// <param name="newproductid"></param>
        /// <returns></returns>
        public string Replacementbuildingmaterials(string desingerid,string did, string originalid, string newproductid)
        {
            if (originalid==newproductid)
            {
                return "{\"errorcode\":1,\"msg\":\"你没替换任何建材\"}";
            }

            #region 取字典
            Dictionary<string, string> dicp = new Dictionary<string, string>();
            object o = Commen.DataCache.GetCache(desingerid + did);
            if (o != null)
            {
                dicp = o as Dictionary<string, string>;

                if (dicp.ContainsKey(originalid))
                {
                    dicp[originalid] = newproductid;
                }
                else
                {
                    dicp.Add(originalid,newproductid);
                }

                Commen.DataCache.SetCache(desingerid + did, dicp, DateTime.Now.AddHours(1), TimeSpan.Zero);
            }
            else
            {

                dicp.Add(originalid,newproductid);


                Commen.DataCache.SetCache(desingerid + did, dicp, DateTime.Now.AddHours(1), TimeSpan.Zero);
            }
            #endregion
            return "{\"errorcode\":0,\"msg\":\"ok,服务器已缓存\"}"; ;
        }
    }
}
