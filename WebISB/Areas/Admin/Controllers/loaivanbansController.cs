using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using WebISB.Models;

namespace WebISB.Areas.Admin.Controllers
{
    public class loaivanbansController : Controller
    {
        private ISBEntities db = new ISBEntities();
        private Loaivanban_Models mod = new Loaivanban_Models();
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
        // GET: Admin/loaivanbans
        [SessionCheck]
        public ActionResult Index()
        {
            List<Get_loaiVanban_Result> result = mod.Get_Loaivanban();
            return View(result);
        }

        // GET: Admin/loaivanbans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaivanban loaivanban = db.loaivanbans.Find(id);
            if (loaivanban == null)
            {
                return HttpNotFound();
            }
            return View(loaivanban);
        }

        // GET: Admin/loaivanbans/Insert
        [SessionCheck]
        public ActionResult Insert()
        {
            var model = new loaivanban();
            model.DateCreate = DateTime.Now;
            model.UserCreate = Session["user_id"].ToString();
            return View(model);
        }

        // POST: Admin/loaivanbans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [SessionCheck]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(loaivanban loaivanban)
        {
            if (ModelState.IsValid)
            {
                if (loaivanban.TenLoaiVB != null)
                {
                    loaivanban.DateCreate = DateTime.Now;
                    loaivanban.DateUpdate = null;                  
                    try
                    {
                        db.loaivanbans.Add(loaivanban);
                        db.SaveChanges();
                        ViewBag.tam = 1;
                        ViewBag.Message = "Add data successfully!";
                    }
                    catch
                    {
                        ViewBag.tam = 2;
                        ViewBag.Message = "Add data fail!";
                    }
                    
                }
                else
                {
                    ViewBag.tam = 2;
                    ViewBag.Message = "The input data is empty!";
                }
            }

            return View(loaivanban);
        }

        // GET: Admin/loaivanbans/Edit/5
        [SessionCheck]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaivanban loaivanban = db.loaivanbans.Find(id);
            loaivanban.DateUpdate = DateTime.Now;
            if (loaivanban == null)
            {
                return HttpNotFound();
            }
            return View(loaivanban);
        }

        // POST: Admin/loaivanbans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(loaivanban loaivanban)
        {
            if (ModelState.IsValid)
            {
                if(loaivanban.TenLoaiVB != null)
                {
                    loaivanban.DateUpdate = DateTime.Now;
                    try
                    {
                        db.Entry(loaivanban).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.tam = 1;
                        ViewBag.Message = "Update data success!";
                    }
                    catch
                    {
                        ViewBag.tam = 2;
                        ViewBag.Message = "Update data failed!";
                    }
                }
                else
                {
                    ViewBag.tam = 2;
                    ViewBag.Message = "The input data is empty!";
                }
                
            }
            return View(loaivanban);
        }
        [SessionCheck]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Del(int? id)
        {
            bool result = false;
            var u = db.loaivanbans.Where(x => x.id == id).FirstOrDefault();
            if (u != null)
            {
                db.loaivanbans.Remove(u);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [SessionCheck]
        // GET: Admin/loaivanbans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaivanban loaivanban = db.loaivanbans.Find(id);
            if (loaivanban == null)
            {
                return HttpNotFound();
            }
            try
            {
                db.loaivanbans.Remove(loaivanban);
                db.SaveChanges();
                ViewBag.tmp = 1;
                ViewBag.Message= "Delete data Success!";
            }
            catch
            {
                ViewBag.tmp = 2;
                ViewBag.Message = "Delete data Fail!";
            }
            
            return View();
        }

        // POST: Admin/loaivanbans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            loaivanban loaivanban = db.loaivanbans.Find(id);
            db.loaivanbans.Remove(loaivanban);
            db.SaveChanges();
            return RedirectToAction("Index");
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
