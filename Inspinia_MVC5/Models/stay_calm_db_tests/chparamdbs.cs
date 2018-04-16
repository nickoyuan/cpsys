using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace stay_calm_db_tests
{
    public class chparamdbs
    {
        [Key]
        public int Id { get; set; }
        public Double xparams { get; set; }
        public Double yparams { get; set; }

        public Double zparams { get; set; }
        public Double aparams { get; set; }

        [ForeignKey("sitetoboards")]
        public int serial_to_chparams { get; set; }
        public virtual sitetoboards sitetoboards { get; set; }
    }
}