using CosmosTest.Common;
using CosmosTest.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosTest.Service
{
    public class CosmosDbClient : ICosmosClient
    {
        private readonly CosmosOptions _options;
        private readonly CosmosClient _client;

        public CosmosDbClient(IOptions<CosmosOptions> options)
        {
            _options = options.Value;
            _client = new CosmosClient(_options.EndpointUri, _options.PrimaryKey, GetClientOptions(_options));
        }

        public async Task<List<LeadSummary>> GetLeadsAsync(LeadsSearch search)
        {
            Database database = _client.GetDatabase("CosmosTest");
            Container container = database.GetContainer("Leads");

            try
            {
                var response = new List<LeadSummary>();
                var cosmosQuery = CreateCosmosQuery(container, search);
                var count = await cosmosQuery.CountAsync();
                var iterator = cosmosQuery
                    .Skip(search.Skip)
                    .Take(search.Take)
                    .ToFeedIterator();

                while (iterator.HasMoreResults)
                {
                    foreach (var lead in await iterator.ReadNextAsync())
                    {
                        response.Add(lead);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IQueryable<LeadSummary> CreateCosmosQuery(Container container, LeadsSearch search)
        {
            var queryable = container.GetItemLinqQueryable<LeadSummary>()
                .OrderBy(l => l.Created)
                .AsQueryable();

            if (search.From.HasValue)
            {
                queryable = queryable.Where(l => l.Created >= search.From.Value);
            }

            if (search.To.HasValue)
            {
                var toDate = GetInclusiveToDate(search.To.Value);
                queryable = queryable.Where(l => l.Created <= toDate);
            }

            if (!string.IsNullOrWhiteSpace(search.MarketCode))
            {
                queryable = queryable.Where(lead => lead.Market.ToUpper() == search.MarketCode.ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(search.CountryCode))
            {
                queryable = queryable.Where(lead => lead.LeadRequest.CountryCode.ToUpper() == search.MarketCode.ToUpper());
            }

            return queryable;
        }

        private DateTime GetInclusiveToDate(DateTime date)
        {
            return date.TimeOfDay == TimeSpan.Zero ? date.AddDays(1).AddSeconds(-1) : date;
        }

        private CosmosClientOptions GetClientOptions(CosmosOptions options)
        {
            return new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Direct,
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                },
                ApplicationRegion = options.ApplicationRegion
            };
        }
    }
}
