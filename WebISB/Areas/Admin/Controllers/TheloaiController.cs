using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebISB.Library;
using WebISB.Models;

namespace WebISB.Areas.Admin.Controllers
{
    public class TheloaiController : Controller
    {
        private ISBEntities db = new ISBEntities();

        private Mystring mystring = new Mystring();

        Theloai_Models mod = new Theloai_Models();
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


        //[AjaxAuthorizeAttribute]
        // GET: Admin/Theloai
        [SessionCheck]
        public ActionResult Index()
        {
                List<FN_Get_TheLoai_Result> result = mod.GetTheLoai();
                return View(result);
        }
        [SessionCheck]
        public ActionResult Trash()
        {
                List<FN_Get_TheLoai_Trash_Result> results = mod.GetTheLoaiTrash();
                return View(results);
        }

        public ActionResult Status(int? id)
        {
            int res;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            theloai theloai = db.theloais.Find(id);

            if (theloai.status == 1)
            {
                theloai.status = 2;
                res = mod.Update_Status_LT(id, 2);
            }
            else
            {
                theloai.status = 1;
                res = mod.Update_Status_LT(id, 1);
            }
            db.Entry(theloai).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [SessionCheck]
        public ActionResult Deltrash(int? id)
        {
            int res;
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                theloai theloai = db.theloais.Find(id);
                theloai.status = 0;
                db.Entry(theloai).State = EntityState.Modified;
                db.SaveChanges();

                res = mod.Update_Status_LT(id, 0);//update loaitin & tintuc

                return RedirectToAction("Index");
        }
        [SessionCheck]
        public ActionResult Retrash(int? id)
        {
            int res;
                if (id == null)
                {
                    return RedirectToAction("Trash", "Theloai");
                }
                theloai theloai = db.theloais.Find(id);
                theloai.status = 2;
                db.Entry(theloai).State = EntityState.Modified;
                db.SaveChanges();

                res = mod.Update_Status_LT(id, 2);//update loaitin & tintuc

                ViewBag.Message = "Data recovery success!";
                return RedirectToAction("Trash", "Theloai");
        }
        // GET: Admin/Theloai/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            theloai theloai = db.theloais.Find(id);
            if (theloai == null)
            {
                return HttpNotFound();
            }
            return View(theloai);
        }

        // GET: Admin/Theloai/Create
        [SessionCheck]
        public ActionResult Insert()
        {
                //loaitin loaitin = new loaitin();
                var model = new theloai();
                model.created_at = DateTime.Now;
                return View(model);
        }

        // POST: Admin/Theloai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Insert(theloai theloai)
        {
                if (ModelState.IsValid)
                {
                    if (theloai.Ten != null)
                    {
                        ViewBag.tam = 1;
                        theloai.created_at = DateTime.Now;
                        theloai.updated_at = null;
                        theloai.TenKhongDau = mystring.convertToUnSign(theloai.Ten);

                        db.theloais.Add(theloai);
                        db.SaveChanges();
                        ViewBag.Message = "Add data successfully!";
                    }
                    else
                    {
                        ViewBag.tam = 2;
                        ViewBag.Message = "The input data is empty!";
                    }

                }
                return View(theloai);
        }

        // GET: Admin/Theloai/Edit/5
        [SessionCheck]
        public ActionResult Edit(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                theloai theloai = db.theloais.Find(id);
                theloai.updated_at = DateTime.Now;
                if (theloai == null)
                {
                    return HttpNotFound();
                }
                return View(theloai);
        }

        // POST: Admin/Theloai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Edit(theloai theloai)
        {
                if (ModelState.IsValid)
                {
                    theloai.updated_at = DateTime.Now;
                    theloai.TenKhongDau = mystring.convertToUnSign(theloai.Ten);
                    db.Entry(theloai).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Update data successfully!";
                    //return RedirectToAction("Index");
                }
                return View(theloai);
        }
        // GET: Admin/Theloai/Delete/5
        [SessionCheck]
        public ActionResult Delete(int? id)
        {
            int res;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                res = mod.Delete_Loaitin_Tintuc(id);//delete loaitin & tintuc

                theloai theloai = db.theloais.Find(id);
                db.theloais.Remove(theloai);
                db.SaveChanges();
                ViewBag.Message = "Delete data successfully!";
                return RedirectToAction("Trash", "Theloai");
        }

        // POST: Admin/Theloai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            theloai theloai = db.theloais.Find(id);
            db.theloais.Remove(theloai);
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
