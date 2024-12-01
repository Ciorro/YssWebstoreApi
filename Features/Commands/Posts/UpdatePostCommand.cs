using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Post;

namespace YssWebstoreApi.Features.Commands.Posts
{
    public class UpdatePostCommand : IRequest<ulong?>
    {
        public required ulong PostId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string? CoverUrl { get; set; }

        public UpdatePostCommand() { }

        [SetsRequiredMembers]
        public UpdatePostCommand(ulong postId, UpdatePost updatePost)
        {
            Title = updatePost.Title;
            Content = updatePost.Content;
        }
    }
}
