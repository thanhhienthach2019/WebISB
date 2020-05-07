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
using WebISB.Models;

namespace WebISB.Areas.Admin.Controllers
{
    public class vanbanController : Controller
    {
        private ISBEntities db = new ISBEntities();
        Banhanhvb_Models mod_bh = new Banhanhvb_Models();

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
        [SessionCheck]
        // GET: Admin/vanban
        public ActionResult Index()
        {
            string dv = Session["donvi"].ToString();
            List<Get_vanban_dashboard_Result> result = mod_bh.Get_vanban_dashboard(dv);
            return View(result);
        }

        // GET: Admin/vanban/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vanban vanban = db.vanbans.Find(id);
            if (vanban == null)
            {
                return HttpNotFound();
            }
            return View(vanban);
        }

        // GET: Admin/vanban/Insert
        [SessionCheck]
        public ActionResult Insert()
        {
            var model = new vanban();
            ViewBag.Idloaivb = new SelectList(db.loaivanbans, "id", "TenLoaiVB");       
            string user = Session["user_fullname"].ToString();
            model.Ngaydang = DateTime.Now;
            model.Nguoidang = user;
            
            return View(model);
        }

        // POST: Admin/vanban/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Insert(vanban vanban)
        {
            ViewBag.Idloaivb = new SelectList(db.loaivanbans, "id", "TenLoaiVB");        
            if (ModelState.IsValid)
            {
                if (vanban.Sovb != null)
                {
                    ViewBag.tam = 1;
                    vanban.Ngaydang =DateTime.Now;
                    vanban.Nguoidang = Session["user_id"].ToString();
                    vanban.Donvigui = Session["donvi"].ToString();
                    vanban.Noidung = "";
                    vanban.Id = Guid.NewGuid();
                    //upload file
                    var f = Request.Files["img[]"];
                    if (f != null && f.ContentLength > 0)
                    {
                        //string[] tam = f.FileName.Split('.');
                        //string filename = tam[0].Trim();

                        //string TenFile =  "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + f.FileName.Substring(f.FileName.LastIndexOf("."));
                        string TenFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + f.FileName;
                        string pathfiles = Path.Combine(Server.MapPath("~/File/"), TenFile);
                        vanban.TenFile = TenFile;
                        vanban.Url = "~/File/" + TenFile;
                        f.SaveAs(pathfiles);
                    }
                    else
                    {
                        vanban.Url = "";
                    }
                    db.vanbans.Add(vanban);
                    db.SaveChanges();
                    //ViewBag.Message = System.Text.Encoding.UTF8.GetBytes("Thêm dữ liệu thành công!");
                    ViewBag.Message = "Add data successfully!";
                }
                else
                {
                    ViewBag.tam = 2;
                    ViewBag.Message = "The input data is empty!";
                }
            }

            ViewBag.Idloaivb = new SelectList(db.loaivanbans, "id", "TenLoaiVB", vanban.Idloaivb);
            return View(vanban);
        }

        // GET: Admin/vanban/Edit/5
        [SessionCheck]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vanban vanban = db.vanbans.Find(id);
            if (vanban == null)
            {
                return HttpNotFound();
            }
            ViewBag.Nguoidang = new SelectList(db.users, "id", "name", vanban.Nguoidang);
            ViewBag.Idloaivb = new SelectList(db.loaivanbans, "id", "TenLoaiVB", vanban.Idloaivb);
            vanban.Ngaydang = DateTime.Now;

            return View(vanban);
        }

        // POST: Admin/vanban/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vanban vanban)
        {
            ViewBag.Idloaivb = new SelectList(db.loaivanbans, "id", "TenLoaiVB");
            ViewBag.Nguoidang = new SelectList(db.users, "id", "name");
            if (ModelState.IsValid)
            {
                //upload file
                var f = Request.Files["img[]"];
                if (f != null && f.ContentLength > 0)
                {
                    string TenFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + f.FileName;
                    string pathfiles = Path.Combine(Server.MapPath("~/File/"), TenFile);
                    vanban.TenFile = TenFile;
                    vanban.Url = "~/File/" + TenFile;
                    f.SaveAs(pathfiles);
                }

                db.Entry(vanban).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.tam = 1;
                ViewBag.Message = "Update data successfully!";
            }
            ViewBag.Idloaivb = new SelectList(db.loaivanbans, "id", "TenLoaiVB", vanban.Idloaivb);
            ViewBag.Nguoidang = new SelectList(db.users, "id", "name", vanban.Nguoidang);
            return View(vanban);
        }

        // GET: Admin/vanban/Delete/5
        [SessionCheck]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Delete(Guid id)
        {
            int del;
            bool result = false;
            try
            {
                del = mod_bh.sp_delete_vanban(id);
                vanban vanban = db.vanbans.Find(id);
                db.vanbans.Remove(vanban);
                db.SaveChanges();
                result = true;             
            }
            catch
            {
                result = true;
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
