using NBPNeo4J.Models;
using Neo4j.Driver;
using System.Runtime.CompilerServices;

namespace NBPNeo4J.Repositories
{

    public interface IHubRepository
    {
        Task<Hub> CreateHubAsync(Hub hub);
        Task ConnectTwoHubs(string hub1Id, string hub2Id, double distance);
        Task RemoveHubConnection(string hub1Id, string hub2Id);
        Task<Hub> GetHubAsync(string id);
        Task DeleteHubAsync(string id);
        Task<Hub> UpdateHubInformationAsync(Hub hub);
        Task<List<Hub>> GetAllHubsAsync();
        //mozda ne treba, vec se vrati taj hub sa svim podacima
        Task<List<Part>> GetAllPartsForHubAsync(string hubId);
        Task AddPartToHubAsync(string hubId, Part part);
        Task<int> IncreasePartQuantityAsync(string hubId, string partId, int addedQuantity);
        Task<int> DecreasePartQuantityAsync(string hubId, string partId, int removedQuantity);

    }

    public class HubRepository : IHubRepository
    {
        private readonly IDriver _driver;

        public HubRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<Hub> CreateHubAsync(Hub hub)
        {
            string query = @"
                CREATE (h:Hub { 
                    Id: $Id, 
                    Name: $Name, 
                    Address: $Address, 
                    City: $City, 
                    Latitude: $Latitude, 
                    Longitude: $Longitude 
                })
                RETURN h.Id AS Id, h.Name AS Name, h.Address AS Address, h.City AS City, h.Latitude AS Latitude, h.Longitude AS Longitude";

            var parameters = new
            {
                hub.Id,
                hub.Name,
                hub.Address,
                hub.City,
                hub.Latitude,
                hub.Longitude
            };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();


            return new Hub
            {
                Id = queryResults[0]["Id"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Address = queryResults[0]["Address"].As<string>(),
                City = queryResults[0]["City"].As<string>(),
                Latitude = queryResults[0]["Latitude"].As<double>(),
                Longitude = queryResults[0]["Longitude"].As<double>()
            };
        }

        public async Task ConnectTwoHubs(string hub1Id, string hub2Id, double distance)
        {
            string query = @"
                MATCH (h1:Hub { Id: $Id1 }), (h2:Hub { Id: $Id2 })
                CREATE (h1)-[:HUB_CONNECTED_TO_HUB { Distance: $Distance }]->(h2)
                CREATE (h2)-[:HUB_CONNECTED_TO_HUB { Distance: $Distance }]->(h1)";

            var parameters = new
            {
                Id1 = hub1Id,
                Id2 = hub2Id,
                Distance = distance
            };

            await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
        }

        public async Task RemoveHubConnection(string hub1Id, string hub2Id)
        {
            string query = @"
                MATCH (h1:Hub { Id: $Id1 })-[r:HUB_CONNECTED_TO_HUB]->(h2:Hub { Id: $Id2 })
                DELETE r
                MATCH (h1:Hub { Id: $Id2 })-[r:HUB_CONNECTED_TO_HUB]->(h2:Hub { Id: $Id1 })
                DELETE r";

            var parameters = new
            {
                Id1 = hub1Id,
                Id2 = hub2Id
            };

            await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
        }

        public async Task<Hub> GetHubAsync(string id)
        {
            string query = @"
                MATCH (h:Hub { Id: $Id })
                OPTIONAL MATCH (h)-[r:HUB_CONNECTED_TO_HUB]->(connectedHub:Hub)
                RETURN h, collect(r) AS relationships, collect(connectedHub) AS connectedHubs";

            var parameters = new { Id = id };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            var hubNode = queryResults[0]["h"].As<INode>();
            var relationships = queryResults[0]["relationships"].As<List<IRelationship>>();
            var connectedHubs = queryResults[0]["connectedHubs"].As<List<INode>>();

            var hub = new Hub
            {
                Id = hubNode["Id"].As<string>(),
                Name = hubNode["Name"].As<string>(),
                Address = hubNode["Address"].As<string>(),
                City = hubNode["City"].As<string>(),
                Latitude = hubNode["Latitude"].As<double>(),
                Longitude = hubNode["Longitude"].As<double>(),
                ConnectedHubs = new List<HubConnectedToHub<Hub>>()
            };
            for (int i = 0; i < connectedHubs.Count; i++)
            {
                var connectedHubNode = connectedHubs[i];
                var relationship = relationships[i];

                var connectedHub = new Hub
                {
                    Id = connectedHubNode["Id"].As<string>(),
                    Name = connectedHubNode["Name"].As<string>(),
                    Address = connectedHubNode["Address"].As<string>(),
                    City = connectedHubNode["City"].As<string>(),
                    Latitude = connectedHubNode["Latitude"].As<double>(),
                    Longitude = connectedHubNode["Longitude"].As<double>()
                };

                hub.ConnectedHubs.Add(new HubConnectedToHub<Hub>(connectedHub, relationship["Distance"].As<double>()));
            }
            return hub;

        }

        public async Task DeleteHubAsync(string id)
        {
            string query = "MATCH (h:Hub { Id: $Id }) DETACH DELETE h";
            var parameters = new { Id = id };
            var (_, information) = await _driver.ExecutableQuery(query).WithParameters(parameters).ExecuteAsync();

            if (information.Counters.NodesDeleted != 1)
            {
                throw new Exception("Hub not found");
            }
        }

        public async Task<Hub> UpdateHubInformationAsync(Hub hub)
        {
            string query = @"
                MATCH (h:Hub { Id: $Id })
                SET h.Name = $Name, h.Address = $Address, h.City = $City, h.Latitude = $Latitude, h.Longitude = $Longitude
                RETURN h.Id AS Id, h.Name AS Name, h.Address AS Address, h.City AS City, h.Latitude AS Latitude, h.Longitude AS Longitude";

            var parameters = new { hub.Id, hub.Name, hub.Address, hub.City, hub.Latitude, hub.Longitude };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            return new Hub
            {
                Id = queryResults[0]["Id"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Address = queryResults[0]["Address"].As<string>(),
                City = queryResults[0]["City"].As<string>(),
                Latitude = queryResults[0]["Latitude"].As<double>(),
                Longitude = queryResults[0]["Longitude"].As<double>()
            };
        }

        public async Task<List<Hub>> GetAllHubsAsync()
        {
            string query = @"
                MATCH (h:Hub)
                OPTIONAL MATCH (h)-[r:HUB_CONNECTED_TO_HUB]->(connectedHub:Hub)
                RETURN h, collect(r) AS relationships, collect(connectedHub) AS connectedHubs";

            var (queryResults, executionInformation) = await _driver.ExecutableQuery(query).ExecuteAsync();

            var hubs = new List<Hub>();

            foreach (var queryResult in queryResults)
            {
                var hubNode = queryResult["h"].As<INode>();
                var relationships = queryResult["relationships"].As<List<IRelationship>>();
                var connectedHubs = queryResult["connectedHubs"].As<List<INode>>();

                var hub = new Hub
                {
                    Id = hubNode["Id"].As<string>(),
                    Name = hubNode["Name"].As<string>(),
                    Address = hubNode["Address"].As<string>(),
                    City = hubNode["City"].As<string>(),
                    Latitude = hubNode["Latitude"].As<double>(),
                    Longitude = hubNode["Longitude"].As<double>(),
                    ConnectedHubs = new List<HubConnectedToHub<Hub>>()
                };
                for (int i = 0; i < connectedHubs.Count; i++)
                {
                    var connectedHubNode = connectedHubs[i];
                    var relationship = relationships[i];

                    var connectedHub = new Hub
                    {
                        Id = connectedHubNode["Id"].As<string>(),
                        Name = connectedHubNode["Name"].As<string>(),
                        Address = connectedHubNode["Address"].As<string>(),
                        City = connectedHubNode["City"].As<string>(),
                        Latitude = connectedHubNode["Latitude"].As<double>(),
                        Longitude = connectedHubNode["Longitude"].As<double>()
                    };

                    hub.ConnectedHubs.Add(new HubConnectedToHub<Hub>(connectedHub, relationship["Distance"].As<double>()));
                }

                hubs.Add(hub);
            }

            return hubs;
        }

        public async Task<List<Part>> GetAllPartsForHubAsync(string hubId)
        {
            throw new NotImplementedException();
        }

        public async Task AddPartToHubAsync(string hubId, Part part)
        {
            throw new NotImplementedException();
        }

        public async Task<int> IncreasePartQuantityAsync(string hubId, string partId, int addedQuantity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DecreasePartQuantityAsync(string hubId, string partId, int removedQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
