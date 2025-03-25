using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiOwn.Extensions;

public static class ModelStateExtensions
{
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        var result = new List<string>();
        foreach (var item in modelState.Values)
            result.AddRange(item.Errors.Select(e => e.ErrorMessage));
        return result;
    }
}