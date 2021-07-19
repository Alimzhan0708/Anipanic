using System.Threading;
using System.Threading.Tasks;
using Domain.ValueTypes;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IPictureService
    {
        Task<MoviePicture> AddPicture(IFormFile picture, CancellationToken cancellationToken);
        Task<string> DeletePicture(string pictureId, CancellationToken cancellationToken);
    }
}