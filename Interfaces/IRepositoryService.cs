using checkPIVABatch.Models;

namespace checkPIVABatch.Interfaces
{
    internal interface IRepositoryService
    {
        public IEnumerable<TaxInterrogationHistory> GetAllTaxInterrogationHistory();
    }
}
