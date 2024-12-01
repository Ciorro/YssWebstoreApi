using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Posts
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, ulong?>
    {
        private readonly IRepository<Post> _posts;

        public UpdatePostCommandHandler(IRepository<Post> posts)
        {
            _posts = posts;
        }

        public async Task<ulong?> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _posts.GetAsync(request.PostId);
            if (post is not null)
            {
                post.Title = request.Title;
                post.Content = request.Content;

                return await _posts.UpdateAsync(post);
            }

            //TODO: return error
            return null;
        }
    }
}
