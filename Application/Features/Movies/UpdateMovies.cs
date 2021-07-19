using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Movies;
using Application.Interfaces.DbContexts;
using Application.Interfaces.Services;
using Application.Wrappers;
using AutoMapper;
using Domain.ValueTypes;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies
{
    public class UpdateMovies
    {
        public class Command : IRequest<RequestResult<Unit>>
        {
            public UpdateMoviesDto UpdateMovieDto { get; }

            public Command(UpdateMoviesDto updateMovieDto)
            {
                UpdateMovieDto = updateMovieDto;
            }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(v => v.UpdateMovieDto)
                    .SetValidator(new UpdateMoviesDtoValidtor());
            }
        }
        
        public class Handler: IRequestHandler<Command, RequestResult<Unit>>
        {
            private readonly IAppDbContext _appDbContext;
            private readonly IMapper _mapper;
            private readonly IPictureService _pictureService;

            public Handler(IAppDbContext appDbContext, IMapper mapper, IPictureService pictureService)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
                _pictureService = pictureService;
            }
            
            public async Task<RequestResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var movie = await _appDbContext.Movies
                    .SingleOrDefaultAsync(m => m.Id == Guid.Parse(request.UpdateMovieDto.MovieId));
                
                if(movie is null) return RequestResult<Unit>.Failutre((int) HttpStatusCode.BadRequest, "Movie wasn't found");

                _mapper.Map(request.UpdateMovieDto, movie);
                
                if ( request.UpdateMovieDto.Picture != null && request.UpdateMovieDto.Picture.Length > 0)
                {
                    var moviePicture = JsonSerializer.Deserialize<MoviePicture>(movie.MoviePicture);
                    await _pictureService.DeletePicture(moviePicture.PictureId, cancellationToken);
                    var movieNewPicture = await _pictureService.AddPicture(request.UpdateMovieDto.Picture, cancellationToken);
                    movie.MoviePicture = JsonSerializer.Serialize(movieNewPicture);
                }

                await _appDbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<Unit>.Success(value: Unit.Value);
            }
        }
    }
}