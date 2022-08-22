namespace ReddellJ.UsageImporter.Domain
{
    public interface IFileSystem
    {
        string ReadAllText(string filename);
    }
}
