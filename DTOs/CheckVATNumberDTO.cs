
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
        public string CountryCode { get; set; }
        public string VatNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public bool Valid { get; set; }
        public string RequestIdentifier { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TraderName { get; set; }
        public string TraderStreet { get; set; }
        public string TraderPostalCode { get; set; }
        public string TraderCity { get; set; }
        public string TraderCompanyType { get; set; }
        public string TraderNameMatch { get; set; }
        public string TraderStreetMatch { get; set; }
        public string TraderPostalCodeMatch { get; set; }
        public string TraderCityMatch { get; set; }
        public string TraderCompanyTypeMatch { get; set; }
    }
}
