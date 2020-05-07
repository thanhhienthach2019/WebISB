using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebISB.Models;

namespace WebISB.Areas.Admin.Controllers
{
    public class BanhanhvbController : Controller
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
        // GET: Admin/Banhanhvb
        [SessionCheck]
        public ActionResult Index()
        {
            string dv = Session["donvi"].ToString();
            List<Get_banhanhvanban_dashboard_Result> result = mod_bh.Get_banhanhvanban_dashboard(dv);
            return View(result);
        }

        // GET: Admin/Banhanhvb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banhanhvb banhanhvb = db.Banhanhvbs.Find(id);
            if (banhanhvb == null)
            {
                return HttpNotFound();
            }
            return View(banhanhvb);
        }

        // GET: Admin/Banhanhvb/Insert
        [SessionCheck]
        public ActionResult Insert()
        {
            var model = new Banhanhvb();
            var tieudes =
                db.vanbans
                .Select(s => new
                {
                    Id = s.Id,
                    Sovb = s.Sovb,
                    Trichyeu = s.Trichyeu,
                    Tieude = s.Sovb +"-"+ s.Trichyeu
                }).ToList();
            ViewBag.Idvanban = new SelectList(tieudes, "Id", "Tieude");
            model.Ngayphathanh = DateTime.Now;
            ViewBag.Nguoibh = Session["user_fullname"].ToString();
            return View(model);
        }

        // POST: Admin/Banhanhvb/Insert
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(Banhanhvb banhanhvb)
        {
            string checkedCheckBoxes = Request.Form["checkbox"];
            string[] arrListStr = checkedCheckBoxes.Split(',');
            string Str_check = Request.Form["Idvanban"].ToString();
            string Nguoibh;
            Banhanhvb banhanhvb_db;

            DateTime Ngaybh;
            Nguoibh = Session["user_id"].ToString();
            Ngaybh = DateTime.Now;    
            if (!string.IsNullOrEmpty(checkedCheckBoxes))
            {
                for (int i = 0; i < arrListStr.Count(); i++)
                {
                    banhanhvb_db = new Banhanhvb();
                    switch (arrListStr[i])
                    {
                        case "1":
                            banhanhvb_db.Donvinhan = "1100000000";
                            
                            break;
                        case "2":
                            banhanhvb_db.Donvinhan = "1101000000";
                            break;
                        case "3":
                            banhanhvb_db.Donvinhan = "1102000000";
                            break;
                        case "4":
                            banhanhvb_db.Donvinhan = "1103000000";

                            break;
                        case "5":
                            banhanhvb_db.Donvinhan = "1104000000";

                            break;
                        case "6":
                            banhanhvb_db.Donvinhan = "1105000000";

                            break;
                        case "7":
                            banhanhvb_db.Donvinhan = "1106000000";

                            break;
                        case "8":
                            banhanhvb_db.Donvinhan = "1107000000";

                            break;
                        case "9":
                            banhanhvb_db.Donvinhan = "1108000000";

                            break;
                        case "10":
                            banhanhvb_db.Donvinhan = "1109000000";

                            break;
                        case "11":
                            banhanhvb_db.Donvinhan = "1110000000";

                            break;
                        case "12":
                            banhanhvb_db.Donvinhan = "1111000000";

                            break;
                        case "13":
                            banhanhvb_db.Donvinhan = "1112000000";

                            break;
                        case "14":
                            banhanhvb_db.Donvinhan = "1113000000";

                            break;
                        case "15":
                            banhanhvb_db.Donvinhan = "1114000000";

                            break;
                        case "16":
                            banhanhvb_db.Donvinhan = "1115000000";

                            break;
                        case "17":
                            banhanhvb_db.Donvinhan = "1116000000";

                            break;
                        case "18":
                            banhanhvb_db.Donvinhan = "1117000000";

                            break;
                        case "19":
                            banhanhvb_db.Donvinhan = "1118000000";

                            break;
                        case "20":
                            banhanhvb_db.Donvinhan = "1124000000";

                            break;
                        case "21":
                            banhanhvb_db.Donvinhan = "1125000000";

                            break;
                        case "22":
                            banhanhvb_db.Donvinhan = "1128000000";

                            break;
                        case "23":
                            banhanhvb_db.Donvinhan = "1200000000";

                            break;
                        case "24":
                            banhanhvb_db.Donvinhan = "1300000000";

                            break;

                        default:
                            break;
                    }
                    if (ModelState.IsValid)
                    {

                        //var exists = db.Banhanhvbs.SingleOrDefault(x => x.Id == banhanhvb.Id) != null;
                        //if (db.Banhanhvbs.SingleOrDefault(x => x.Id == banhanhvb.Id) != null)
                        //{
                        //    banhanhvb.Id = Guid.NewGuid().ToString();
                        //}else
                        //{
                        //    db.Banhanhvbs.Add(banhanhvb);
                        //}
                     
                        banhanhvb_db.Id = Guid.NewGuid();
                        banhanhvb_db.Nguoibanhanh = Nguoibh;
                        banhanhvb_db.Ngayphathanh = Ngaybh;
                        banhanhvb_db.Idvanban = Guid.Parse(Str_check);

                        db.Banhanhvbs.Add(banhanhvb_db);
                        db.SaveChanges();
                        
                        
                    }

                }
                ViewBag.Message = "Add data successfully!";
                ViewBag.tam = 1;

            }
            else
            {
                ViewBag.tam = 2;
                ViewBag.Message = "The input data is empty!";
            }
            var tieudes =
                db.vanbans
                .Select(s => new
                {
                    Id = s.Id,
                    Sovb = s.Sovb,
                    Trichyeu = s.Trichyeu,
                    Tieude = s.Sovb + "-" + s.Trichyeu
                }).ToList();
            ViewBag.Idvanban = new SelectList(tieudes, "Id", "Tieude",Str_check);
            return View();
        }

        // GET: Admin/Banhanhvb/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banhanhvb banhanhvb = db.Banhanhvbs.Find(id);
            if (banhanhvb == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idvanban = new SelectList(db.vanbans, "Id", "Sovb", banhanhvb.Idvanban);
            ViewBag.Idvanban = new SelectList(db.vanbans, "Id", "Sovb", banhanhvb.Idvanban);
            return View(banhanhvb);
        }

        // POST: Admin/Banhanhvb/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [SessionCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Idvanban,Donvinhan,Ngayphathanh")] Banhanhvb banhanhvb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(banhanhvb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Idvanban = new SelectList(db.vanbans, "Id", "Sovb", banhanhvb.Idvanban);
            ViewBag.Idvanban = new SelectList(db.vanbans, "Id", "Sovb", banhanhvb.Idvanban);
            return View(banhanhvb);
        }

        // GET: Admin/Banhanhvb/Delete/5
        [SessionCheck]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                ViewBag.Message = "Delete data Fail!";
                ViewBag.tam = 2;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Banhanhvb banhanhvb = db.Banhanhvbs.Find(id);
                db.Banhanhvbs.Remove(banhanhvb);
                db.SaveChanges();

                ViewBag.Message = "Delete data successfully!";
                ViewBag.tam = 1;
            }

            return View();
        }

        // POST: Admin/Banhanhvb/Delete/5
        [SessionCheck]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            if(!string.IsNullOrEmpty(id.ToString()))
            {
                Banhanhvb banhanhvb = db.Banhanhvbs.Find(id);
                db.Banhanhvbs.Remove(banhanhvb);
                db.SaveChanges();

                ViewBag.Message = "Delete data successfully!";
                ViewBag.tam = 1;
            }
            else
            {
                ViewBag.Message = "Delete data Fail!";
                ViewBag.tam = 2;
            }
            

            return View();
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
