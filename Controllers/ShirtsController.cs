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
    public string GetShirts()
    {
        return "Reading all the shirts";
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

    [HttpPost]
    public string CreateShirt([FromForm] Shirt shirt)
    {
        return $"creating a shirt";
    }


    [HttpPut("{id}")]
    public string UpdateShirt(int id)
    {
        return $"Updating shirt: {id}";
    }

    [HttpDelete("{id}")]
    public string DeleteShirt(int id)
    {
        return $"Deleting shirt: {id}";
    }
}