using ApiDemo.Data;
using ApiDemo.Repository;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters;

// Ensures that id is valid
public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
{
    private readonly ApplicationDbContext _db;

    public Shirt_ValidateShirtIdFilterAttribute(ApplicationDbContext db)
    {
        _db = db;
    }

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
            else
            {
                var shirt = _db.Shirts.Find(shirtId.Value);
                if (shirt is null)
                {
                    context.ModelState.AddModelError("ShirtId", "ShirtId doesn't exist");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    context.HttpContext.Items["shirt"] = shirt;
                    // store shirt obj in Http Context -> to pass shirt back to method 
                    // to avoid redundant DB calls
                }
            }
        }
    }
}