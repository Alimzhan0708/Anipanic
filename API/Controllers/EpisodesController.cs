using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Episodes;
using Application.Dtos.Movies;
using Application.Features.Episodes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EpisodesController: BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> ListEpisodes([FromQuery] string movieId, CancellationToken cancellationToken)
        {
            var query = new ListEpisodes.Query(movieId);
            return await HandleResult<ListEpisodes.Query, List<EpisodeDto>>(query, cancellationToken);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsEpisodes([FromQuery] string episodeId, CancellationToken cancellationToken)
        {
            var query = new DetailsEpisodes.Query(episodeId);
            return await HandleResult<DetailsEpisodes.Query, EpisodeDto>(query, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisodes([FromForm] CreateEpisodesDto createEpisodesDto, CancellationToken cancellationToken)
        {
            var command = new CreateEpisodes.Command(createEpisodesDto);
            return await HandleResult<CreateEpisodes.Command, Unit>(command, cancellationToken);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateEpisodes([FromForm] UpdateEpisodesDto updateEpisodesDto, CancellationToken cancellationToken)
        {
            var command = new UpdateEpisodes.Command(updateEpisodesDto);
            return await HandleResult<UpdateEpisodes.Command, Unit>(command, cancellationToken);
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteEpisodes([FromQuery] string episodeId, CancellationToken cancellationToken)
        {
            var command = new DeleteEpisodes.Command(episodeId);
            return await HandleResult<DeleteEpisodes.Command, Unit>(command, cancellationToken);
        }
    }
}