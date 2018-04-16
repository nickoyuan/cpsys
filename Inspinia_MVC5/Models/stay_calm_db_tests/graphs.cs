using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stay_calm_db_tests
{
    public class graphs
    {
        [Key]
        public int graphid { get; set; }

        public double value { get; set; }

        public DateTime plotdate { get; set; }

        [ForeignKey("graph_limit")]
        public int graph_limit_id { get; set; }

        public virtual graph_limit graph_limit { get; set; }
    }
}