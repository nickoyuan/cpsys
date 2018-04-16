using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class hgstatus_link
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("siteuser")]
        public int site_serial_id { get; set; }

        public virtual siteuser siteuser { get; set; }

        [ForeignKey("hgstatus_all")]
        public int errors_id { get; set; }

        public virtual hgstatus_all hgstatus_all { get; set; }

        public int alert_type { get; set; }
    }
}
