using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Domain
{
    public class CsvReaderFileResult
    {
        public int Total { get; set; }
        public int Invalid { get; set; }
        public Usage[] Usages { get; set; }
    }
}
