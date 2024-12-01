using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Image;

namespace YssWebstoreApi.Features.Commands.Images
{
    public class CreateImageCommand : IRequest<ulong?>
    {
        public string? Title { get; set; }
        public required string FileName { get; set; }
        public required Stream Stream { get; set; }
        public required ulong AccountId { get; set; }

        public CreateImageCommand() { }

        [SetsRequiredMembers]
        public CreateImageCommand(ulong accountId, CreateImage createImageDTO)
        {
            Stream = createImageDTO.File.OpenReadStream();
            FileName = createImageDTO.File.FileName;
            Title = createImageDTO.Title;
            AccountId = accountId;
        }
    }
}
