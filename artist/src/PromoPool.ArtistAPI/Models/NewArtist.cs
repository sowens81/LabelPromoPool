﻿using System.ComponentModel.DataAnnotations;

namespace PromoPool.ArtistAPI.Models
{
    public class NewArtist
    {
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

        [Required(ErrorMessage = "Artist Published state is required!")]
        public bool Published { get; set; }

    }
}
