using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stay_calm_db_comms
{
    public class hgstatus_controller_alert
    {

        public List<string> sites { get; set; }

        public List<string> alert_error { get; set; }

        public List<string> alert_critial { get; set; }
        public List<string> alert_warning { get; set; }
        public List<string> alert_info { get; set; }
        public List<string> alert_contractor { get; set; }

        public List<string> alert_type_names { get; set; }
    }
}