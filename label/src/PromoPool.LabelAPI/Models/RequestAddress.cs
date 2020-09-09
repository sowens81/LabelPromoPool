using System.ComponentModel.DataAnnotations;

namespace PromoPool.LabelAPI.Models
{
    public class RequestAddress
    {
        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Locality { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
