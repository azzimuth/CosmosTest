using CosmosTest.Models;
using CosmosTest.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CosmosTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly ILogger<LeadsController> _logger;
        private readonly ICosmosClient _cosmosClient;

        public LeadsController(ILogger<LeadsController> logger, ICosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] LeadsSearch query)
        {
            var result = _cosmosClient.GetLeadsAsync(query);
            return Ok(result);
        }
    }
}
