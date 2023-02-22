using Microsoft.EntityFrameworkCore;
using Pan.Affiliation.Infrastructure.Persistence.Entities.Shared;
using System.ComponentModel.DataAnnotations;

namespace Pan.Affiliation.Infrastructure.Persistence.Entities
{
    [Index(nameof(DocumentNumber), IsUnique = true)]
    public class Customer : BaseEntity
    {
        [MaxLength(255), Required]
        public string? Name { get; set; }

        [MaxLength(14), Required] 
        public string? DocumentNumber { get; set; }

        public ICollection<Address>? Addresses { get; set; }

        public Domain.Modules.Customers.Entities.Customer ToDomainEntity()
            => new(Id, Name, DocumentNumber, Addresses?.Select(a => a.ToDomainEntity()).ToList());

        public static Customer FromDomainEntity(Domain.Modules.Customers.Entities.Customer customer) =>
            new()
            {
                Name = customer.Name,
                DocumentNumber = customer.DocumentNumber,
                Id = customer.Id,
                Addresses = customer
                    .Addresses
                    .Select(a => Address.FromDomainEntity(a, customer))
                    .ToList()
            };
    }
}