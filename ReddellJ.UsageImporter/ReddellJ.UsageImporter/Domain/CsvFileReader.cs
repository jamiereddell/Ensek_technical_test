using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Domain
{
    public class CsvFileReader : ICsvFileReader
    {
        private IFileSystem fileSystem;

        public CsvFileReader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public CsvReaderFileResult ReadFile(string filename)
        {
            var fileContents = fileSystem.ReadAllText(filename);
            var fileLines = fileContents.Split("\n");

            //Assumption that the first row is column headings, if it is comment below
            fileLines = fileLines.Skip(1).ToArray();
            var readings = new List<Usage>();

            foreach (var line in fileLines)
            {
                var lineParts = line.Split(",", StringSplitOptions.RemoveEmptyEntries);

                if (lineParts.Length == 2)
                {
                    if (int.TryParse(lineParts[0].Trim(), out var accountId) && lineParts[1].Trim().Length == 5)
                    {
                        readings.Add(new Usage { AccountId = accountId, Reading = lineParts[1].Trim() });
                    }                    
                }
            }

            return new CsvReaderFileResult { 
                Total = fileLines.Length, 
                Invalid = fileLines.Length - readings.Count,  
                Usages = readings.ToArray() 
            };
        }
    }
}
