using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebISB.Models;
using PagedList.Mvc;
using PagedList;
using System.Web.Routing;

namespace WebISB.Controllers
{
    [HandleError]
    public class TrangChuController : Controller
    {
        Tintuc_Models modtt = new Tintuc_Models();
        Menu_Models mod = new Menu_Models();
        SlidePage_Models mod_sl = new SlidePage_Models();
        Donvi_Models mod_dv = new Donvi_Models();
        Banhanhvb_Models mod_bh = new Banhanhvb_Models();
        Loaivanban_Models mod_lvb = new Loaivanban_Models();
        private ISBEntities db = new ISBEntities();

        public class AjaxAuthorizeAttribute : AuthorizeAttribute
        {
            private class Http401Result : ActionResult
            {
                public override void ExecuteResult(ControllerContext context)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.HttpContext.Response.Write("Your session has expired. Please login again to continue.");
                    context.HttpContext.Response.End();
                }
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new Http401Result();
                }
                else
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }
            }

        }


        public class SessionCheck : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                if (session != null && session["user_id"].Equals(""))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { Action ="Login",Controller = "Auth", Area = "Admin" }));
                }
            }
        }
        // GET: TrangChu
        //[SessionCheck]
        public ActionResult Index()
        {
            ViewBag.donvi = new SelectList(mod_dv.GetDonVi(), "MS_DV", "TenDonVi");
            ViewBag.loaivanban = new SelectList(mod_lvb.Get_Loaivanban_ddl(), "id", "TenLoaiVB");
            return View();
        }
        [SessionCheck]
        public ActionResult VanbanNB()
        {

            ViewBag.donvi = new SelectList(mod_dv.GetDonVi(), "MS_DV", "TenDonVi");
            ViewBag.loaivanban = new SelectList(db.loaivanbans, "id", "TenLoaiVB");
            return View();
        }
        //[SessionCheck]
        public ActionResult search(string sovb, string noiphathanh, string loaivanban, string trichyeu)
        {
                List<FN_Get_Banhanhvb_Tim_Result> result = mod_bh.Get_vanbanden_find(sovb,noiphathanh,loaivanban,trichyeu);
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);   
        }
        //[SessionCheck]
        public ActionResult searchvbdi(string sovb, string noiphathanh, string loaivanban, string trichyeu)
        {
            string dv = Session["donvi"].ToString();
            List<FN_Get_Banhanhvb_Tim_vbdi_Result> result = mod_bh.Get_vanbandi_find(dv,sovb, noiphathanh, loaivanban, trichyeu);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        //[SessionCheck]
        public ActionResult GetData()
        {
            //List<FN_Get_Banhanhvb_Result> empList = mod_bh.GetBanhanhvb();
            List<Get_Banhanhvb_vb_Result> empList = mod_bh.GetBanhanhvb_vb();
            return Json(new {data = empList }, JsonRequestBehavior.AllowGet);
        }
        [SessionCheck]
        public ActionResult GetDataNB_vbden()
        {
            string dv = Session["donvi"].ToString();
            List<FN_Get_Noibo_vbden_Result> List = mod_bh.Get_vanbannbden(dv);
            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }
        [SessionCheck]
        public ActionResult searchvbnbden(string sovb, string noiphathanh, string loaivanban, string trichyeu)
        {
            string dv = Session["donvi"].ToString();
            List<Get_Banhanhvb_NB_Den_Result> result = mod_bh.Get_vanbannb_den_find(sovb, noiphathanh, loaivanban, trichyeu,dv);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        [SessionCheck]
        public ActionResult searchvbnbdi(string sovb, string loaivanban, string trichyeu, string donvinhan)
        {
            string dvgui = Session["donvi"].ToString();
            List<Get_vanbandi_NoiBo_search_Result> result = mod_bh.Get_vanbandi_noibo_search(sovb, loaivanban, trichyeu, dvgui, donvinhan);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        [SessionCheck]
        //public ActionResult Index(int page = 1,int pagesize = 2)
        //{
        //    //return View();
        //    List<FN_Get_TinTuc_TC_Result> result = modtt.GetTinTuc_TC().ToList();
        //    PagedList<FN_Get_TinTuc_TC_Result> model = new PagedList<FN_Get_TinTuc_TC_Result>(result, page, pagesize);
        //    return View(model);
        //}
        public ActionResult SideMenu()
        {
            return PartialView("_MenuPage");
        }
        //[SessionCheck]
        public PartialViewResult vbd()
        {
            ViewBag.donvi = new SelectList(mod_dv.GetDonVi(), "MS_DV", "TenDonVi");
            ViewBag.loaivanban = new SelectList(db.loaivanbans, "id", "TenLoaiVB");

            return PartialView("_Vanbandi");
        }
        [SessionCheck]
        public PartialViewResult vbnbdi()
        {
            ViewBag.donvi = new SelectList(mod_dv.GetDonVi(), "MS_DV", "TenDonVi");
            ViewBag.loaivanban = new SelectList(db.loaivanbans, "id", "TenLoaiVB");

            return PartialView("_Vanbannbdi");
        }
        //[SessionCheck]
        public ActionResult GetData_vbd()
        {
            string dv = Session["donvi"].ToString();
            List<FN_Get_vanbandi_Result> List = mod_bh.Get_vanbandi(dv);
 
            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }
        [SessionCheck]
        public ActionResult GetDataNB_vbd()
        {
            string dv = Session["donvi"].ToString();
            List<Get_vanbandi_NoiBo_Result> List = mod_bh.Get_vanbandi_noibo(dv);

            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SlidePage()
        {
            return PartialView("_SlidePage");
        }
        public PartialViewResult LoadTL(string id)
        {
            List<F_Get_Side_Menu_Result> result = mod.GetMenu(id);
            ViewBag.Count = result.Count();
            ViewBag.Loai = "TL";
            return PartialView("Loadchilde",result);
        }
        public PartialViewResult LoaiTin(string id)
        {
            List<F_Get_Side_Menu_Result> result = mod.GetMenu(id);
            ViewBag.Count = result.Count();
            ViewBag.Loai = "LT";
            return PartialView("Loadchilde", result);
        }
        public PartialViewResult LoadSlidePage()
        {
            List<slide> result = mod_sl.GetSlidePage();
            ViewBag.Count = result.Count();
            return PartialView("SlidePage", result);
        }
        public PartialViewResult Loadcarousel()
        {
            List<slide> result = mod_sl.GetSlidePage();
            ViewBag.Count = result.Count();
            return PartialView("Carousel", result);
        }
    }
}