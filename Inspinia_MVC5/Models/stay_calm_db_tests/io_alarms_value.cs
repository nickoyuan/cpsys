using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class io_alarms_value
    {
        [Key]
        public int Id { get; set; }

        public string value { get; set; }

        public DateTime date { get; set; }

        [ForeignKey("siteuser")]
        public int alarm_link_id { get; set; }
        public virtual siteuser siteuser { get; set; }
    }
}