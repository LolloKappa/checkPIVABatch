namespace checkPIVABatch.Models
{
    internal class TaxInterrogationHistory
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string VatNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public bool Valid { get; set; }
        public string RequestIdentifier { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        override public string ToString()
        {
            return $"{Id} | {CountryCode} | {VatNumber} | {RequestDate} | {Valid} | {RequestIdentifier} | {Name} | {Address}";
        }
    }
}
