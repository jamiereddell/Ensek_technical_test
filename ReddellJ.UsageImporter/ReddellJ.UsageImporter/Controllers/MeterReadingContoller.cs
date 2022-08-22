using Microsoft.AspNetCore.Mvc;
using ReddellJ.UsageImporter.Domain;
using ReddellJ.UsageImporter.Domain.Repository;

namespace ReddellJ.UsageImporter.Controllers
{
    [ApiController]
    [Route("meter-reading-uploads")]
    public class MeterReadingContoller : ControllerBase
    {
        private readonly ICsvFileReader fileReader;
        private readonly IAccountRepository accountRepository;
        private readonly IUsageRepository usageRepository;
        private readonly IUsageFileRepository usageFileRepository;

        public MeterReadingContoller(ICsvFileReader fileReader, IAccountRepository accountRepository, IUsageRepository usageRepository, IUsageFileRepository usageFileRepository)
        {
            this.fileReader = fileReader;
            this.accountRepository = accountRepository;
            this.usageRepository = usageRepository;
            this.usageFileRepository = usageFileRepository;
        }

        [HttpPost()]
        public IActionResult Upload(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return BadRequest("filename is required");
            }

            if (usageFileRepository.Get(filename) != null)
            {
                return Conflict($"file {filename} has already been processed");
            }

            var results = fileReader.ReadFile(filename);
            int invalidAccounts = 0;
            foreach (var result in results.Usages)
            {
                var account = accountRepository.Get(result.AccountId);
                if (account == null)
                {
                    invalidAccounts++;
                    continue;
                }

                usageRepository.Save(result);
            }

            usageFileRepository.Save(filename);

            return Ok(
                new UploadResponse
                {
                    Processed = results.Usages.Length - invalidAccounts,
                    Invalid = results.Invalid + invalidAccounts
                });
        }
    }
}