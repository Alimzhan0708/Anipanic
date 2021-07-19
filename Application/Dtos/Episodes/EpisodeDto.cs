using System;

namespace Application.Dtos.Episodes
{
    public class EpisodeDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Url { get; set; }
        public Guid MovieId { get; set; }
    }
}