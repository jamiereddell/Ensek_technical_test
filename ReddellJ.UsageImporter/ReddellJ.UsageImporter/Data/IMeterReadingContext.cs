using ReddellJ.UsageImporter.Data.Entities;
using System.Data.Entity;

namespace ReddellJ.UsageImporter.Data
{
    public interface IMeterReadingContext
    {
        int SaveChanges();

        IDbSet<Usage> Usages { get; set; }
        IDbSet<UsageFile> UsageFiles { get; set; }
        IDbSet<Account> Accounts { get; set; }
    }
}