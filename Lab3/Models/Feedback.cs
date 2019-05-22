using System;

namespace Lab3.Models
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}