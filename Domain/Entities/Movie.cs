using System.Collections.Generic;

namespace Domain.Entities
{
    public class Movie: BaseEntity
    {
        public string RussianName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public string MoviePicture { get; set; }
        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    }
}