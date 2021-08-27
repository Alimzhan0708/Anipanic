using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Wrappers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController: ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected async Task<IActionResult> HandleResult<TRequest, T>(TRequest request, CancellationToken cancellationToken) where TRequest: IRequest<RequestResult<T>>
        {
            var validationErrors = await ValidateAsync(request, cancellationToken);
            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }

            var requestResult = await Mediator.Send(request, cancellationToken);
            if (requestResult is null)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return !requestResult.IsSuccess 
                ? StatusCode(requestResult.StatusCode, requestResult.Error) 
                : Ok(requestResult.Body);
        }
        
        private async Task<List<string>> ValidateAsync<T>(T request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            
            var validators = HttpContext.RequestServices.GetServices<IValidator<T>>().ToList();
            
            if (validators.Any())
            {
                var context = new ValidationContext<T>(request);
                var validationTasks = validators.Select(v => v.ValidateAsync(context, cancellationToken));
                var validationResults = await Task.WhenAll(validationTasks);
                var failures = validationResults
                    .SelectMany(r => r.Errors).Where(f => f != null).ToArray();
                if (failures.Any())
                {
                    errors = failures.Select(f => f.ErrorMessage).ToList();
                }
            }

            return errors;
        }
    }
}