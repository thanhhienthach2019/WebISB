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
    public class loaitailieuAdController : Controller
    {
        private ISBEntities db = new ISBEntities();
        private Loaitailieu_Models mod = new Loaitailieu_Models();
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
        // GET: Admin/loaitailieu
        [SessionCheck]
        public ActionResult Index()
        {
            List<Get_LoaiTaiLieu_Result> result = mod.GetLoaiTaiLieu();
            return View(result);
        }

        // GET: Admin/loaitailieu/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaitailieu loaitailieu = db.loaitailieux.Find(id);
            if (loaitailieu == null)
            {
                return HttpNotFound();
            }
            return View(loaitailieu);
        }

        // GET: Admin/loaitailieu/Create
        [SessionCheck]
        public ActionResult Insert()
        {
            var model = new loaitailieu();
            model.Create_at = DateTime.Now;
            model.User_create = Session["user_fullname"].ToString();
            return View(model);
        }

        // POST: Admin/loaitailieu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [SessionCheck]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(loaitailieu loaitailieu)
        {
            if (ModelState.IsValid)
            {
                if (loaitailieu.MaLoai != null)
                {
                    loaitailieu.Create_at = DateTime.Now;
                    loaitailieu.User_create = Session["user_id"].ToString();
                    try
                    {
                        db.loaitailieux.Add(loaitailieu);
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

            return View(loaitailieu);
        }
        [SessionCheck]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Del(string id)
        {
            bool result = false;
            var u = db.loaitailieux.Where(x => x.MaLoai == id).FirstOrDefault();
            if (u != null)
            {
                db.loaitailieux.Remove(u);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/loaitailieu/Edit/5
        [SessionCheck]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaitailieu loaitailieu = db.loaitailieux.Find(id);
            if (loaitailieu == null)
            {
                return HttpNotFound();
            }
            return View(loaitailieu);
        }

        // POST: Admin/loaitailieu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(loaitailieu loaitailieu)
        {
            if (ModelState.IsValid)
            {
                if (loaitailieu.MaLoai != null)
                {
                    try
                    {
                        db.Entry(loaitailieu).State = EntityState.Modified;
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
            return View(loaitailieu);
        }

        // GET: Admin/loaitailieu/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaitailieu loaitailieu = db.loaitailieux.Find(id);
            if (loaitailieu == null)
            {
                return HttpNotFound();
            }
            return View(loaitailieu);
        }

        // POST: Admin/loaitailieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            loaitailieu loaitailieu = db.loaitailieux.Find(id);
            db.loaitailieux.Remove(loaitailieu);
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
