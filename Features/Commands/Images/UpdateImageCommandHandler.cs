using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Images
{
    public class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand, ulong?>
    {
        private readonly IRepository<Image> _images;

        public UpdateImageCommandHandler(IRepository<Image> images)
        {
            _images = images;
        }

        public async Task<ulong?> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _images.GetAsync(request.ImageId);
            if (image is not null)
            {
                image.Title = request.Title;
                return await _images.UpdateAsync(image);
            }

            //TODO: Return error
            return null;
        }
    }
}
