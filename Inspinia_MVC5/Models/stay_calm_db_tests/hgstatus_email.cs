using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class hgstatus_email
    {
        [Key]
        public int id { get; set; }

        public int alert_type_number { get; set; }

        public int send_type_number { get; set; }

        [ForeignKey("newusers")]
        public int userid_to_error { get; set; }

        public virtual newusers newusers { get; set; }
    }
}
