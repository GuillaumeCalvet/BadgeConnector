using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BadgeConnector.Models
{
    public class BadgeGroup
    {
        [Key]
        public int BadgeGroupId { get; set; }
        public string Name { get; set; }
        public ICollection<Badge> Badges { get; set; }
    }
}