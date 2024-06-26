using ApiDemo.Models;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters;

// ensure id provided in path == id provided in json body
public class Shirt_ValidateUpdateShirtFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var id = context.ActionArguments["id"] as int?;
        var shirt = context.ActionArguments["shirt"] as Shirt;

        if (id.HasValue && shirt is not null && id != shirt.ShirtId)
        {
            context.ModelState.AddModelError("ShirtId", "ShirtId is not the same as id");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}