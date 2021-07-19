using Application.Resources.MovieErrorMessages;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Application.Dtos.Movies
{
    public class CreateMoviesDto
    {
        public string RussianName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
    }

    public class CreateMoviesDtoValidator : AbstractValidator<CreateMoviesDto>
    {

        public CreateMoviesDtoValidator()
        {
            RuleFor(p => p.RussianName)
                .NotEmpty().MaximumLength(127);
            RuleFor(p => p.EnglishName)
                .NotEmpty().MaximumLength(127);
            RuleFor(p => p.Description)
                .NotEmpty().MaximumLength(511);
        }
    }
}