using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stay_calm_db_tests
{
    public class graph_data_type
    {
        [Key]
        public int Id { get; set; }

        public string data_type { get; set; }

        public int serial_id { get; set; }

        public virtual ICollection<stay_calm_db_tests.graph_limit> graph_limit { get; set; }
    }
}