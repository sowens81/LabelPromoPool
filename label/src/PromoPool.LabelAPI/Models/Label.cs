using System;
using MongoDB.Bson.Serialization.Attributes;


namespace PromoPool.LabelAPI.Models
{
    public class Label
    {
        public Label()
        {
            Id = Guid.NewGuid();
        }

        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Url { get; set; }

        public string Email { get; set; }

        public Address Address { get; set; }

    }
}
