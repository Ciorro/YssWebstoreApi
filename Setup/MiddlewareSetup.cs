using YssWebstoreApi.Api.Middlewares;

namespace YssWebstoreApi.Installers
{
    public static class MiddlewareSetup
    {
        public static void UseVerification(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<VerificationMiddleware>();
        }
    }
}
