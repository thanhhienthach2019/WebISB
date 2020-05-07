using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static System.Collections.Specialized.BitVector32;

namespace WebISB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            var httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 400:
                        // Bad Request
                        action = "BadRequest";
                        break;
                    case 404:
                        // page not found
                        action = "NotFound";
                        break;
                    case 403:
                        // forbidden
                        action = "Forbidden";
                        break;
                    case 500:
                        // server error
                        action = "HttpError500";
                        break;
                    default:
                        action = "Unknown";
                        break;
                }

                // clear error on server
                Server.ClearError();

                Response.Redirect(String.Format("~/Error/{0}", action));
            }
            else
            {
                // this is my modification, which handles any type of an exception.
                Response.Redirect(String.Format("~/Error/Unknown"));
            }
        }
        protected void Session_Start()
        {
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
        }
    }
}
