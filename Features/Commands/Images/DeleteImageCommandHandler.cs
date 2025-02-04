using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Images
{
    public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand, bool>
    {
        private readonly IAttachmentRepository<Image> _images;

        public DeleteImageCommandHandler(IAttachmentRepository<Image> images)
        {
            _images = images;
        }

        public async Task<bool> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            return await _images.DeleteAndDetachAsync(request.ImageId);
        }
    }
}
