using SendGrid;
using stay_calm_db_comms;
using stay_calm_db_tests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace elementalelec.controller
{
    public class sv01Controller : Controller
    {
        
        public async Task<ActionResult> get(String mserial, String mversion, String chparam, String oldsddata, String trantype, String transubtype, String hgparam, String hgstatus, String idvalues, String mdnt)
        {
            // EFConfiguration.SuspendExecutionStrategy = true;

            if (chparam != null)
            {
                userContext test = new userContext();
                var chserial = test.chparamdbs.Where(x => x.sitetoboards.serialboard == mserial).FirstOrDefault();


                if (chserial != null)
                {
                    String xparams_str = chserial.xparams.ToString();
                    String yparams_str = chserial.yparams.ToString();

                    String zparams_str = chserial.zparams.ToString();
                    String aparams_str = chserial.aparams.ToString();

                    ViewBag.is_found = "EEstatus=" + 1;
                    ViewBag.chparms = "," + xparams_str + "," + yparams_str + "," + zparams_str + "," + aparams_str + ",";
                }
                else
                {
                    ViewBag.is_found = "EEstatus=0";
                    return View();
                }



                return View();
            }

            try
                {
                String utc = DateTime.UtcNow.ToString("yyyy,MM,dd,HH,mm,ss", CultureInfo.InvariantCulture);

                string newmversion = "";
                    int character = 0;

                    foreach (char c in mversion)
                    {


                        if (character == 1)
                        {

                            newmversion = newmversion + "0";

                        }

                        else
                        {
                            newmversion = newmversion + c;
                        }

                        character = character + 1;
                    }

                    

                    //   String url = "ttp://elementalelec.azurewebsites.net/relax/load?ph=0&ch=" + idvalues_id + "&temp=" + idvalues_value + "&dateplot=" + dateformat + "&serialsite=" + mserial;
                    // ttp://elementalelec.azurewebsites.net/sv01/get?mserial=530173&mversion=10413&trantype=HG&transubtype=status&hgstatus=0,0,0,0,0,0,0,68,69,1,0,0,40&mdnt=2014,5,5,1,59,53

                    String url = "";

                    if (hgparam != null)
                    {
                        if (oldsddata != null)
                        {
                        
                            url = "http://svr01.cpsys.com.au/receiver.php?mserial=" + mserial + "&mversion=" + newmversion
                           + "&oldsddata=" + oldsddata + "&trantype=" + trantype
                           + "&transubtype=" + transubtype + "&hgparam=" + hgparam + "&mdnt=" + utc;

                        }

                        else
                        {
                            url = "http://svr01.cpsys.com.au/receiver.php?mserial=" + mserial + "&mversion=" + newmversion + "&trantype=" + trantype
                            + "&transubtype=" + transubtype + "&hgparam=" + hgparam + "&mdnt=" + utc;
                        }
                    }
                    else
                    {
                        if (idvalues != null && trantype == "AC")
                        {
                        // Must be able to store 255,255 rather than 255,0
                        //ttp://elementalelec.azurewebsites.net/sv01/get?mserial=0890CA&mversion=10414&trantype=AC&idvalues=255
                        //ttp://svr01.cpsys.com.au/receiver.php?mserial=0890CA&mversion=10414&trantype=AC&idvalues=255,0

                        // edited IO ALARM value for 255,0
                        string edit_idvalues = null;

                        // Checking each character for errors
                        foreach (char c in idvalues)
                            {
                                if (c < '0' || c > '9')
                                {
                                    if (c == ',')
                                    {
                                        // Changing the ID values to 255,0 and 255,255 to 255,0
                                        string[] chan = idvalues.Split(',');
                                        edit_idvalues = chan[0] + ",0";

                                    }
                                    else
                                    {
                                        ViewBag.is_found = 0;
                                        return View();
                                    }
                                }

                            }

                            if (edit_idvalues == null)
                            {
                                edit_idvalues = idvalues + ",0";
                            }

                        //  idvalues = idvalues + ",0";
                        url = "http://svr01.cpsys.com.au/receiver.php?mserial=" + mserial + "&mversion=" + newmversion + "&trantype=" + trantype
                                 + "&idvalues=" + edit_idvalues + "&mdnt=" + utc; 

                        }

                        else if (hgstatus != null)
                        {
                            url = "http://svr01.cpsys.com.au/receiver.php?mserial=" + mserial + "&mversion=" + newmversion + "&trantype=" + trantype
                            + "&transubtype=" + transubtype + "&hgstatus=" + hgstatus + "&mdnt=" + utc;

                        }

                        else if (idvalues != null && trantype != "AC")
                        {
                            string str_re_do = "";
                            string addition = "";
                            string break_idval = idvalues;

                            string four_one = "";
                            string four_zero = "";

                            var primeArray = break_idval.Split(',');

                            for (int i = 0; i < primeArray.Length; i++)
                            {

                                string idvalues_id = break_idval.Split(',')[i];
                                string idvalues_value = break_idval.Split(',')[i + 1];

                             if(idvalues_id == "140")
                             {
                                double multi;
                                double.TryParse(idvalues_value, out multi);

                                  // If the serial number DOES NOT EQUAL "B46567" 
                                   if (mserial == "B46567")
                                   {
                                      // Do nothing 
                                   }
                                    
                                   else if(mserial == "0890CA")
                                    {
                                      // Do nothing 
                                    }
    
                                     else {

                                        multi = multi / 10;

                                        idvalues_value = multi.ToString("0.##");
                                    }

                                     four_zero = idvalues_value;

                                 }

                         
                               else if (idvalues_id == "141")
                                {
                                   double multi;
                                   double.TryParse(idvalues_value, out multi);

                                    multi = multi * 10;

                                    idvalues_value = multi.ToString("0.##");

                                    four_one = idvalues_value;
                                }
                                else
                                {
                                    addition = idvalues_id + "," + idvalues_value + ",";
                                    str_re_do = addition + str_re_do;
                                }


                                i = i + 1;
                            }


                            // 141 = conductivity ms/cm    140 = UV
                            str_re_do = str_re_do + "140," + four_one + "," + "141," + four_zero;

                            if (oldsddata != null)
                            {

                                //String utc = DateTime.UtcNow.ToString("yyyy,MM,dd,HH,mm,ss", CultureInfo.InvariantCulture);

                                url = "http://svr01.cpsys.com.au/receiver.php?mserial=" + mserial + "&mversion=" + newmversion
                                       + "&oldsddata=" + oldsddata + "&trantype=" + trantype
                                       + "&transubtype=" + transubtype + "&idvalues=" + str_re_do + "&mdnt=" + utc;
                            }
                            else
                            {
                                url = "http://svr01.cpsys.com.au/receiver.php?mserial=" + mserial + "&mversion=" + newmversion + "&trantype=" + trantype
                                 + "&transubtype=" + transubtype + "&idvalues=" + str_re_do + "&mdnt=" + utc;
                            }
                        }


                    }


                   //"Fire and forget" is only an acceptable approach if you don't care whether http actually completes.
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                   // myRequest.Method = "POST";
                   //Queues a method for execution. The method executes when a thread pool thread becomes available.
                    ThreadPool.QueueUserWorkItem(o => { myRequest.GetResponse(); });


                }
                catch (Exception ex)
                {
                    ViewBag.is_found = "EEstatus=1";
                    return View();
                }

            userContext db = new userContext();

            if (mdnt == null)
            {
                mdnt = DateTime.UtcNow.ToString("yyyy,MM,dd,HH,mm,ss", CultureInfo.InvariantCulture);
            }

            String break_date;
            String[] date_value;
            String year;
            String month;
            String day;
            String hour;
            String minute;
            String second;

            try
            {
                // Splitting the dates 
                break_date = mdnt;
                date_value = break_date.Split(',');


                year = date_value[0];
                month = date_value[1];
                day = date_value[2];
                hour = date_value[3];
                minute = date_value[4];
                second = date_value[5];

            }
            catch (Exception ex)
            {
                ViewBag.is_found = 0;
                return View();
            }


            // Check if year is valid 
            if (year.Count() != 4)
            {
                ViewBag.is_found = 0;
                return View();

            }


            String dateformat = month + "/" + day + "/" + year + " " + hour
                 + ":" + minute + ":" + second;

            DateTime date;

            // Check if date is valid 
            if (DateTime.TryParse(dateformat, out date))
            {   // Successful
            }

            else
            {
                ViewBag.is_found = 0;
                return View();
            }


  

            // Store in last seen 
            var last_seen = await db.sitetoboards.Where(x => x.serialboard == mserial).FirstOrDefaultAsync();

            if (last_seen != null)
            {
                last_seen.lastseen = date;
                last_seen.serialboard = mserial;
                db.Entry(last_seen).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else
            {
                ViewBag.is_found = 0;
                return View();
            }


            // Not using response at all  
           /* var response = await (from x in db.response_comms
                                  where x.sitetoboards.serialboard == mserial
                                  select new
                                  {
                                      x.once_on_server
                                  }).FirstOrDefaultAsync();
           */

                // I/O Alarms 
                if (trantype == "AC")
                {
                    var alarm_find = await (from x in db.io_alarms
                                            where x.siteuser.sitetoboards.serialboard == mserial
                                            select x.siteuser.Id).FirstOrDefaultAsync();

                    if (alarm_find.Equals(0))
                    {
                        ViewBag.is_found = 0;
                        return View();
                    }

                    if (idvalues == null)
                    {
                        ViewBag.is_found = 0;
                        return View();
                    }

                    io_alarms_value alarm_v = new io_alarms_value();

                    alarm_v.alarm_link_id = alarm_find;
                    alarm_v.date = date;
                    alarm_v.value = idvalues;

                    db.io_alarms_value.Add(alarm_v);
                    await db.SaveChangesAsync();

                    ViewBag.is_found = "EEstatus=" + 1;
                }

                if (transubtype != null)
                {
                    // HG status
                    if (trantype == "HG" && transubtype == "status")
                    {

                        if (hgstatus == null)
                        {
                            ViewBag.is_found = 0;
                            return View();
                        }


                        // Check for any random characters 
                        foreach (char c in hgstatus)
                        {
                            if (c != ',' && (c < '0' || c > '9'))
                            {
                                ViewBag.is_found = 0;
                                return View();
                            }

                        }

                        string break_hgstat = "";
                        break_hgstat = hgstatus;
                        string status_break = "";
                        int com = 0;
                        hgstatus_db db_hg_status = new hgstatus_db();

                        hgstatus_email_class get_ones = new hgstatus_email_class();
                        get_ones.statusid = new List<int>();


                        for (int i = 0; i < 13; i++)
                        {
                            status_break = break_hgstat.Split(',')[i];

                            //byte[] bytes = new byte[status_break.Length * sizeof(char)];
                            //System.Buffer.BlockCopy(status_break.ToCharArray(), 0, bytes, 0, bytes.Length);

                            com = int.Parse(status_break);


                            //ttp://stackoverflow.com/questions/23905188/convert-an-integer-to-a-binary-string-with-leading-zeros
                            // This look at every single byte 
                            string binary = Convert.ToString(com, 2).PadLeft(8, '0');


                            int digit_int = 0;
                            string digit;

                            if (i == 0)
                            {
                                // Do Acid- base pump 
                                digit = binary[5].ToString();
                                Int32.TryParse(digit, out digit_int);
                                db_hg_status.fourb_byte_acid_base_pump = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(3);
                                }



                                // Chlorine addition pump
                                digit = binary[6].ToString();
                                Int32.TryParse(digit, out digit_int);
                                db_hg_status.fourb_ch_addition_pump = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(2);
                                }





                                // Chlorine Main pump
                                digit = binary[7].ToString();
                                Int32.TryParse(digit, out digit_int);
                                db_hg_status.fourb_ch_main_pump = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(1);
                                }

                            }

                            // i == 1  Not in use ! 

                            if (i == 2)
                            {

                                digit = binary[0].ToString();

                                Int32.TryParse(digit, out digit_int);
                                db_hg_status.sixb_low_reagent = digit_int;
                                // Low Reagent , if on/off

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(4);
                                }


                            }

                            if (i == 3)
                            {

                                digit = binary[5].ToString();
                                Int32.TryParse(digit, out digit_int);
                                db_hg_status.sevenb_flow_meter = digit_int;
                                // Flow-meter

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(5);
                                }


                            }

                            if (i == 4)
                            {
                                // incorrect password
                                digit = binary[5].ToString();
                                Int32.TryParse(digit, out digit_int);
                                db_hg_status.eightb_incorrect_pass = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(6);
                                }



                                //Parameter out of range 
                                digit = binary[6].ToString();
                                Int32.TryParse(digit, out digit_int);
                                db_hg_status.eightb_parameter_out_of_range = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(7);
                                }




                                //Amount of parameters over the maximum
                                digit = binary[7].ToString();
                                Int32.TryParse(digit, out digit_int);
                                db_hg_status.eightb_amount_parameters_over_max = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(8);
                                }


                            }

                            // Byte 9 i==5 & byte 10 is i==6
                            /*      if (i == 5)
                              {
                                  String adding_binary_nine = break_hgstat.Split(',')[i];
                                  com = int.Parse(adding_binary_nine);
                                  string byte_nine = Convert.ToString(com, 2).PadLeft(8, '0');

                                  String adding_binary_ten = break_hgstat.Split(',')[i + 1];
                                  com = int.Parse(adding_binary_ten);
                                  string byte_ten = Convert.ToString(com, 2).PadLeft(8, '0');


                                  String newNumber = string.Concat(byte_ten + byte_nine);
                                  String final = Convert.ToInt32(newNumber, 2).ToString();

                                  Int32.TryParse(final, out digit_int);
                                  // Event low byte and Event high byte to decimal 
                                   db_hg_status.nine_tenb_combine = digit_int;
                                  // LEAVING IT OUT FOR NOW.

                              }
                           */

                            // Byte 11
                            if (i == 7)
                            {

                                // Store all values 


                                // M3/ H / GPM 
                                string win = binary[0].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.elevenb_M3_H_GPM = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(10);
                                }



                                // Total Chlorine On/Off 
                                win = binary[1].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.elevenb_total_ch_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(11);
                                }



                                //Celsius/Fahrenheit
                                win = binary[2].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.elevenb_Celsius_F = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(12);
                                }


                                //Chlorine <0.1 alarm
                                // enable / disable
                                win = binary[3].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.elevenb_ch_alarm_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(14);
                                }



                                //Chlorine averaging enable/disable
                                win = binary[4].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.elevenb_ch_avg_onoff = digit_int;


                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(15);
                                }


                                //Turbidity module connected/disconnected
                                win = binary[5].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.elevenb_turbidity_onoff = digit_int;


                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(16);
                                }


                                //Flow sensor connected/disconnected
                                win = binary[6].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.elevenb_flow_sensor_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(17);
                                }



                                //Alkali/Acid
                                win = binary[7].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.elevenb_byte_alkali_acid = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(18);
                                }


                            }


                            // Byte 12
                            if (i == 8)
                            {
                                // Store all values 

                                // count == 0 
                                // nothing .... ??? 

                                // Temperature On/Off
                                string win = binary[1].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.twelveb_temp_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(19);
                                }




                                // Input  4-20 (3) On/Off
                                // Temperature On/Off
                                win = binary[2].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.twelveb_input_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(20);
                                }




                                // flow 4-20   (2) On/Off
                                win = binary[3].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.twelveb_flow_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(21);
                                }



                                // Conductivity 4-20(1)  On/Off
                                win = binary[4].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.twelveb_conductivity_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(22);
                                }



                                //pH     On/Off
                                win = binary[5].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.twelveb_ph_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(23);
                                }


                                //ORP  On/Off
                                win = binary[6].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.twelveb_ORP_onoff = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(24);
                                }



                                // Free chlorine On/Off
                                win = binary[7].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.twelveb_free_ch_onoff = digit_int;


                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(25);
                                }


                            }
                            //Byte 13
                            if (i == 9)
                            {


                                // low chlorine
                                string win = binary[0].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.thirteenb_low_chlor = digit_int;


                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(26);
                                }


                                //  Replace light
                                win = binary[1].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.thirteenb_replace_light = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(27);
                                }



                                // Unclean cell
                                win = binary[2].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.thirteenb_unclean_cell = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(28);
                                }



                                // ORP> XXX
                                win = binary[3].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.thirteenb_ORP = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(29);
                                }



                                // Chlorine <0.1
                                win = binary[4].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.thirteenb_ch_below_zeropointone = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(30);
                                }



                                // No reagents
                                win = binary[5].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.thirteenb_no_reagents = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(31);
                                }



                                // Low Flow
                                win = binary[6].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.thirteenb_low_flow = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(32);
                                }



                                // No Flow
                                win = binary[7].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.thirteenb_no_flow = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(33);
                                }



                            }

                            //Byte 14
                            if (i == 10)
                            {


                                // Colorimetr comm. error
                                string win = binary[0].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fourteenb_Colorimetr_comm_error = digit_int;
                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(34);
                                }



                                //  External OFF
                                win = binary[1].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fourteenb_external_off = digit_int;
                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(35);
                                }



                                // byte 5 = ??
                                //  win = binary[2].ToString();
                                //  Int32.TryParse(win, out digit_int);
                                //  db_hg_status.fourte = digit_int;

                                // High NTU
                                win = binary[3].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fourteenb_High_NTU = digit_int;
                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(36);
                                }


                                // Low ORP
                                win = binary[4].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fourteenb_Low_ORP = digit_int;
                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(37);
                                }



                                // High Ph
                                win = binary[5].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fourteenb_High_Ph = digit_int;
                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(38);
                                }




                                // Low Ph
                                win = binary[6].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fourteenb_Low_Ph = digit_int;
                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(39);
                                }




                                //High chlor.
                                win = binary[1].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fourteenb_High_chlor = digit_int;
                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(40);
                                }




                            }


                            //Byte 15
                            if (i == 11)
                            {

                                // High temperature
                                string win = binary[0].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fifteenb_High_temperature = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(41);
                                }



                                //  Low temperature
                                win = binary[1].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fifteenb_Low_temperature = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(42);
                                }



                                // Piston stuck
                                win = binary[2].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fifteenb_Piston_stuck = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(43);
                                }



                                // Ph overfeed time
                                win = binary[3].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fifteenb_Ph_overfeed_time = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(44);
                                }




                                // Chlor overfeed time
                                win = binary[4].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fifteenb_Chlor_overfeed_time = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(45);
                                }



                                //No DPD3
                                win = binary[5].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fifteenb_No_DPD3 = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(46);
                                }


                                // High combine chlorine
                                win = binary[6].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fifteenb_High_combine_chlorine = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(47);
                                }



                                //High total chlor
                                win = binary[7].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.fifteenb_High_total_chlor = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(48);
                                }


                            }


                            //Byte 16
                            if (i == 12)
                            {

                                // count ==0 
                                // TBD


                                //  Low Cassette
                                string win = binary[1].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.sixteen_Low_Cassette = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(49);
                                }


                                // Cassette (1 = no cassette)
                                win = binary[2].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.sixteen_Cassette = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(50);
                                }



                                //End of Cassette
                                win = binary[3].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.sixteen_End_of_Cassette = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(51);
                                }


                                // Door (1 = open)
                                win = binary[4].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.sixteen_Door = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(52);
                                }



                                //pH Tank Empty
                                win = binary[5].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.sixteen_pH_Tank_Empty = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(53);
                                }


                                // CL Tank Empty
                                win = binary[6].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.sixteen_CL_Tank_Empty = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(54);
                                }

                                //Calibration Fail
                                win = binary[7].ToString();
                                Int32.TryParse(win, out digit_int);
                                db_hg_status.sixteen_Calibration_Fail = digit_int;

                                if (digit_int == 1)
                                {
                                    get_ones.statusid.Add(55);
                                }



                            }


                        }


                        var id = await (from x in db.siteuser
                                        where x.sitetoboards.serialboard == mserial
                                        select x.Id).FirstOrDefaultAsync();

                        db_hg_status.serial_site_id = id;
                        db_hg_status.date_record = date;
                        db.hgstatus_db.Add(db_hg_status);
                        await db.SaveChangesAsync();

                    // Uncomment for debug
                    //   ViewBag.ip = userIpAddress;

                    /*

                     // EMAIL CODE
                     // string date = , string comms
                     hgstatus_email_class emailing = new hgstatus_email_class();
                     emailing.statusstring = new List<string>();
                     //   emailing.dbstatusstring = new List<string>();

                     emailing.alert_type_number = new List<int>();
                     emailing.find_email_check = new List<string>();

                     emailing.find_email_send = new List<string>();

                     // Splitting the errors
                     emailing.store_in_one = new List<string>();
                     emailing.store_in_two = new List<string>();
                     emailing.store_in_three = new List<string>();
                     emailing.store_in_four = new List<string>();


                     // DateTime date_format = DateTime.Parse(date);

                     // Getting the site ID that is attached to this serial
                     var site_id_search = await (from x in db.siteuser
                                                 where x.sitetoboards.serialboard == mserial
                                                 select x.Id).FirstOrDefaultAsync();


                     // Getting the latest data using date the newest data 
                     var find_hg = await (from x in db.hgstatus_db
                                          where x.date_record == date && x.serial_site_id == site_id_search
                                          select x).Take(1).ToListAsync();

                     // if Can't find date and Serial of recent data points
                     if (find_hg.Count() == 0)
                     {
                         ViewBag.is_found = "0";
                         return View();
                     }

                     foreach (var item in find_hg)
                     {
                         if (item.eightb_amount_parameters_over_max == 1)
                         {
                             emailing.statusstring.Add("Parameter amount over the maximum");
                             // emailing.dbstatusstring.Add("eightb_amount_parameters_over_max");
                         }

                         if (item.eightb_incorrect_pass == 1)
                         {
                             emailing.statusstring.Add("Incorrect Password");
                             //   emailing.dbstatusstring.Add("eightb_incorrect_pass");
                         }

                         if (item.eightb_parameter_out_of_range == 1)
                         {
                             emailing.statusstring.Add("Parameter is out of Range");
                             //     emailing.dbstatusstring.Add("eightb_parameter_out_of_range");
                         }

                         if (item.elevenb_byte_alkali_acid == 1)
                         {
                             emailing.statusstring.Add("ACID/ALKALI");
                             //    emailing.dbstatusstring.Add("elevenb_byte_alkali_acid");

                         }

                         if (item.elevenb_Celsius_F == 1)
                         {
                             emailing.statusstring.Add("Celsius/Fahrenheit");
                             //     emailing.dbstatusstring.Add("elevenb_Celsius_F");
                         }

                         if (item.elevenb_ch_alarm_onoff == 1)
                         {
                             emailing.statusstring.Add("Chlorine <0.1 alarm enabled");
                             //    emailing.dbstatusstring.Add("elevenb_ch_alarm_onoff");
                         }

                         if (item.elevenb_ch_avg_onoff == 1)
                         {
                             emailing.statusstring.Add("Chlorine averaging enabled");
                             //    emailing.dbstatusstring.Add("elevenb_ch_avg_onoff");
                         }

                         if (item.elevenb_flow_sensor_onoff == 1)
                         {
                             emailing.statusstring.Add("Flow sensor connected");
                             //   emailing.dbstatusstring.Add("elevenb_flow_sensor_onoff");
                         }

                         if (item.elevenb_M3_H_GPM == 1)
                         {
                             emailing.statusstring.Add("M3/H/GPM");
                             //    emailing.dbstatusstring.Add("elevenb_M3_H_GPM");
                         }

                         if (item.elevenb_total_ch_onoff == 1)
                         {
                             emailing.statusstring.Add("Total Chlorine On");
                             //    emailing.dbstatusstring.Add("elevenb_total_ch_onoff");
                         }

                         if (item.elevenb_turbidity_onoff == 1)
                         {
                             emailing.statusstring.Add("Turbidity module connected");
                             //    emailing.dbstatusstring.Add("elevenb_turbidity_onoff");
                         }


                         if (item.fifteenb_Chlor_overfeed_time == 1)
                         {
                             emailing.statusstring.Add("Chlor overfeed time");
                             //     emailing.dbstatusstring.Add("fifteenb_Chlor_overfeed_time");
                         }

                         if (item.fifteenb_High_combine_chlorine == 1)
                         {
                             emailing.statusstring.Add("High combine chlorine");
                             //    emailing.dbstatusstring.Add("fifteenb_High_combine_chlorine");
                         }

                         if (item.fifteenb_High_temperature == 1)
                         {
                             emailing.statusstring.Add("High temperature");
                             //     emailing.dbstatusstring.Add("fifteenb_High_temperature");
                         }

                         if (item.fifteenb_High_total_chlor == 1)
                         {
                             emailing.statusstring.Add("High total chlor");
                             //     emailing.dbstatusstring.Add("fifteenb_High_total_chlor");
                         }

                         if (item.fifteenb_Low_temperature == 1)
                         {
                             emailing.statusstring.Add("Low temperature");
                             //     emailing.dbstatusstring.Add("fifteenb_Low_temperature");
                         }

                         if (item.fifteenb_No_DPD3 == 1)
                         {
                             emailing.statusstring.Add("No DPD3");
                             //    emailing.dbstatusstring.Add("fifteenb_No_DPD3");
                         }

                         if (item.fifteenb_Ph_overfeed_time == 1)
                         {
                             emailing.statusstring.Add("Ph overfeed time");
                             //  emailing.dbstatusstring.Add("fifteenb_Ph_overfeed_time");
                         }

                         if (item.fifteenb_Piston_stuck == 1)
                         {
                             emailing.statusstring.Add("Piston stuck");
                             //    emailing.dbstatusstring.Add("fifteenb_Piston_stuck");
                         }

                         if (item.fourb_byte_acid_base_pump == 1)
                         {
                             emailing.statusstring.Add("Relay3-Acid/ Base Pump");
                             //     emailing.dbstatusstring.Add("fourb_byte_acid_base_pump");
                         }

                         if (item.fourb_ch_addition_pump == 1)
                         {
                             emailing.statusstring.Add("Relay2-Chlor Addition Pump");
                             //    emailing.dbstatusstring.Add("fourb_ch_addition_pump");
                         }

                         if (item.fourb_ch_main_pump == 1)
                         {
                             emailing.statusstring.Add("Relay1-Chlor main pump");
                             //     emailing.dbstatusstring.Add("fourb_ch_main_pump");
                         }

                         if (item.fourteenb_Colorimetr_comm_error == 1)
                         {
                             emailing.statusstring.Add("Colorimetr comm error");
                             //    emailing.dbstatusstring.Add("fourteenb_Colorimetr_comm_error");
                         }

                         if (item.fourteenb_external_off == 1)
                         {
                             emailing.statusstring.Add("External OFF");
                             //    emailing.dbstatusstring.Add("fourteenb_external_off");
                         }

                         if (item.fourteenb_High_chlor == 1)
                         {
                             emailing.statusstring.Add("High chlor");
                             //    emailing.dbstatusstring.Add("fourteenb_High_chlor");
                         }

                         if (item.fourteenb_High_NTU == 1)
                         {
                             emailing.statusstring.Add("High NTU");
                             //    emailing.dbstatusstring.Add("fourteenb_High_NTU");
                         }


                         if (item.fourteenb_Low_ORP == 1)
                         {
                             emailing.statusstring.Add("Low ORP");
                             //   emailing.dbstatusstring.Add("fourteenb_Low_ORP");
                         }

                         if (item.fourteenb_High_Ph == 1)
                         {
                             emailing.statusstring.Add("High Ph");
                             //   emailing.dbstatusstring.Add("fourteenb_High_Ph");
                         }

                         if (item.fourteenb_Low_Ph == 1)
                         {
                             emailing.statusstring.Add("Low Ph");
                             //   emailing.dbstatusstring.Add("fourteenb_Low_Ph");
                         }


                         // Do not store or need for now
                         //  if (item.nine_tenb_combine == 1)
                         //  {
                         //     emailing.statusstring.Add("Amount of Events in Data Base");
                         //    emailing.dbstatusstring.Add("nine_tenb_combine");
                         //  }

                         if (item.sevenb_flow_meter == 1)
                         {
                             emailing.statusstring.Add("Flow-meter");
                             //     emailing.dbstatusstring.Add("sevenb_flow_meter");
                         }

                         if (item.sixb_low_reagent == 1)
                         {
                             emailing.statusstring.Add("Low Reagent");
                             //     emailing.dbstatusstring.Add("sixb_low_reagent");
                         }

                         if (item.sixteen_Calibration_Fail == 1)
                         {
                             emailing.statusstring.Add("Calibration Fail");
                             //    emailing.dbstatusstring.Add("sixteen_Calibration_Fail");
                         }

                         if (item.sixteen_Cassette == 1)
                         {
                             emailing.statusstring.Add("no cassette");
                             //   emailing.dbstatusstring.Add("sixteen_Cassette");
                         }

                         if (item.sixteen_CL_Tank_Empty == 1)
                         {
                             emailing.statusstring.Add("CL Tank Empty");
                             //     emailing.dbstatusstring.Add("sixteen_CL_Tank_Empty");
                         }

                         if (item.sixteen_Door == 1)
                         {
                             emailing.statusstring.Add("Door open");
                             //   emailing.dbstatusstring.Add("sixteen_Door");
                         }

                         if (item.sixteen_End_of_Cassette == 1)
                         {
                             emailing.statusstring.Add("End of Cassette");
                             //   emailing.dbstatusstring.Add("sixteen_End_of_Cassette");
                         }

                         if (item.sixteen_Low_Cassette == 1)
                         {
                             emailing.statusstring.Add("Low Cassette");
                             //    emailing.dbstatusstring.Add("sixteen_Low_Cassette");
                         }

                         if (item.sixteen_pH_Tank_Empty == 1)
                         {
                             emailing.statusstring.Add("PH Tank Empty");
                             //    emailing.dbstatusstring.Add("sixteen_pH_Tank_Empty");
                         }

                         if (item.thirteenb_ch_below_zeropointone == 1)
                         {
                             emailing.statusstring.Add("Chlorine<0.1");
                             //  emailing.dbstatusstring.Add("thirteenb_ch_below_zeropointone");
                         }



                         if (item.thirteenb_low_chlor == 1)
                         {
                             emailing.statusstring.Add("Low chlor");
                             //   emailing.dbstatusstring.Add("thirteenb_low_chlor");
                         }

                         if (item.thirteenb_low_flow == 1)
                         {
                             emailing.statusstring.Add("Low Flow");
                             //  emailing.dbstatusstring.Add("thirteenb_low_flow");
                         }

                         if (item.thirteenb_no_flow == 1)
                         {
                             emailing.statusstring.Add("No Flow");
                             //  emailing.dbstatusstring.Add("thirteenb_no_flow");
                         }

                         if (item.thirteenb_no_reagents == 1)
                         {
                             emailing.statusstring.Add("No Reagents");
                             //    emailing.dbstatusstring.Add("thirteenb_no_reagents");
                         }

                         if (item.thirteenb_ORP == 1)
                         {
                             emailing.statusstring.Add("ORP>XXX");
                             //   emailing.dbstatusstring.Add("thirteenb_ORP");
                         }

                         if (item.thirteenb_replace_light == 1)
                         {
                             emailing.statusstring.Add("Replace light");
                             //   emailing.dbstatusstring.Add("thirteenb_replace_light");
                         }

                         if (item.thirteenb_unclean_cell == 1)
                         {
                             emailing.statusstring.Add("Unclean cell");
                             //   emailing.dbstatusstring.Add("thirteenb_unclean_cell");
                         }

                         if (item.twelveb_conductivity_onoff == 1)
                         {
                             emailing.statusstring.Add("Conductivity 4-20(1)On");
                             //   emailing.dbstatusstring.Add("twelveb_conductivity_onoff");
                         }

                         if (item.twelveb_flow_onoff == 1)
                         {
                             emailing.statusstring.Add("flow 4-20(2)On");
                             //   emailing.dbstatusstring.Add("twelveb_flow_onoff");
                         }


                         if (item.twelveb_free_ch_onoff == 1)
                         {
                             emailing.statusstring.Add("Free chlorine On");
                             //    emailing.dbstatusstring.Add("twelveb_free_ch_onoff");
                         }

                         if (item.twelveb_input_onoff == 1)
                         {
                             emailing.statusstring.Add("Input  4-20(3)On");
                             //    emailing.dbstatusstring.Add("twelveb_input_onoff");
                         }

                         if (item.twelveb_ORP_onoff == 1)
                         {
                             emailing.statusstring.Add("ORP On");
                             //   emailing.dbstatusstring.Add("twelveb_ORP_onoff");
                         }

                         if (item.twelveb_ph_onoff == 1)
                         {
                             emailing.statusstring.Add("pH On");
                             //   emailing.dbstatusstring.Add("twelveb_ph_onoff");
                         }

                         if (item.twelveb_temp_onoff == 1)
                         {
                             emailing.statusstring.Add("Temperature On");
                             //  emailing.dbstatusstring.Add("twelveb_temp_onoff");
                         }


                     }

                     ViewBag.is_found = "EEstatus=" + 1;

                     // This will trigger if no status is saved
                     if (!emailing.statusstring.Any())
                     {
                         //ViewBag.is_found =  0;
                         return View();
                     }

                     for (int i = 0; i < emailing.statusstring.Count(); i++)
                     {
                         // Must be here or it will give error
                         var normal_string = emailing.statusstring[i];


                         // var linq_string = emailing.dbstatusstring[i];
                         //////////////////////////////////////////////

                         // Finding all the variables that are 1's 
                         // Extracting all the ID's that contain a 1 
                         var find_id = await (from x in db.hgstatus_all
                                              where x.error_types.Equals(normal_string)
                                              select x.id).FirstOrDefaultAsync();


                         // This will trigger if parameters in strings don't match
                         if (find_id == 0)
                         {
                             ViewBag.is_found = 0;
                             return View();
                         }





                         // Finding Site ID 
                         // Getting all the Errors that are linked to this Site ID 
                         // Extracting the Alert Type 1,2,3,4 
                         var find_alert_type = await (from x in db.hgstatus_link
                                                      where x.site_serial_id == site_id_search
                                                      && x.errors_id == find_id
                                                      select x.alert_type).FirstOrDefaultAsync();

                         ////////////// SENDING EMAILS DO LATER
                         if (find_alert_type == 1)
                         {
                             emailing.store_in_one.Add(normal_string);
                         }

                         if (find_alert_type == 2)
                         {
                             emailing.store_in_two.Add(normal_string);
                         }

                         if (find_alert_type == 3)
                         {
                             emailing.store_in_three.Add(normal_string);
                         }

                         if (find_alert_type == 4)
                         {
                             emailing.store_in_four.Add(normal_string);
                         }
                     }


                     // finding all User data from the serial site
                     var alert = await (from x in db.site_user_link
                                        where x.siteuser.sitetoboards.serialboard == mserial
                                        select x.newusers).ToListAsync();



                     foreach (var item in alert)
                     {

                         // Check if they want any emails 
                         int all_none = 0;


                         // Getting numbers 1 = email  0= nothing, for all errors
                         // From each User 
                         var hg_emil = await (from x in db.hgstatus_email
                                              where x.newusers.ID == item.ID
                                              select x).ToListAsync();



                         var myMessage = new SendGridMessage();

                         myMessage.From = new MailAddress("nick@elementalelec.com.au");
                         // Get the email

                         myMessage.Subject = "HG Status from Elemental Electronics Server";
                         String message = "List of Errors:" + "\n";

                         foreach (var link in hg_emil)
                         {
                             // For all the ones that are 1
                             if (link.send_type_number != 0)
                             {

                                 if (link.alert_type_number == 1 && link.send_type_number == 1)
                                 {


                                     for (int x = 0; x < emailing.store_in_one.Count(); x++)
                                     {
                                         all_none = all_none + 1;
                                         message += "(" + emailing.store_in_one[x] + "),";
                                     }
                                 }

                                 else if (link.alert_type_number == 2 && link.send_type_number == 1)
                                 {

                                     for (int x = 0; x < emailing.store_in_two.Count(); x++)
                                     {
                                         all_none = all_none + 1;
                                         message += "(" + emailing.store_in_two[x] + "),";
                                     }
                                 }

                                 else if (link.alert_type_number == 3 && link.send_type_number == 1)
                                 {

                                     for (int x = 0; x < emailing.store_in_three.Count(); x++)
                                     {
                                         all_none = all_none + 1;
                                         message += "(" + emailing.store_in_three[x] + "),";
                                     }
                                 }

                                 else if (link.alert_type_number == 4 && link.send_type_number == 1)
                                 {

                                     for (int x = 0; x < emailing.store_in_four.Count(); x++)
                                     {
                                         all_none = all_none + 1;
                                         message += "(" + emailing.store_in_four[x] + "),";
                                     }
                                 }

                             }
                         }

                         if (all_none > 0)
                         {
                             myMessage.AddTo(item.email);
                             myMessage.Html = message;
                             // Create network credentials to access your SendGrid account
                             var username = "azure_a102fcaade38cdf0ab4fdaf0f2008c9b@azure.com";
                             var pswd = "4V242nb6mKk36hm";
                             var credentials = new NetworkCredential(username, pswd);
                             // Create an Web transport for sending email.
                             var transportWeb = new Web(credentials);

                             // Send the email, which returns an awaitable task.
                             await transportWeb.DeliverAsync(myMessage);
                         }

                     }
                      */

                }


                if ((trantype == "GT" && transubtype == "GT") || (trantype == "HG" && transubtype == "param"))
                    {


                        string break_idval = "";
                        List<int> idvalues_integer = new List<int>();
                        List<double> idvalues_double = new List<double>();


                        if (idvalues == null && hgparam == null)
                        {
                            ViewBag.is_found = 0;
                            return View();
                        }

                        if (idvalues != null)
                        {
                            break_idval = idvalues;

                        }

                        if (hgparam != null)
                        {
                            break_idval = hgparam;
                        }

                        Boolean first = true;
                        // Check for any random characters 
                        foreach (char c in break_idval)
                        {

                            if (c == '-' || c == ',' || c == '.')
                            {
                                if(first == true)
                                {
                                 ViewBag.is_found = 0;
                                 return View();
                                }
                            }
                            
                            else if (c < '0' || c > '9')
                            {
                                ViewBag.is_found = 0;
                                return View();
                            }
                            
                            else {
                                
                               if(first == true)
                                {
                                first = false;
                                }  
                                
                             }

                        }


                        var primeArray = break_idval.Split(',');

                        for (int i = 0; i < primeArray.Length; i++)
                        {

                            string idvalues_id = break_idval.Split(',')[i];

                            idvalues_integer.Add(Convert.ToInt32(idvalues_id));
                            string idvalues_value;

                            try
                            {
                                idvalues_value = break_idval.Split(',')[i + 1];
                            }
                            catch (Exception ex)
                            {
                                ViewBag.is_found = 0;
                                return View();
                            }




                            double out_double = 0.0;
                            double.TryParse(idvalues_value, out out_double);

                            idvalues_double.Add(out_double);


                            i = i + 1;
                        }



                        graph_update entry = new graph_update();
                        entry.serial = mserial;

                        for (int i = 0; i < idvalues_integer.Count(); i++)
                        {
                            int find_int_id = idvalues_integer[i];

                            if (find_int_id == 17)
                            {
                                entry.BI_Temperature = idvalues_double[i];
                            }

                            else if (find_int_id == 137)
                            {
                                entry.TempProbeInt = idvalues_double[i];
                            }

                            else if (find_int_id == 20)
                            {
                                entry.HG_Value_of_Flow_met = idvalues_double[i];
                            }

                            else if (find_int_id == 21)
                            {
                                entry.BI_Chlorine = idvalues_double[i];
                            }

                            else if (find_int_id == 22)
                            {
                                entry.BI_pH = idvalues_double[i];
                            }

                            else if (find_int_id == 23)
                            {
                                entry.HG_ORP = idvalues_double[i];
                            }

                            else if (find_int_id == 24)
                            {
                                entry.HG_Turbidity = idvalues_double[i];
                            }

                            else if (find_int_id == 31)
                            {
                                entry.HG_PH_P_factor = idvalues_double[i];
                            }

                            else if (find_int_id == 32)
                            {
                                entry.HG_PH_pump_period = idvalues_double[i];
                            }


                            else if (find_int_id == 33)
                            {
                                entry.BI_Total_Chlorine = idvalues_double[i];
                            }

                            else if (find_int_id == 34)
                            {
                                entry.HG_Total_Chlorine_Ra = idvalues_double[i];
                            }

                            else if (find_int_id == 38)
                            {
                                entry.HG_Comm_Chlorine_Co = idvalues_double[i];
                            }
                            else if (find_int_id == 48)
                            {
                                entry.HG_Combine_chlorine = idvalues_double[i];
                            }
                            else if (find_int_id == 59)
                            {
                                entry.HG_Conductivity_from = idvalues_double[i];
                            }
                            else if (find_int_id == 60)
                            {
                                entry.HG_Input_2_from_NTU = idvalues_double[i];
                            }
                            else if (find_int_id == 61)
                            {
                                entry.HG_Input_3_from_NTU = idvalues_double[i];
                            }
                            else if (find_int_id == 75)
                            {
                                entry.HG_Temperature_calib = idvalues_double[i];
                            }
                            else if (find_int_id == 113)
                            {
                                entry.HG_Free_CL_Feed_Rate = idvalues_double[i];
                            }
                            else if (find_int_id == 114)
                            {
                                entry.HG_pH_Feed_Rate = idvalues_double[i];
                            }
                            else if (find_int_id == 136)
                            {
                                entry.Temp_Probe_External = idvalues_double[i];
                            }
                            else if (find_int_id == 138)
                            {
                                entry.Total_Alkalinity = idvalues_double[i];
                            }
                            else if (find_int_id == 139)
                            {
                                entry.Calcium_Hardness = idvalues_double[i];
                            }
                            else if (find_int_id == 141)
                            {

                                idvalues_double[i] = idvalues_double[i] * 10;

                                 entry.Conductivity_1 = idvalues_double[i];
                            }
                            else if (find_int_id == 140)
                            {

                            idvalues_double[i] = idvalues_double[i] / 10;

                            entry.WL_Conductivity_mScm = idvalues_double[i];
                            }
                            else if (find_int_id == 142)
                            {
                                entry.Conductivity_3 = idvalues_double[i];
                            }
                            else if (find_int_id == 143)
                            {
                                entry.Conductivity_4 = idvalues_double[i];
                            }
                            else if (find_int_id == 144)
                            {
                                entry.Total_Dissolved_Solids = idvalues_double[i];
                            }
                            else if (find_int_id == 145)
                            {
                                entry.Phosphate = idvalues_double[i];
                            }
                            else if (find_int_id == 146)
                            {
                                entry.Chlorinator_Average = idvalues_double[i];
                            }
                            else if (find_int_id == 147)
                            {
                                entry.Cyanuric_Acid = idvalues_double[i];
                            }
                            else if (find_int_id == 148)
                            {
                                entry.Router_Reboot = idvalues_double[i];
                            }
                            else if (find_int_id == 149)
                            {
                                entry.Board_Reboot = idvalues_double[i];
                            }
                            else if (find_int_id == 150)
                            {
                                entry.Sim900_Signal_Strength = idvalues_double[i];
                            }
                            else if (find_int_id == 151)
                            {
                                entry.Board_Start = idvalues_double[i];
                            }
                            else if (find_int_id == 1)
                            {
                                entry.Chlorine_set_Point_1 = idvalues_double[i];
                            }
                            else if (find_int_id == 3)
                            {
                                entry.Chlorine_Low_Alarm = idvalues_double[i];
                            }
                            else if (find_int_id == 4)
                            {
                                entry.Chlorine_High_Alarm = idvalues_double[i];
                            }
                            else if (find_int_id == 5)
                            {
                                entry.Ph_set_Point = idvalues_double[i];
                            }
                            else if (find_int_id == 6)
                            {
                                entry.Ph_Low_Alarm = idvalues_double[i];
                            }
                            else if (find_int_id == 7)
                            {
                                entry.Ph_High_Alarm = idvalues_double[i];
                            }
                            else if (find_int_id == 26)
                            {
                                entry.ID_number = idvalues_double[i];
                            }
                            else if (find_int_id == 39)
                            {
                                entry.Low_Reagents = idvalues_double[i];
                            }
                            else if (find_int_id == 44)
                            {
                                entry.Temperature_Low_Alarm = idvalues_double[i];
                            }
                            else if (find_int_id == 45)
                            {
                                entry.Temperature_High_Alarm = idvalues_double[i];
                            }
                            else if (find_int_id == 46)
                            {
                                entry.Temperature_set_Point = idvalues_double[i];
                            }
                            else if (find_int_id == 54)
                            {
                                entry.Type_of_controller = idvalues_double[i];
                            }
                            else if (find_int_id == 62)
                            {
                                entry.Version_number = idvalues_double[i];
                            }
                            else if (find_int_id == 63)
                            {
                                entry.Protocol_version = idvalues_double[i];
                            }
                            else if (find_int_id == 40)
                            {
                                entry.ORP_set_Point_1 = idvalues_double[i];
                            }
                            else if (find_int_id == 41)
                            {
                                entry.ORP_set_Point_2 = idvalues_double[i];
                            }

                            else if (find_int_id == 153)
                            {
                                entry.Temp_sensor_1 = idvalues_double[i];
                            }

                            else if (find_int_id == 154)
                            {
                                entry.Temp_sensor_2 = idvalues_double[i];
                            }

                            else if (find_int_id == 155)
                            {
                                entry.Temp_sensor_3 = idvalues_double[i];
                            }

                            else if (find_int_id == 156)
                            {
                                entry.Temp_sensor_4 = idvalues_double[i];
                            }
                            else if (find_int_id == 157)
                            {
                                entry.Temp_sensor_5 = idvalues_double[i];
                            }







                        }

                        entry.plotdate = date;
                        db.graph_update.Add(entry);
                        await db.SaveChangesAsync();
                        ViewBag.is_found = "EEstatus=" + 1;



                    }
                }
            


            
            ViewBag.is_found = "EEstatus=1";
            return View();
        }

        public async Task<ActionResult> comms_response(String serialsite, String radio_select, String sitename, String freq)
        {
            userContext db = new userContext();

            int freq_int = 10;
        Int32.TryParse(freq, out freq_int);


            if (serialsite == null)
            {
                serialsite = "0890CA";
             }
            else
            {
                serialsite = serialsite.Replace(" ", string.Empty);

            }

            if (radio_select != null)
            {
                if (radio_select == "radio_ourserver")
                {
                    if (sitename != null)
                    {

                        

                        // response_comms response = new response_comms();
                        // db.SaveChanges();

                        if (sitename == "select_EE")
                        {
                            var response = db.response_comms.Where(x => x.sitetoboards.serialboard == serialsite).FirstOrDefault();
                            
                            if(response !=null)
                            {
                                response.response_bit = 1;
                                response.frequency = freq_int;
                                response.once_on_server = 1.1;
                                db.Entry(response).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                          
                            }

                          // Store on server only 
                          // store 1.1
                        }

                        else if (sitename == "select_storeandsend")
                        {

                            var response = db.response_comms.Where(x => x.sitetoboards.serialboard == serialsite).FirstOrDefault();

                            if (response != null)
                            {
                                response.response_bit = 1;
                                response.frequency = freq_int;
                                response.once_on_server = 1.2;
                                db.Entry(response).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                            }
                            // Store on Server and Forward to CPSYS
                            // store 1.2
                        }

                        else if (sitename == "select_cpsys")
                        {
                            var response = db.response_comms.Where(x => x.sitetoboards.serialboard == serialsite).FirstOrDefault();

                            if (response != null)
                            {
                                response.response_bit = 1;
                                response.frequency = freq_int;
                                response.once_on_server = 1.3;
                                db.Entry(response).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                            }

                            //Do not Store on Server and Forward to CPSYS
                            // store 1.3
                        }


                    }
                }

                if (radio_select == "radio_cpsys")
                {
                    var response = db.response_comms.Where(x => x.sitetoboards.serialboard == serialsite).FirstOrDefault();

                    if (response != null)
                    {
                        response.response_bit = 2;
                        response.frequency = freq_int;
                        response.once_on_server = 0;
                        db.Entry(response).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                    }


                }

                if (radio_select == "radio_both")
                {
                    var response = db.response_comms.Where(x => x.sitetoboards.serialboard == serialsite).FirstOrDefault();

                    if (response != null)
                    {
                        response.response_bit = 3;
                        response.frequency = freq_int;
                        response.once_on_server = 1.2;
                        db.Entry(response).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }

                }

            }


            // Default values 


            //Selecting sites that are already in the response comms
            var site_to_comms = await(from y in db.response_comms
                                      select y.sitetoboards).ToListAsync();

            cpsys_url adding_sites = new cpsys_url();
            adding_sites.serial_site = new List<String>();

            int count = 0;

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

            //@Viewbag.freq
            var frequency = await(from x in db.response_comms
                                  where x.sitetoboards.serialboard == serialsite
                                  select x.frequency).FirstOrDefaultAsync();


            ViewBag.freq = frequency;


          
            

            //@ViewBag.selected
            var checked_db = await(from x in db.response_comms
                                   where x.sitetoboards.serialboard == serialsite
                                   select x.response_bit).FirstOrDefaultAsync();



            if(checked_db == 1)
            {  
                ViewBag.selected_one = "checked";

                var radio_check = await(from x in db.response_comms
                                        where x.sitetoboards.serialboard == serialsite
                                        select x.once_on_server).FirstOrDefaultAsync();

                if(radio_check == 1.1)
                {   //selected
                    ViewBag.radio_one = "selected";
                }

                else if (radio_check == 1.2)
                {
                    ViewBag.radio_two = "selected";
                }

                else if (radio_check == 1.3)
                {
                    ViewBag.radio_three = "selected";
                }

            }

            if (checked_db == 2)
            {
                ViewBag.selected_two = "checked";
            }

            if (checked_db == 3)
            {
                ViewBag.selected_three = "checked";
            }

             return View(adding_sites);
        }


        public async Task<ActionResult> comms_response_uri(String serialstring)
        {
            userContext db = new userContext();

            // ID VALUES
            hgstatus_controller_alert use_string = new hgstatus_controller_alert();
        use_string.sites = new List<string>();
            

            //try {
              var what_to_return = await(from x in db.response_comms
                                         where x.sitetoboards.serialboard == serialstring
                                         select x).FirstOrDefaultAsync();
           
          

            if (what_to_return == null)
            {
                use_string.sites.Add("0");
                return View(use_string);
    }
            else
            {
                ViewBag.freq = what_to_return.frequency + ",";
                ViewBag.response = what_to_return.response_bit + ",";
            }

//}
//catch (Exception ex)
//{
//    Response.Write("Exception :" + ex.Message);
//}



// Date time 
String utc = DateTime.UtcNow.ToString("yyyy,MM,dd,HH,mm,ss", CultureInfo.InvariantCulture);

            for (int i = 0; i<utc.Length; i++)
            {
                char c = utc[i];

                if (i == 4 || i == 7 || i == 10 || i == 13 || i == 16)
                {
                    if (c == ',')
                    { }

                    else
                    {
                        ViewBag.is_found = 0;
                        use_string.sites.Add("0");
                        return View(use_string);
                    }
                }



                else if (c< '0' || c> '9')
                {

                    ViewBag.is_found = 0;
                    use_string.sites.Add("0");
                    return View(use_string);
                }

            }


            //String send_format = "tdnt:" + utc;
            ViewBag.gmt_now = "tdnt:" + utc + ",";


            //11/01/2016 1:45:58 AM
            //yyyy,MM,dd,HH,mm,ss
            String date_format = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);



            for (int i = 0; i<date_format.Length; i++)
            {
                char c = date_format[i];
                // || 

                if (i == 2 || i == 5)
                {
                    if (c == '/')
                    { }

                    else
                    {
                        ViewBag.is_found = 0;
                        use_string.sites.Add("0");
                        return View(use_string);
                    }
                }

                else if (i == 10)
                {
                    if (c == ' ')
                    { }

                    else
                    {
                        ViewBag.is_found = 0;
                        use_string.sites.Add("0");
                        return View(use_string);
                    }
                }

                else if (i == 13 || i == 16)
                {
                    if (c == ':')
                    { }

                    else
                    {
                        ViewBag.is_found = 0;
                        use_string.sites.Add("0");
                        return View(use_string);
                    }
                }


                else if (c< '0' || c> '9')
                {

                    ViewBag.is_found = 0;
                    use_string.sites.Add("0");
                    return View(use_string);
                }

            }

            
            //DateTime date;
            //// Check if date is valid 
            //DateTime.TryParse(date_format, out date);


            // Last Seen 
            //var last_seen = db.sitetoboards.Where(x => x.serialboard == serialstring).FirstOrDefault();

            //if (last_seen != null)
            //{
            //    last_seen.lastseen = date;
            //    last_seen.serialboard = serialstring;
            //    db.Entry(last_seen).State = EntityState.Modified;
            //    db.SaveChanges();

            //}
            //else
            //{
            //    ViewBag.is_found = 0;
            //    return View();
            //}
            ///////////////////////////
            
            var find = await(from x in db.graph_limit
                             where x.siteuser.sitetoboards.serialboard == serialstring
                             select x.graph_data_type.serial_id).ToListAsync();

            if (find.Count() == 0)
            {
               // ViewBag.findcount = 0;
                use_string.sites.Add("0");
                return View(use_string);
            }
            
            var last = " ";
last = find.Last().ToString();

            foreach (var item in find)
            {

               if (item.ToString() == "137" || item.ToString() == "150")
               {

               }
               else if (item.ToString() == last)
                {
                    use_string.sites.Add(item.ToString() + " EEstatus=1");
                }
               else
                {
                    use_string.sites.Add(item.ToString() + ",");
                }
            }

            ViewBag.findcount = find.Count()-2 + ",";

            

            return View(use_string);
        }

        public async Task<ActionResult> commresponse(String serialstring)
        {

            hgstatus_controller_alert use_string = new hgstatus_controller_alert();
            use_string.sites = new List<string>();

            userContext db = new userContext();

            //try {
            var what_to_return = await(from x in db.response_comms
                                           where x.sitetoboards.serialboard == serialstring
                                           select x).FirstOrDefaultAsync();

        response_string saving_string = new response_string();

            if (what_to_return == null)
                {

                // **** This is used for seeing what URL Gave us ****
                //string url_save = "0";
                //saving_string.com_val = serialstring + url_save;
                //db.response_string.Add(saving_string);
                //await db.SaveChangesAsync();

                use_string.sites.Add("0");
                    return View(use_string);
    }
                else
                {
                    ViewBag.freq = what_to_return.frequency + ",";
                    ViewBag.response = what_to_return.response_bit + ",";
                }

//}
//catch (Exception ex)
//{
//    Response.Write("Exception :" + ex.Message);
//}



// Date time 
String utc = DateTime.UtcNow.ToString("yyyy,MM,dd,HH,mm,ss", CultureInfo.InvariantCulture);

                for (int i = 0; i<utc.Length; i++)
                {
                    char c = utc[i];

                    if (i == 4 || i == 7 || i == 10 || i == 13 || i == 16)
                    {
                        if (c == ',')
                        { }

                        else
                        {
                        // **** This is used for seeing what URL Gave us ****
                        //string url_save = serialstring + utc + ViewBag.freq + ViewBag.response + ",0";
                        //saving_string.com_val = url_save;
                        //db.response_string.Add(saving_string);
                        //await db.SaveChangesAsync();
                     
                        use_string.sites.Add("0");
                         return View(use_string);

                        }
                    }



                    else if (c< '0' || c> '9')
                    {
                    
                    // **** This is used for seeing what URL Gave us ****
                    //string url_save = serialstring + utc + ViewBag.freq + ViewBag.response + ",0";
                    //saving_string.com_val = url_save;
                    //db.response_string.Add(saving_string);
                    //db.SaveChanges();


                        use_string.sites.Add("0");
                        return View(use_string);
                    }

                }


               
                ViewBag.gmt_now = "tdnt:" + utc + ",";


                //11/01/2016 1:45:58 AM
                //yyyy,MM,dd,HH,mm,ss
                String date_format = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);


                for (int i = 0; i<date_format.Length; i++)
                {
                    char c = date_format[i];
                    // || 

                    if (i == 2 || i == 5)
                    {
                        if (c == '/')
                        { }

                        else
                        {
                        // **** This is used for seeing what URL Gave us ****
                        //string url_save = serialstring + ViewBag.gmt_now + date_format + ViewBag.freq + ViewBag.response + ",0";
                        //saving_string.com_val = url_save;
                        //db.response_string.Add(saving_string);
                        //db.SaveChanges();
                        
                            use_string.sites.Add("0");
                            return View(use_string);
                        }
                    }

                    else if (i == 10)
                    {
                        if (c == ' ')
                        { }

                        else
                        {
                        // **** This is used for seeing what URL Gave us ****
                        //string url_save = serialstring + ViewBag.gmt_now + date_format + ViewBag.freq + ViewBag.response + ",0";
                        //saving_string.com_val = url_save;
                        //db.response_string.Add(saving_string);
                        //db.SaveChanges();

                      
                            use_string.sites.Add("0");
                            return View(use_string);
                        }
                    }

                    else if (i == 13 || i == 16)
                    {
                        if (c == ':')
                        { }

                        else
                        {
                           
                            use_string.sites.Add("0");
                            return View(use_string);
                        }
                    }


                    else if (c< '0' || c> '9')
                    {

                       
                        use_string.sites.Add("0");
                        return View(use_string);
                    }

                }

                
                var find = await(from x in db.graph_limit
                                 where x.siteuser.sitetoboards.serialboard == serialstring
                                 select x.graph_data_type.serial_id).ToListAsync();

                if (find.Count() == 0)
                {
                // **** This is used for seeing what URL Gave us ****
                //string url_save = serialstring + ViewBag.gmt_now + date_format + ViewBag.freq + ViewBag.response + ",0";
                //saving_string.com_val = url_save;
                //db.response_string.Add(saving_string);
                //db.SaveChanges();

                use_string.sites.Add("0");
                return View(use_string);
                }

                var last = " ";
last = find.Last().ToString();

string add = " ";

                foreach (var item in find)
                {

                    if (item.ToString() == last)
                    {
                        use_string.sites.Add(item.ToString() + " EEstatus=1");

                        add = add + item.ToString() + " EEstatus=1";
                    }
                    else
                    {
                        use_string.sites.Add(item.ToString() + ",");

                        add = add + item.ToString() + ",";
                    }
                }

                ViewBag.findcount = find.Count() + ",";



            //ttp://stackoverflow.com/questions/5475306/how-do-i-copy-sql-azure-database-to-my-local-development-server
            //ttps://msdn.microsoft.com/en-us/library/ms178371(v=vs.100).aspx

          //  string url_send = ViewBag.gmt_now + ViewBag.freq + ViewBag.response + ViewBag.findcount + add;
        
          //  string url = "ttp://elementalelec-staged.azurewebsites.net/sv01/comm_save?data=" + url_send;

            //response_string saving_string = new response_string();

           // saving_string.com_val = url_send;

          //  db.response_string.Add(saving_string);
          //  db.SaveChanges();

            //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            ////set up web request...
            //ThreadPool.QueueUserWorkItem(o => { myRequest.GetResponse(); });



            return View(use_string);
        }



    }
}