using System;

namespace Domain.Entities
{
    public class Episode: BaseEntity
    {
        public int Number { get; set; }
        public string Url { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}