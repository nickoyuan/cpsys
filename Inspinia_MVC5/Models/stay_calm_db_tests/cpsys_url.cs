using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stay_calm_db_tests
{
    public class cpsys_url
    {
        public List<Double> ph { get; set; }
        public List<Double> temp { get; set; }
        public List<Double> chlorine { get; set; }
        public List<string> serial_site { get; set; }

        public DateTime dateplot { get; set; }

    }
}