using newtest.Models.site_history;
using stay_calm_db_tests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class HistorySiteController : Controller
    {

        public ActionResult site_data()
        {
            return View();
        }

        public ActionResult testing()
        {
            graph_partial_test graphPartialTest = new graph_partial_test();
            graphPartialTest.selected_num = new List<string>();

            graphPartialTest.selected_num.Add("hello");
            return View(graphPartialTest);
        }

        private userContext db = new userContext();

        // HttpContext.Server.ScriptTimeout = 300;
        

        public ActionResult site_data_test(string sitename)
        {
           

            regular_site_proof regularSiteProof = new regular_site_proof();
            regularSiteProof.sites = new List<string>();
            regularSiteProof.site_id = new List<string>();
            regularSiteProof.graph_data_names = new List<string>();
            regularSiteProof.graph_option_selected_id = new List<string>();


            
            // Pull data from all the Sites
            var all_sites = (from a in db.siteuser
                             select new {
                                 a.user_site,
                                 a.Id
                             }).ToList();

            

            if (sitename == null)
            {
                sitename = all_sites.Select(x => x.Id).FirstOrDefault().ToString();
            }
            
            // All data types for the site selected
            var data_type = (from x in db.graph_limit
                             where x.siteuser.Id.ToString() == sitename
                             select new
                             {
                                 x.graph_data_type.data_type,
                                 x.graph_data_type.serial_id
                             }).ToList();

            int count = 0;

            // Storing all the Sites and Site ID into LIST<String>
            foreach (var item in all_sites)
            {

                regularSiteProof.sites.Add(item.user_site);
                regularSiteProof.site_id.Add(item.Id.ToString());

                
                    if (sitename == item.Id.ToString())
                    {
                        ViewBag.site_selected = count;
                    }
                    else
                    { count = count + 1; }
                

            }

            // Storing all the Data type for the chosen Site ID 
            foreach (var item in data_type)
            { 
                regularSiteProof.graph_data_names.Add(item.data_type);
                regularSiteProof.graph_option_selected_id.Add(item.serial_id.ToString());
            }

            String today = DateTime.UtcNow.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            String yesterday = DateTime.UtcNow.AddDays(-1.0).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
           



            ViewBag.yesterday = yesterday;
            ViewBag.today = today;
           



            return View(regularSiteProof);
        }



        public PartialViewResult graph_site_data(string[] options, string select_from, string to_select, string sitename)
        {
            graph_partial_test graphPartialTest = new graph_partial_test();
            graphPartialTest.selected_num = new List<string>();
            graphPartialTest.selected_name = new List<string>();
            graphPartialTest.sitename = new List<string>();
            graphPartialTest.data_from = new List<string>();
            graphPartialTest.data_to = new List<string>();
            

            graphPartialTest.sitename.Add(sitename);
            graphPartialTest.data_from.Add(select_from);
            graphPartialTest.data_to.Add(to_select);


            var selected_names = db.graph_data_type.ToList();

            
            for(int i = 0; i < options.Length; i++)
            {
                graphPartialTest.selected_num.Add(options[i]);

                foreach(var item in selected_names)
                {
                    //== opertator is to check identity. (i.e: a==b are these two are the same object?)
                    //.Equals() is to check value. (i.e: a.Equals(b) are both holding identical values ?)

                    if (item.serial_id.ToString().Equals(options[i]))
                    {
                        graphPartialTest.selected_name.Add(item.data_type);
                    }
                }


            }
            
            return PartialView(graphPartialTest);
        }

        public PartialViewResult graph_plot(string options, string from, string to, string sitename)
        {

            


            graph_partial_test graphmodel = new graph_partial_test();
            graphmodel.selected_num = new List<string>();
            graphmodel.year_graph = new List<long>();
            graphmodel.year_last_date = new List<string>();
            graphmodel.value_of_graph = new List<double?>();
            graphmodel.graph_min = new List<string>();
            graphmodel.graph_max = new List<string>();
            graphmodel.graph_avg = new List<string>();
            graphmodel.graph_data_date = new List<string>();
            graphmodel.graph_data_late = new List<string>();

            // Options = Selected Number of Data type 
            // From = date from 
            // To = Date to
            // sitename = site name 


            graphmodel.selected_num.Add(options);
            ViewBag.name = "ele" + options;
            
            string str1 = "";

            TimeZoneInfo systemTimeZoneById = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            DateTime date_from = DateTime.Parse(from);
            DateTime date_to = DateTime.Parse(to).AddDays(1.0);

            ViewBag.timezone = "AUS Eastern Standard Time";
            ViewBag.datefrom = from;
            ViewBag.dateto = to;


            int result;
            int.TryParse(options, out result);
            int serial_to_int;
            int.TryParse(sitename, out serial_to_int);

            var serial_number = (from x in db.siteuser
                                 where x.Id == serial_to_int
                                 select x.sitetoboards.serialboard).FirstOrDefault();

            if (serial_number.Count() == 0)
            {
                return PartialView();
            }

            if (result == 17)
            {

                var list = (from x in db.graph_update
                                  where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                                  select new {
                                      x.plotdate,
                                      x.BI_Temperature
                                  }).ToList();

               
                if (list.Count() == 0)
                {

                    return PartialView();
                }
                foreach (var data in list)
                {
                    if (data.BI_Temperature.HasValue)
                    {
                        addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.BI_Temperature, graphmodel);
                    }

                }
                str1 = "BI Temperature";
            }

            else if (result == 137)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    TempProbeInt = x.TempProbeInt
                }).ToList();

                if (list.Count() == 0)
                {
                    return PartialView();
                }

                foreach (var data in list)
                {
                    if (data.TempProbeInt.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.TempProbeInt, graphmodel);
                }
                str1 = "TempProbeInt";
            }
            else if (result == 20)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                 {
                    plotdate = x.plotdate,
                    HG_Value_of_Flow_met = x.HG_Value_of_Flow_met
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Value_of_Flow_met.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Value_of_Flow_met, graphmodel);
                }
                str1 = "HG Value of Flow met";
            }
            else if (result == 21)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    BI_Chlorine = x.BI_Chlorine
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.BI_Chlorine.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.BI_Chlorine, graphmodel);
                }
                str1 = "BI Chlorine";
            }
            else if (result == 22)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    BI_pH = x.BI_pH
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.BI_pH.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.BI_pH, graphmodel);
                }
                str1 = "BI pH";
            }
            else if (result == 23)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_ORP = x.HG_ORP
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_ORP.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_ORP, graphmodel);
                }
                str1 = "HG ORP";
            }
            else if (result == 24)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Turbidity = x.HG_Turbidity
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Turbidity.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Turbidity, graphmodel);
                }
                str1 = "HG Turbidity";
            }
            else if (result == 31)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_PH_P_factor = x.HG_PH_P_factor
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_PH_P_factor.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_PH_P_factor, graphmodel);
                }
                str1 = "HG PH P factor";
            }
            else if (result == 32)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_PH_pump_period = x.HG_PH_pump_period
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_PH_pump_period.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_PH_pump_period, graphmodel);
                }
                str1 = "HG PH pump period";
            }
            else if (result == 33)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    BI_Total_Chlorine = x.BI_Total_Chlorine
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.BI_Total_Chlorine.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.BI_Total_Chlorine, graphmodel);
                }
                str1 = "BI Total Chlorine";
            }
            else if (result == 34)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Total_Chlorine_Ra = x.HG_Total_Chlorine_Ra
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Total_Chlorine_Ra.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Total_Chlorine_Ra, graphmodel);
                }
                str1 = "HG Total Chlorine Ra";
            }
            else if (result == 38)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Comm_Chlorine_Co = x.HG_Comm_Chlorine_Co
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Comm_Chlorine_Co.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Comm_Chlorine_Co, graphmodel);
                }
                str1 = "HG Comm Chlorine Co";
            }
            else if (result == 48)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Combine_chlorine = x.HG_Combine_chlorine
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Combine_chlorine.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Combine_chlorine, graphmodel);
                }
                str1 = "HG Combine chlorine";
            }
            else if (result == 59)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Conductivity_from = x.HG_Conductivity_from
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Conductivity_from.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Conductivity_from, graphmodel);
                }
                str1 = "HG Conductivity from";
            }
            else if (result == 60)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Input_2_from_NTU = x.HG_Input_2_from_NTU
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Input_2_from_NTU.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Input_2_from_NTU, graphmodel);
                }
                str1 = "HG Input 2 from NTU";
            }
            else if (result == 61)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Input_3_from_NTU = x.HG_Input_3_from_NTU
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Input_3_from_NTU.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Input_3_from_NTU, graphmodel);
                }
                str1 = "HG Input 3 from NTU";
            }
            else if (result == 75)
            {

                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Temperature_calib = x.HG_Temperature_calib
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Temperature_calib.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Temperature_calib, graphmodel);
                }
                str1 = "HG Temperature calib";
            }
            else if (result == 113)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_Free_CL_Feed_Rate = x.HG_Free_CL_Feed_Rate
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_Free_CL_Feed_Rate.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_Free_CL_Feed_Rate, graphmodel);
                }
                str1 = "HG Free CL Feed Rate";
            }
            else if (result == 114)
            {

                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    HG_pH_Feed_Rate = x.HG_pH_Feed_Rate
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.HG_pH_Feed_Rate.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.HG_pH_Feed_Rate, graphmodel);
                }
                str1 = "HG Free CL Feed Rate";
            }
            else if (result == 136)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temp_Probe_External = x.Temp_Probe_External
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temp_Probe_External.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temp_Probe_External, graphmodel);
                }
                str1 = "Temp Probe External";
            }
            else if (result == 138)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Total_Alkalinity = x.Total_Alkalinity
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Total_Alkalinity.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Total_Alkalinity, graphmodel);
                }
                str1 = "Total Alkalinity";
            }
            else if (result == 139)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Calcium_Hardness = x.Calcium_Hardness
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Calcium_Hardness.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Calcium_Hardness, graphmodel);
                }
                str1 = "Calcium Hardness";
            }
            else if (result == 141)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Conductivity_1 = x.Conductivity_1
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Conductivity_1.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Conductivity_1, graphmodel);
                }
                str1 = "Conductivity 1";
            }
            else if (result == 140)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    WL_Conductivity_mScm = x.WL_Conductivity_mScm
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.WL_Conductivity_mScm.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.WL_Conductivity_mScm, graphmodel);
                }
                str1 = "WL Conductivity mScm";
            }
            else if (result == 142)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Conductivity_3 = x.Conductivity_3
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Conductivity_3.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Conductivity_3, graphmodel);
                }
                str1 = "Conductivity 3";
            }
            else if (result == 143)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Conductivity_4 = x.Conductivity_4
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Conductivity_4.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Conductivity_4, graphmodel);
                }
                str1 = "Conductivity 4";
            }
            else if (result == 144)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Total_Dissolved_Solids = x.Total_Dissolved_Solids
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Total_Dissolved_Solids.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Total_Dissolved_Solids, graphmodel);
                }
                str1 = "Total Dissolved Solids";
            }
            else if (result == 145)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Phosphate = x.Phosphate
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Phosphate.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Phosphate, graphmodel);
                }
                str1 = "Phosphate";
            }
            else if (result == 146)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to) && x.Chlorinator_Average!= null
                            select new
                            {
                    plotdate = x.plotdate,
                    Chlorinator_Average = x.Chlorinator_Average
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Chlorinator_Average.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Chlorinator_Average, graphmodel);
                }
                str1 = "Chlorinator Average";
            }
            else if (result == 147)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Cyanuric_Acid = x.Cyanuric_Acid
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Cyanuric_Acid.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Cyanuric_Acid, graphmodel);
                }
                str1 = "Cyanuric Acid";
            }
            else if (result == 148)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Router_Reboot = x.Router_Reboot
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Router_Reboot.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Router_Reboot, graphmodel);
                }
                str1 = "Router Reboot";
            }
            else if (result == 149)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Board_Reboot = x.Board_Reboot
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Board_Reboot.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Board_Reboot, graphmodel);
                }
                str1 = "Board Reboot";
            }
            else if (result == 150)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Sim900_Signal_Strength = x.Sim900_Signal_Strength
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Sim900_Signal_Strength.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Sim900_Signal_Strength, graphmodel);
                }
                str1 = "Sim900 Signal Strength";
            }
            else if (result == 151)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Board_Start = x.Board_Start
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Board_Start.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Board_Start, graphmodel);
                }
                str1 = "Board Start";
            }
            else if (result == 1)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Chlorine_set_Point_1 = x.Chlorine_set_Point_1
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Chlorine_set_Point_1.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Chlorine_set_Point_1, graphmodel);
                }
                str1 = "Chlorine set Point 1";
            }
            else if (result == 3)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Chlorine_Low_Alarm = x.Chlorine_Low_Alarm
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Chlorine_Low_Alarm.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Chlorine_Low_Alarm, graphmodel);
                }
                str1 = "Chlorine Low Alarm";
            }
            else if (result == 4)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Chlorine_High_Alarm = x.Chlorine_High_Alarm
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Chlorine_High_Alarm.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Chlorine_High_Alarm, graphmodel);
                }
                str1 = "Chlorine High Alarm";
            }
            else if (result == 5)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Ph_set_Point = x.Ph_set_Point
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Ph_set_Point.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Ph_set_Point, graphmodel);
                }
                str1 = "Ph set Point";
            }
            else if (result == 6)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Ph_Low_Alarm = x.Ph_Low_Alarm
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Ph_Low_Alarm.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Ph_Low_Alarm, graphmodel);
                }
                str1 = "Ph Low Alarm";
            }
            else if (result == 7)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Ph_High_Alarm = x.Ph_High_Alarm
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Ph_High_Alarm.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Ph_High_Alarm, graphmodel);
                }
                str1 = "Ph High Alarm";
            }
            else if (result == 26)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    ID_number = x.ID_number
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.ID_number.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.ID_number, graphmodel);
                }
                str1 = "ID number";
            }
            else if (result == 39)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Low_Reagents = x.Low_Reagents
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Low_Reagents.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Low_Reagents, graphmodel);
                }
                str1 = "Low Reagents";
            }
            else if (result == 44)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temperature_Low_Alarm = x.Temperature_Low_Alarm
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temperature_Low_Alarm.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temperature_Low_Alarm, graphmodel);
                }
                str1 = "Temperature Low Alarm";
            }
            else if (result == 45)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temperature_High_Alarm = x.Temperature_High_Alarm
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temperature_High_Alarm.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temperature_High_Alarm, graphmodel);
                }
                str1 = "Temperature High Alarm";
            }
            else if (result == 46)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temperature_set_Point = x.Temperature_set_Point
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temperature_set_Point.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temperature_set_Point, graphmodel);
                }
                str1 = "Temperature set Point";
            }
            else if (result == 54)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Type_of_controller = x.Type_of_controller
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Type_of_controller.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Type_of_controller, graphmodel);
                }
                str1 = "Type of controller";
            }
            else if (result == 62)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Version_number = x.Version_number
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Version_number.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Version_number, graphmodel);
                }
                str1 = "Version number";
            }
            else if (result == 63)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Protocol_version = x.Protocol_version
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Protocol_version.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Protocol_version, graphmodel);
                }
                str1 = "Protocol version";
            }
            else if (result == 40)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    ORP_set_Point_1 = x.ORP_set_Point_1
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.ORP_set_Point_1.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.ORP_set_Point_1, graphmodel);
                }
                str1 = "ORP set Point 1";
            }
            else if (result == 41)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    ORP_set_Point_2 = x.ORP_set_Point_2
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.ORP_set_Point_2.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.ORP_set_Point_2, graphmodel);
                }
                str1 = "ORP set Point 2";
            }
            else if (result == 153)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temp_sensor_1 = x.Temp_sensor_1
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temp_sensor_1.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temp_sensor_1, graphmodel);
                }
                str1 = "Temp Sensor 1";
            }
            else if (result == 154)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temp_sensor_2 = x.Temp_sensor_2
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temp_sensor_2.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temp_sensor_2, graphmodel);
                }
                str1 = "Temp Sensor 2";
            }
            else if (result == 155)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temp_sensor_3 = x.Temp_sensor_3
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temp_sensor_3.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temp_sensor_3, graphmodel);
                }
                str1 = "Temp Sensor 3";
            }
            else if (result == 156)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temp_sensor_4 = x.Temp_sensor_4
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temp_sensor_4.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temp_sensor_4, graphmodel);
                }
                str1 = "Temp Sensor 4";
            }
            else if (result == 157)
            {
                var list = (from x in db.graph_update
                            where x.serial == serial_number && (x.plotdate >= date_from && x.plotdate <= date_to)
                            select new
                            {
                    plotdate = x.plotdate,
                    Temp_sensor_5 = x.Temp_sensor_5
                }).ToList();
                if (list.Count() == 0)
                    return this.PartialView();
                foreach (var data in list)
                {
                    if (data.Temp_sensor_5.HasValue)
                        this.addtomodelfunc(TimeZoneInfo.ConvertTime(data.plotdate, systemTimeZoneById), data.Temp_sensor_5, graphmodel);
                }
                str1 = "Temp Sensor 5";
            }
            
            ViewBag.title = str1;

            if (graphmodel.value_of_graph.Count() == 0)
            {
                return PartialView();
            }

            string str2 = graphmodel.value_of_graph.Max().ToString();
            
            string str3 = graphmodel.value_of_graph.Min().ToString();
            
            string str4 = graphmodel.value_of_graph.Average().ToString();
         
            string str5 = graphmodel.value_of_graph.Last().ToString();

            string str6 = graphmodel.year_last_date.Last();

            graphmodel.graph_min.Add(str3);
            graphmodel.graph_max.Add(str2);
            graphmodel.graph_avg.Add(str4);
            graphmodel.graph_data_late.Add(str5);
            graphmodel.graph_data_date.Add(str6);


            return PartialView(graphmodel);
        }

        public void addtomodelfunc(DateTime date, double? value, graph_partial_test graphmodel)
        {
            graphmodel.year_last_date.Add(date.ToString());
            long totalMilliseconds = (long)new DateTime(Convert.ToInt32(date.ToString("yyyy")), Convert.ToInt32(date.ToString("MM")), Convert.ToInt32(date.ToString("dd")), Convert.ToInt32(date.ToString("HH")), Convert.ToInt32(date.ToString("mm")), Convert.ToInt32(date.ToString("ss")), DateTimeKind.Unspecified).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            graphmodel.year_graph.Add(totalMilliseconds);
            graphmodel.value_of_graph.Add(value);
        }


    }
}