using FakeShop.Data.Interfaces;
using FakeShop.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace FakeShop.Data;

public class ProductRepository : IProductRepository
{
    private readonly Container productsContainer;

    public ProductRepository(CosmosClient cosmosClient, IConfiguration configuration)
    {
        var databaseName = configuration["CosmosDbSettings:Databasename"];
        productsContainer = cosmosClient.GetContainer(databaseName, "Products");
    }

    public async Task<IEnumerable<ProductDocument>> GetAsync()
    {
        var query = productsContainer
            .GetItemQueryIterator<ProductDocument>();
        var results = new List<ProductDocument>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<IEnumerable<ProductDocument>> GetInStockAsync()
    {
        var query = productsContainer
            .GetItemLinqQueryable<ProductDocument>()
            .Where(p => p.IsInStock)
            .ToFeedIterator();
        var results = new List<ProductDocument>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<IEnumerable<ProductDocument>> GetFeaturedAsync()
    {
        var query = productsContainer
            .GetItemLinqQueryable<ProductDocument>()
            .Where(p => p.Featured)
            .ToFeedIterator();
        var results = new List<ProductDocument>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<ProductDocument> GetAsync(string productId)
    {
        var query = productsContainer
            .GetItemLinqQueryable<ProductDocument>()
            .Where(p => p.ProductId == productId)
            .Take(1)
            .ToFeedIterator();
        var results = new List<ProductDocument>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results.FirstOrDefault();
    }

    public async Task<ProductDocument> AddAsync(ProductDocument product) =>
        await productsContainer.CreateItemAsync(product, new PartitionKey(product.ProductId));

    public async Task DeleteAsync(string id) =>
        await productsContainer.DeleteItemAsync<ProductDocument>(id, new PartitionKey(id));

    public async Task<ProductDocument> UpdateAsync(ProductDocument product) =>
        await productsContainer.UpsertItemAsync(product, new PartitionKey(product.ProductId));
}
