using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ee.Models.chparamModel
{
    public class chparamdb
    {
        public List<Double> ph { get; set; }
        public List<Double> temp { get; set; }
        public List<Double> chlorine { get; set; }
        public List<string> serial_site { get; set; }

        public DateTime dateplot { get; set; }
    }
}