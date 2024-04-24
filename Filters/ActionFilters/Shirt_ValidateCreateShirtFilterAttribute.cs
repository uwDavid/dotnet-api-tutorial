using ApiDemo.Models;
using ApiDemo.Repository;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters;

public class Shirt_ValidateCreateShirtFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var shirt = context.ActionArguments["shirt"] as Shirt;
        //note that CreateShirt(Shirt shirt)

        if (shirt is null)
        {
            context.ModelState.AddModelError("Shirt", "Shirt object is null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
            // short-circuits the method
        }
        else
        {
            var existingShirt = ShirtRepository.GetShritByProperties(shirt.Brand, shirt.Gender, shirt.Color, shirt.Size);
            if (existingShirt is not null)
            {
                context.ModelState.AddModelError("Shirt", "Shirt already exists");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}