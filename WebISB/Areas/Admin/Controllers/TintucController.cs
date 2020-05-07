using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebISB.Models;
using WebISB.Library;
using System.IO;
using System.Text;
using System.Web.Routing;
using System.Web.Helpers;

namespace WebISB.Areas.Admin.Controllers
{
    public class TintucController : Controller
    {
        private ISBEntities db = new ISBEntities();
        private Mystring mystring = new Mystring();

        Tintuc_Models mod = new Tintuc_Models();
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
        // GET: Admin/Tintuc
        public ActionResult Index()
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                List<FN_Get_TinTuc_Result> result = mod.GetTintucs();
                return View(result);
            }   
        }
        public ActionResult Trash()
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                List<FN_Get_TinTuc_Trash_Result> results = mod.GetTintucTrash();
                //var model = db.tintucs.Where(m => m.status == 0).ToList();
                return View(results);
            }
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
               return RedirectToAction("Index");
            }
            tintuc tintuc = db.tintucs.Find(id);
            if(tintuc.status==1)
            {
                tintuc.status = 2;
            }else
            {
                tintuc.status = 1;
            }
            db.Entry(tintuc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Deltrash(int? id)
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                tintuc tintuc = db.tintucs.Find(id);
                tintuc.status = 0;
                db.Entry(tintuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [SessionCheck]
        public ActionResult Retrash(int? id)
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                if (id == null)
                {
                    return RedirectToAction("Trash", "Tintuc");
                }
                tintuc tintuc = db.tintucs.Find(id);
                tintuc.status = 2;
                db.Entry(tintuc).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Data recovery success!";
                return RedirectToAction("Trash", "Tintuc");
            }
        }
        [SessionCheck]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        public ActionResult Insert()
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                tintuc tintuc = new tintuc();
                tintuc.User_Create = Session["user_id"].ToString();
                ViewBag.idLoaiTin = new SelectList(db.loaitins, "id", "Ten");
                ViewBag.User_Create = new SelectList(db.users, "id", "name");

                return View();
            }

        }
        // GET: Admin/Tintuc/Edit/5
        [SessionCheck]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        public ActionResult Edit(int? id)
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tintuc tintuc = db.tintucs.Find(id);
                if (tintuc == null)
                {
                    return HttpNotFound();
                }

                ViewBag.idLoaiTin = new SelectList(db.loaitins, "id", "Ten", tintuc.idLoaiTin);
                ViewBag.User_Create = new SelectList(db.users, "id", "name", tintuc.User_Create);
                return View(tintuc);
            }
        }
        public string convert(string str)
        {
            //string unicodeString = "This string contains the unicode character Pi (\u03a0)";

            // Create two different encodings.
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte array.
            byte[] unicodeBytes = unicode.GetBytes(str);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            // Convert the new byte[] into a char[] and then into a string.
            char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            string asciiString = new string(asciiChars);

            // Display the strings created before and after the conversion.
            Console.WriteLine("Original string: {0}", str);
            Console.WriteLine("Ascii converted string: {0}", asciiString);

            return str + " => " + asciiString;
        }

        // POST: Admin/Tintuc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [SessionCheck]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Insert(tintuc tintuc)
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ViewBag.idLoaiTin = new SelectList(db.loaitins, "id", "Ten");
                ViewBag.User_Create = new SelectList(db.users, "id", "name");
                if (ModelState.IsValid)
                {
                    if(tintuc.TieuDe != null ||tintuc.NoiDung != null)
                    {
                        ViewBag.tam = 1;
                        tintuc.SoLuotXem = 0;
                        tintuc.created_at = DateTime.Now;
                        tintuc.updated_at = null;

                        tintuc.status = 2;
                        tintuc.TieuDeKhongDau = mystring.convertToUnSign(tintuc.TieuDe);
                        string ckInput = mystring.RemoveHTMLTags(tintuc.NoiDung);
                        //upload file
                        var f = Request.Files["img[]"];
                        if (f != null && f.ContentLength > 0)
                        {
                            tintuc.ImagePath = DateTime.Now.ToString("ddMMyyyyHHmmss") + f.FileName.Substring(f.FileName.LastIndexOf("."));
                            string pathfiles = Path.Combine(Server.MapPath("~/Image/Tintuc/"), tintuc.ImagePath);
                            f.SaveAs(pathfiles);
                        }
                        else
                        {
                            tintuc.ImagePath = "noimage.png";
                        }

                        db.tintucs.Add(tintuc);
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

                ViewBag.idLoaiTin = new SelectList(db.loaitins, "id", "Ten", tintuc.idLoaiTin);
                ViewBag.User_Create = new SelectList(db.users, "id", "name", tintuc.User_Create);
                return View(tintuc);
            }
        }
        // POST: Admin/Tintuc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]//tat co che bao mat de su dung dc ckedit
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tintuc tintuc)
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ViewBag.idLoaiTin = new SelectList(db.loaitins, "id", "Ten");
                ViewBag.User_Create = new SelectList(db.users, "id", "name");
                if (ModelState.IsValid)
                {
                    tintuc.updated_at = DateTime.Now;
                    tintuc.TieuDeKhongDau = mystring.convertToUnSign(tintuc.TieuDe);
                    //upload file
                    var f = Request.Files["img[]"];
                    if (f != null && f.ContentLength > 0)
                    {
                        tintuc.ImagePath = DateTime.Now.ToString("ddMMyyyyHHmmss") + f.FileName.Substring(f.FileName.LastIndexOf("."));
                        string pathfiles = Path.Combine(Server.MapPath("~/Image/Tintuc/"), tintuc.ImagePath);
                        f.SaveAs(pathfiles);
                    }
                    db.Entry(tintuc).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Update data successfully!";
                    //return RedirectToAction("Index");
                }
                ViewBag.idLoaiTin = new SelectList(db.loaitins, "id", "Ten", tintuc.idLoaiTin);
                ViewBag.User_Create = new SelectList(db.users, "id", "name", tintuc.User_Create);
                return View(tintuc);
            }
        }

        // GET: Admin/Tintuc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["user_id"].Equals(""))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tintuc tintuc = db.tintucs.Find(id);
                db.tintucs.Remove(tintuc);
                db.SaveChanges();
                ViewBag.Message = "Delete data successfully!";
                return RedirectToAction("Trash", "Tintuc");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public string TenLoaitin(int idLoaitin)
        {
            var item = db.loaitins.Where(m => m.id == idLoaitin).Select(m => m.Ten).First();
            return item;
        }
        public string TenUser(string User_Create)
        {
            var item = db.users.Where(m => m.id == User_Create).Select(m => m.name).First();
            return item;
        }
    }
}
