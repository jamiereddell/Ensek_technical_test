using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Domain.Repository
{
    public interface IAccountRepository
    {
        Account Get(int accountId);
    }
}
