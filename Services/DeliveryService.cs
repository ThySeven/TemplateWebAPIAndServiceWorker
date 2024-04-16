using Planning_Service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Planning_Service.Services;

public class DeliverysService
{
    private readonly IMongoCollection<Delivery> _deliveryCollection;

    public DeliverysService(
        IOptions<DeliveryDatabaseSettings> deliveryDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            deliveryDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            deliveryDatabaseSettings.Value.DatabaseName);

        _deliveryCollection = mongoDatabase.GetCollection<Delivery>(
            deliveryDatabaseSettings.Value.DeliverysCollectionName);
    }

    public async Task<List<Delivery>> GetAsync() =>
        await _deliveryCollection.Find(_ => true).ToListAsync();

    public async Task<Delivery?> GetAsync(string id) =>
        await _deliveryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Delivery newDelivery) =>
        await _deliveryCollection.InsertOneAsync(newDelivery);

    public async Task UpdateAsync(string id, Delivery updatedDelivery) =>
        await _deliveryCollection.ReplaceOneAsync(x => x.Id == id, updatedDelivery);

    public async Task RemoveAsync(string id) =>
        await _deliveryCollection.DeleteOneAsync(x => x.Id == id);
}