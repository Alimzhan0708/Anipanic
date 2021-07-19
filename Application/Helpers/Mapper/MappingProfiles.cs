using Application.Dtos.Episodes;
using Application.Dtos.Movies;
using Application.Helpers.Resolvers;
using AutoMapper;
using Domain.Entities;

namespace Application.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            #region Movies

            CreateMap<Movie, MovieDto>()
                .ForMember(m => m.PictureUrl, opt => opt.MapFrom<MovieDtoResolver>());
            CreateMap<CreateMoviesDto, Movie>()
                .ForMember(m => m.MoviePicture, opt => opt.Ignore());
            CreateMap<UpdateMoviesDto, Movie>()
                .ForMember(m => m.MoviePicture, opt => opt.Ignore());

            #endregion

            #region Episodes
            
            CreateMap<Episode, EpisodeDto>();
            CreateMap<CreateEpisodesDto, Episode>();
            CreateMap<UpdateEpisodesDto, Episode>();

            #endregion
        }
    }
}