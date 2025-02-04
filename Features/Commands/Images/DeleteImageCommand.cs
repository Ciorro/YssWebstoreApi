using MediatR;

namespace YssWebstoreApi.Features.Commands.Images
{
    public class DeleteImageCommand(ulong imageId) : IRequest<bool>
    {
        public ulong ImageId { get; } = imageId;
    }
}
