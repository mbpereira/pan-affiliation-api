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

        public IEnumerable<Address>? Addresses { get; set; }
    }
}