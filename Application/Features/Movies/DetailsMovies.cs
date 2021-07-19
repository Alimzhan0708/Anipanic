using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Movies;
using Application.Interfaces.DbContexts;
using Application.Wrappers;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies
{
    public class DetailsMovies
    {
        public class Query : IRequest<RequestResult<MovieDto>>
        {
            public string MovieId { get; }

            public Query(string movieId)
            {
                MovieId = movieId;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.MovieId).NotEmpty();
            }
        }
        
        public class Handler: IRequestHandler<Query, RequestResult<MovieDto>>
        {
            private readonly IAppDbContext _appDbContext;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext appDbContext, IMapper mapper)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
            }
            
            public async Task<RequestResult<MovieDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var movie = await _appDbContext.Movies
                    .SingleOrDefaultAsync(m => m.Id ==  Guid.Parse(request.MovieId), cancellationToken);
                if(movie is null) return RequestResult<MovieDto>.Failutre((int) HttpStatusCode.BadRequest, "Movie wasn't found");
                var movieDto = _mapper.Map<MovieDto>(movie);

                return RequestResult<MovieDto>.Success(movieDto);
            }
        }
    }
}