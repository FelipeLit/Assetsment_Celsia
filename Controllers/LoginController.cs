using System.Security.Claims;
using assetsment_Celsia.Dtos;
using assetsment_Celsia.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace assetsment_Celsia.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public IActionResult Signin()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> CreateAdministrador(DtoAdmin admin)
        {
            if (!ModelState.IsValid)
            {
                return View(admin);
            }

            try
            {
                var result = await _loginRepository.CreateAdmin(admin);

                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Email already in use.");
                    // Asegúrate de que la acción 'Create' renderice la vista correcta
                    return View("Create", admin);
                }

                // Redireccionar a una vista de éxito o al listado de administradores
                return RedirectToAction("Signin", "Login");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the admin. Please try again later.");
                return View("Create", admin);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SigninUser(string email, string password)
        {
            var user = await _loginRepository.AuthenticateUser(email, password);
            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Autenticación fallida
                TempData["errorMessage"] = "User or password incorrect.";
                return RedirectToAction("Signin", "Login");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Signin", "Login");
        }
    }
}
