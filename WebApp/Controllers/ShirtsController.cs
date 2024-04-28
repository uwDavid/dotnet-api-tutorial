using Microsoft.AspNetCore.Mvc;

using WebApp.Data;
using WebApp.Models;

using WebApp.Repository;

namespace WebApp.Controllers;

public class ShirtsController : Controller
{
    private readonly IWebApiExecutor _webApiExecutor;


    public ShirtsController(IWebApiExecutor webApiExecutor)
    {
        _webApiExecutor = webApiExecutor;
    }


    public async Task<IActionResult> Index()
    {
        return View(await _webApiExecutor.InvokeGet<List<Shirt>>("shirts"));
    }

    public IActionResult CreateShirt()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateShirt(Shirt shirt)
    {
        if (ModelState.IsValid)
        {
            var response = await _webApiExecutor.InvokePost("shirts", shirt);
            if (response is not null)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View(shirt);
    }

    // defaults to HTTPGET
    // endpoint - /shirts/UpdateShirt?shirtId=2
    public async Task<IActionResult> UpdateShirt(int shirtId)
    {
        var shirt = await _webApiExecutor.InvokeGet<Shirt>($"shirts/{shirtId}");
        if (shirt is not null)
        {
            return View(shirt);
        }

        return NotFound();
    }

    [HttpPost]  //mvc control action method, rather than api. Form only allows POST or GET
    public async Task<IActionResult> UpdateShirt(Shirt shirt)
    {
        if (ModelState.IsValid)
        {
            // invoke the API PUT method
            await _webApiExecutor.InvokePut($"shirts/{shirt.ShirtId}", shirt);
            // note: shirtId comes from hidden form field
            return RedirectToAction(nameof(Index));
        }
        return View(shirt);
        // if model is not valid => return to same page
    }

    public async Task<IActionResult> DeleteShirt(int shirtId)
    {
        await _webApiExecutor.InvokeDelete($"shirts/{shirtId}");
        return RedirectToAction(nameof(Index));
    }
}