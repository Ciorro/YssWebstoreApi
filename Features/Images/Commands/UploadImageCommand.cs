using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Models.DTOs.Resources;

namespace YssWebstoreApi.Features.Images.Commands
{
    public class UploadImageCommand : ICommand<Result<ResourceResponse>>
    {
        public Guid AccountId { get; init; }
        public IFormFile File { get; init; }

        public UploadImageCommand(Guid accountId, IFormFile file)
        {
            AccountId = accountId;
            File = file;
        }
    }
}
