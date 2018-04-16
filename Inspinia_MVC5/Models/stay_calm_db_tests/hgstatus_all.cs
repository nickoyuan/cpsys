using System.Collections.Generic;

namespace stay_calm_db_tests
{
    public class hgstatus_all
    {
        public int id { get; set; }

        public string error_types { get; set; }

        public virtual ICollection<stay_calm_db_tests.hgstatus_link> hgstatus_link { get; set; }
    }
}
