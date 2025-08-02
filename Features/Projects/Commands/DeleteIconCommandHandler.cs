using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteIconCommandHandler
        : ICommandHandler<DeleteIconCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IStorage _storage;

        public DeleteIconCommandHandler(IRepository<Project> projectRepository, IStorage storage)
        {
            _projectRepository = projectRepository;
            _storage = storage;
        }

        public async Task<Result> HandleAsync(DeleteIconCommand message, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(message.ProjectId);
            
            if (project is null || project.Icon is null)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (project.AccountId != message.AccountId)
            {
                return AuthErrors.AccessDenied;
            }

            await _storage.Delete(project.Icon.Path);
            project.Icon = null;
            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
