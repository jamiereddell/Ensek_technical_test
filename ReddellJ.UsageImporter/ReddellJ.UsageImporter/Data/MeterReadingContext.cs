using Microsoft.EntityFrameworkCore;
using ReddellJ.UsageImporter.Data.Entities;
using ReddellJ.UsageImporter.Data.Entities.Mapping;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DbContext = System.Data.Entity.DbContext;

namespace ReddellJ.UsageImporter.Data
{
    public class MeterReadingContext : DbContext, IMeterReadingContext
    {
        public IDbSet<Usage> Usages { get; set; }
        public IDbSet<UsageFile> UsageFiles { get; set; }
        public IDbSet<Account> Accounts { get; set; }

        static MeterReadingContext()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MeterReadingContext>());
        }

        public MeterReadingContext(string connectionString) : base(connectionString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new UsageMap());
            modelBuilder.Configurations.Add(new UsageFileMap());
            modelBuilder.Configurations.Add(new AccountMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
