using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pan.Affiliation.Infrastructure.Persistence.Entities.Shared
{
    public abstract class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }
    }
}
