using Microsoft.AspNetCore.Http.Features;
using YssWebstoreApi.Middlewares.Attributes;

namespace YssWebstoreApi.Middlewares
{
    public class VerificationMiddleware
    {
        private readonly RequestDelegate _next;

        public VerificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
                var attribute = endpoint?.Metadata.GetMetadata<AllowUnverifiedAttribute>();

                bool isVerified = httpContext.User.Claims.Any(x =>
                {
                    return x.Type == "is_verified" && x.Value == "True";
                });

                if (!isVerified && attribute is null)
                {
                    httpContext.Response.ContentType = "text/plain";
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Unverified user.");

                    return;
                }
            }

            await _next(httpContext);
        }
    }
}
