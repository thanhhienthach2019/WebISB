using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using WebISB.Models;

namespace WebISB.Controllers
{
    [HandleError]
    public class tailieutController : Controller
    {
        private ISBEntities db = new ISBEntities();
        Tailieu_Models mod = new Tailieu_Models();
        Donvi_Models mod_dv = new Donvi_Models();
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
        //[SessionCheck]
        // GET: tailieu
        public ActionResult Department(string characters,string unit)
        {        
            List<FN_Get_Tailieu_Result> result = mod.GetTaiLieu(characters, unit);
            var firstUnit = mod.GetTaiLieu(characters, unit).FirstOrDefault(c => c.MS_DV == unit);
            
            //PagedList<FN_Get_Tailieu_Result> model = new PagedList<FN_Get_Tailieu_Result>(result, page, 2);
            ViewBag.nameUnit = firstUnit.VietTat;
            ViewBag.nameCharac = firstUnit.TenLoai;
         
            ViewBag.count = result.Count();
            return View(result);
        }
        //[SessionCheck]
        public ActionResult DownloadFile(string file = "")
        {
            file = HostingEnvironment.MapPath(file);
            if (System.IO.File.Exists(file))
            {
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = Path.GetFileName(file);
                return File(file, contentType, fileName);
            }else
            {
                ViewData["Message"] = "Your application description page.";
                return View();
            }
        }
    }
}
