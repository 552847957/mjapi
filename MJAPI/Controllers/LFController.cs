using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LFBLL;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

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
            string userid = Request.Cookies["userid"].Value;
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

            Regex re = new Regex(@"(\d)_");

            if (Request.Cookies["userid"] == null)
            {
                return RedirectToAction("Index");
            }
            string userid = Request.Cookies["userid"].Value;
            System.Data.DataTable dt = new LFBLL.LFlogin().Getsingle(userid, id);
            if (dt == null || dt.Rows.Count < 1)
            {
                return RedirectToAction("Measure");
            }
            else
            {
                ViewData["single"] = dt.Rows[0];

                #region 遍历图片
                Dictionary<string, string> dic = new Dictionary<string, string>();

                string picarrstr = dt.Rows[0]["pics"].ToSafeString();
                if (picarrstr.Length != 0)
                {
                    string[] arr = picarrstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var pic = arr[i];
                        Match m = re.Match(pic);
                        if (m.Success)
                        {
                            if (dic.ContainsKey(m.Groups[1].Value))
                            {
                                string lis = dic[m.Groups[1].Value];
                                lis = lis + "," + arr[i];
                                dic[m.Groups[1].Value] = lis;
                            }
                            else
                            {
                                dic.Add(m.Groups[1].Value, arr[i]);
                            }
                        }

                        //http://gy.mj100.com/amount/UploadDps/1429769258637029/1429769343243797/瀹炴櫙2_down.jpg   
                    }
                }
                #endregion

                ViewBag.dic = dic;
                //ViewBag.single = dt.Rows[0];
            }



            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string Login(string loginname, string pwd)
        {
            return new LFBLL.LFlogin().Login(loginname, pwd);
        }


        public string SaveItem(string CustomerName, string phone, string address, string demanddesc, string id)
        {
            return new LFBLL.LFlogin().SaveItem(CustomerName, phone, address, demanddesc, id);
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
            return new LFBLL.LFlogin().DelteProject(id, userid).ToSafeString();

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



        public FileResult GetFile(string url)
        {
           
           WebClient web=new WebClient();

            var stream=web.OpenRead(url);


            return File(stream,"image/",Path.GetFileName(url)); 
        
        }
    }
}
