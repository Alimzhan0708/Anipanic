using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Episodes;
using Application.Interfaces.DbContexts;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Episodes
{
    public class ListEpisodes
    {
        public class Query : IRequest<RequestResult<List<EpisodeDto>>>
        {
            public string MovieId { get; }

            public Query(string movieId)
            {
                MovieId = movieId;
            }
        }
        
        public class Handler: IRequestHandler<Query, RequestResult<List<EpisodeDto>>>
        {
            private readonly IAppDbContext _appDbContext;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext appDbContext, IMapper mapper)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
            }
            
            public async Task<RequestResult<List<EpisodeDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var episodes = await _appDbContext.Episodes
                    .Where(e => e.MovieId == Guid.Parse(request.MovieId))
                    .ToListAsync(cancellationToken);
                var listEpisodesDtos = _mapper.Map<List<EpisodeDto>>(episodes);
                return RequestResult<List<EpisodeDto>>.Success(listEpisodesDtos);
            }
        }
    }
}