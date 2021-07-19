using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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
    public class DeleteMovies
    {
        public class Command : IRequest<RequestResult<Unit>>
        {
            public string MovieId { get; }

            public Command(string movieId)
            {
                MovieId = movieId;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.MovieId).NotEmpty();
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
                    .SingleOrDefaultAsync(m => m.Id == Guid.Parse(request.MovieId), cancellationToken);
                if(movie is null) return RequestResult<Unit>.Failutre((int) HttpStatusCode.BadRequest, "Movie wasn't found");
                var moviePicture = JsonSerializer.Deserialize<MoviePicture>(movie.MoviePicture);
                await _pictureService.DeletePicture(moviePicture.PictureId, cancellationToken);
                _appDbContext.Movies.Remove(movie);
                await _appDbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<Unit>.Success(Unit.Value);
            }
        }
    }
}