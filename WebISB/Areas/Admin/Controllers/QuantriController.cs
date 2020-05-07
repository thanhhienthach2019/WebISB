using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebISB.ExtensionMethods;
using WebISB.Models;

namespace WebISB.Areas.Admin.Controllers
{
    public class QuantriController : Controller
    {
        private ISBEntities db = new ISBEntities();
        private User_Models mod = new User_Models();
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;
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

        // GET: Admin/Quantri
        [SessionCheck]
        public ActionResult Index()
        {
            List<Get_User_Result> result = mod.GetUser();
            return View(result);
        }

        // GET: Admin/Quantri/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Quantri/Insert
        [SessionCheck]
        public ActionResult Insert()
        {
            var model = new user();
            ViewBag.donvi = new SelectList(db.DonVis, "MS_DV", "TenDonVi");
            model.created_at = DateTime.Now;
            return View(model);
        }

        // POST: Admin/Quantri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(user user)
        {
            if (ModelState.IsValid)
            {
                if(user.id != null)
                {
                    user.created_at = DateTime.Now;
                    user.updated_at = null;
                    user.password = user.password.PasswordEncryption();
                    try
                    {
                        db.users.Add(user);
                        db.SaveChanges();
                        ViewBag.tam = 1;
                        ViewBag.Message = "Add data successfully!";
                    }
                    catch
                    {
                        ViewBag.tam = 2;
                        ViewBag.Message = "Add data failed!";
                    }
                    
                }
                else
                {
                    ViewBag.tam = 2;
                    ViewBag.Message = "The input data is empty!";
                }
                

            }

            ViewBag.donvi = new SelectList(db.DonVis, "MS_DV", "TenDonVi", user.donvi);
            return View(user);
        }
        // GET: Admin/Quantri/Edit/5
        [SessionCheck]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            user.updated_at = DateTime.Now;
       
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.donvi = new SelectList(db.DonVis, "MS_DV", "TenDonVi", user.donvi);
            return View(user);
        }

        // POST: Admin/Quantri/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(user user)
        {
            if (ModelState.IsValid)
            {
                user.updated_at = DateTime.Now;            
                try
                {
                    if(string.IsNullOrEmpty(user.password))
                    {
                        var user_tmp = new user() { id = user.id, name = user.name, email = user.email, created_at = user.created_at, updated_at = user.updated_at, donvi = user.donvi };
                        db.Entry(user_tmp).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.tam = 1;
                        ViewBag.Message = "Update data success!";
                        //db.users.Attach(user);
                        //db.Entry(user).Property(x => x.name).IsModified = true;
                    }
                    else
                    {
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.tam = 1;
                        ViewBag.Message = "Update data success!";
                    }
                    
                }
                catch
                {
                    ViewBag.tam = 2;
                    ViewBag.Message = "Update data failed!";
                }
                
                
            }
            ViewBag.donvi = new SelectList(db.DonVis, "MS_DV", "TenDonVi", user.donvi);
            return View(user);
        }
        [SessionCheck]
        public JsonResult Delete(string id)
        {
            bool result = false;
            var u = db.users.Where(x => x.id == id).FirstOrDefault();
            if (u != null)
            {
                db.users.Remove(u);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/Quantri/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    user user = db.users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        // POST: Admin/Quantri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
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
