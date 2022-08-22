using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Domain.Repository
{
    public interface IUsageRepository
    {
        bool Save(Usage reading);
    }
}
