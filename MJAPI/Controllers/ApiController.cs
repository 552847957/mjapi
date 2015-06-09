using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;



namespace MJAPI.Controllers
{
    [CustomException]
    public class ApiController : Controller
    {
        /// <summary>
        /// api测试
        /// </summary>
        /// <returns></returns>
        public string Test(string v)
        {
            return "这是一个测试";
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string Login(string loginname, string pwd)
        {
            if (loginname.IsEmpty() || pwd.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            BLL.LoginBll bll = new BLL.LoginBll();

            return bll.Login(loginname, pwd);
        }

        /// <summary>
        /// 登录扩展
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <param name="UniqueId"></param>
        /// <returns></returns>
        public string LoginExt(string loginname, string pwd, string UniqueId)
        {

           
            if (loginname.IsEmpty() || pwd.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            BLL.LoginBll bll = new BLL.LoginBll();

            return bll.Login(loginname, pwd,UniqueId);
        }

        /// <summary>
        /// 得到userid
        /// </summary>
        /// <param name="UniqueId"></param>
        /// <returns></returns>
        public string GetUseridByUniqueId(string UniqueId)
        {
            return new BLL.LoginBll().Getuserid(UniqueId);
        }

        /// <summary>
        /// 模版房间列表
        /// </summary>
        /// <returns></returns>
        public string GetModelList(string roomtype, string order,string userroomid)
        {
            BLL.ModelRoom bll = new BLL.ModelRoom();


            return bll.GetModelRoomS2(roomtype, order,userroomid);
        }


        /// <summary>
        /// 得到主材明细
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public string GetZcMx(string did)
        {

            BLL.ZC bll = new BLL.ZC();

            return bll.GetZcMx(did);

        }


        /// <summary>
        /// 得到主材明细
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public string GetGyMx(string did)
        {

            BLL.GY bll = new BLL.GY();

            return bll.GetGyMx(did);


        }

        /// <summary>
        /// 得到推荐主材
        /// </summary>
        /// <returns></returns>
        public string TjZc(string productid)
        {
            if (productid.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"productid参数有空值\"}"; ;
            }
            return new BLL.ZC().RecommendZc(productid);
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="pra"></param>
        /// <returns></returns>
        public string Addroommodule(string parm)
        {
            if (parm.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            BLL.UserRoom bll = new BLL.UserRoom();

            return bll.AddRom(parm);
        }


        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="pra"></param>
        /// <returns></returns>
        public string Addroommodule2(string parm)
        {
            if (parm.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            BLL.UserRoom bll = new BLL.UserRoom();

            return bll.AddRomExt(parm);
        }


        /// <summary>
        /// 使用模版房间
        /// </summary>
        /// <param name="pra"></param>
        /// <returns></returns>
        public string SyFj(string parm)
        {
            //System.IO.File.AppendAllText(HttpContext.Server.MapPath("") + "sylog.txt", parm + "     :" + DateTime.Now.ToSafeString() + "\r\n\r\n");
            BLL.UserRoom bll = new BLL.UserRoom();
            return bll.SyFj(parm);



        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public string Regist(string pwd, string name, string gender, string phone)
        {
            if (pwd.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"pwd为空\"}"; ;
            }
            if (name.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"name为空\"}"; ;
            }
            if (gender.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"gender为空\"}"; ;
            }
            if (phone.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"phone为空\"}"; ;
            }

            BLL.LoginBll bll = new BLL.LoginBll();


            return bll.Regist(phone, pwd, name, gender);


        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendRegistMSg(string phone)
        {
            if (phone.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            BLL.SenMSg bll = new BLL.SenMSg();


            return bll.Send(phone);
        }


        /// <summary>
        /// 验证短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string validatecode(string phone, string code)
        {
            if (phone.IsEmpty() || code.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            object o = Commen.DataCache.GetCache(phone);
            if (o != null && o.ToSafeString() == code)
            {
                return "{\"success\":\"true\",\"msg\":\"验证码正确\",\"code\":" + code + ",\"code2\":\"" + o.ToSafeString() + "\"}";
            }
            else
            {
                return "{\"success\":\"false\",\"msg\":\"验证码错误\",\"code\":" + code + ",\"code2\":\"" + o.ToSafeString() + "\"}";
            }

        }


        /// <summary>
        /// 完善用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public string UpdateUser(string userid, string name, string gender, string address)
        {
            if (userid.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"userid不能为空\"}"; ;
            }
            BLL.LoginBll bll = new BLL.LoginBll();
            return bll.UpdateUser(userid, name, gender, address);

        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendReSetPwdMsg(string phone)
        {
            if (phone.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            BLL.SenMSg bll = new BLL.SenMSg();


            return bll.SendResetPwd(phone);
        }


        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        public string ReSetPwd(string phone, string code, string newpwd)
        {
            if (phone.IsEmpty() || code.IsEmpty() || newpwd.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }

            object o = Commen.DataCache.GetCache("pwd" + phone);

            if (o == null || o.ToSafeString() != code)
            {
                return "{\"success\":\"false\",\"msg\":\"验证码错误\"}"; ;
            }
            else
            {
                BLL.LoginBll bll = new BLL.LoginBll();
                return bll.ResetPWd(phone, newpwd);
            }

        }

        /// <summary>
        /// 得到用户房间模版列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserModule(string userid)
        {

            BLL.UserRoom bll = new BLL.UserRoom();

            return bll.GetUserModule(userid);

        }

        /// <summary>
        /// 单独删除用户的某一个模块
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="userromid"></param>
        /// <returns></returns>
        public string DeleteSingleModule(string userid, string userromid)
        {
            if (userid.IsEmpty() || userromid.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ; ;
            }
            BLL.UserRoom bll = new BLL.UserRoom();


            return bll.DeleteSingleModule(userid, userromid);
        }


        /// <summary>
        /// 详情合并 某个房间详情
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        public string GetModelDetail(string did)
        {
            if (string.IsNullOrEmpty(did))
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}";
            }
            BLL.UserRoom bll = new BLL.UserRoom();
            return bll.GetModelDetail(did); ;
        }

        /// <summary>
        /// 提交意见
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string AddComment(string userid, string content)
        {
            if (userid.IsEmpty() || content.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}";
            }

            BLL.LoginBll bll = new BLL.LoginBll();

            return bll.AddComment(userid, content);

        }

        /// <summary>
        /// 得到装修清单
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string DecorateList(string userid)
        {
            if (userid.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"userid不能为空\"}"; ;
            }
            return new BLL.UserRoom().DecorateList(userid);
        }

        /// <summary>
        /// 发送预约短信
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendOrderMsg(string phone)
        {
            if (!Regex.IsMatch(phone, "^\\d{11}$"))
            {

                return "{\"success\":\"false\",\"msg\":\"手机号格式错误\"}"; ;

            }
            if (phone.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            BLL.SenMSg bll = new BLL.SenMSg();


            return bll.SendOrderMsg(phone);
        }


        /// <summary>
        /// 预约量房
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="userid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string MakeAppointment(string name, string phone, string userid, string code)
        {
            if (!Regex.IsMatch(phone, "^\\d{11}$"))
            {

                return "{\"success\":\"false\",\"msg\":\"手机号格式错误\"}"; ;

            }

            if (name.IsEmpty() || phone.IsEmpty() || userid.IsEmpty() || code.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"参数有空值\"}"; ;
            }
            return new BLL.UserRoom().MakeAppointment(name, phone, userid, code);
        }


        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        public string UploadHeadImg(string userid)
        {
            if (userid.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"userid不能为空\"}"; ;
            }
            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength < 10)
            {
                return "{\"success\":\"false\",\"msg\":\"上传图片过小\"}"; ;
            }

            string fileext = Path.GetExtension(file.FileName);

            if (fileext.ToLower() != ".jpg" && fileext.ToLower() != ".png" && fileext.ToLower() != ".gif")
            {
                return "{\"success\":\"false\",\"msg\":\"请上传图片\"}"; ;
            }
            string temppath = "~/UpLoad/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

            string path = System.Web.HttpContext.Current.Request.MapPath(temppath);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string filename = Guid.NewGuid().ToString().Substring(0, 6) + userid + fileext;

            file.SaveAs(path + filename);


            string url = "http://mobile.mj100.com" + temppath.Replace("~", "") + filename;


            new BLL.LoginBll().UpdateImg(userid, url);

            //UpLoad/2015/4/9c7bIMG_0843.JPG


            return "{\"success\":\"true\",\"msg\":\"上传成功\",\"url\":\"http://mobile.mj100.com" + temppath.Replace("~", "") + filename + "\"}";
        }


        /// <summary>
        /// 删除方案
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string DeleteAll(string userid)
        {
            if (userid.IsEmpty())
            {
                return "{\"success\":\"false\",\"msg\":\"userid不能为空\"}"; ; ;
            }

            return new BLL.UserRoom().DeleteAll(userid);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="loginname"></param>
        /// <returns></returns>
        public string DelteUser(string loginname)
        {
            if (loginname.IsEmpty())
            {
                return "hehe";
            }
            new BLL.LoginBll().DeleteUser(loginname);
            return loginname + "已删除";
        }

    }
}
