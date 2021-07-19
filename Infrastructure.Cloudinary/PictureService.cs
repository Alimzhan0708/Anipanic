using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Domain.ValueTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Cloudinary
{
    public class PictureService: IPictureService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;
        
        public PictureService(IOptions<CloudinarySettings> settings)
        {
            var account = new Account(
                settings.Value.CloudName, 
                settings.Value.ApiKey, 
                settings.Value.ApiSecret
                );

            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public async Task<MoviePicture> AddPicture(IFormFile picture, CancellationToken cancellationToken)
        {
            await using var stream = picture.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(picture.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);
            
            var pictureUploadResult = new MoviePicture
            {
                PictureId = uploadResult.PublicId,
                PictureUrl = uploadResult.SecureUrl.ToString()
            };

            return pictureUploadResult;
        }

        public async Task<string> DeletePicture(string pictureId, CancellationToken cancellationToken)
        {
            var deleteParams = new DeletionParams(pictureId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok" ? result.Result : null;
        }
    }
}