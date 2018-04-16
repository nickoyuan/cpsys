using System;
using System.Collections.Generic;

namespace stay_calm_db_tests
{
    public class sitetoboards
    {
        public int Id { get; set; }

        public string serialboard { get; set; }

        public DateTime lastseen { get; set; }

        public virtual ICollection<stay_calm_db_tests.siteuser> siteuser { get; set; }

        public virtual ICollection<stay_calm_db_tests.response_comms> response_comms { get; set; }
    }
}
