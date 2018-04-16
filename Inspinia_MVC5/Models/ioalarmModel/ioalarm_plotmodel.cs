﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newtest.Models.ioalarmmodel
{
    public class ioalarm_plotmodel
    {

        public List<string> selected_num { get; set; }

        public List<long> year_graph { get; set; }

        public List<string> year_last_date { get; set; }

        public List<double> value_of_graph { get; set; }

        public List<string> graph_min { get; set; }

        public List<string> graph_max { get; set; }

        public List<string> graph_avg { get; set; }

        public List<string> graph_data_date { get; set; }

        public List<string> graph_data_late { get; set; }

        public List<string> alarm_graph_store { get; set; }

        public List<string> sitename { get; set; }

        public List<string> data_from { get; set; }

        public List<string> data_to { get; set; }


    }
}