using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Posts
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly IRepository<Post> _posts;
        private readonly IAttachmentRepository<Image> _images;

        public DeletePostCommandHandler(IRepository<Post> posts, IAttachmentRepository<Image> images)
        {
            _posts = posts;
            _images = images;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _posts.GetAsync(request.PostId);
            if (post is not null)
            {
                var resultId = await _posts.DeleteAsync(request.PostId);
                
                if (post.ImageId.HasValue)
                {
                    return await _images.DeleteAndDetachAsync(post.ImageId.Value);
                }
            }

            return false;
        }
    }
}
