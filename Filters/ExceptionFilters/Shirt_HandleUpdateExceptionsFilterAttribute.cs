using ApiDemo.Data;
using ApiDemo.Repository;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters;

// Exception filter for Update()
// When shirt is deleted before update is commited to database.
public class Shirt_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
{
    private readonly ApplicationDbContext _db;


    public Shirt_HandleUpdateExceptionsFilterAttribute(ApplicationDbContext db)
    {
        _db = db;
    }


    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);

        var strShirtId = context.RouteData.Values["id"] as string;
        // We use .ShirtExists(id) 
        // note we use context.RouteData, and id is string
        if (int.TryParse(strShirtId, out int shirtId))
        {
            var shirt = _db.Shirts.FirstOrDefault(x => x.ShirtId == shirtId);
            //if (!ShirtRepository.ShirtExists(shirtId))
            if (shirt is null)
            // The moment Shirt obj is accessed via DB Context => EF Core already tracks it
            // this is why we use FirstOrDefault() instead of Find()
            // b/c Find() looks inside EF Core change record as well
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