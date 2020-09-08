using System.ComponentModel.DataAnnotations;

namespace PromoPool.LabelAPI.Models
{
    public class NewLabel
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "URL is required!")]
        [Url]
        public string Url { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        public NewAddress Address { get; set; }

    }
}
