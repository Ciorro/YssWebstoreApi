using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UploadImageCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }
        public IFormFile Image { get; }

        public UploadImageCommand(Guid accountId, Guid projectId, IFormFile image)
        {
            AccountId = accountId;
            ProjectId = projectId;
            Image = image;
        }
    }
}
