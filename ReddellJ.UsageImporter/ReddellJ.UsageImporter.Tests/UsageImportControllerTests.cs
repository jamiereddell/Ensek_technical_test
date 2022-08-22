using Moq;
using ReddellJ.UsageImporter.Domain;
using ReddellJ.UsageImporter.Controllers;
using Microsoft.AspNetCore.Mvc;
using ReddellJ.UsageImporter.Domain.Repository;
using ReddellJ.UsageImporter.Data.Entities;

namespace ReddellJ.UsageImporter.Tests
{
    public class MeterReadingContollerTests
    {
        private Mock<ICsvFileReader> fileReader;
        private Mock<IAccountRepository> accountRepository;
        private Mock<IUsageRepository> usageRepository;
        private Mock<IUsageFileRepository> usageFileRepository;
        private MeterReadingContoller controller;

        [SetUp]
        public void Setup()
        {
            fileReader = new Mock<ICsvFileReader>();
            accountRepository = new Mock<IAccountRepository>();
            usageRepository = new Mock<IUsageRepository>();
            usageFileRepository = new Mock<IUsageFileRepository>();

            controller = new MeterReadingContoller(
                fileReader.Object, 
                accountRepository.Object, 
                usageRepository.Object, 
                usageFileRepository.Object);
        }
        [Test]
        public void FilenameIsRequired()
        {
            var response = controller.Upload(null);

            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void FileIsRead()
        {
            fileReader.Setup(x => x.ReadFile("sample.csv"))
                .Returns(new CsvReaderFileResult { 
                    Total = 1, 
                    Invalid = 0, 
                    Usages = new [] { new Usage { AccountId = 1001, Reading = "00123" } } 
                });
            accountRepository.Setup(x => x.Get(1001)).Returns(new Account { Id = 1001, Name = "ACME" });

            var response = controller.Upload("sample.csv");

            fileReader.Verify(x => x.ReadFile("sample.csv"), Times.Once);
        }

        [Test]
        public void UsageIsInserted()
        {
            fileReader.Setup(x => x.ReadFile("sample.csv"))
                .Returns(new CsvReaderFileResult
                {
                    Total = 1,
                    Invalid = 0,
                    Usages = new[] { new Usage { AccountId = 1001, Reading = "00123" } }
                });
            accountRepository.Setup(x => x.Get(1001)).Returns(new Account { Id = 1001, Name = "ACME" });

            var response = controller.Upload("sample.csv");

            usageRepository.Verify(x => x.Save(It.Is<Usage>(x => x.AccountId == 1001 && x.Reading == "00123")), Times.Once);
        }

        [Test]
        public void UsageFileIsInserted()
        {
            fileReader.Setup(x => x.ReadFile("sample.csv"))
               .Returns(new CsvReaderFileResult
               {
                   Total = 1,
                   Invalid = 0,
                   Usages = new[] { new Usage { AccountId = 1001, Reading = "00123" } }
               });
            accountRepository.Setup(x => x.Get(1001)).Returns(new Account { Id = 1001, Name = "ACME" });

            var response = controller.Upload("sample.csv");

            usageFileRepository.Verify(x => x.Save("sample.csv"), Times.Once);
        }

        [Test]
        public void UsageFileCannotBeProcessedTwice()
        {
            fileReader.Setup(x => x.ReadFile("sample.csv"))
               .Returns(new CsvReaderFileResult
               {
                   Total = 1,
                   Invalid = 0,
                   Usages = new[] { new Usage { AccountId = 1001, Reading = "00123" } }
               });
            accountRepository.Setup(x => x.Get(1001)).Returns(new Account { Id = 1001, Name = "ACME" });
            usageFileRepository.Setup(x => x.Get("sample.csv")).Returns(new UsageFile { FileName = "sample.csv" });

            var response = controller.Upload("sample.csv");

            Assert.That(response, Is.InstanceOf<ConflictObjectResult>());
        }

        [Test]
        public void UsageIsNotInsertedForAccountsThatDoNotExist()
        {
            fileReader.Setup(x => x.ReadFile("sample.csv"))
               .Returns(new CsvReaderFileResult
               {
                   Total = 1,
                   Invalid = 0,
                   Usages = new[] { new Usage { AccountId = 1001, Reading = "00123" } }
               });
            var response = controller.Upload("sample.csv");

            usageRepository.Verify(x => x.Save(It.Is<Usage>(x => x.AccountId == 1001 && x.Reading == "00123")), Times.Never);
        }

        [Test]
        public void StatsReturnedForSuccessfulInserts()
        {
            fileReader.Setup(x => x.ReadFile("sample.csv"))
               .Returns(new CsvReaderFileResult
               {
                   Total = 2,
                   Invalid = 0,
                   Usages = new[] { 
                       new Usage { AccountId = 1001, Reading = "00123" }, 
                       new Usage {AccountId = 1002, Reading = "00123" }
               }});

            accountRepository.Setup(x => x.Get(1001)).Returns(new Account { Id = 1001, Name = "ACME" });

            var response = controller.Upload("sample.csv") as OkObjectResult;
            Assert.That(response, Is.InstanceOf<OkObjectResult>());

            var responseValue = response.Value as UploadResponse;
            Assert.That(responseValue, Is.Not.Null);
            Assert.That(responseValue.Processed, Is.EqualTo(1));
            Assert.That(responseValue.Invalid, Is.EqualTo(1));
        }
    }

   
}