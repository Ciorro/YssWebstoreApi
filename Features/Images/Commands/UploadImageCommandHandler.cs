using LiteBus.Commands.Abstractions;
using SkiaSharp;
using YssWebstoreApi.Api.DTO.Resources;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Images.Commands
{
    public class UploadImageCommandHandler 
        : ICommandHandler<UploadImageCommand, Result<ResourceResponse>>
    {
        private readonly IRepository<Resource> _resourceRepository;
        private readonly IFileStorage _fileStorage;
        private readonly TimeProvider _timeProvider;

        public UploadImageCommandHandler(
            IRepository<Resource> resourceRepository,
            IFileStorage fileStorage,
            TimeProvider timeProvider)
        {
            _resourceRepository = resourceRepository;
            _fileStorage = fileStorage;
            _timeProvider = timeProvider;
        }

        public async Task<Result<ResourceResponse>> HandleAsync(UploadImageCommand message, CancellationToken cancellationToken = default)
        {
            var fileStream = message.File.OpenReadStream();
            var image = SKImage.FromEncodedData(fileStream);

            SKData encodedImage = image.Encode(SKEncodedImageFormat.Jpeg, 80);

            var id = Guid.CreateVersion7();
            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;

            var resource = new Resource()
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = $"img/{id}.jpg",
                Size = encodedImage.Size
            };

            await _fileStorage.Create(resource.Path, encodedImage.ToArray());
            await _resourceRepository.InsertAsync(resource);

            return new ResourceResponse()
            {
                Id = id,
                Url = _fileStorage.GetUrl(resource.Path)
            };
        }
    }
}
