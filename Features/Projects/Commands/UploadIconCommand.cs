using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UploadIconCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }
        public IFormFile Icon { get; }

        public UploadIconCommand(Guid accountId, Guid projectId, IFormFile icon)
        {
            AccountId = accountId;
            ProjectId = projectId;
            Icon = icon;
        }
    }
}
