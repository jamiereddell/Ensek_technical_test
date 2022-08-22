using System.Data.Entity.ModelConfiguration;

namespace ReddellJ.UsageImporter.Data.Entities.Mapping
{
    public class UsageFileMap : EntityTypeConfiguration<UsageFile>
    {
        public UsageFileMap()
        {
            HasKey(x => x.Id);
            Property(x => x.FileName).IsRequired().IsMaxLength();
        }
    }
}
