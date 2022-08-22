namespace ReddellJ.UsageImporter.Domain
{
    public class FileSystemAdapter : IFileSystem
    {
        public string ReadAllText(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}
