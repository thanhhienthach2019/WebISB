using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebISB.Models;

namespace WebISB.Controllers
{
    public class loaitailieuController : Controller
    {
        private ISBEntities db = new ISBEntities();
        Loaitailieu_Models mod = new Loaitailieu_Models();
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

        // GET: loaitailieu
        public ActionResult Index()
        {
            List<Get_LoaiTaiLieu_Result> result = mod.GetLoaiTaiLieu();
            //PagedList<FN_Get_Tailieu_Result> model = new PagedList<FN_Get_Tailieu_Result>(result, page, 2);
            return View(result);
        }
        //Get: loai tai lieu - phong ban
        public ActionResult donvi(string characters)
        {
            string charac = "";
            charac = characters;
            if (characters == "BM")
            {
                characters = "Biểu mẫu";
            }
            else if(characters == "QD")
            {
                characters = "Quy định";
            }
            else if(characters == "QC")
            {
                characters = "Quy chế";
            }
            else
            {
                characters = "";
            }
            
            List<Get_Tailieu_PhongBan_Result> result = mod.Get_Tailieu_Phongban(charac);
            ViewBag.characters = characters;
            ViewBag.count = result.Count();

            return View(result);
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
