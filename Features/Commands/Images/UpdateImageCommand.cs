using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Image;

namespace YssWebstoreApi.Features.Commands.Images
{
    public class UpdateImageCommand : IRequest<ulong?>
    {
        public required ulong ImageId { get; set; }
        public string? Title { get; set; }

        public UpdateImageCommand() { }

        [SetsRequiredMembers]
        public UpdateImageCommand(ulong imageId)
        {
            ImageId = imageId;
        }

        [SetsRequiredMembers]
        public UpdateImageCommand(ulong imageId, UpdateImage updateImageDTO)
            : this(imageId)
        {
            Title = updateImageDTO.Title;
        }
    }
}
