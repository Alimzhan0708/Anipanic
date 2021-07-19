using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Movies;
using Application.Interfaces.DbContexts;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies
{
    public class ListMovies
    {
        public class Query : IRequest<RequestResult<List<MovieDto>>>
        {

        }

        public class Handler : IRequestHandler<Query, RequestResult<List<MovieDto>>>
        {
            private readonly IAppDbContext _appDbContext;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext appDbContext, IMapper mapper)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
            }

            public async Task<RequestResult<List<MovieDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var movies = await _appDbContext.Movies
                    .ToListAsync(cancellationToken);

                var moviesDtos = _mapper.Map<List<MovieDto>>(movies);

                return RequestResult<List<MovieDto>>.Success(moviesDtos);
            }
        }
    }
}