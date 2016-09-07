using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WLX.Data.Models;

namespace WLX.Data
{
    public class EntityContext : DbContext
    {
        public EntityContext()
            : base("name = DefaultConnection")
        {
            Database.SetInitializer<EntityContext>(null);
        }
        public DbSet<Customer> Customers { get; set; }
        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<HotmomSchool> HotmomSchool { get; set; }
        public DbSet<SMSLog> SMSLogs { get; set; }
        public DbSet<UserCheckMobile> UserCheckMobiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); //被用来防止生成复数表名
        }

    }
}
