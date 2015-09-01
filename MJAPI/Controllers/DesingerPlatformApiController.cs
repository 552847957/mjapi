using System;
using System.Collections.Generic;
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

    }
}
