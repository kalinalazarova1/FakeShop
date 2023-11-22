using FakeShop.Data.Interfaces;
using FakeShop.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace FakeShop.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly Container usersContainer;

        public UserRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDbSettings:Databasename"];
            usersContainer = cosmosClient.GetContainer(databaseName, "Users");
        }

        public async Task<UserDocument> GetAsync(string email)
        {
            var query = usersContainer
            .GetItemLinqQueryable<UserDocument>()
            .Where(p => p.Email == email)
            .Take(1)
            .ToFeedIterator();
            var results = new List<UserDocument>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results.FirstOrDefault();
        }

        public async Task<UserDocument> AddAsync(UserDocument user) =>
            await usersContainer.CreateItemAsync(user, new PartitionKey(user.UserId));

        public async Task<UserDocument> UpdateAsync(UserDocument user) =>
            await usersContainer.UpsertItemAsync(user, new PartitionKey(user.UserId));
    }
}
