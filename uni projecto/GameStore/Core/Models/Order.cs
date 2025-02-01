using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string GameId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
