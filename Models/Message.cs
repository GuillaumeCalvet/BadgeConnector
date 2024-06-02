using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BadgeConnector.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string RawMessage { get; set; }
        public int BadgeId { get; set; }
        public Badge Badge { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}