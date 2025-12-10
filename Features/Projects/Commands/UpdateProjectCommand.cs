using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities.Tags;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UpdateProjectCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }
        public string Name { get; }
        public string Description { get; }
        public TagCollection Tags { get; init; } = [];

        public UpdateProjectCommand(Guid accountId, Guid projectId, string name, string description)
        {
            AccountId = accountId;
            ProjectId = projectId;
            Name = name;
            Description = description;
        }
    }
}
