using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Models.DTOs.Posts;

namespace YssWebstoreApi.Features.Posts.Queries
{
    public class GetPostByIdQuery(Guid postId) : IQuery<Result<PostResponse>>
    {
        public Guid PostId { get; } = postId;
    }
}
