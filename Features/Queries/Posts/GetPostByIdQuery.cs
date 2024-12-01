using MediatR;
using YssWebstoreApi.Models.DTOs.Post;

namespace YssWebstoreApi.Features.Queries.Posts
{
    public class GetPostByIdQuery(ulong postId) : IRequest<PublicPost?>
    {
        public ulong PostId { get; } = postId;
    }
}
