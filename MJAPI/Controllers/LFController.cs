using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LFBLL;

namespace MJAPI.Controllers
{
    public class LFController : Controller
    {
        //
        // GET: /LF/

        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Measure()
        {
            if (Request.Cookies["userid"] == null)
            {
                return RedirectToAction("Index");
            }

            //delCookie("loginname");
            //delCookie("userid");
            //delCookie("headimg");

           
           
            string userid = Request.Cookies["userid"].Value;

          //  ViewBag.listdemand = new LFBLL.LFlogin().GetDemandlist(userid);
            ViewBag.listdemand = new LFBLL.LFlogin().GetDemandlist(userid); ;

            return View();
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {

            if (Request.Cookies["userid"] == null)
            {
                return RedirectToAction("Index");
            }
            string userid = Request.Cookies["userid"].Value;
            System.Data.DataTable dt = new LFBLL.LFlogin().Getsingle(userid,id);
            if (dt==null||dt.Rows.Count<1)
            {
                return RedirectToAction("Measure");
            }
            else
            {
                ViewData["single"] = dt.Rows[0];
                //ViewBag.single = dt.Rows[0];
            }

           

            return View();
        }


        public string Login(string loginname, string pwd)
        {
            return new LFBLL.LFlogin().Login(loginname,pwd);
        }


        public string SaveItem(string CustomerName, string phone, string address, string demanddesc,string id)
        {
            return  new LFBLL.LFlogin().SaveItem(CustomerName,phone,address,demanddesc,id);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string DelteProject(string id)
        {
           
            string userid = Request.Cookies["userid"].Value;
            return new LFBLL.LFlogin().DelteProject(id,userid).ToSafeString();
            
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public string LoginOut()
        {
            HttpCookie loginname = new HttpCookie("loginname");
            loginname.Expires = DateTime.Now.AddDays(-2);
            Response.AppendCookie(loginname);

            HttpCookie useridc = new HttpCookie("userid");
            useridc.Expires = DateTime.Now.AddDays(-2);
            Response.AppendCookie(useridc);


            HttpCookie headimg = new HttpCookie("headimg");
            headimg.Expires = DateTime.Now.AddDays(-2);
            Response.AppendCookie(headimg);

            return "";
        }
    }
}
