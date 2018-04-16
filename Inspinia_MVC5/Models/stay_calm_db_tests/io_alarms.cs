using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class io_alarms
    {
        [Key]
        public int Id { get; set; }

        public int switch_number { get; set; }

        public string alarm_name { get; set; }

        [ForeignKey("siteuser")]
        public int site_id_link { get; set; }

        public virtual siteuser siteuser { get; set; }
    }
}
