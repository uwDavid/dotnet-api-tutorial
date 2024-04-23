using ApiDemo.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters;

public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var shirtId = context.ActionArguments["id"] as int?;
        // this targets GetShirtById(int id)  

        if (shirtId.HasValue)
        {
            if (shirtId.Value <= 0)
            {
                context.ModelState.AddModelError("ShirtId", "ShirtId is invalid");
                // This model error => will show in problem details' key-value pair 
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else if (!ShirtRepository.ShirtExists(shirtId.Value))
            {
                context.ModelState.AddModelError("ShirtId", "ShirtId doesn't exist");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
        }
    }
}