using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using assetsment_Celsia.Models;
using assetsment_Celsia.Interfaces;

namespace assetsment_Celsia.Controllers;

public class TransactionsController : Controller
{
    private readonly ITransactionRepository _repository;
    public TransactionsController(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {

        return View();
    }


}