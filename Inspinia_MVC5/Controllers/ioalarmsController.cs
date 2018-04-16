
using newtest.Models.ioalarmmodel;
using stay_calm_db_tests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class ioalarmsController : Controller
    {
        private userContext db = new userContext();

        public ActionResult ioalarm_history(string site)
        {
                  ioalarm_visualdisplay ioalarmVisualdisplay = new ioalarm_visualdisplay();
                  ioalarmVisualdisplay.alarm_names = new List<string>();
                  ioalarmVisualdisplay.site_all = new List<string>();
                  ioalarmVisualdisplay.site_id = new List<string>();
                  ioalarmVisualdisplay.alarm_ids = new List<string>();

                    var all_sites = (from a in db.siteuser
                                     select new
                                     {
                                         a.user_site,
                                         a.Id
                                     }).ToList();

                    if (site == null)
                    {
                       site = all_sites.Select(x => x.Id).FirstOrDefault().ToString();
                    }

                     int count = 0;

                    foreach (var item in all_sites)
                    {

                        ioalarmVisualdisplay.site_all.Add(item.user_site);
                        ioalarmVisualdisplay.site_id.Add(item.Id.ToString());


                        if (site == item.Id.ToString())
                        {
                            ViewBag.site_selected = count;
                        }
                        else
                        { count = count + 1; }


                    }

                 var alarm_list = db.io_alarms.Where(y => y.siteuser.Id.ToString() == site).ToList();
                  
                  
                  if (alarm_list.Count() == 0)
                  {
                    ioalarmVisualdisplay.alarm_names.Add("This site contains no Alarms");
                    ioalarmVisualdisplay.alarm_ids.Add("null");
                  }
                  else
                  {
                    foreach (var items in alarm_list)
                    {
                      ioalarmVisualdisplay.alarm_names.Add(items.alarm_name);
                      string str = items.switch_number.ToString();
                      ioalarmVisualdisplay.alarm_ids.Add(str);
                    }
                  }

                String today = DateTime.UtcNow.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                String yesterday = DateTime.UtcNow.AddDays(-1.0).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);




                ViewBag.yesterday = yesterday;
                ViewBag.today = today;

               return View(ioalarmVisualdisplay);
          }


        public PartialViewResult ioalarm_data(string[] options, string sitename, string select_from, string to_select)
        {
            ioalarm_selection ioalarmSelection = new ioalarm_selection();
            ioalarmSelection.selected_num = new List<string>();
            ioalarmSelection.selected_name = new List<string>();
            ioalarmSelection.selected_for_id = new List<string>();
            ioalarmSelection.sitename = new List<string>();
            ioalarmSelection.data_from = new List<string>();
            ioalarmSelection.data_to = new List<string>();

            int site_int;
            int.TryParse(sitename, out site_int);

            ioalarmSelection.sitename.Add(sitename);
            ioalarmSelection.data_from.Add(select_from);
            ioalarmSelection.data_to.Add(to_select);

            var list = db.io_alarms.Where(x => x.siteuser.Id == site_int).Select(x => new
            {
                switch_number = x.switch_number,
                alarm_name = x.alarm_name
            }).ToList();

            for (int index = 0; index < options.Length; ++index)
            {
                ioalarmSelection.selected_num.Add(options[index]);
                ioalarmSelection.selected_for_id.Add("alarm" + options[index]);

                foreach (var data in list)
                {
                    if (data.switch_number.ToString() == options[index])
                        ioalarmSelection.selected_name.Add(data.alarm_name);
                }
            }
            return PartialView(ioalarmSelection);
        }


        // loadingids = Alarm + option[index]  e.g. alarm1
        // alarm_name =  alarm name  e.g. sw1
        // Sitename = Site name 
        // options = number = sw1 = 1 

        public PartialViewResult ioalarm_plot(string options, string from, string to, string sitename, string loadingids, string alarm_name)
        {
            ioalarm_plotmodel ioalarmPlotmodel = new ioalarm_plotmodel();
            ioalarmPlotmodel.selected_num = new List<string>();
            ioalarmPlotmodel.year_graph = new List<long>();
            ioalarmPlotmodel.year_last_date = new List<string>();
            ioalarmPlotmodel.value_of_graph = new List<double>();

            ioalarmPlotmodel.graph_min = new List<string>();
            ioalarmPlotmodel.graph_max = new List<string>();
            ioalarmPlotmodel.graph_avg = new List<string>();

            ioalarmPlotmodel.graph_data_date = new List<string>();
            ioalarmPlotmodel.graph_data_late = new List<string>();
            ioalarmPlotmodel.alarm_graph_store = new List<string>();
            ioalarmPlotmodel.sitename = new List<string>();
            ioalarmPlotmodel.data_from = new List<string>();
            ioalarmPlotmodel.data_to = new List<string>();

            alarm_name = alarm_name.Replace(" ", "_");

            ViewBag.title = alarm_name;

            ViewBag.name = "ele" + options;

            ioalarmPlotmodel.selected_num.Add(options);

            int switch_number = 0;
            int.TryParse(options, out switch_number);

            DateTime date_from = DateTime.Parse(from).AddHours(-11);
            DateTime date_to = DateTime.Parse(to).AddDays(1);

           
            String  Gmt_convert = "AUS Eastern Standard Time";

            ViewBag.timezone = Gmt_convert;
            ViewBag.datefrom = date_from;
            ViewBag.dateto = date_to;


            string binary = "";
            string bin_string = "";
            int binary_form = 0;
            int binary_value = 0;

            /* var get_data_points = (from x in db.io_alarms_value
                                    where x.siteuser.user_site == sitename && (x.date >= date_from && x.date <= date_to)
                                    select x).ToList();
            */
            int site_int;
            int.TryParse(sitename, out site_int);

            var get_data_points = (from x in db.io_alarms_value
                                   where x.siteuser.Id == site_int
                                   select x).ToList();



            var sorting_dates = get_data_points.OrderBy(s => s.date);

            var final_data = sorting_dates.Where(x => x.date >= date_from && x.date <= date_to).ToList();
            


            if (final_data.Count() == 0)
            {
                return PartialView();
            }
            else { 

                    foreach (var item in final_data)
                   {

                    var est = TimeZoneInfo.FindSystemTimeZoneById(Gmt_convert);
                    var targetTime = TimeZoneInfo.ConvertTime(item.date, est);

                    ioalarmPlotmodel.alarm_graph_store.Add(targetTime.ToString());

                    // If there is a comma 
                    string first_bin;
                    string second_bin;

                    if (item.value.Contains(','))
                    {
                        string[] brk = item.value.Split(',');
                        first_bin = brk[0];
                        second_bin = brk[1];
                    }
                    else
                    {
                        first_bin = item.value;
                        second_bin = "0";
                    }

                    // if first alarm = 1, then 8-1 = 7 
                    // if eight alarm = 8, then 8-8 = 0

                    if (switch_number > 8)
                    {
                        int.TryParse(second_bin, out binary_form);
                        binary = Convert.ToString(binary_form, 2).PadLeft(8, '0');
                        bin_string = binary[16 - switch_number].ToString();

                        int.TryParse(bin_string, out binary_value);
                        ioalarmPlotmodel.value_of_graph.Add(binary_value);
                    }
                    else
                    {
                        int.TryParse(first_bin, out binary_form);
                        binary = Convert.ToString(binary_form, 2).PadLeft(8, '0');
                        bin_string = binary[8 - switch_number].ToString();

                        int.TryParse(bin_string, out binary_value);
                        ioalarmPlotmodel.value_of_graph.Add(binary_value);
                    }




                    var time_convert_y = targetTime.ToString("yyyy");
                    int year = Convert.ToInt32(time_convert_y);

                    var time_convert_m = targetTime.ToString("MM");
                    int month = Convert.ToInt32(time_convert_m);

                    var time_convert_d = targetTime.ToString("dd");
                    int day = Convert.ToInt32(time_convert_d);

                    var time_convert_h = targetTime.ToString("HH");
                    int hour = Convert.ToInt32(time_convert_h);

                    var time_convert_min = targetTime.ToString("mm");
                    int min = Convert.ToInt32(time_convert_min);

                    var time_convert_sec = targetTime.ToString("ss");
                    int sec = Convert.ToInt32(time_convert_sec);



                    DateTime epoch = new DateTime(year, month, day, hour, min, sec, DateTimeKind.Unspecified);
                    long final_date = (long)epoch.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

                    ioalarmPlotmodel.year_graph.Add(final_date);

                }

                var highest_data = ioalarmPlotmodel.value_of_graph.Max().ToString();
                var lowest_data = ioalarmPlotmodel.value_of_graph.Min().ToString();
                var average_date = ioalarmPlotmodel.value_of_graph.Average().ToString("0.000");

                var last_data = ioalarmPlotmodel.value_of_graph.Last().ToString();

                var last_date = ioalarmPlotmodel.alarm_graph_store.Last().ToString();

                ioalarmPlotmodel.graph_min.Add(lowest_data);
                ioalarmPlotmodel.graph_max.Add(highest_data);
                ioalarmPlotmodel.graph_avg.Add(average_date);

                ioalarmPlotmodel.graph_data_date.Add(last_data);
                ioalarmPlotmodel.graph_data_late.Add(last_date);
                

            }





                return PartialView(ioalarmPlotmodel);
           }






        }
}