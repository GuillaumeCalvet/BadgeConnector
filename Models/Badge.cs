using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BadgeConnector.Models
{
    public class Badge
    {
        [Key]
        public int BadgeId { get; set; }
        public string Name { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}