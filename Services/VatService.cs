using checkPIVABatch.Interfaces;
using Microsoft.Extensions.Logging;

namespace checkPIVABatch.Services
{
    internal class VatService: IVatService
    {
        private readonly ILogger _logger;
        public VatService(ILogger<VatService> logger) 
        {
            _logger = logger;
        }
    }
}
