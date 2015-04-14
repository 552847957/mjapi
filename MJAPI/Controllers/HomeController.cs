using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MJAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "欢迎使用 ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.json = "{\"name\":\"dapeng\"}";
            return View();
        }


        public JsonResult Test(string key)
        {
            List<Person> lis = new List<Person>();
            Person p = new Person();
            p.name = "2222";
            p.age = "3333";
            lis.Add(p);
            lis.Add(p);

            lis.Select(u => {return
                new { name=u.name}; });
            var liss = from u in lis
                       select new {u.name }
                       ;

            return Json(liss, JsonRequestBehavior.AllowGet);
        }
    }

    public class Person
    {
        public string name;

        public string age;
    }
}
