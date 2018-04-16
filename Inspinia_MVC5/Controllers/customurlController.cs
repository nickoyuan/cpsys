using stay_calm_db_tests;
using ee.Models.chparamModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class customurlController : Controller
    {
        public async Task<ActionResult> chparams(String serialsite, String xpa, String ypa, String zpa, String apa)
        {

            if (serialsite == null)
            {
                serialsite = "0890CA";
            }
            else
            {
                serialsite = serialsite.Replace(" ", string.Empty);

            }
            userContext db = new userContext();
            if (xpa != null)
            {
               
                var ch_response = db.chparamdbs.Where(x => x.sitetoboards.serialboard == serialsite).FirstOrDefault();

                double x_int = 10;
                double.TryParse(xpa, out x_int);

                double y_int = 10;
                double.TryParse(ypa, out y_int);

                double z_int = 10;
                double.TryParse(zpa, out z_int);

                double a_int = 10;
                double.TryParse(apa, out a_int);


                if (ch_response != null)
                {
                    ch_response.xparams = x_int;
                    ch_response.yparams = y_int;

                    ch_response.zparams = z_int;
                    ch_response.aparams = a_int;

                    db.Entry(ch_response).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                }
            }

            chparamdb adding_sites = new chparamdb();
            adding_sites.serial_site = new List<String>();

            int count = 0;
            var site_to_comms = await (from y in db.chparamdbs
                                       select y.sitetoboards).ToListAsync();

            foreach (var item in site_to_comms)
            {
                adding_sites.serial_site.Add(item.serialboard);

                if (item.serialboard.Contains(serialsite))
                {
                    ViewBag.site_selected = count;
                }
                else
                { count = count + 1; }
            }


            var x_display = await (from x in db.chparamdbs
                                   where x.sitetoboards.serialboard == serialsite
                                   select new {
                                          x.xparams,
                                          x.yparams,
                                          x.zparams,
                                          x.aparams
                                         }).FirstOrDefaultAsync();
            

            ViewBag.x_pa = x_display.xparams.ToString();
            ViewBag.y_pa = x_display.yparams.ToString();


            ViewBag.z_pa = x_display.zparams.ToString();
            ViewBag.a_pa = x_display.aparams.ToString();




            return View(adding_sites);
        }



    }
}