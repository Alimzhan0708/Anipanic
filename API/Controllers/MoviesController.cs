using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos.Movies;
using Application.Features.Movies;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MoviesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> ListMovies(CancellationToken cancellationToken)
        {
            var query = new ListMovies.Query();
            return await HandleResult<ListMovies.Query, List<MovieDto>>(query, cancellationToken);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsMovies([FromQuery] string movieId, CancellationToken cancellationToken)
        {
            var query = new DetailsMovies.Query(movieId);
            return await HandleResult<DetailsMovies.Query, MovieDto>(query, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovies([FromForm] CreateMoviesDto createMoviesDto,
            CancellationToken cancellationToken)
        {
            var command = new CreateMovies.Command(createMoviesDto);
            return await HandleResult<CreateMovies.Command, Unit>(command, cancellationToken);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovies([FromForm] UpdateMoviesDto updateMoviesDto, CancellationToken cancellationToken)
        {
            var command = new UpdateMovies.Command(updateMoviesDto);
            return await HandleResult<UpdateMovies.Command, Unit>(command, cancellationToken);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovies([FromQuery] string movieId, CancellationToken cancellationToken)
        {
            var command = new DeleteMovies.Command(movieId);
            return await HandleResult<DeleteMovies.Command, Unit>(command, cancellationToken);
        }
    }
}