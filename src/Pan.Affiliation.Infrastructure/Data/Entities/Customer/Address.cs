using Pan.Affiliation.Infrastructure.Data.Entities.Shared;
using System.ComponentModel.DataAnnotations;

namespace Pan.Affiliation.Infrastructure.Data.Entities.Customer
{
    public class Address : BaseEntity
    {
        [MaxLength(8), Required]
        public string? ZipCode { get; set; }

        [MaxLength(300), Required]
        public string? Street { get; set; }

        public int? Number { get; set; }

        [MaxLength(100), Required]
        public string? City { get; set; }

        [MaxLength(2), Required]
        public string? State { get; set; }

        [MaxLength(50), Required]
        public string? Country { get; set; }

        [MaxLength(75)]
        public string? Complement { get; set; }

        [MaxLength(150), Required]
        public string? Neighborhood { get; set; }

        public CustomerInformation? Customer { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}