using MediatR;
using YssWebstoreApi.Helpers;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Images
{
    public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, ulong?>
    {
        private readonly IAttachmentRepository<Image> _images;

        public CreateImageCommandHandler(IAttachmentRepository<Image> images)
        {
            _images = images;
        }

        public async Task<ulong?> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            string path = PathHelper.GetRandomPathName("images", Path.GetExtension(request.FileName));

            var image = new Image
            {
                Title = request.Title,
                Path = path,
                AccountId = request.AccountId
            };

            return await _images.CreateAndAttachAsync(image, request.Stream);
        }
    }
}
