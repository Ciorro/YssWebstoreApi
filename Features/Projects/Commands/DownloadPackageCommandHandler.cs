using Dapper;
using LiteBus.Commands.Abstractions;
using System.Data;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DownloadPackageCommandHandler
        : ICommandHandler<DownloadPackageCommand, ValueResult<string>>
    {
        private readonly IDbConnection _db;
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectStorage _projectStorage;
        private readonly TimeProvider _timeProvider;

        public DownloadPackageCommandHandler(
            IDbConnection dbConnection, IRepository<Project> projectRepository, IProjectStorage projectStorage, TimeProvider timeProvider)
        {
            _db = dbConnection;
            _projectRepository = projectRepository;
            _projectStorage = projectStorage;
            _timeProvider = timeProvider;
        }

        public async Task<ValueResult<string>> HandleAsync(DownloadPackageCommand message, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(message.ProjectId);
            var package = project?.Packages.FirstOrDefault(x => x.Id == message.PackageId);

            if (package is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            string downloadUrl = await _projectStorage.GetPackageUrl(package.Path);

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            await _db.ExecuteAsync(
                """
                INSERT INTO Downloads (
                    Id,
                    CreatedAt,
                    UpdatedAt,
                    ProjectId,
                    AccountId
                ) VALUES (
                    @Id,
                    @CreatedAt,
                    @UpdatedAt,
                    @ProjectId,
                    @AccountId
                ) ON CONFLICT (ProjectId, AccountId) DO UPDATE SET UpdatedAt = @UpdatedAt;
                """,
                new
                {
                    Id = id,
                    CreatedAt = creationTime,
                    UpdatedAt = creationTime,
                    ProjectId = message.ProjectId,
                    AccountId = message.AccountId ?? null
                });

            return downloadUrl;
        }
    }
}
