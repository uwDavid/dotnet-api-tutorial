using ApiDemo.Data;
using ApiDemo.Models;
using ApiDemo.Repository;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters;

public class Shirt_ValidateCreateShirtFilterAttribute : ActionFilterAttribute
{
    private readonly ApplicationDbContext _db;


    public Shirt_ValidateCreateShirtFilterAttribute(ApplicationDbContext db)
    {
        _db = db;

    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var shirt = context.ActionArguments["shirt"] as Shirt;
        //note that attribute applies to method CreateShirt(Shirt shirt)

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
            // var existingShirt = ShirtRepository.GetShritByProperties(shirt.Brand, shirt.Gender, shirt.Color, shirt.Size);

            var existingShirt = _db.Shirts.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(shirt.Brand) &&
                !string.IsNullOrWhiteSpace(x.Brand) &&
                x.Brand.ToLower() == shirt.Brand.ToLower() &&

                !string.IsNullOrWhiteSpace(shirt.Gender) &&
                !string.IsNullOrWhiteSpace(x.Gender) &&
                x.Gender.ToLower() == shirt.Gender.ToLower() &&

                !string.IsNullOrWhiteSpace(shirt.Color) &&
                !string.IsNullOrWhiteSpace(x.Color) &&
                x.Color.ToLower() == shirt.Color.ToLower() &&

                shirt.Size.HasValue &&
                x.Size.HasValue &&
                shirt.Size.Value == x.Size.Value
            );


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