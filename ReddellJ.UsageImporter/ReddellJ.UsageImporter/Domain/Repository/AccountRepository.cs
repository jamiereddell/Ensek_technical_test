using ReddellJ.UsageImporter.Data;
using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Domain.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMeterReadingContext context;

        public AccountRepository(IMeterReadingContext context)
        {
            this.context = context;
        }
        public Account Get(int accountId)
        {
            return context.Accounts.FirstOrDefault(x => x.Id == accountId);
        }
    }
}
