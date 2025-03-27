using ApiOwn.ViewsModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiOwn.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extractApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "ApiKey não informada"
            };
            return;
        }

        if (!Configuration.ApiKey.Equals(extractApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 403,
                Content = "Acesso negado"
            };
            return;
        }
        
        await next();
    }
}