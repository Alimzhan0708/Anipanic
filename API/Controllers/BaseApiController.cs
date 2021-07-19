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

        protected async Task<List<string>> ValidateAsync<IRequest>(IRequest request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            
            var validators = HttpContext.RequestServices.GetServices<IValidator<IRequest>>();
            if (validators.Any())
            {
                var context = new ValidationContext<IRequest>(request);
                var validationTasks = validators
                    .Select(v => v.ValidateAsync(context, cancellationToken));
                var validationResults = await Task.WhenAll(validationTasks);
                var failures = validationResults
                    .SelectMany(r => r.Errors).Where(f => f != null);
                if (failures.Count() > 0)
                {
                    errors = failures.Select(f => f.ErrorMessage).ToList();
                }
            }

            return errors;
        }

        protected IActionResult HandleResult<T>(RequestResult<T> requestResult)
        {
            object body = requestResult.IsSuccess ? (object) requestResult.Body : requestResult.Error;
            return new ObjectResult(body)
            {
                StatusCode = requestResult.StatusCode
            };
        }
    }
}