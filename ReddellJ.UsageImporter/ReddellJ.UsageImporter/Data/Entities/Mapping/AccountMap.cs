using System.Data.Entity.ModelConfiguration;

namespace ReddellJ.UsageImporter.Data.Entities.Mapping
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            HasKey(x => x.Id);

            Property(x => x.Name).IsRequired().IsMaxLength();
        }
    }
}
