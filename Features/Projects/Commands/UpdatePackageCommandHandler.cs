using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UpdatePackageCommandHandler
         : ICommandHandler<UpdatePackageCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly TimeProvider _timeProvider;

        public UpdatePackageCommandHandler(IRepository<Project> projectRepository, TimeProvider timeProvider)
        {
            _projectRepository = projectRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UpdatePackageCommand message, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(message.ProjectId);

            if (project is null)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (project.AccountId != message.AccountId)
            {
                return AuthErrors.AccessDenied;
            }

            var package = project.Packages.SingleOrDefault(x => x.Id == message.PackageId);
            if (package is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            package.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            package.Name = message.Name;

            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
