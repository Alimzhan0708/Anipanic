using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Movies
{
    public class UpdateMoviesDto
    {
        public string MovieId { get; set; }
        public string RussianName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
    }

    public class UpdateMoviesDtoValidtor : AbstractValidator<UpdateMoviesDto>
    {
        public UpdateMoviesDtoValidtor()
        {
            RuleFor(p => p.MovieId).NotEmpty();
            RuleFor(p => p.RussianName).NotEmpty().MaximumLength(127);
            RuleFor(p => p.EnglishName).NotEmpty().MaximumLength(127);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(511);
        }
    }
}