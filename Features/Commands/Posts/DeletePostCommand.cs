using MediatR;

namespace YssWebstoreApi.Features.Commands.Posts
{
    public class DeletePostCommand(ulong postId) : IRequest<ulong?>
    {
        public ulong PostId { get; } = postId;
    }
}
