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
    public class UpdateEpisodes
    {
        public class Command : IRequest<RequestResult<Unit>>
        {
            public UpdateEpisodesDto UpdateEpisodesDto { get; }

            public Command(UpdateEpisodesDto updateEpisodesDto)
            {
                UpdateEpisodesDto = updateEpisodesDto;
            }
        }
        
        public class Handler: IRequestHandler<Command, RequestResult<Unit>>
        {
            private readonly IAppDbContext _appDbContext;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext appDbContext, IMapper mapper)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
            }
            
            public async Task<RequestResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var episode = await _appDbContext.Episodes
                    .SingleOrDefaultAsync(e => e.Id == Guid.Parse(request.UpdateEpisodesDto.Id), cancellationToken);
                if(episode is null) return RequestResult<Unit>.Failutre((int) HttpStatusCode.BadRequest, "Episode was not found");
                _mapper.Map(request.UpdateEpisodesDto, episode);
                await _appDbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<Unit>.Success(Unit.Value);
            }
        }
    }
}