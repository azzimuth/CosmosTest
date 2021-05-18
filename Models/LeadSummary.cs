using System;

namespace CosmosTest.Models
{
    public class LeadSummary
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string Market { get; set; }
        public LeadRequest LeadRequest { get; set; }
    }

    public class LeadRequest
    {
        public string CountryCode { get; set; }
    }
}
