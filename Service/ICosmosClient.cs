using CosmosTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosTest.Service
{
    public interface ICosmosClient
    {
        Task<List<LeadSummary>> GetLeadsAsync(LeadsSearch search);
    }
}
