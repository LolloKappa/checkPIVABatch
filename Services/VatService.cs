using checkPIVABatch.DTOs;
using checkPIVABatch.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace checkPIVABatch.Services
{
    internal class VatService: IVatService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public VatService(ILogger<VatService> logger, HttpClient httpClient, IConfiguration configuration) 
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Result<CheckVATNumberResponseDTO>> CheckVATNumber(CheckVATNumberPostDTO checkVATNumberPostDTO)
        {
            _logger.LogInformation("Check VAT Number");
            var result = new Result<CheckVATNumberResponseDTO>()
            {
                Data = null,
                Success = false
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(_configuration["CheckVatNumberEndpoint"], checkVATNumberPostDTO);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var checkVATNumberResponseDTO = JsonSerializer.Deserialize<CheckVATNumberResponseDTO>(responseContent);
                    result.Data = checkVATNumberResponseDTO;
                    result.Success = true;
                }

                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var checkVATNumberResponseDTO = JsonSerializer.Deserialize<CheckVATNumberResponseErrorDTO>(responseContent);
                    var errorMessage = string.Concat("Error during VAT number check, bad request: ", response.ReasonPhrase);
                    _logger.LogError(errorMessage);
                    result.Message = errorMessage;
                    result.Success = false;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var checkVATNumberResponseDTO = JsonSerializer.Deserialize<CheckVATNumberResponseErrorDTO>(responseContent);
                    var errorMessage = string.Concat("Error during VAT number check, bad request: ", response.ReasonPhrase);
                    _logger.LogError(errorMessage);
                    result.Message = errorMessage;
                    result.Success = false;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = string.Concat("Error during VAT number check: ", ex.Message);
                _logger.LogError(errorMessage);
                result.Message = errorMessage;
                result.Success = false;
            }

            return result;
        }
    }
}
