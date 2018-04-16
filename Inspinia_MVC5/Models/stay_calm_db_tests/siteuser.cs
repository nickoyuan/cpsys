using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class siteuser
    {
        [Key]
        public int Id { get; set; }

        public string user_site { get; set; }

        public string GMT { get; set; }

        public double? longitude { get; set; }

        public double? latitude { get; set; }

        [ForeignKey("sitetoboards")]
        public int serial_board_id { get; set; }

        public virtual sitetoboards sitetoboards { get; set; }

        public virtual ICollection<stay_calm_db_tests.io_alarms_value> io_alarms_value { get; set; }

        public virtual ICollection<stay_calm_db_tests.graph_limit> graph_limit { get; set; }

        public virtual ICollection<stay_calm_db_tests.hgstatus_link> hgstatus_link { get; set; }

        public virtual ICollection<stay_calm_db_tests.hgstatus_db> hgstatus_db { get; set; }

        public virtual ICollection<stay_calm_db_tests.io_alarms> io_alarms { get; set; }
    }
}