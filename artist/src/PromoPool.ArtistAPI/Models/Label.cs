using System;
using System.ComponentModel.DataAnnotations;


namespace PromoPool.ArtistAPI.Models
{
    public class Label
    {
        [Required(ErrorMessage = "Label Id is required!")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Label Name is required!")]
        public string Name { get; set; }
    }
}
