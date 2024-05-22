using System.Text.Json.Serialization;

namespace checkPIVABatch.DTOs
{
    internal class CheckVATNumberPostDTO
    {
        public string CountryCode { get; set; }
        public string VatNumber { get; set; }
        public string RequesterMemberStateCode { get; set; }
        public string RequesterNumber { get; set; }
        public string TraderName { get; set; }
        public string TraderStreet { get; set; }
        public string TraderPostalCode { get; set; }
        public string TraderCity { get; set; }
        public string TraderCompanyType { get; set; }
    }

    internal class CheckVATNumberResponseDTO
    {
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
        [JsonPropertyName("vatNumber")]
        public string VatNumber { get; set; }
        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get; set; }
        [JsonPropertyName("valid")]
        public bool Valid { get; set; }
        [JsonPropertyName("requestIdentifier")]
        public string RequestIdentifier { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("traderName")]
        public string TraderName { get; set; }
        [JsonPropertyName("traderStreet")]
        public string TraderStreet { get; set; }
        [JsonPropertyName("traderPostalCode")]
        public string TraderPostalCode { get; set; }
        [JsonPropertyName("traderCity")]
        public string TraderCity { get; set; }
        [JsonPropertyName("traderCompanyType")]
        public string TraderCompanyType { get; set; }
        [JsonPropertyName("traderNameMatch")]
        public string TraderNameMatch { get; set; }
        [JsonPropertyName("traderStreetMatch")]
        public string TraderStreetMatch { get; set; }
        [JsonPropertyName("traderPostalCodeMatch")]
        public string TraderPostalCodeMatch { get; set; }
        [JsonPropertyName("traderCityMatch")]
        public string TraderCityMatch { get; set; }
        [JsonPropertyName("traderCompanyTypeMatch")]
        public string TraderCompanyTypeMatch { get; set; }
    }

    internal class CheckVATNumberResponseErrorDTO
    {
        [JsonPropertyName("actionSucceed")]
        public bool ActionSucceed { get; set; }
        [JsonPropertyName("errorWrappers")]
        public List<ErrorWrapperDTO> ErrorWrappers { get; set; }
    }

    public class ErrorWrapperDTO
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
