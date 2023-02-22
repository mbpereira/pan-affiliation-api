using Pan.Affiliation.Infrastructure.Persistence.Entities.Shared;
using DomainAddress = Pan.Affiliation.Domain.Modules.Customers.Entities.Address;
using DomainCustomer = Pan.Affiliation.Domain.Modules.Customers.Entities.Customer;
using System.ComponentModel.DataAnnotations;

namespace Pan.Affiliation.Infrastructure.Persistence.Entities
{
    public class Address : BaseEntity
    {
        [MaxLength(8), Required]
        public string? PostalCode { get; set; }

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

        public Customer? Customer { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        internal Domain.Modules.Customers.Entities.Address ToDomainEntity() => new()
        {
            Id = Id,
            PostalCodeVo = PostalCode!,
            City = City,
            Complement = Complement,
            Country = Country,
            Neighborhood = Neighborhood,
            Number = Number,
            State = State,
            Street = Street
        };

        public static Address FromDomainEntity(DomainAddress address, DomainCustomer customer)
            => new()
            {
                Id = address.Id,
                PostalCode = address.PostalCode!,
                City = address.City,
                Complement = address.Complement,
                Country = address.Country,
                Neighborhood = address.Neighborhood,
                Number = address.Number,
                State = address.State,
                Street = address.Street,
                CustomerId = customer.Id
            };
    }
}