using checkPIVABatch.DTOs;

namespace checkPIVABatch.Interfaces
{
    internal interface IVatService
    {
        public Task<Result<CheckVATNumberResponseDTO>> CheckVATNumber(CheckVATNumberPostDTO checkVATNumberPostDTO);
    }
}
