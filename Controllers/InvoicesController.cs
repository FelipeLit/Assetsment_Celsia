using assetsment_Celsia.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace assetsment_Celsia.Controllers;

public class InvoicesController : Controller
{
    private readonly IInvoicesRepository _repository;
    public InvoicesController(IInvoicesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }


}