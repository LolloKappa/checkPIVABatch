using checkPIVABatch.DTOs;
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

        public Result<IEnumerable<TaxInterrogationHistory>> GetAllTaxInterrogationHistory()
        {
            _logger.LogInformation("Get All TaxInterrogationHistory");
            var result = new Result<IEnumerable<TaxInterrogationHistory>>()
            {
                Data = null,
                Success = false
            };

            try
            {
                var taxInterrogationHistories = _checkIVABatchDBContext.taxInterrogationsHistories.ToList();
                result.Data = taxInterrogationHistories;
                result.Success = true;
            }
            catch (Exception ex)
            {
                var errorMessage = string.Concat("Error during DB query: ", ex.Message);
                _logger.LogError(errorMessage);
                result.Message = errorMessage;
                result.Success = false;
            }

            return result;
        }

        public Result<TaxInterrogationHistory> GetTaxInterrogationHistoryById(int id)
        {
            _logger.LogInformation("Get TaxInterrogationHistory by Id");
            var result = new Result<TaxInterrogationHistory>()
            {
                Data = null,
                Success = false
            };

            try
            {
                var taxInterrogationHistory = _checkIVABatchDBContext.taxInterrogationsHistories.FirstOrDefault(x => x.Id == id);
                if (taxInterrogationHistory is null)
                {
                    var errorMessage = string.Concat("TaxInterrogationHistory with Id ", id, " not found");
                    _logger.LogWarning(errorMessage);
                    result.Success = false;
                    result.Message = errorMessage;

                    return result;
                }

                result.Data = taxInterrogationHistory;
                result.Success = true;
            }
            catch (Exception ex)
            {
                var errorMessage = string.Concat("Error during DB query: ", ex.Message);
                _logger.LogError(errorMessage);
                result.Message = errorMessage;
                result.Success = false;
            }

            return result;
        }

        public Result<TaxInterrogationHistory> AddTaxInterrogationHistory(TaxInterrogationHistory taxInterrogationHistoryToAdd)
        {
            _logger.LogInformation("Add TaxInterrogationHistory");

            var result = new Result<TaxInterrogationHistory>()
            {
                Data = taxInterrogationHistoryToAdd,
                Success = false
            };

            try
            {
                var insertedTaxInterrogationHistory = _checkIVABatchDBContext.taxInterrogationsHistories.Add(taxInterrogationHistoryToAdd);
                _checkIVABatchDBContext.SaveChanges();

                // return the inserted entity with associated Id
                _logger.LogInformation($"TaxInterrogationHistory with Id {insertedTaxInterrogationHistory.Entity.Id} added to DB");
                result.Data = insertedTaxInterrogationHistory.Entity;
                result.Success = true;
            }
            catch (Exception ex)
            {
                var errorMessage = string.Concat("Error during DB insertion: ", ex.Message);
                _logger.LogError(errorMessage);
                result.Message = errorMessage;
                result.Success = false;
            }

            return result;
        }

        public Result<TaxInterrogationHistory> UpdateTaxInterrogationHistory(int id, TaxInterrogationHistory taxInterrogationHistoryToUpdate)
        {
            _logger.LogInformation("Update TaxInterrogationHistory");

            var result = new Result<TaxInterrogationHistory>()
            {
                Data = taxInterrogationHistoryToUpdate,
                Success = false
            };

            try
            {
                // check if the entity exists
                var taxInterrogationHistoryResult = this.GetTaxInterrogationHistoryById(id);
                if (taxInterrogationHistoryResult.Success == false)
                    return taxInterrogationHistoryResult;

                var updatedTaxInterrogationHistory = _checkIVABatchDBContext.taxInterrogationsHistories.Update(taxInterrogationHistoryToUpdate);
                _checkIVABatchDBContext.SaveChanges();
                result.Data = updatedTaxInterrogationHistory.Entity;
                result.Success = true;
            }
            catch (Exception ex)
            {
                var errorMessage = string.Concat("Error during DB insertion: ", ex.Message);
                _logger.LogError(errorMessage);
                result.Message = errorMessage;
                result.Success = false;
            }

            // return the updated entity
            return result;
        }
    }
}
