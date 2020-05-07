using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebISB.Models;
using PagedList.Mvc;
using PagedList;
using System.Web.Routing;

namespace WebISB.Controllers
{
    [HandleError]
    public class theloaitController : Controller
    {
        private ISBEntities db = new ISBEntities();
        Tintuc_Models mod = new Tintuc_Models();
        public class SessionCheck : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                if (session != null && session["user_id"].Equals(""))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { Action = "Login", Controller = "Auth", Area = "Admin" }));
                }
            }
        }
        // GET: TinTucTC
        //[SessionCheck]
        public ActionResult check(string key,string loai,int? id, int page = 1)
        {/*, int page = 1*/
            if(loai == "the-loai")
            {
                loai = "TL";
            }
            List<FN_Get_TinTuc_Loai_TC_Result> result = mod.GetTinTucLoai_TC(loai,id).ToList();
            PagedList<FN_Get_TinTuc_Loai_TC_Result> model = new PagedList<FN_Get_TinTuc_Loai_TC_Result>(result, page, 2);
            ViewBag.loai = loai;
            ViewBag.id = id;
            ViewBag.check = "";
            if(key == "tu-lieu")
            {
                key = "Tư liệu";
                ViewBag.check = "tu-lieu";
            }
            else
            if(key == "thong-tin")
            {
                key = "Thông tin";
                ViewBag.check = "thong-tin";
            }
            else
            if(key == "bao-cao")
            {
                key = "Báo cáo";
                ViewBag.check = "bao-cao";
            }
            else
            {
                key = "";
            }
            ViewBag.key = key;
            ViewBag.count = result.Count();
            return View(model);
        }

        
    }
}
