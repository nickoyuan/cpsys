using newtest.Models.sitelistmodel;
using stay_calm_db_tests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class siteaccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private userContext db = new userContext();

    public ActionResult site_list()
    {
      sitelist_model sitelistModel = new sitelist_model();
      sitelistModel.serialnumber = new List<string>();
      sitelistModel.serialid = new List<string>();
      sitelistModel.sites = new List<string>();
      sitelistModel.site_id = new List<string>();

            var taken = (from x in db.siteuser
                         select x.sitetoboards.Id).ToList();

            var list = (from x in db.sitetoboards
                        where !taken.Contains(x.Id)
                        select new {
                            x.serialboard,
                            x.Id
                        }).ToList();
            
      if (list.Count() == 0)
      {
        sitelistModel.serialnumber.Add("No Avaliable Serials Please add more");
        sitelistModel.serialid.Add("");
      }
      else
      {
        foreach (var data in list)
        {
          sitelistModel.serialnumber.Add(data.serialboard);
          sitelistModel.serialid.Add(data.Id.ToString());
        }
      }



            var siteuser = (from x in db.siteuser
                            select new {
                                x.user_site,
                                x.Id
                            }).ToList();
     
      foreach (var data in siteuser)
      {
        sitelistModel.sites.Add(data.user_site);
        sitelistModel.site_id.Add(data.Id.ToString());
      }
      return View(sitelistModel);
    }

        public PartialViewResult user(string username, string password, string[] options)
        {
            usermodellist usermodellist = new usermodellist();
            usermodellist.username = new List<string>();
            usermodellist.password = new List<string>();
            usermodellist.email = new List<string>();


            if(username !=null)
            {

                newusers add_user = new newusers();
                add_user.username = username;
                add_user.password = password;
                db.USERS.Add(add_user);
                db.SaveChanges();

                if (options != null)
                {
                    int num = db.USERS.Where(x => x.username == username).Select(x => x.ID).FirstOrDefault();
                    int result = 0;
                    for (int index = 0; index < options.Length; ++index)
                    {
                        int.TryParse(options[index], out result);
                        db.site_user_link.Add(new site_user_link()
                        {
                            siteuserId = result,
                            newusersID = num
                        });
                        db.SaveChanges();
                    }
                }
            }


            var users = (from x in db.USERS
                         select x).ToList();

            foreach (newusers newusers in users)
            {
                usermodellist.username.Add(newusers.username);
                usermodellist.password.Add(newusers.password);
                usermodellist.email.Add(newusers.email);
            }
            return PartialView(usermodellist);
        }


        public PartialViewResult serial(string serial)
        {
            serialmodellist serialmodellist = new serialmodellist();
            serialmodellist.serialnumber = new List<string>();
            serialmodellist.is_attached = new List<string>();
            serialmodellist.site = new List<string>();
            serialmodellist.site_id = new List<string>();

            if (serial != null)
            {
                serial.Replace(" ", string.Empty);
                var newboardid = (from x in db.sitetoboards
                                  where x.serialboard == serial
                                  select x.serialboard).FirstOrDefault();

                if(newboardid != serial)
                {
                    String date_format = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);


                    DateTime date;
                    // Check if date is valid 
                    DateTime.TryParse(date_format, out date);

                    serial.Replace(" ", string.Empty);

                    sitetoboards newboards = new sitetoboards();
                    newboards.serialboard = serial;
                    newboards.lastseen = date;
                    db.sitetoboards.Add(newboards);
                    db.SaveChanges();


                    var site_id = (from x in db.sitetoboards
                                      where x.serialboard == serial
                                      select x.Id).FirstOrDefault();

                    response_comms add_response = new response_comms();
                    add_response.response_bit = 1;
                    add_response.frequency = 10;
                    add_response.once_on_server = 1.1;
                    add_response.comms_board_id = site_id;

                    db.response_comms.Add(add_response);
                    db.SaveChanges();

                    chparamdbs add_new_chparams = new chparamdbs();
                    add_new_chparams.xparams = 0;
                    add_new_chparams.yparams = 0;
                    add_new_chparams.zparams = 0;
                    add_new_chparams.aparams = 0;
                    add_new_chparams.serial_to_chparams = site_id;
                    db.chparamdbs.Add(add_new_chparams);
                    db.SaveChanges();
                }
                else
                {
                    
                }
                
            }
            else
            {
                serial = "0";
            }

           

            var sitetoboards1 = (from x in db.sitetoboards
                                 select new
                                 {
                                     x.serialboard,
                                     x.Id
                                 }).ToList();

            var all_site_serials = (from x in db.siteuser
                                    select new
                                    {   x.user_site,
                                        x.serial_board_id
                                    }).ToList();
            
            foreach (var item in sitetoboards1)
            {
                serialmodellist.serialnumber.Add(item.serialboard);
                serialmodellist.site_id.Add(item.Id.ToString());

                if (item.serialboard == serial)
                {
                    serialmodellist.is_attached.Add("No");
                    serialmodellist.site.Add("None");
                }
                else
                {
                    
                    if (all_site_serials.Where(x=> x.serial_board_id == item.Id).FirstOrDefault() == null)
                    {
                        serialmodellist.is_attached.Add("No");
                        serialmodellist.site.Add("None");
                    }
                    else
                    {
                        serialmodellist.is_attached.Add("Attached");
                        serialmodellist.site.Add(all_site_serials.Where(x => x.serial_board_id == item.Id).Select(x => x.user_site).FirstOrDefault());
                    }
                    

                }
            }
            return PartialView(serialmodellist);
        }


        public PartialViewResult site(string serial, string sitename, string tmzone)
        {
            sitemodellist sitemodellist = new sitemodellist();
            sitemodellist.status = new List<string>();
            sitemodellist.date_lastseen = new List<string>();
            sitemodellist.sites = new List<string>();
            sitemodellist.timezone = new List<string>();
            sitemodellist.serialnumber = new List<string>();
            sitemodellist.hgstatus_count = new List<string>();
            sitemodellist.ioalarms_count = new List<string>();
            sitemodellist.graph_data_count = new List<string>();

            if (sitename != null && serial != null)
            {

                sitename.Replace(" ", string.Empty);
                int result = 0;
                int.TryParse(serial, out result);
                

                siteuser attaching = new siteuser();
                attaching.user_site = sitename;
                attaching.serial_board_id = result;
                attaching.GMT = tmzone;
                db.siteuser.Add(attaching);
                db.SaveChanges();

                // Adding the IO Alarms
                var id_limits = (from y in db.siteuser
                                 where y.user_site == sitename && y.serial_board_id == result
                                 select y.Id).FirstOrDefault();

                // IO Alarms 
                io_alarms naming_alarms = new io_alarms();
                naming_alarms.switch_number = 1;
                naming_alarms.alarm_name = "sw1";
                naming_alarms.site_id_link = id_limits;
                db.io_alarms.Add(naming_alarms);
                db.SaveChanges();


                // Graph Data types + Limits 
                graph_limit site_to_twenty_two = new graph_limit();

                site_to_twenty_two.graph_data_type_id = 5;
                site_to_twenty_two.site_id_to_limit = id_limits;  // 16
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();


                site_to_twenty_two.graph_data_type_id = 1;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 4;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 38;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 6;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 10;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 13;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();


                site_to_twenty_two.graph_data_type_id = 35;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();


                site_to_twenty_two.graph_data_type_id = 36;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 37;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 39;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 40;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();


                site_to_twenty_two.graph_data_type_id = 41;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 42;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 43;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 44;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 45;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 46;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 47;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 48;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 49;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 50;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 2;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 23;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 24;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 20;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                site_to_twenty_two.graph_data_type_id = 33;
                site_to_twenty_two.site_id_to_limit = id_limits;
                site_to_twenty_two.lower_limit_to = 0;
                site_to_twenty_two.lower_limit_from = 0;
                site_to_twenty_two.upper_limit_to = 0;
                site_to_twenty_two.upper_limit_from = 0;

                db.graph_limit.Add(site_to_twenty_two);
                db.SaveChanges();

                hgstatus_link adding_all_hg = new hgstatus_link();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 1;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 2;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 3;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 4;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 5;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 6;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();


                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 7;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 8;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 10;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();


                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 11;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 12;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 14;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 15;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 16;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 17;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 18;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();


                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 19;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();


                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 20;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 21;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 22;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 23;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 24;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 25;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 26;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 27;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 28;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 29;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 30;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 31;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 32;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 33;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 34;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();


                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 35;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 36;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();


                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 37;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();


                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 38;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 39;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 40;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 41;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 42;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 43;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 44;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 45;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 46;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 47;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 48;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 49;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 50;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 51;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 52;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 53;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();

                adding_all_hg.site_serial_id = id_limits;
                adding_all_hg.errors_id = 54;
                adding_all_hg.alert_type = 1;
                db.hgstatus_link.Add(adding_all_hg);
                db.SaveChanges();
                
            }

            var siteuser = (from x in db.siteuser
                            select new 
            {
                lastseen = x.sitetoboards.lastseen,
                user_site = x.user_site,
                Id = x.Id,
                serialboard = x.sitetoboards.serialboard,
                GMT = x.GMT,
                hgstatus_link = x.hgstatus_link,
                io_alarms = x.io_alarms,
                graph_limit = x.graph_limit,
                
            }).ToList();

            foreach (var item in siteuser)
            {
                sitemodellist.sites.Add(item.user_site);
                sitemodellist.serialnumber.Add(item.serialboard);
                sitemodellist.timezone.Add(item.GMT);



                int num1 = item.hgstatus_link.Count();
                string str1 = num1.ToString();
                sitemodellist.hgstatus_count.Add(str1);

               
                num1 = item.io_alarms.Count();
                string str2 = num1.ToString();
                sitemodellist.ioalarms_count.Add(str2);

                
                num1 = item.graph_limit.Count();
                string str3 = num1.ToString();
                sitemodellist.graph_data_count.Add(str3);



                DateTime lastseen = item.lastseen;
                TimeSpan timeSpan = DateTime.Now - lastseen;
                double totalDays = timeSpan.TotalDays;
                
               
                    string status = "Down";
                    string date;

                    if (totalDays > 10)
                    {

                        String Gmt_convert = item.GMT;

                        if (Gmt_convert == null)
                        {
                            Gmt_convert = "AUS Eastern Standard Time";
                        }

                        var est = TimeZoneInfo.FindSystemTimeZoneById(Gmt_convert);
                        var targetTime = TimeZoneInfo.ConvertTime(lastseen, est);

                        date = targetTime.ToString();
                    }
                    else
                    {

                        if (totalDays.ToString("0") != "0")
                        {
                            date = totalDays.ToString("0") + " days ago";
                        }
                        else
                        {
                            double minutes = timeSpan.TotalMinutes;
                            date = minutes.ToString("0") + " minutes ago";

                            // UP or DOWN 
                            if (minutes < (10 * 3))
                            {
                                status = "UP";
                            }

                            if (minutes.ToString("0") == "0")
                            {
                                double seconds = timeSpan.TotalSeconds;
                                date = seconds.ToString("0") + " seconds ago";
                            }

                        }
                    }
                    sitemodellist.status.Add(status);
                    sitemodellist.date_lastseen.Add(date);
                
            }
            return PartialView(sitemodellist);
        }


        public ActionResult edit_users()
        {
            return View();
        }


    }
}