using System;

namespace Application.Dtos.Episodes
{
    public class CreateEpisodesDto
    {
        public int Number { get; set; }
        public string Url { get; set; }
        public Guid MovieId { get; set; }
    }
}