using NBPNeo4J.Models;
using Neo4j.Driver;
using System.Runtime.CompilerServices;

namespace NBPNeo4J.Repositories
{

    public interface IHubRepository
    {
        Task<Hub> CreateHubAsync(Hub hub);
        Task<Hub> GetHubAsync(string id);
        Task DeleteHubAsync(string id);
        Task<Hub> UpdateHubAsync(Hub hub);
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
            throw new NotImplementedException();
        }

        public async Task<Hub> GetHubAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteHubAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Hub> UpdateHubAsync(Hub hub)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Hub>> GetAllHubsAsync()
        {
            throw new NotImplementedException();
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
