using System.Data.Entity.ModelConfiguration;

namespace ReddellJ.UsageImporter.Data.Entities.Mapping
{
    public class UsageMap : EntityTypeConfiguration<Usage>
    {
        public UsageMap()
        {
            HasKey(x => x.Id);
            Property(x => x.Reading).IsRequired().HasMaxLength(5);
            Property(x => x.AccountId).IsRequired();
        }
    }
}
