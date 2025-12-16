using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Packages;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Resources;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Entities.Tags;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Posts.Commands;
using YssWebstoreApi.Features.Projects.Commands;
using YssWebstoreApi.Features.Projects.Queries;
using YssWebstoreApi.Features.Search.Queries;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IQueryMediator _queryMediator;
        private readonly ICommandMediator _commandMediator;

        public ProjectsController(IQueryMediator queryMediator, ICommandMediator commandMediator)
        {
            _queryMediator = queryMediator;
            _commandMediator = commandMediator;
        }

        [HttpGet]
        public async Task<ValueResult<Page<ProjectSearchResult>>> SearchProjects(ProjectSearchRequest searchRequest)
        {
            ValueResult<Page<ProjectSearchResult>> result = await _queryMediator.QueryAsync(
                new SearchProjectsQuery()
                {
                    SearchText = searchRequest.SearchQuery,
                    AccountName = searchRequest.Account,
                    Tags = new TagCollection(searchRequest.Tags ?? []),
                    PinnedOnly = searchRequest.PinnedOnly,
                    PageOptions = searchRequest.PageOptions,
                    SortOptions = searchRequest.SortOptions
                });

            return result;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ValueResult<ProjectResponse>> GetProjectById(Guid id)
        {
            ValueResult<ProjectResponse> result = await _queryMediator.QueryAsync(
                new GetProjectByIdQuery(id));

            return result;
        }

        [HttpGet("{slug}")]
        public async Task<ValueResult<ProjectResponse>> GetProjectBySlug(string slug)
        {
            ValueResult<ProjectResponse> result = await _queryMediator.QueryAsync(
                new GetProjectBySlugQuery(slug));

            return result;
        }

        [HttpPost("games"), Authorize]
        public Task<Result> CreateGameProject(CreateGameRequest request)
            => CreateProject(request);

        [HttpPost("tools"), Authorize]
        public Task<Result> CreateToolProject(CreateToolRequest request)
            => CreateProject(request);

        [HttpPost("assets"), Authorize]
        public Task<Result> CreateAssetProject(CreateAssetRequest request)
            => CreateProject(request);

        private async Task<Result> CreateProject(CreateProjectRequest request)
        {
            Result result = await _commandMediator.SendAsync(
                new CreateProjectCommand(User.GetAccountId(), request.Name, request.Description)
                {
                    Tags = request.GetTags()
                });

            return result;
        }

        [HttpPut("games/{projectId:Guid}"), Authorize]
        public Task<Result> UpdateGameProject(UpdateGameRequest request, Guid projectId)
            => UpdateProject(request, projectId);

        [HttpPut("tools/{projectId:Guid}"), Authorize]
        public Task<Result> UpdateToolProject(UpdateToolRequest request, Guid projectId)
            => UpdateProject(request, projectId);

        [HttpPut("assets/{projectId:Guid}"), Authorize]
        public Task<Result> UpdateAssetProject(UpdateAssetRequest request, Guid projectId)
            => UpdateProject(request, projectId);

        private async Task<Result> UpdateProject(UpdateProjectRequest request, Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new UpdateProjectCommand(User.GetAccountId(), projectId, request.Name, request.Description)
                {
                    Tags = request.GetTags()
                });

            return result;
        }

        [HttpDelete("{projectId:Guid}"), Authorize]
        public async Task<Result> DeleteProject(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteProjectCommand(User.GetAccountId(), projectId));

            return result;
        }

        [HttpPost("{projectId:Guid}/icon"), Authorize]
        public async Task<ValueResult<string>> UploadIcon(Guid projectId, IFormFile file)
        {
            ValueResult<string> result = await _commandMediator.SendAsync(
                new UploadIconCommand(User.GetAccountId(), projectId, file));

            return result;
        }

        [HttpDelete("{projectId:Guid}/icon"), Authorize]
        public async Task<Result> DeleteIcon(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteIconCommand(User.GetAccountId(), projectId));

            return result;
        }

        [HttpPost("{projectId:Guid}/banner"), Authorize]
        public async Task<ValueResult<string>> UploadBanner(Guid projectId, IFormFile file)
        {
            ValueResult<string> result = await _commandMediator.SendAsync(
                new UploadBannerCommand(User.GetAccountId(), projectId, file));

            return result;
        }

        [HttpDelete("{projectId:Guid}/banner"), Authorize]
        public async Task<Result> DeleteBanner(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteBannerCommand(User.GetAccountId(), projectId));

            return result;
        }

        [HttpGet("{projectId:Guid}/images")]
        public async Task<ValueResult<IList<ResourceResponse>>> GetProjectImages(Guid projectId)
        {
            ValueResult<IList<ResourceResponse>> result = await _queryMediator.QueryAsync(
                new GetProjectImagesQuery(projectId));

            return result;
        }

        [HttpPost("{projectId:Guid}/images"), Authorize]
        public async Task<ValueResult<string>> UploadImage(Guid projectId, IFormFile file)
        {
            ValueResult<string> result = await _commandMediator.SendAsync(
                new UploadImageCommand(User.GetAccountId(), projectId, file));

            return result;
        }

        [HttpDelete("{projectId:Guid}/images/{imageId:Guid}"), Authorize]
        public async Task<Result> DeleteImage(Guid projectId, Guid imageId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteImageCommand(User.GetAccountId(), projectId, imageId));

            return result;
        }

        [HttpPut("{projectId:Guid}/images/{imageId:Guid}/order"), Authorize]
        public async Task<Result> ReorderImage(Guid projectId, Guid imageId, [FromBody] int newOrder)
        {
            Result result = await _commandMediator.SendAsync(
                new ReorderImageCommand(User.GetAccountId(), projectId, imageId, newOrder));

            return result;
        }

        [HttpPost("{projectId:Guid}/packages"), Authorize]
        [RequestSizeLimit(104_857_600)]
        [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
        public async Task<Result> UploadPackage(Guid projectId, UploadPackageRequest request)
        {
            Result result = await _commandMediator.SendAsync(
                new UploadPackageCommand
                {
                    AccountId = User.GetAccountId(),
                    ProjectId = projectId,
                    Name = request.Name,
                    Version = request.Version,
                    File = request.File,
                    TargetOS = request.TargetOS
                });

            return result;
        }

        [HttpDelete("{projectId:Guid}/packages/{packageId:Guid}"), Authorize]
        public async Task<Result> DeletePackage(Guid projectId, Guid packageId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeletePackageCommand(User.GetAccountId(), projectId, packageId));

            return result;
        }

        [HttpPut("{projectId:Guid}/packages/{packageId:Guid}"), Authorize]
        public async Task<Result> UpdatePackage(Guid projectId, Guid packageId, UpdatePackageRequest request)
        {
            Result result = await _commandMediator.SendAsync(
                new UpdatePackageCommand
                {
                    AccountId = User.GetAccountId(),
                    ProjectId = projectId,
                    PackageId = packageId,
                    Name = request.Name
                });

            return result;
        }

        [HttpGet("{projectId:Guid}/packages")]
        public async Task<ValueResult<IList<PackageResponse>>> GetProjectPackages(Guid projectId)
        {
            ValueResult<IList<PackageResponse>> result = await _queryMediator.QueryAsync(
                new GetProjectPackagesQuery(projectId));

            return result;
        }

        [HttpGet("{projectId:Guid}/packages/{packageId:Guid}/download")]
        public async Task<ValueResult<string>> DownloadPackage(Guid projectId, Guid packageId)
        {
            Guid? accountId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                accountId = User.GetAccountId();
            }

            ValueResult<string> result = await _commandMediator.SendAsync(
                new DownloadPackageCommand(projectId, packageId)
                {
                    AccountId = accountId
                });

            return result;
        }

        [HttpPost("{projectId:Guid}/pin"), Authorize]
        public async Task<Result> PinProject(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new SetProjectPinnedCommand(User.GetAccountId(), projectId, true));

            return result;
        }

        [HttpDelete("{projectId:Guid}/pin"), Authorize]
        public async Task<Result> UnpinProject(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new SetProjectPinnedCommand(User.GetAccountId(), projectId, false));

            return result;
        }

        [HttpGet("{projectId:Guid}/stats")]
        public async Task<ValueResult<StatisticsResponse>> GetProjectStats(Guid projectId, StatisticsRequest? request)
        {
            ValueResult<StatisticsResponse> result = await _queryMediator.QueryAsync(
                new GetStatsByProjectIdQuery(projectId)
                {
                    RangeStart = request?.RangeStart ?? DateOnly.MinValue,
                    RangeEnd = request?.RangeEnd ?? DateOnly.MaxValue
                });

            return result;
        }
    }
}
