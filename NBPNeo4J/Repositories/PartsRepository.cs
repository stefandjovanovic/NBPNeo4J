using NBPNeo4J.Models;
using Neo4j.Driver;
using System.Runtime.CompilerServices;

namespace NBPNeo4J.Repositories
{
    public interface IPartsRepository
    {
        Task<Part> CreatePartAsync(Part part, string partCategoryId);
        Task<Part> GetPartAsync(string serialCode);
        Task<Part> UpdatePartInformationAsync(Part part);
        Task DeletePartAsync(string serialCode);
        Task<List<Part>> GetAllPartsAsync();

        Task<PartCategory> GetPartCategoryAsync(string id);
        Task<IEnumerable<PartCategory>> GetPartCategoriesAsync(); 
        Task<PartCategory> CreatePartCategoryAsync(PartCategory partCategory); 
        Task<PartCategory> UpdatePartCategoryAsync(PartCategory partCategory); 
        Task DeletePartCategoryAsync(string id);

        Task<List<Part>> GetAllPartsOfCategoryAsync(string categoryId);
    }
    public class PartsRepository : IPartsRepository
    {
        private readonly IDriver _driver;
        public PartsRepository(IDriver driver) 
        {
            _driver = driver;
        }

        public async Task<Part> CreatePartAsync(Part part, string partCategoryId)
        {
            string query = @"
                CREATE (p:Part {
                    SerialCode: $SerialCode,
                    Name: $Name,
                    Description: $Description,
                    Image: $Image,
                    Price: $Price,
                    PartCategoryId: $PartCategoryId
                })
                WITH p
                MATCH (pc:PartCategory {Id: $PartCategoryId})
                CREATE (p)-[:PART_BELONGS_TO_CATEGORY]->(pc)
                RETURN p.SerialCode AS SerialCode,
                       p.Name AS Name,
                       p.Description AS Description,
                       p.Image AS Image,
                       p.Price AS Price,
                       p.PartCategoryId AS PartCategoryId";

            var parameters = new
            {
                part.SerialCode,
                part.Name,
                part.Description,
                part.Image,
                part.Price,
                PartCategoryId = partCategoryId
            };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            return new Part
            {
                SerialCode = queryResults[0]["SerialCode"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Description = queryResults[0]["Description"].As<string>(),
                Image = queryResults[0]["Image"].As<string>(),
                Price = queryResults[0]["Price"].As<double>(),
                PartCategoryId = queryResults[0]["PartCategoryId"].As<string>()
            };
        }


        public async Task DeletePartAsync(string serialCode)
        {
            string query = @"
                MATCH (p:Part {SerialCode: $SerialCode})
                DETACH DELETE p"; 

            var parameters = new { SerialCode = serialCode };

            await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
        }



        public async Task<List<Part>> GetAllPartsAsync()
        {
            string query = @"
                MATCH (p:Part)
                OPTIONAL MATCH (p)-[r:PART_BELONGS_TO_CATEGORY]->(partCategory: PartCategory)
                RETURN p, collect(r) AS relationship, collect(partCategory)  AS partCategory";

            var (queryResults, executionInformation) = await _driver.ExecutableQuery(query).ExecuteAsync();

            var parts = new List<Part>();

            foreach(var queryResult in queryResults)
            {
                var partNode = queryResult["p"].As<INode>();
                var relationships = queryResult["relationships"].As<IRelationship>();
                var partCategory = queryResult["partCategory"].As<INode>();

                var part = new Part
                {
                    SerialCode = partNode["SerialCode"].As<string>(),
                    Name = partNode["Name"].As<string>(),
                    Description = partNode["Description"].As<string>(),
                    Image = partNode["Image"].As<string>(),
                    Price = partNode["Price"].As<double>(),
                    PartCategoryId = partNode["PartCategoryId"].As<string>()
                };

                parts.Add(part);
            }

            return parts;
        }

        public async Task<Part> GetPartAsync(string serialCode)
        {
            string query = @"
                MATCH (p:Part { SerialCode: $SerialCode })
                OPTIONAL MATCH (p)-[r:PART_BELONGS_TO_CATEGORY]->(partCategory: PartCategory)
                RETURN p, collect(r) AS relationship, collect(partCategory)  AS partCategory";

            var parameters = new { SerialCode = serialCode };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            var partNode = queryResults[0]["p"].As<INode>();
            var relationships = queryResults[0]["relationships"].As<IRelationship>();
            var partCategory = queryResults[0]["partCategory"].As<INode>();

            var part = new Part
            {
                SerialCode = partNode["SerialCode"].As<string>(),
                Name = partNode["Name"].As<string>(),
                Description = partNode["Description"].As<string>(),
                Image = partNode["Image"].As<string>(),
                Price = partNode["Price"].As<double>(),
                PartCategoryId = partNode["PartCategoryId"].As<string>()
            };

            return part;
        }

        public async Task<IEnumerable<PartCategory>> GetPartCategoriesAsync()
        {
            string query = @"
            MATCH (pc:PartCategory)
            RETURN pc.Id AS Id, pc.Name AS Name, pc.Description AS Description";

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .ExecuteAsync();

            return queryResults.Select(record => new PartCategory
            {
                Id = record["Id"].As<string>(),
                Name = record["Name"].As<string>(),
                Description = record["Description"].As<string>()
            });
        }

        public async Task<PartCategory> GetPartCategoryAsync(string id)
        {
            string query = @"
            MATCH (pc:PartCategory { Id: $Id })
            RETURN pc.Id AS Id, pc.Name AS Name, pc.Description AS Description";

            var parameters = new { Id = id };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            if (queryResults.Count == 0)
                return null;

            return new PartCategory
            {
                Id = queryResults[0]["Id"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Description = queryResults[0]["Description"].As<string>()
            };
        }

        public async Task<PartCategory> CreatePartCategoryAsync(PartCategory partCategory)
        {
            string query = @"
            CREATE (pc:PartCategory {
                Id: $Id,
                Name: $Name,
                Description: $Description
            })
            RETURN pc.Id AS Id, pc.Name AS Name, pc.Description AS Description";

            var parameters = new
            {
                Id = Guid.NewGuid().ToString(), // Generate unique ID
                partCategory.Name,
                partCategory.Description
            };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            return new PartCategory
            {
                Id = queryResults[0]["Id"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Description = queryResults[0]["Description"].As<string>()
            };
        }

        public async Task<PartCategory> UpdatePartCategoryAsync(PartCategory partCategory)
        {
            string query = @"
            MATCH (pc:PartCategory { Id: $Id })
            SET pc.Name = $Name, pc.Description = $Description
            RETURN pc.Id AS Id, pc.Name AS Name, pc.Description AS Description";

            var parameters = new
            {
                partCategory.Id,
                partCategory.Name,
                partCategory.Description
            };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            if (queryResults.Count == 0)
                throw new Exception("PartCategory not found.");

            return new PartCategory
            {
                Id = queryResults[0]["Id"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Description = queryResults[0]["Description"].As<string>()
            };
        }

        public async Task DeletePartCategoryAsync(string id)
        {
            string query = @"
            MATCH (pc:PartCategory { Id: $Id })
            DETACH DELETE pc";

            var parameters = new { Id = id };

            await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
        }

        public async Task<Part> UpdatePartInformationAsync(Part part)
        {
            string query = @"
                    MATCH (p:Part {SerialCode: $SerialCode})
                    SET p.Name = $Name,
                        p.Description = $Description,
                        p.Image = $Image,
                        p.Price = $Price
                    WITH p
        
                    // Remove existing category relationship
                    OPTIONAL MATCH (p)-[r:PART_BELONGS_TO_CATEGORY]->()
                    DELETE r
        
                    WITH p
                    // Create new relationship to category
                    MATCH (pc:PartCategory {Id: $PartCategoryId})
                    CREATE (p)-[:PART_BELONGS_TO_CATEGORY]->(pc)
        
                    SET p.PartCategoryId = $PartCategoryId
        
                    RETURN p.SerialCode AS SerialCode,
                           p.Name AS Name,
                           p.Description AS Description,
                           p.Image AS Image,
                           p.Price AS Price,
                           p.PartCategoryId AS PartCategoryId";

            var parameters = new
            {
                part.SerialCode,
                part.Name,
                part.Description,
                part.Image,
                part.Price,
                part.PartCategoryId
            };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            if (queryResults.Count == 0)
                throw new Exception("Part not found.");

            return new Part
            {
                SerialCode = queryResults[0]["SerialCode"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Description = queryResults[0]["Description"].As<string>(),
                Image = queryResults[0]["Image"].As<string>(),
                Price = queryResults[0]["Price"].As<double>(),
                PartCategoryId = queryResults[0]["PartCategoryId"].As<string>()
            };
        }

        public async Task<List<Part>> GetAllPartsOfCategoryAsync(string categoryId)
        {
            string query = @"
                MATCH (p:Part)-[:PART_BELONGS_TO_CATEGORY]->(c:PartCategory {Id: $categoryId})
                RETURN p.SerialCode AS SerialCode, 
                       p.Name AS Name, 
                       p.Description AS Description, 
                       p.Image AS Image, 
                       p.Price AS Price, 
                       p.PartCategoryId AS PartCategoryId";

            var parameters = new { categoryId };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            List<Part> parts = new List<Part>();

            foreach (var record in queryResults)
            {
                parts.Add(new Part
                {
                    SerialCode = record["SerialCode"].As<string>(),
                    Name = record["Name"].As<string>(),
                    Description = record["Description"].As<string>(),
                    Image = record["Image"].As<string>(),
                    Price = record["Price"].As<double>(),
                    PartCategoryId = record["PartCategoryId"].As<string>()
                });
            }

            return parts;
        }
    }
}
