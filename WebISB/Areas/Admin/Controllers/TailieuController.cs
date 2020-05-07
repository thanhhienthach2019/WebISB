using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using WebISB.Library;
using WebISB.Models;

namespace WebISB.Areas.Admin.Controllers
{
    public class TailieuController : Controller
    {
        private ISBEntities db = new ISBEntities();
        private Mystring mystring = new Mystring();

        Tailieu_Models mod = new Tailieu_Models();
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
        //check request token
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public class ValidateJsonAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
                if (filterContext == null)
                {
                    throw new ArgumentNullException("filterContext");
                }

                var httpContext = filterContext.HttpContext;
                var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
                AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
            }
        }
        // GET: Admin/Tailieu
        [SessionCheck]
        public ActionResult Index()
        {
            string ms_dv;
            ms_dv = Session["donvi"].ToString();
            List<Get_tailieu_admin_Result> result = mod.GetTaiLieuAdmin(ms_dv);
            return View(result);
       
        }
        [SessionCheck]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]//check token
        public JsonResult Deltrash(int? id)
        {
                bool result = false;
                try
                {
                    tailieu tailieu = db.tailieux.Find(id);
                    tailieu.status = 0;
                    db.Entry(tailieu).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                catch
                {
                    result = false;
                }              
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/Tailieu/Edit/5
        [SessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

                tailieu tailieu = db.tailieux.Find(id);
                tailieu.create_up = DateTime.Now;
                ViewBag.MaLoai = new SelectList(db.loaitailieux, "MaLoai", "TenLoai", tailieu.MaLoai);
                ViewBag.MS_DV = new SelectList(db.DonVis, "MS_DV", "TenDonVi", tailieu.MS_DV);
                //result = true;
                if (tailieu == null)
                {
                    return HttpNotFound();
                }
            return View(tailieu);
        }
        [SessionCheck]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Retrash(int? id)
        {
                bool result = false;
                tailieu tailieu = db.tailieux.Find(id);
                tailieu.status = 1;
                
                try
                {
                    db.Entry(tailieu).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;

                }
                catch
                {
                    result = false;
                }
            return Json(result, JsonRequestBehavior.AllowGet);
            //return View(tailieu);

        }
        // GET: Admin/Tailieu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tailieu tailieu = db.tailieux.Find(id);
            if (tailieu == null)
            {
                return HttpNotFound();
            }
            return View(tailieu);
        }
        // GET: Admin/Tailieu/Create
        [SessionCheck]
        public ActionResult Insert()
        {
            var model = new tailieu();
            model.create_at =DateTime.Now;

            ViewBag.MaLoai = new SelectList(db.loaitailieux, "MaLoai", "TenLoai");
            ViewBag.MS_DV = new SelectList(db.DonVis, "MS_DV", "TenDonVi");
            return View(model);
        }

        // POST: Admin/Tailieu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        //[ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Insert(tailieu tailieu)
        {
                if (ModelState.IsValid)
                {
                    if (tailieu.MaSo != null)
                    {
                        ViewBag.tam = 1;
                        tailieu.create_at = DateTime.Now;
                        tailieu.create_up = null;
                        tailieu.status = 1;
                        tailieu.user_create = Session["user_id"].ToString();
                        //upload file
                        var f = Request.Files["img[]"];
                        if (f != null && f.ContentLength > 0)
                        {
                            tailieu.TenFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + f.FileName.Substring(f.FileName.LastIndexOf("."));
                            string pathfiles = Path.Combine(Server.MapPath("~/File/"), tailieu.TenFile);
                            tailieu.FilePath = "~/File/"+ tailieu.TenFile;
                            f.SaveAs(pathfiles);
                        }
                        else
                        {
                            tailieu.FilePath = "";
                        }

                        db.tailieux.Add(tailieu);
                        db.SaveChanges();
                        //ViewBag.Message = System.Text.Encoding.UTF8.GetBytes("Thêm dữ liệu thành công!");
                        ViewBag.Message = "Add data successfully!";
                        //return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.tam = 2;
                        ViewBag.Message = "The input data is empty!";
                    }

                }
            ViewBag.MaLoai = new SelectList(db.loaitailieux, "MaLoai", "TenLoai");
            ViewBag.MS_DV = new SelectList(db.DonVis, "MS_DV", "TenDonVi");
            return View(tailieu);
        }

       

        // POST: Admin/Tailieu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tailieu tailieu)
        {
                if (ModelState.IsValid)
                {
                    tailieu.create_up = DateTime.Now;
                    tailieu.status = 1;
                    //upload file
                    var f = Request.Files["img[]"];
                    if (f != null && f.ContentLength > 0)
                    {
                        tailieu.TenFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + f.FileName.Substring(f.FileName.LastIndexOf("."));
                        string pathfiles = Path.Combine(Server.MapPath("~/File/"), tailieu.TenFile);
                        tailieu.FilePath = "~/File/" + tailieu.TenFile;
                        f.SaveAs(pathfiles);
                    }
                    

                    db.Entry(tailieu).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Update data successfully!";
                    ViewBag.tam = 1;
                    //return RedirectToAction("Index");
                }
            ViewBag.MaLoai = new SelectList(db.loaitailieux, "MaLoai", "TenLoai", tailieu.MaLoai);
            ViewBag.MS_DV = new SelectList(db.DonVis, "MS_DV", "TenDonVi", tailieu.MS_DV);

            return View(tailieu);
        }
        [SessionCheck]
        public ActionResult Trash()
        {
            string ms_dv;
            ms_dv = Session["donvi"].ToString();
            List<FN_Get_Tailieu_Trash_Result> results = mod.GetTaiLieuTrash(ms_dv);
            return View(results);
        }

        // GET: Admin/Tailieu/Delete/5
        [SessionCheck]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Delete(int? id)
        {
            bool result = false;
            try
            {
                tailieu tailieu = db.tailieux.Find(id);
                db.tailieux.Remove(tailieu);
                db.SaveChanges();
                result = true;
            }
            catch
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
    
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
