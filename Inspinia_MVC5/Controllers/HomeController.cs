// Decompiled with JetBrains decompiler
// Type: newtest.Controllers.HomeController
// Assembly: newtest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 461946BA-2AB1-41EE-A219-D965F6459033
// Assembly location: D:\rmit_work\Visual Studio 2015 Work\elementtest\site\site\wwwroot\bin\newtest.dll

using Microsoft.CSharp.RuntimeBinder;
using newtest.Models.loginmodel;
using stay_calm_db_tests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace newtest.Controllers
{
    public class HomeController : Controller
    {
       

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(userlogin u)
        {
            // EFConfiguration.SuspendExecutionStrategy = true;

              

            if (ModelState.IsValid)
            {

                userContext db = new userContext();

                var v = await db.USERS.Where(model => model.username.Equals(u.username) && model.password.Equals(u.password)).FirstOrDefaultAsync();

                if (v != null)
                {

                    if ("TEST" == u.username.ToString() && "TEST" == u.password.ToString())
                    {

                        return RedirectToAction("site_data_test", "HistorySite");
                    }
                    



                }

            }

            // EFConfiguration.SuspendExecutionStrategy = false;

            return View(u);
        }


    }
}
