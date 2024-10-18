using YssWebstoreApi.Middlewares;

namespace YssWebstoreApi.Installers
{
    public static class MiddlewareInstaller
    {
        public static void UseVerification(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<VerificationMiddleware>();
        }
    }
}
