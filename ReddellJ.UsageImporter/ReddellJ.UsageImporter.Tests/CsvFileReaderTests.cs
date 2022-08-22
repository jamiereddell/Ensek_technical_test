using Moq;
using ReddellJ.UsageImporter.Domain;

namespace ReddellJ.UsageImporter.Tests
{
    [TestFixture]
    public class CsvFileReaderTests
    {
        [Test]
        public void ReadFile_ReturnsMeterReadingsForValidData()
        {
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(x => x.ReadAllText("readings.csv")).Returns(@"AccountId, Reading
10001, 00100,");

            var reader = new CsvFileReader(fileSystem.Object);
            var result = reader.ReadFile("readings.csv");

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Total, Is.EqualTo(1));
                Assert.That(result.Usages.Count, Is.EqualTo(1));
                Assert.That(result.Usages[0].AccountId, Is.EqualTo(10001));
                Assert.That(result.Usages[0].Reading, Is.EqualTo("00100"));
            });
        }

        [Test]
        public void ReadFile_DiscardsMeterReadingsWithInvalidAccountNumbers()
        {
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(x => x.ReadAllText("readings.csv")).Returns(@"AccountId, Reading
ABC, 00100,");

            var reader = new CsvFileReader(fileSystem.Object);
            var result = reader.ReadFile("readings.csv");

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Total, Is.EqualTo(1));
                Assert.That(result.Invalid, Is.EqualTo(1));
                Assert.That(result.Usages.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void ReadFile_DiscardsMeterReadingsWithInvalidReadings()
        {
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(x => x.ReadAllText("readings.csv")).Returns(@"AccountId, Reading
1001, 100,");

            var reader = new CsvFileReader(fileSystem.Object);
            var result = reader.ReadFile("readings.csv");

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Total, Is.EqualTo(1));
                Assert.That(result.Invalid, Is.EqualTo(1));
                Assert.That(result.Usages.Count, Is.EqualTo(0));
            });
        }
    }
}
