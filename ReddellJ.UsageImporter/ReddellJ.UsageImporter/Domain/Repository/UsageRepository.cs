using ReddellJ.UsageImporter.Data;
using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Domain.Repository
{
    public class UsageRepository : IUsageRepository
    {
        private readonly IMeterReadingContext context;

        public UsageRepository(IMeterReadingContext context)
        {
            this.context = context;
        }

        public bool Save(Usage usage)
        {
            context.Usages.Add(usage);
            return context.SaveChanges() == 1;
        }
    }
}
