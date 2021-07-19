using System;
using System.Net;
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
    public class DetailsEpisodes
    {
        public class Query : IRequest<RequestResult<EpisodeDto>>
        {
            public string EpisodeId { get; }

            public Query(string episodeId)
            {
                EpisodeId = episodeId;
            }
        }
        
        public class Handler: IRequestHandler<Query, RequestResult<EpisodeDto>>
        {
            private readonly IAppDbContext _appDbContext;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext appDbContext, IMapper mapper)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
            }
            
            public async Task<RequestResult<EpisodeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var episode = await _appDbContext.Episodes
                    .SingleOrDefaultAsync(e => e.Id == Guid.Parse(request.EpisodeId), cancellationToken);
                if(episode is null) return RequestResult<EpisodeDto>.Failutre((int) HttpStatusCode.BadRequest,"Episode was not found");
                var episodeDto = _mapper.Map<EpisodeDto>(episode);
                return RequestResult<EpisodeDto>.Success(episodeDto);
            }
        }
    }
}