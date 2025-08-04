﻿using System.Text;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Helpers;
using YssWebstoreApi.Persistance.Storage.Images;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Persistance.Storage.Packages;

namespace YssWebstoreApi.Persistance.Storage
{
    public class ProjectStorage : IProjectStorage
    {
        const string PackagesBucket = "packages";
        const string ImagesDirectory = "projects";
        const string IconFileName = "icon.png";

        private readonly IImageStorage _imageStorage;
        private readonly IStorage _storage;
        private readonly TimeProvider _timeProvider;

        public ProjectStorage(IImageStorage imageStorage, IStorage storage, TimeProvider timeProvider)
        {
            _imageStorage = imageStorage;
            _storage = storage;
            _timeProvider = timeProvider;
        }

        public async Task<Resource> UploadIcon(Guid projectId, IFormFile file)
        {
            string path = PathHelper.UnixCombine(
                ImagesDirectory,
                projectId.ToString(),
                IconFileName);

            string? url = await _imageStorage.Upload(path, file, ImageProperties.AvatarImage);

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            return new Resource
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = path,
                PublicUrl = url
            };
        }

        public async Task<Resource> UploadImage(Guid projectId, IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";

            string path = PathHelper.UnixCombine(
                ImagesDirectory,
                projectId.ToString(),
                fileName);

            string? url = await _imageStorage.Upload(path, file, "jpg");

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            return new Resource
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = path,
                PublicUrl = url
            };
        }

        public async Task<Resource> UploadPackage(Guid projectId, PackageInfo packageInfo, IFormFile file)
        {
            var fileNameBuilder = new StringBuilder()
                .Append(packageInfo.Name)
                .Append('-')
                .Append(packageInfo.Version)
                .Append('-')
                .Append(packageInfo.TargetOs.ToString().ToLowerInvariant())
                .Append(Path.GetExtension(file.FileName));

            string path = PathHelper.UnixCombine(projectId.ToString(), fileNameBuilder.ToString());
            string? url = await _storage.Upload(PackagesBucket, path, file.OpenReadStream());

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            return new Resource
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = path,
                PublicUrl = url
            };
        }

        public async Task<string?> GetPackageUrl(string path)
        {
            return await _storage.GetPrivateUrl(PackagesBucket, path, TimeSpan.FromSeconds(30));
        }

        public async Task DeletePackage(string path)
        {
            await _storage.Delete(PackagesBucket, path);
        }
    }
}
