using ApiDemo.Repository;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters;

// Exception filter for Update()
public class Shirt_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
{

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        var strShirtId = context.RouteData.Values["id"] as string;
        // We use .ShirtExists(id) 
        // note we use context.RouteData, and id is string
        if (int.TryParse(strShirtId, out int shirtId))
        {
            if (!ShirtRepository.ShirtExists(shirtId))
            {
                context.ModelState.AddModelError("ShirtId", "Shirt doesn't exist anymore");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
        }
    }
}