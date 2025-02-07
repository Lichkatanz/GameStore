﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Models
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}
