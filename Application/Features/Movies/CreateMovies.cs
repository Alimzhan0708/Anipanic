using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Movies;
using Application.Interfaces.DbContexts;
using Application.Interfaces.Services;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Application.Features.Movies
{
    public class CreateMovies
    {
        public class Command : IRequest<RequestResult<Unit>>
        {
           public CreateMoviesDto CreateMovieDto { get; }

           public Command(CreateMoviesDto createMovieDto)
           {
               CreateMovieDto = createMovieDto;
           }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(v => v.CreateMovieDto)
                    .SetValidator(new CreateMoviesDtoValidator());
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
                var newMovie = _mapper.Map<Movie>(request.CreateMovieDto);
                var picture = await _pictureService.AddPicture(request.CreateMovieDto.Picture, cancellationToken);
                newMovie.MoviePicture = JsonSerializer.Serialize(picture);
                _appDbContext.Movies.Add(newMovie);
                await _appDbContext.SaveChangesAsync(cancellationToken);
                
                return RequestResult<Unit>.Success(Unit.Value);
            }
        }
    }
}