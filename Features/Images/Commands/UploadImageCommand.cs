using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Api.DTO.Resources;
using YssWebstoreApi.Utils;

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
