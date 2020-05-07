using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebISB.Models;

namespace WebISB.Controllers
{
    [HandleError]
    public class GioiThieuController : Controller
    {
        private ISBEntities db = new ISBEntities();
        GioiThieu_Models mod = new GioiThieu_Models();
        // GET: GioiThieu
        public ActionResult Index()
        {
            List<gioithieu> result = mod.Get_GioiThieu();
            return View(result);
        }
    }
}