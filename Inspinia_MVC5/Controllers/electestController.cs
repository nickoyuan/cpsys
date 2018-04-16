using stay_calm_db_tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class electestController : Controller
    {
        private userContext db = new userContext();

        public ActionResult user_current()
        {
            return View(db.USERS.ToList());
        }

        public ActionResult view_ioalarm_type(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // Me viewing all HG Status that is connected to site 
            var hg_attached = (from d in db.io_alarms
                               where d.siteuser.Id == id
                               select d).ToList();

            if (hg_attached == null)
            {
                return HttpNotFound();
            }




            // Send this also to view 
            ViewBag.idforlinkinglimit = id;

            return View(hg_attached);
        }

        public ActionResult view_sites()
        {
            return View(db.siteuser.ToList());
        }




    }
}