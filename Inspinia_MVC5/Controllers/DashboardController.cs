using ee.Models.dashboardmodel;
using stay_calm_db_tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class DashboardController : Controller
    {

        public ActionResult Dashboard()
        {
            return View();
        }

        private userContext db = new userContext();
        siteuser siteuser = new siteuser();

        public PartialViewResult criticalview()
        {
            dashboard_alertmodel bi_alerts = new dashboard_alertmodel();
            bi_alerts.Site = new List<string>();
            bi_alerts.Date = new List<string>();
            bi_alerts.Alert = new List<string>();


            siteuser siteuser = new siteuser();
            
            /*
            Expression < Func < siteuser, \u003C\u003Ef__AnonymousType5 < string, int>>> selector = x => new
            {
                user_site = x.user_site,
                Id = x.Id
            };

            foreach (var data in siteuser.Select(selector).ToList())
            {
                var item = data;
                List<int> list1 = db.hgstatus_link.Where<hgstatus_link>((Expression<Func<hgstatus_link, bool>>)(v => v.site_serial_id == item.Id && v.alert_type == 4)).Select<hgstatus_link, int>((Expression<Func<hgstatus_link, int>>)(v => v.errors_id)).ToList<int>();
                if (list1.Count<int>() != 0)
                {
                    List<hgstatus_db> list2 = db.hgstatus_db.Where<hgstatus_db>((Expression<Func<hgstatus_db, bool>>)(x => x.serial_site_id == item.Id)).OrderByDescending<hgstatus_db, DateTime>((Expression<Func<hgstatus_db, DateTime>>)(x => x.date_record)).ToList<hgstatus_db>();
                    if (list2.Count<hgstatus_db>() != 0)
                    {
                        foreach (hgstatus_db values in list2)
                        {
                            foreach (int errorlist in list1)
                                this.hgsataus_model(bi_alerts, errorlist, values, item.user_site);
                        }
                    }
                }
            }
            */
            return PartialView(bi_alerts);
        }




























    }
}