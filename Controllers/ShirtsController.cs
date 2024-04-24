using ApiDemo.Filters;
using ApiDemo.Models;
using ApiDemo.Repository;

using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controllers;

[ApiController]
[Route("shirts")]
public class ShirtsController : ControllerBase
{

    [HttpGet]
    public IActionResult GetShirts()
    {
        return Ok(ShirtRepository.GetShirts());
    }

    [HttpGet("{id}")]
    [Shirt_ValidateShirtIdFilter]
    public IActionResult GetShirtById(int id)
    {
        return Ok(ShirtRepository.GetShritById(id));

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
    [Shirt_ValidateCreateShirtFilter]
    public IActionResult CreateShirt([FromBody] Shirt shirt)
    {

        /* use action filter version above
        if (shirt is null) return BadRequest();
        var existingShirt = ShirtRepository.GetShritByProperties(shirt.Brand, shirt.Gender, shirt.Color, shirt.Size);
        // if we can find shirt with similar property => we don't want to create it
        if (existingShirt is not null) return BadRequest();
        */
        // else add shirt
        ShirtRepository.AddShirt(shirt);

        // return obj follows web api conventions
        // created status code + location header + json(obj created) 
        return CreatedAtAction(nameof(GetShirtById), new { id = shirt.ShirtId }, shirt);

    }


    [HttpPut("{id}")]
    [Shirt_ValidateShirtIdFilter] // validate id as well
    [Shirt_ValidateUpdateShirtFilter]
    [Shirt_HandleUpdateExceptionsFilter]
    public IActionResult UpdateShirt(int id, Shirt shirt)
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

        ShirtRepository.UpdateShirt(shirt);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Shirt_ValidateShirtIdFilter]
    public IActionResult DeleteShirt(int id)
    {
        var shirt = ShirtRepository.GetShritById(id);
        ShirtRepository.DeleteShirt(id);
        return Ok(shirt);
    }
}