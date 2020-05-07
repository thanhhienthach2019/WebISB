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
    public class PhanQuyenController : Controller
    {
        private ISBEntities db = new ISBEntities();
        private User_Models mod = new User_Models();
        private Quantri_Models mod_qt = new Quantri_Models();
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
        // GET: Admin/PhanQuyen
        [SessionCheck]
        public ActionResult Index()
        {
            List<Get_PhanQuyen_Result> result = mod_qt.GetPhanQuyen();
            return View(result);
        }

        // GET: Admin/PhanQuyen/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanQuyen phanQuyen = db.PhanQuyens.Find(id);
            if (phanQuyen == null)
            {
                return HttpNotFound();
            }
            return View(phanQuyen);
        }

        // GET: Admin/PhanQuyen/Create
        [SessionCheck]
        [ValidateInput(false)]
        public ActionResult Insert()
        {
            var model = new PhanQuyen();
            var user =
                db.users
                .Select(s => new
                {
                    id = s.id,
                    username = s.id + " - " + s.name
                }).ToList();
            ViewBag.id = new SelectList(user, "id", "username");
            return View();
        }

        // POST: Admin/PhanQuyen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(PhanQuyen phanQuyen)
        {
            string Str_id = Request.Form["id"].ToString();
            if (ModelState.IsValid)
            {
                if (phanQuyen.id != null)
                {
                    if (phanQuyen.Quyen == false)//user
                    {
                        phanQuyen.mVanban = true;
                        phanQuyen.mTheloaiTin = false;
                        phanQuyen.mLoaiTin = false;
                        phanQuyen.mTinTuc = false;
                        phanQuyen.mTaiLieu = true;
                        phanQuyen.mGioiThieu = false;
                        phanQuyen.mLienHe = false;
                        phanQuyen.mLoaivanban = true;
                        phanQuyen.mQuantri = false;

                    }
                    else
                    {
                        phanQuyen.mVanban = true;
                        phanQuyen.mTheloaiTin = true;
                        phanQuyen.mLoaiTin = true;
                        phanQuyen.mTinTuc = true;
                        phanQuyen.mTaiLieu = true;
                        phanQuyen.mGioiThieu = true;
                        phanQuyen.mLienHe = true;
                        phanQuyen.mLoaivanban = true;
                        phanQuyen.mQuantri = true;
                    }
                    try
                    {
                        db.PhanQuyens.Add(phanQuyen);
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
            var user =
                db.users
                .Select(s => new
                {
                    id = s.id,
                    username = s.id + " - " + s.name
                }).ToList();
            ViewBag.id = new SelectList(user, "id", "username");
            return View(phanQuyen);
        }

        // GET: Admin/PhanQuyen/Edit/5
        [SessionCheck]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanQuyen phanQuyen = db.PhanQuyens.Find(id);
            var user =
                db.users
                .Select(s => new
                {
                    id = s.id,
                    username = s.id + " - " + s.name
                }).ToList();
            ViewBag.id = new SelectList(user, "id", "username",phanQuyen.id);
 
            if (phanQuyen == null)
            {
                return HttpNotFound();
            }
            return View(phanQuyen);
        }

        // POST: Admin/PhanQuyen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [SessionCheck]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhanQuyen phanQuyen)
        {
            if (ModelState.IsValid)
            {
                if (phanQuyen.id != null)
                {
                    if (phanQuyen.Quyen == false)//user
                    {
                        phanQuyen.mVanban = true;
                        phanQuyen.mTheloaiTin = false;
                        phanQuyen.mLoaiTin = false;
                        phanQuyen.mTinTuc = false;
                        phanQuyen.mTaiLieu = true;
                        phanQuyen.mGioiThieu = false;
                        phanQuyen.mLienHe = false;
                        phanQuyen.mLoaivanban = true;
                        phanQuyen.mQuantri = false;

                    }
                    else
                    {
                        phanQuyen.mVanban = true;
                        phanQuyen.mTheloaiTin = true;
                        phanQuyen.mLoaiTin = true;
                        phanQuyen.mTinTuc = true;
                        phanQuyen.mTaiLieu = true;
                        phanQuyen.mGioiThieu = true;
                        phanQuyen.mLienHe = true;
                        phanQuyen.mLoaivanban = true;
                        phanQuyen.mQuantri = true;
                    }
                    try
                    {
                        db.Entry(phanQuyen).State = EntityState.Modified;
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
            var user =
                db.users
                .Select(s => new
                {
                    id = s.id,
                    username = s.id + " - " + s.name
                }).ToList();
            ViewBag.id = new SelectList(user, "id", "username", phanQuyen.id);
            return View(phanQuyen);
        }
        [SessionCheck]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Del(string id)
        {
            bool result = false;
            var u = db.PhanQuyens.Where(x => x.id == id).FirstOrDefault();
            if (u != null)
            {
                db.PhanQuyens.Remove(u);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/PhanQuyen/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanQuyen phanQuyen = db.PhanQuyens.Find(id);
            if (phanQuyen == null)
            {
                return HttpNotFound();
            }
            return View(phanQuyen);
        }

        // POST: Admin/PhanQuyen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PhanQuyen phanQuyen = db.PhanQuyens.Find(id);
            db.PhanQuyens.Remove(phanQuyen);
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
