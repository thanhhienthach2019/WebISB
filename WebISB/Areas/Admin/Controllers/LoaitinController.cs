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
    public class LoaitinController : Controller
    {
        private ISBEntities db = new ISBEntities();
        private Mystring mystring = new Mystring();

        Loaitin_Models mod = new Loaitin_Models();
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
        // GET: Admin/Loaitin
        [SessionCheck]
        public ActionResult Index()
        {
                List<FN_Get_LoaiTin_Result> result = mod.GetLoaiTin();
                return View(result);
        }
        public ActionResult Status(Int16? id)
        {
            int res;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            loaitin loaitin = db.loaitins.Find(id);
            if (loaitin.status == 1)
            {
                //res_count = mod.Count_Status_Tl(id, 2);
                loaitin.status = 2;
                res = mod.Update_Status_TT(id, 2);
                             
            }
            else
            {
                loaitin.status = 1;
                res = mod.Update_Status_TT(id, 1);
            }
            db.Entry(loaitin).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Loaitin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loaitin loaitin = db.loaitins.Find(id);
            if (loaitin == null)
            {
                return HttpNotFound();
            }
            return View(loaitin);
        }

        // GET: Admin/Loaitin/Insert
        [SessionCheck]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        public ActionResult Insert()
        {
                //loaitin loaitin = new loaitin();
                var model = new loaitin();
                ViewBag.idTheLoai = new SelectList(db.theloais, "id", "Ten");
                model.created_at = DateTime.Now;
                return View(model);
        }

        // POST: Admin/Loaitin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Insert(loaitin loaitin)
        {
                ViewBag.idTheLoai = new SelectList(db.theloais, "id", "Ten");
                if (ModelState.IsValid)
                {
                    if(loaitin.Ten != null)
                    {
                        ViewBag.tam = 1;
                        loaitin.created_at = DateTime.Now;
                        loaitin.updated_at = null;
                        loaitin.TenKhongDau = mystring.convertToUnSign(loaitin.Ten);

                        db.loaitins.Add(loaitin);
                        db.SaveChanges();
                        //ViewBag.Message = System.Text.Encoding.UTF8.GetBytes("Thêm dữ liệu thành công!");
                        ViewBag.Message = "Add data successfully!";
                        //return RedirectToAction("Index");
                    }else
                    {
                        ViewBag.tam = 2;
                        ViewBag.Message = "The input data is empty!";
                    }

                }

                ViewBag.idTheLoai = new SelectList(db.theloais, "id", "Ten", loaitin.idTheLoai);
                return View(loaitin);
        }
        [SessionCheck]
        public ActionResult Trash()
        {
                List<FN_Get_LoaiTin_Trash_Result> results = mod.GetLoaiTinTrash();
                //var model = db.tintucs.Where(m => m.status == 0).ToList();
                return View(results);
        }
        [SessionCheck]
        public ActionResult Deltrash(int? id)
        {
            int res;
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                loaitin loaitin = db.loaitins.Find(id);
                loaitin.status = 0;
                db.Entry(loaitin).State = EntityState.Modified;
                db.SaveChanges();

                res = mod.Update_Status_TT(id, 0);
                return RedirectToAction("Index");
        }
        [SessionCheck]
        public ActionResult Retrash(int? id)
        {
            int res;
                if (id == null)
                {
                    return RedirectToAction("Trash", "Loaitin");
                }
                loaitin loaitin = db.loaitins.Find(id);
                loaitin.status = 2;
                db.Entry(loaitin).State = EntityState.Modified;
                db.SaveChanges();

                res = mod.Update_Status_TT(id, 2);

                ViewBag.Message = "Data recovery success!";
            return RedirectToAction("Trash", "Loaitin");
        }

        [SessionCheck]
        // GET: Admin/Loaitin/Edit/5
        public ActionResult Edit(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                loaitin loaitin = db.loaitins.Find(id);
                if (loaitin == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idTheLoai = new SelectList(db.theloais, "id", "Ten", loaitin.idTheLoai);
                return View(loaitin);
        }
        // POST: Admin/Loaitin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Edit(loaitin loaitin)
        {
                if (ModelState.IsValid)
                {
                    loaitin.updated_at = DateTime.Now;
                    loaitin.TenKhongDau = mystring.convertToUnSign(loaitin.Ten);
                    db.Entry(loaitin).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Update data successfully!";
                    //return RedirectToAction("Index");
                }
                ViewBag.idTheLoai = new SelectList(db.theloais, "id", "Ten", loaitin.idTheLoai);
                return View(loaitin);
        }
        // GET: Admin/Loaitin/Delete/5
        [SessionCheck]
        public ActionResult Delete(int? id)
        {
            int res;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                res = mod.Delete_Tintuc(id);//delete tintuc
                loaitin loaitin = db.loaitins.Find(id);
                db.loaitins.Remove(loaitin);
                db.SaveChanges();
                ViewBag.Message = "Delete data successfully!";
                return RedirectToAction("Trash", "Loaitin");
        }

        // POST: Admin/Loaitin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            loaitin loaitin = db.loaitins.Find(id);
            db.loaitins.Remove(loaitin);
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
        public string TenTheloai(int idTheLoai)
        {
            var item = db.theloais.Where(m => m.id == idTheLoai).Select(m => m.Ten).First();
            return item;
        }
    }
}
