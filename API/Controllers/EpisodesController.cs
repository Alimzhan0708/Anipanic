using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Episodes;
using Application.Features.Episodes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EpisodesController: BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> ListEpisodes([FromQuery] string movieId, CancellationToken cancellationToken)
        {
            var query = new ListEpisodes.Query(movieId);
            var errors = await ValidateAsync(query, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(query, cancellationToken);
            return HandleResult(requestResult);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsEpisodes([FromQuery] string episodeId, CancellationToken cancellationToken)
        {
            var query = new DetailsEpisodes.Query(episodeId);
            var errors = await ValidateAsync(query, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(query, cancellationToken);
            return HandleResult(requestResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisodes([FromForm] CreateEpisodesDto createEpisodesDto, CancellationToken cancellationToken)
        {
            var command = new CreateEpisodes.Command(createEpisodesDto);
            var errors = await ValidateAsync(command, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(command, cancellationToken);
            return HandleResult(requestResult);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateEpisodes([FromForm] UpdateEpisodesDto updateEpisodesDto, CancellationToken cancellationToken)
        {
            var command = new UpdateEpisodes.Command(updateEpisodesDto);
            var errors = await ValidateAsync(command, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(command, cancellationToken);
            return HandleResult(requestResult);
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteEpisodes([FromQuery] string episodeId, CancellationToken cancellationToken)
        {
            var command = new DeleteEpisodes.Command(episodeId);
            var errors = await ValidateAsync(command, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(command, cancellationToken);
            return HandleResult(requestResult);
        }
    }
}