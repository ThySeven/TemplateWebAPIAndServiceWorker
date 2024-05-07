using WebAPIBackgrundWorkerTemplate.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace WebAPIBackgrundWorkerTemplate.Services;

public class TestTemplateService
{
    //
    private readonly IMongoCollection<Test> _TestCollection;

    public TestTemplateService(
        IOptions<DatabaseSettings> deliveryDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            deliveryDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            deliveryDatabaseSettings.Value.DatabaseName);

        _TestCollection = mongoDatabase.GetCollection<Test>(
            deliveryDatabaseSettings.Value.DeliveryCollectionName);
        Console.WriteLine("Test");
    }

    public async Task<List<Test>> GetAsync() =>
        await _TestCollection.Find(_ => true).ToListAsync();

    public async Task<Test?> GetAsync(string id) =>
        await _TestCollection.Find(x => x.id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Test newDelivery) =>
        await _TestCollection.InsertOneAsync(newDelivery);

    public async Task UpdateAsync(string id, Test updatedDelivery) =>
        await _TestCollection.ReplaceOneAsync(x => x.id == id, updatedDelivery);

    public async Task RemoveAsync(string id) =>
        await _TestCollection.DeleteOneAsync(x => x.id == id);
}