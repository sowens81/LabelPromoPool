using System.ComponentModel.DataAnnotations;

namespace PromoPool.ArtistAPI.Models
{
    public class UpdateArtist
    {
        [Required(ErrorMessage = "Id is required!")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Artist Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Artist Profile Picture URL is required!")]
        [Url]
        public string ProfilePictureURL { get; set; }

        [Required(ErrorMessage = "Artist Beatport URL is required!")]
        [Url]
        public string BeatportUrl { get; set; }

        [Required(ErrorMessage = "Artist SoundCloud URL is required!")]
        [Url]
        public string SoundCloudUrl { get; set; }
    }
}
