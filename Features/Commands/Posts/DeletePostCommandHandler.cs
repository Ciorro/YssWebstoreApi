using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Posts
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ulong?>
    {
        private readonly IRepository<Post> _posts;
        private readonly IAttachmentRepository<Image> _images;

        public DeletePostCommandHandler(IRepository<Post> posts, IAttachmentRepository<Image> images)
        {
            _posts = posts;
            _images = images;
        }

        public async Task<ulong?> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _posts.GetAsync(request.PostId);
            if (post is not null)
            {
                var resultId = await _posts.DeleteAsync(request.PostId);
                
                if (post.ImageId.HasValue)
                {
                    await _images.DeleteAndDetachAsync(post.ImageId.Value);
                }

                return resultId;
            }

            return null;
        }
    }
}
