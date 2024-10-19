using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Packages
{
    public class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, ulong?>
    {
        private readonly IRepository<Package> _packages;
        private readonly HttpClient _httpClient;

        public CreatePackageCommandHandler(IRepository<Package> packages, IHttpClientFactory httpFactory)
        {
            _packages = packages;
            _httpClient = httpFactory.CreateClient();
        }

        public async Task<ulong?> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            var createdPackage = new Package
            {
                ProductId = request.ProductId,
                Name = request.Name,
                Version = request.Version,
                DownloadUrl = request.DownloadUrl,
                TargetOs = request.TargetOS,
                FileSize = 0
            };

            using (var fileSizeRequest = new HttpRequestMessage(HttpMethod.Head, createdPackage.DownloadUrl))
            {
                var fileSizeResponse = await _httpClient.SendAsync(fileSizeRequest);
                if (fileSizeResponse.IsSuccessStatusCode)
                {
                    createdPackage.FileSize = (ulong)(fileSizeResponse.Content.Headers.ContentLength ?? 0);
                }
            }

            return await _packages.CreateAsync(createdPackage);
        }
    }
}
