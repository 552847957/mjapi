using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace MJAPI.Controllers
{
    public class ApiController : Controller
    {
        /// <summary>
        /// api测试
        /// </summary>
        /// <returns></returns>
        public string Test()
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
        /// 模版房间列表
        /// </summary>
        /// <returns></returns>
        public string GetModelList(string roomtype, string order)
        {
            BLL.ModelRoom bll = new BLL.ModelRoom();


            return bll.GetModelRoomS(roomtype, order);
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
        /// 使用模版房间
        /// </summary>
        /// <param name="pra"></param>
        /// <returns></returns>
        public string SyFj(string parm)
        {
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
    }
}
