using System.Data.Common;

using ApiDemo.Data;
using ApiDemo.Filters;
using ApiDemo.Models;
using ApiDemo.Repository;

using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controllers;

[ApiController]
[Route("shirts")]
public class ShirtsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    // this constructor means that => it will require an ApplicationDbContext
    // then it will go through Services list => create an instance of ApplicationDbContext
    // thus, we will have an instance of ApplicationDbContext in our controller
    public ShirtsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetShirts()
    {
        return Ok(_db.Shirts.ToList());
    }

    [HttpGet("{id}")]
    // [Shirt_ValidateShirtIdFilter]
    [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))] // this allows DI to work in filter
    public IActionResult GetShirtById(int id)
    {
        // var shirt = _db.Shirts.Find(id);
        var shirt = HttpContext.Items["shirt"];
        // action filter calls db to validate FindById 
        // if found -> store shirt obj in Http Context
        // we use Http Context to obtain shirt to avoid Db call
        return Ok(shirt);

        // move validation to Action Filter
        /*
        if (id <= 0)
        {
            return BadRequest();
        }
        var shirt = ShirtRepository.GetShritById(id);
        if (shirt is null) return NotFound();
        return Ok(shirt);
        */
    }

    // Data via Route
    // [HttpGet("{id}/{color}")]
    // public string GetShirtById(int id, [FromRoute] string color)
    // {
    //     return $"Reading shirt {id}, color: {color}";
    // }

    // Data via Query String
    // shirts/9?color=blue
    // [HttpGet("{id}")]
    // public string GetShirtById(int id, [FromQuery] string color)
    // {
    //     return $"Reading shirt {id}, color: {color}";
    // }

    // Data via Header
    // shrits/9 - add header key: Color
    // [HttpGet("{id}")]
    // public string GetShirtById(int id, [FromHeader(Name = "Color")] string color)
    // {
    //     return $"Reading shirt {id}, color: {color}";
    // }

    // Data via Body-Form
    // [HttpPost]
    // public IActionResult CreateShirt([FromForm] Shirt shirt)

    [HttpPost]
    [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
    public IActionResult CreateShirt([FromBody] Shirt shirt)
    {

        /* use action filter version above
        if (shirt is null) return BadRequest();
        var existingShirt = ShirtRepository.GetShritByProperties(shirt.Brand, shirt.Gender, shirt.Color, shirt.Size);
        // if we can find shirt with similar property => we don't want to create it
        if (existingShirt is not null) return BadRequest();
        */
        // else add shirt
        _db.Shirts.Add(shirt);
        _db.SaveChanges();

        // return obj follows web api conventions
        // created status code + location header + json(obj created) 
        return CreatedAtAction(nameof(GetShirtById), new { id = shirt.ShirtId }, shirt);

    }


    [HttpPut("{id}")]
    [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
    [Shirt_ValidateUpdateShirtFilter]
    [TypeFilter(typeof(Shirt_HandleUpdateExceptionsFilterAttribute))]
    public IActionResult UpdateShirt(int id, [FromBody] Shirt shirt)
    {
        // validation done by filters
        // if (id != shirt.ShirtId) return BadRequest();
        // try
        // {
        //     ShirtRepository.UpdateShirt(shirt);
        // }
        // catch
        // {
        //     // in case if someone delete the shirt, before update 
        //     if (!ShirtRepository.ShirtExists(id))
        //     {
        //         return NotFound();
        //     }
        //     throw; // else throw error
        // }

        // ShirtRepository.UpdateShirt(shirt);

        // validate shirt id => already found the shirt => stored in http context
        var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
        // Console.WriteLine(shirtToUpdate);
        // The moment Shirt obj is accessed via DB Context => EF Core already tracks it
        // this is why we use FirstOrDefault() instead of Find()
        // Find() method looks at items in EF Core change log as well
        shirtToUpdate.Brand = shirt.Brand;
        shirtToUpdate.Price = shirt.Price;
        shirtToUpdate.Size = shirt.Size;
        shirtToUpdate.Color = shirt.Color;
        shirtToUpdate.Gender = shirt.Gender;

        _db.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
    public IActionResult DeleteShirt(int id)
    {
        // var shirt = ShirtRepository.GetShritById(id);
        // ShirtRepository.DeleteShirt(id);

        var shirtToDelete = HttpContext.Items["shirt"] as Shirt;

        _db.Shirts.Remove(shirtToDelete);
        // mark shirt as deleted in EF Core Change Tracker
        _db.SaveChanges();
        return Ok(shirtToDelete);
    }
}