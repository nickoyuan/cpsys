using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class response_comms
    {
        [Key]
        public int id { get; set; }

        public int response_bit { get; set; }

        public double frequency { get; set; }

        public double once_on_server { get; set; }

        [ForeignKey("sitetoboards")]
        public int comms_board_id { get; set; }

        public virtual sitetoboards sitetoboards { get; set; }
    }
}