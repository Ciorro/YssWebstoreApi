using System.Net;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects
{
    public static class ProjectErrors
    {
        public static readonly Error PinnedProjectLimit =
            new Error(ErrorHelper.GetName(), (int)HttpStatusCode.BadRequest, "Pinned projects limit reached");

    }
}
