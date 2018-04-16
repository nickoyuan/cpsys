using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class graph_limit
    {
        [Key]
        public int id { get; set; }

        public double upper_limit_to { get; set; }

        public double upper_limit_from { get; set; }

        public double lower_limit_to { get; set; }

        public double lower_limit_from { get; set; }

        [ForeignKey("graph_data_type")]
        public int graph_data_type_id { get; set; }

        public virtual graph_data_type graph_data_type { get; set; }

        [ForeignKey("siteuser")]
        public int site_id_to_limit { get; set; }

        public virtual siteuser siteuser { get; set; }

        public virtual ICollection<stay_calm_db_tests.graphs> graphs { get; set; }
    }
}