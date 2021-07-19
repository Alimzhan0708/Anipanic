using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.DbContexts;
using Application.Wrappers;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Episodes
{
    public class DeleteEpisodes
    {
        public class Command : IRequest<RequestResult<Unit>>
        {
            public string EpisodeId { get; }

            public Command(string episodeId)
            {
                EpisodeId = episodeId;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.EpisodeId).NotEmpty();
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
                    .SingleOrDefaultAsync(e => e.Id == Guid.Parse(request.EpisodeId), cancellationToken);
                if(episode is null) return RequestResult<Unit>.Failutre((int) HttpStatusCode.BadRequest,"Episode was not found");
                _appDbContext.Episodes.Remove(episode);
                await _appDbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<Unit>.Success(Unit.Value);
            }
        }
    }
}