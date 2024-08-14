using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using assetsment_Celsia.Models;
using assetsment_Celsia.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace assetsment_Celsia.Controllers;

public class ClientesController : Controller
{
    private readonly IClientRepository _repository;
    public ClientesController(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return NotFound();
        }

        var client = await _repository.GetClients(pageNumber, pageSize);

        if (client.PageNumber != 1 && pageNumber > client.PageCount)
        {
            return NotFound();
        }

        ViewBag.Clientes = client;
        return View(client);
    }
    public async Task<IActionResult> CreateUser()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateUser(Client client)
    {
        Console.WriteLine("Entering Create method.");

        try
        {

            await _repository.AddClient(client);
            return RedirectToAction("Index", "Clientes");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Email", ex.Message);
            return View(client);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred: " + ex.Message);
            return View(client);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var client = await _repository.GetClientByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }
    [HttpPost]
    public async Task<IActionResult> EditUser(Client client)
    {

        try
        {
            await _repository.UpdateClientAsync(client);
            return RedirectToAction("Index", "Clientes");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _repository.GetClientByIdAsync(client.Id) == null)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var client = await _repository.GetClientByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

}
