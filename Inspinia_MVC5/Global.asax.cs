﻿using stay_calm_db_tests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Inspinia_MVC5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Testing this bad boy
            MvcHandler.DisableMvcResponseHeader = true;
            //

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // Trying Transient fault 
            DbConfiguration.SetConfiguration(new EFConfiguration());


        }


        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("Server");
            HttpContext.Current.Response.AddHeader("Content-Type", " ;charset= ");
            HttpContext.Current.Response.Headers.Add("Arr-Disable-Session-Affinity", "True");
        }
    }
}
