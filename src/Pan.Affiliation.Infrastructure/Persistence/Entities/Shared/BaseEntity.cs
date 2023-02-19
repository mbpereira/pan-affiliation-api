using System.ComponentModel.DataAnnotations;

namespace Pan.Affiliation.Infrastructure.Persistence.Entities.Shared
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
