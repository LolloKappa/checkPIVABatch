using checkPIVABatch.Interfaces;
using checkPIVABatch.Models;
using Microsoft.Extensions.Logging;

namespace checkPIVABatch.Services
{
    internal class RepositoryService: IRepositoryService
    {
        private readonly ILogger _logger;
        private readonly CheckIVABatchDBContext _checkIVABatchDBContext;
        public RepositoryService(ILogger<RepositoryService> logger, CheckIVABatchDBContext checkIVABatchDBContext) 
        {
            _logger = logger;
            _checkIVABatchDBContext = checkIVABatchDBContext;
        }

        public IEnumerable<TaxInterrogationHistory> GetAllTaxInterrogationHistory()
        {
            _logger.LogInformation("Get All TaxInterrogationHistory");
            return _checkIVABatchDBContext.taxInterrogationsHistories.ToList();
        }
    }
}
