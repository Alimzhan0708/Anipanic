using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Episodes;
using Application.Interfaces.DbContexts;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Episodes
{
    public class CreateEpisodes
    {
        public class Command : IRequest<RequestResult<Unit>>
        {
            public CreateEpisodesDto CreateEpisodesDto { get; }

            public Command(CreateEpisodesDto createEpisodesDto)
            {
                CreateEpisodesDto = createEpisodesDto;
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
                var newEpisode = _mapper.Map<Episode>(request.CreateEpisodesDto);
                _appDbContext.Episodes.Add(newEpisode);
                await _appDbContext.SaveChangesAsync(cancellationToken);
                
                return RequestResult<Unit>.Success(Unit.Value);
            }
        }
    }
}