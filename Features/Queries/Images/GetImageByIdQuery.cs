using MediatR;
using YssWebstoreApi.Models.DTOs.Image;

namespace YssWebstoreApi.Features.Queries.Images
{
    public class GetImageByIdQuery(ulong imageId) : IRequest<ImageInfo?>
    {
        public ulong ImageId { get; } = imageId;
    }
}
