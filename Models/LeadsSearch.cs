using System;

namespace CosmosTest.Models
{
    public class LeadsSearch
    {
        public string MarketCode { get; set; }
        public string CountryCode { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }        

        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
