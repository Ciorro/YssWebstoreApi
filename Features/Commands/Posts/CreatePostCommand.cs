using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Post;

namespace YssWebstoreApi.Features.Commands.Posts
{
    public class CreatePostCommand : IRequest<ulong?>
    {
        public required ulong AccountId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public IFormFile? Cover { get; set; }
        public ulong? ProductId { get; set; }

        public CreatePostCommand() { }

        [SetsRequiredMembers]
        public CreatePostCommand(ulong accountId, CreatePost createPost)
        {
            AccountId = accountId;
            Title = createPost.Title;
            Content = createPost.Content;
            Cover = createPost.Cover;
            ProductId = createPost.ProductId;
        }
    }
}
