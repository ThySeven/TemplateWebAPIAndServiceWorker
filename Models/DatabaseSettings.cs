namespace Planning_Service.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string 
            Name { get; set; } = null!;

        public string DeliveryCollectionName { get; set; } = null!;
    }
}
