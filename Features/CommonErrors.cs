using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features
{
    public static class CommonErrors
    {
        public static readonly Error ResourceNotFound =
            new Error(ErrorHelper.GetName(), "The resource could not be found");
    }
}
