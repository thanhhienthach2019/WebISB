using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using WebISB.Models;

namespace WebISB.Controllers
{
    [HandleError]//Error 404
    public class ChiTietController : Controller
    {
        private ISBEntities db = new ISBEntities();
        Banhanhvb_Models mod = new Banhanhvb_Models();
        Tintuc_Models mod_tt = new Tintuc_Models();
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
        public ActionResult Search(string td,string id)
        {
            if(td=="chi-tiet-van-ban-cong-ty")
            {
                td = "Chi tiết văn bản công ty";
            }else if(td== "chi-tiet-van-ban-noi-bo")
            {
                td = "Chi tiết văn bản nội bộ";
            }else
            {
                td = "Chi tiết văn bản";
            }
            ViewBag.tieude = td;
            ViewBag.id = id;
            //List<Get_Chitietvb_Result> result = mod.Get_chitietvb(id);
            List<Get_Chitietvb_vb_Result> result = mod.Get_chitietvb_vb(id);
            return View(result);
        }
        //[SessionCheck]
        public ActionResult Detail(string key, int id)
        {
            if(key=="chi-tiet-thong-tin")
            {
                ViewBag.key = "thong-tin";
            }else if(key=="chi-tiet-bao-cao")
            {
                ViewBag.key = "bao-cao";
            }
            List<FN_Get_ChiTietTin_Result> result = mod_tt.Get_Chitiettin(id);
            return View(result);
        }
        //[SessionCheck]
        public PartialViewResult Lienquan(int idLT,string key)
        {
            List<FN_Get_ChiTietTin_LT_Result> result = mod_tt.Get_Chitiettin_LT(idLT);
            ViewBag.Count = result.Count();
            ViewBag.key = key;
            return PartialView("TinLQ", result);
        }
        //[SessionCheck]
        public PartialViewResult Noibat(string key)
        {
            List<FN_Get_ChiTietTin_Noibat_Result> result = mod_tt.Get_Chitiettin_Noibat();
            ViewBag.Count = result.Count();
            ViewBag.key = key;
            return PartialView("Noibat", result);
        }
        //[SessionCheck]
        public ActionResult DownloadFile(string file = "")
        {
            file = HostingEnvironment.MapPath(file);

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = Path.GetFileName(file);
            return File(file, contentType, fileName);

        }
    }
}