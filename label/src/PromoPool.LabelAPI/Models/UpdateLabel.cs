using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromoPool.LabelAPI.Models
{
    public class UpdateLabel
    {
        [Required(ErrorMessage = "Id is required!")]
        public string Id { get; set; }

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
        public Address Address { get; set; }
    }
}
