using YssWebstoreApi.Api.Middlewares;

namespace YssWebstoreApi.Setup
{
    public static class MiddlewareSetup
    {
        public static void UseVerification(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<VerificationMiddleware>();
        }
    }
}
