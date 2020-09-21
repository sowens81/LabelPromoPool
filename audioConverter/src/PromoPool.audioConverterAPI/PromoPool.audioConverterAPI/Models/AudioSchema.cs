using System.ComponentModel.DataAnnotations;

namespace PromoPool.audioConverterAPI.Models
{
    public class AudioSchema
    {
        [Required(ErrorMessage = "FileName is required!")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Artist is required!")]
        public string Arist { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Genre is required!")]
        public string Genre { get; set; }
    }
}
