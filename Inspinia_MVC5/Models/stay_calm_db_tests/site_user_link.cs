using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class site_user_link
    {
        public int ID { get; set; }

        public int newusersID { get; set; }

        [ForeignKey("newusersID")]
        public virtual newusers newusers { get; set; }

        public int siteuserId { get; set; }

        [ForeignKey("siteuserId")]
        public virtual siteuser siteuser { get; set; }
    }
}
