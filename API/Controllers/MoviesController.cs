using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Movies;
using Application.Features.Movies;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MoviesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> ListMovies(CancellationToken cancellationToken)
        {
            var query = new ListMovies.Query();
            var errors = await ValidateAsync(query, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(query, cancellationToken);
            return HandleResult(requestResult);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsMovies([FromQuery] string movieId, CancellationToken cancellationToken)
        {
            var query = new DetailsMovies.Query(movieId);
            var errors = await ValidateAsync(query, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(query, cancellationToken);
            return HandleResult(requestResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovies([FromForm] CreateMoviesDto createMoviesDto,
            CancellationToken cancellationToken)
        {
            var command = new CreateMovies.Command(createMoviesDto);
            var errors = await ValidateAsync(command, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(command, cancellationToken);
            return HandleResult(requestResult);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovies(
            [FromForm] UpdateMoviesDto updateMoviesDto,
            CancellationToken cancellationToken
            )
        {
            var command = new UpdateMovies.Command(updateMoviesDto);
            var errors = await ValidateAsync(command, cancellationToken);
            if (errors.Any())
            {
                return BadRequest(errors);
            }
            var requestResult = await Mediator.Send(command, cancellationToken);
            return HandleResult(requestResult);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovies(
            [FromQuery] string movieId,
            CancellationToken cancellationToken
            )
        {
            var command = new DeleteMovies.Command(movieId);
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