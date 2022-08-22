using ReddellJ.UsageImporter.Data;
using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Domain.Repository
{

    public class UsageFileRepository : IUsageFileRepository
    {
        private readonly IMeterReadingContext context;

        public UsageFileRepository(IMeterReadingContext context)
        {
            this.context = context;
        }

        public UsageFile Get(string filename)
        {
            return context.UsageFiles.FirstOrDefault(x => x.FileName == filename);
        }

        public bool Save(string filename)
        {
            context.UsageFiles.Add(new UsageFile { FileName = filename });
            return context.SaveChanges() == 1;
        }
    }

}
