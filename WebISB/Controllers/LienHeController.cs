using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebISB.Models;

namespace WebISB.Controllers
{
    [HandleError]
    public class LienHeController : Controller
    {
        private ISBEntities db = new ISBEntities();
        LienHe_Models mod = new LienHe_Models();

        // GET: LienHe
        public ActionResult Index()
        { 
            List<lienhe> result = mod.Get_LienHe();
            return View(result);
        }
    }
}
