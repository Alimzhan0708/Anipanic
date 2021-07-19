using System.Text.Json;
using Application.Dtos.Movies;
using AutoMapper;
using Domain.Entities;
using Domain.ValueTypes;

namespace Application.Helpers.Resolvers
{
    public class MovieDtoResolver: IValueResolver<Movie, MovieDto, string>
    {
        public string Resolve(Movie source, MovieDto destination, string destMember, ResolutionContext context)
        {
            var picture = JsonSerializer.Deserialize<MoviePicture>(source.MoviePicture);
            var pictureUrl = picture.PictureUrl;
            return pictureUrl;
        }
    }
}