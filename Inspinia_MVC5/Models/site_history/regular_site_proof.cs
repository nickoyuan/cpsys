using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace newtest.Models.site_history
{
    public class regular_site_proof
    {
        public List<string> sites { get; set; }

        public List<string> site_id { get; set; }

        public List<string> graph_data_names { get; set; }

        public List<string> graph_data_ids { get; set; }

        public List<string> graph_options_selected { get; set; }

        public List<string> graph_option_selected_id { get; set; }

        public List<double> graph_data_last_temp { get; set; }

        public List<double> graph_data_last_ch { get; set; }

        public List<double> graph_data_last_ph { get; set; }

        public List<string> plotdate_temp { get; set; }

        public List<string> plotdate_ch { get; set; }

        public List<string> plotdate_ph { get; set; }

        public List<string> datenow { get; set; }
    }
}