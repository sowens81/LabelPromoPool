using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;


namespace PromoPool.ArtistAPI.Models
{
    public class Artist
    {
        public Artist()
        {
            Id = Guid.NewGuid();
            Labels = new List<Label>();
        }

        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ProfilePictureURL { get; set; }

        public string BeatportUrl { get; set; }

        public string SoundCloudUrl { get; set; }
        
        public List<Label> Labels { get; set; }
    }
}
