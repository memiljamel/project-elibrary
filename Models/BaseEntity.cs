using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELibrary.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Column("created_at", Order = 98)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at", Order = 99)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}