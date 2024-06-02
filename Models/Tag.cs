using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadgeConnector.Models
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}