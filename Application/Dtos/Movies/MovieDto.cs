using System;
using FluentValidation;

namespace Application.Dtos.Movies
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string RussianName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
    }
}