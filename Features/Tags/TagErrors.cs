using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Tags
{
    public static class TagErrors
    {
        public static readonly Error InvalidTagFormat =
            new Error(ErrorHelper.GetName(), "The tag format was invalid.");

        public static readonly Error InvalidGroupName =
            new Error(ErrorHelper.GetName(), "The tag group name was invalid.");
    }
}
