using ApiDemo.Models;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters.ActionFilters.V2;

public class Shirt_EnsureDescriptionIsPresentFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var shirt = context.ActionArguments["shirt"] as Shirt;
        if (shirt != null && !shirt.ValidateDescription())
        {
            // short-circuit filter pipeline => then short-circuit the middleware pipeline
            context.ModelState.AddModelError("Shirt", "Shirt description is required.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}