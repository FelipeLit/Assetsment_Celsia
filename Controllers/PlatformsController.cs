using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using assetsment_Celsia.Models;

namespace assetsment_Celsia.Controllers;

public class PlatformsController : Controller
{

    public IActionResult Index()
    {
        return View();
    }


}