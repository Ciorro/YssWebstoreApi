using MediatR;

namespace YssWebstoreApi.Features.Commands.Images
{
    public class DeleteImageCommand(ulong imageId) : IRequest<ulong?>
    {
        public ulong ImageId { get; } = imageId;
    }
}
