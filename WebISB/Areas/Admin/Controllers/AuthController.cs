using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebISB.Models;
using WebISB.Library;
using System.Web.Security;
using System.Web.Routing;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using WebISB.ExtensionMethods;

namespace WebISB.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        Encryptor encty = new Encryptor();
        ISBEntities db = new ISBEntities();
        // GET: Admin/Auth
        public ActionResult Login(user auth)
        {
            if (Session["user_id"].Equals(""))
            {
                if (Request["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        ViewBag.Message = "";
                        auth.password = auth.password.PasswordEncryption();
                        //auth.password = EncodePassword(auth.password);
                        if (!db.users.Where(m => m.id == auth.id).Count().Equals(0))
                        {
                            //su dung Linq
                            var query = from us in db.users
                                        join pq in db.PhanQuyens on us.id equals pq.id
                                        select new { us, pq };
                            query = query.Where(m => m.us.id == auth.id && m.us.password == auth.password);
                            if (!query.Count().Equals(0))
                            {
                                var user_login = query.First();
                                Session["user_id"] = user_login.us.id;
                                Session["user_fullname"] = user_login.us.name;
                                Session["donvi"] = user_login.us.donvi;

                                if (user_login.pq.mVanban == false || user_login.pq.mVanban == null) { Session["mVanban"] = null; } else { Session["mVanban"] = user_login.pq.mVanban.ToString(); }
                                if (user_login.pq.mTheloaiTin == false || user_login.pq.mTheloaiTin == null) { Session["mTheloaiTin"] = null; } else { Session["mTheloaiTin"] = user_login.pq.mTheloaiTin.ToString(); }
                                if (user_login.pq.mLoaiTin == false || user_login.pq.mLoaiTin == null) { Session["mLoaiTin"] = null; } else { Session["mLoaiTin"] = user_login.pq.mLoaiTin.ToString(); }
                                if (user_login.pq.mTaiLieu == false || user_login.pq.mTaiLieu == null) { Session["mTaiLieu"] = null; } else { Session["mTaiLieu"] = user_login.pq.mTaiLieu.ToString(); }
                                if (user_login.pq.mTinTuc == false || user_login.pq.mTinTuc == null) { Session["mTinTuc"] = null; } else { Session["mTinTuc"] = user_login.pq.mTinTuc.ToString(); }
                                if (user_login.pq.mGioiThieu == false || user_login.pq.mGioiThieu == null) { Session["mGioiThieu"] = null; } else { Session["mGioiThieu"] = user_login.pq.mGioiThieu.ToString(); }
                                if (user_login.pq.mLienHe == false || user_login.pq.mLienHe == null) { Session["mLienHe"] = null; } else { Session["mLienHe"] = user_login.pq.mLienHe.ToString(); }
                                if (user_login.pq.mLoaivanban == false || user_login.pq.mLoaivanban == null) { Session["mLoaivanban"] = null; } else { Session["mLoaivanban"] = user_login.pq.mLoaivanban.ToString(); }
                                if (user_login.pq.mQuantri == false || user_login.pq.mQuantri == null) { Session["mQuantri"] = null; } else { Session["mQuantri"] = user_login.pq.mQuantri.ToString(); }
                                //return RedirectToAction("Index", "TrangChu");

                                //return Redirect(Url.Content("~/"));
                                ViewBag.tmp = 1;
                            }
                            //if (!db.users.Where(m => m.id == auth.id && m.password == auth.password).Count().Equals(0))
                            //{
                            //    var user_login = db.users.Where(m => m.id == auth.id && m.password == auth.password).First();
                            //    Session["user_id"] = user_login.id;
                            //    Session["user_fullname"] = user_login.name;
                            //    Session["donvi"] = user_login.donvi;
                            //    //return RedirectToAction("Index", "TrangChu");
                            //    return Redirect(Url.Content("~/"));
                            //}
                            else
                            {
                                ViewBag.Message = "Incorrect password!";
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Account does not exist!!!";
                        }
                    }
                }
                else if (Request["cancel"] != null)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            else
            {
                return Redirect(Url.Content("~/"));
            }

            return View();
        }

        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session["user_id"] = "";
                Session["user_fullname"] = "";
                Session["donvi"] = "";
                Session["mVanban"] = "";
                Session["mTheloaiTin"] = "";
                Session["mLoaiTin"] = "";
                Session["mTinTuc"] = "";
                Session["mTaiLieu"] = "";
                Session["mGioiThieu"] = "";
                Session["mLienHe"] = "";
                ViewBag.tmp = 1;
                ViewBag.Message = "Logged out account!";
            }
            catch (Exception ex)
            {
                ViewBag.tmp = 2;
                ViewBag.Message = "Logging out failed!";
            }

            //return Redirect(Url.Content("~/"));
            return View();
            //return new RedirectToRouteResult(
            //    new RouteValueDictionary(
            //        new
            //        {
            //            //area = "Admin",
            //            //controller = "Auth",
            //            //action = "Login"

            //            controller = "TrangChu",
            //            action = "Index"
            //        }
            //    )
            //);
        }

    }
}