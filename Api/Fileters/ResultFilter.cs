using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Extensions;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Fileters
{
    internal class ResultFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is Result result)
                {
                    if (result.Success)
                    {
                        if (result is ValueResult valueResult)
                        {
                            context.Result = new OkObjectResult(valueResult.GetValue());
                        }
                        else
                        {
                            context.Result = new OkResult();
                        }
                    }
                    else
                    {
                        var error = result.Error!;

                        var problemDetails = new ProblemDetails()
                        {
                            Type = "about:blank",
                            Status = error.HttpStatus,
                            Title = error.Code,
                            Detail = error.Message
                        };

                        context.Result = new ObjectResult(problemDetails)
                        {
                            StatusCode = problemDetails.Status
                        };
                    }
                }
            }
        }
    }
}
