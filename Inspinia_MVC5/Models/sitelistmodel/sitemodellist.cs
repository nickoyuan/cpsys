using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newtest.Models.sitelistmodel
{
    public class sitemodellist
    {
        public List<string> status { get; set; }

        public List<string> date_lastseen { get; set; }

        public List<string> sites { get; set; }

        public List<string> site_id { get; set; }

        public List<string> timezone { get; set; }

        public List<string> serialnumber { get; set; }

        public List<string> serialid { get; set; }

        public List<string> hgstatus_count { get; set; }

        public List<string> ioalarms_count { get; set; }

        public List<string> graph_data_count { get; set; }
    }
}