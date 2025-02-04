using MediatR;

namespace YssWebstoreApi.Features.Commands.Posts
{
    public class DeletePostCommand(ulong postId) : IRequest<bool>
    {
        public ulong PostId { get; } = postId;
    }
}
