namespace YssWebstoreApi.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static void UseVerification(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<VerificationMiddleware>();
        }
    }
}
