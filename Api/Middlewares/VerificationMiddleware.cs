using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.Middlewares.Attributes;

namespace YssWebstoreApi.Api.Middlewares
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
                    return x.Type == "is_verified" && x.Value.ToLower() == "true";
                });

                if (!isVerified && attribute is null)
                {
                    await Results.Problem(new ProblemDetails
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Title = "User not verified.",
                        Detail = "The user is not verified. Please verify your email."
                    }).ExecuteAsync(httpContext);

                    return;
                }
            }

            await _next(httpContext);
        }
    }
}
