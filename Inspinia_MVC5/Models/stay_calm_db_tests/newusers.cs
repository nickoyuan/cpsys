using stay_calm_db_tests;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;


// namespace "The title of the DB collection"
namespace stay_calm_db_tests
{    // File name
    public class newusers
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        //                       plot property is navigation property. 
        // Navigation property hold other entities that are related to this entity. 
        // 
        //  before we had -> public ICollection<graphs> graphs { get; set;}
        public virtual ICollection<hgstatus_email> hgstatus_email { get; set; }

    }

    // File name + Context
    // newusersContext is stay_calm.Models.newusersContext db that gets created 
    // and DbSet <newusers> USERS is the dbo.newusers 
    // If i change the name of newusersContext to plotContext : DbContext
    // Then a new stay_calm.Models.plotContext gets created 
    public class userContext : DbContext
    {    // File name

        public userContext()
        {
            var adapter = (IObjectContextAdapter)this;
            var objectContext = adapter.ObjectContext;
            objectContext.CommandTimeout = 900000; // value in seconds
        }
        

        public DbSet<newusers> USERS { get; set; }
        public DbSet<graphs> graphs { get; set; }
        public DbSet<siteuser> siteuser { get; set; }
        public DbSet<graph_limit> graph_limit { get; set; }

        public DbSet<site_user_link> site_user_link { get; set; }

        public DbSet<graph_data_type> graph_data_type { get; set; }

        public DbSet<response_comms> response_comms { get; set; }

        public DbSet<chparamdbs> chparamdbs { get; set; }

        public DbSet<hgstatus_db> hgstatus_db { get; set; }

        public DbSet<hgstatus_all> hgstatus_all { get; set; }

        public DbSet<hgstatus_link> hgstatus_link { get; set; }

        public DbSet<io_alarms> io_alarms { get; set; }

        public DbSet<sitetoboards> sitetoboards { get; set; }

        public DbSet<io_alarms_value> io_alarms_value { get; set; }

        public DbSet<hgstatus_email> hgstatus_email { get; set; }

        public DbSet<response_string> response_string { get; set; }

        public DbSet<graph_update> graph_update { get; set; }

    }




}