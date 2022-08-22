namespace ReddellJ.UsageImporter.Domain
{
    public interface ICsvFileReader
    {
        CsvReaderFileResult ReadFile(string filename);
    }
}
