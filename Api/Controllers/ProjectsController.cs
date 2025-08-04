using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Packages;
using YssWebstoreApi.Api.DTO.Projects;
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
        public async Task<IActionResult> SearchProjects(ProjectSearchRequest searchRequest)
        {
            Result<Page<ProjectSearchResult>> result = await _queryMediator.QueryAsync(
                new SearchProjectsQuery()
                {
                    SearchText = searchRequest.SearchQuery,
                    AccountName = searchRequest.Account,
                    Tags = new TagCollection(searchRequest.Tags ?? []),
                    PinnedOnly = searchRequest.PinnedOnly,
                    PageOptions = searchRequest.PageOptions,
                    SortOptions = searchRequest.SortOptions
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProjectBySlug(string slug)
        {
            Result<ProjectResponse> result = await _queryMediator.QueryAsync(
                new GetProjectBySlugQuery(slug));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateProject(CreateProjectRequest request)
        {
            Result<Guid> result = await _commandMediator.SendAsync(
                new CreateProjectCommand(User.GetAccountId(), request.Name, request.Description)
                {
                    Tags = new(request.Tags)
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpPost("{projectId:Guid}/icon"), Authorize]
        public async Task<IActionResult> UploadIcon(Guid projectId, IFormFile file)
        {
            Result<string> result = await _commandMediator.SendAsync(
                new UploadIconCommand(User.GetAccountId(), projectId, file));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpDelete("{projectId:Guid}/icon"), Authorize]
        public async Task<IActionResult> DeleteIcon(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteIconCommand(User.GetAccountId(), projectId));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("{projectId:Guid}/images"), Authorize]
        public async Task<IActionResult> UploadImage(Guid projectId, IFormFile file)
        {
            Result<string> result = await _commandMediator.SendAsync(
                new UploadImageCommand(User.GetAccountId(), projectId, file));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpDelete("{projectId:Guid}/images/{imageId:Guid}"), Authorize]
        public async Task<IActionResult> DeleteImage(Guid projectId, Guid imageId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteImageCommand(User.GetAccountId(), projectId, imageId));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPut("{projectId:Guid}/images/{imageId:Guid}/order"), Authorize]
        public async Task<IActionResult> ReorderImage(Guid projectId, Guid imageId, [FromBody] int newOrder)
        {
            Result result = await _commandMediator.SendAsync(
                new ReorderImageCommand(User.GetAccountId(), projectId, imageId, newOrder));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("{projectId:Guid}/packages"), Authorize]
        public async Task<IActionResult> UploadPackage(Guid projectId, UploadPackageRequest request)
        {
            Result<Guid> result = await _commandMediator.SendAsync(
                new UploadPackageCommand
                {
                    AccountId = User.GetAccountId(),
                    ProjectId = projectId,
                    Name = request.Name,
                    Version = request.Version,
                    File = request.File,
                    TargetOS = request.TargetOS
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpDelete("{projectId:Guid}/packages/{packageId:Guid}"), Authorize]
        public async Task<IActionResult> DeletePackage(Guid projectId, Guid packageId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeletePackageCommand(User.GetAccountId(), projectId, packageId));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPut("{projectId:Guid}/packages/{packageId:Guid}"), Authorize]
        public async Task<IActionResult> UpdatePackage(Guid projectId, Guid packageId, UpdatePackageRequest request)
        {
            Result result = await _commandMediator.SendAsync(
                new UpdatePackageCommand
                {
                    AccountId = User.GetAccountId(),
                    ProjectId = projectId,
                    PackageId = packageId,
                    Name = request.Name
                });

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("{projectId:Guid}/packages")]
        public async Task<IActionResult> GetProjectPackages(Guid projectId)
        {
            Result<IList<PackageResponse>> result = await _queryMediator.QueryAsync(
                new GetProjectPackagesQuery(projectId));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpGet("{projectId:Guid}/packages/{packageId:Guid}/download")]
        public async Task<IActionResult> DownloadPackage(Guid projectId, Guid packageId)
        {
            Guid? accountId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                accountId = User.GetAccountId();
            }

            Result<string> result = await _commandMediator.SendAsync(
                new DownloadPackageCommand(projectId, packageId)
                {
                    AccountId = accountId
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpPost("{projectId:Guid}/pin"), Authorize]
        public async Task<IActionResult> PinProject(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new SetProjectPinnedCommand(User.GetAccountId(), projectId, true));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{projectId:Guid}/pin"), Authorize]
        public async Task<IActionResult> UnpinProject(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new SetProjectPinnedCommand(User.GetAccountId(), projectId, false));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
