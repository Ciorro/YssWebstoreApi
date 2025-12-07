using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities.Tags;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class CreateProjectCommand : ICommand<ValueResult<Guid>>
    {
        public Guid AccountId { get; }
        public string Name { get; }
        public string Description { get; }
        public TagCollection Tags { get; init; } = [];

        public CreateProjectCommand(Guid accountId, string name, string description)
        {
            AccountId = accountId;
            Name = name;
            Description = description;
        }
    }
}
