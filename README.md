# Steps to reproduce
1. Download and install Azure Cosmos DB Emulator. Clone the code from this repository
2. Create databases called `CosmosTest`and a container called `Leads`. Set storage capacity to unlimited and the partition key to `/market`
3. Update the `appsetting.json` or user secrets in VisualStudio with corresponding endpoint and primary key
4. Run the solution
5. Set breakpoint at `GetLeadsAsync` method in `CosmosDbClient`
6. Navigate to https://localhost:44316/leads?countryCode=GB and validate that you get a `NullReferenceException` at `var count = await cosmosQuery.CountAsync();` in `CosmosDbClient`
7. Navigate to https://localhost:44316/leads?countryCode=GB&marketCode=GB and verify that you do not get an exception