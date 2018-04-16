using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newtest.Models.sitelistmodel
{
    public class usermodellist
    {
        public List<string> username { get; set; }

        public List<string> password { get; set; }

        public List<string> sitesattached { get; set; }

        public List<string> email { get; set; }
    }
}