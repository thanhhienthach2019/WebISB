using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebISB.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            if(Session["user_id"].Equals(""))
            {
              return  RedirectToAction("Login", "Auth");
            }else
            {
                return View();
            }
            
        }
    }
}