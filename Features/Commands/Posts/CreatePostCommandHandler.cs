using MediatR;
using YssWebstoreApi.Helpers;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Posts
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ulong?>
    {
        private readonly IRepository<Post> _posts;
        private readonly IAttachmentRepository<Image> _images;

        public CreatePostCommandHandler(IRepository<Post> posts, IAttachmentRepository<Image> images)
        {
            _posts = posts;
            _images = images;
        }

        public async Task<ulong?> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            ulong? imageId = null;

            if (request.Cover is not null)
            {
                var image = new Image
                {
                    AccountId = request.AccountId,
                    Path = PathHelper.GetRandomPathName("images", Path.GetExtension(request.Cover.FileName))
                };

                imageId = await _images.CreateAndAttachAsync(image, request.Cover.OpenReadStream());
            }

            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                AccountId = request.AccountId,
                ProductId = request.ProductId,
                ImageId = imageId
            }; 

            return await _posts.CreateAsync(post);
        }
    }
}
