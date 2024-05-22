using checkPIVABatch.DTOs;
using checkPIVABatch.Models;

namespace checkPIVABatch.Interfaces
{
    internal interface IRepositoryService
    {
        public Result<IEnumerable<TaxInterrogationHistory>> GetAllTaxInterrogationHistory();
        public Result<TaxInterrogationHistory> GetTaxInterrogationHistoryById(int id);
        public Result<TaxInterrogationHistory> AddTaxInterrogationHistory(TaxInterrogationHistory taxInterrogationHistory);
        public Result<TaxInterrogationHistory> UpdateTaxInterrogationHistory(int id,TaxInterrogationHistory taxInterrogationHistory);
    }
}
