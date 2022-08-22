using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Domain.Repository
{
    public interface IUsageFileRepository
    {
        UsageFile Get(string filename);
        bool Save(string filename);
    }

}
