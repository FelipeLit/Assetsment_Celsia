using assetsment_Celsia.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace assetsment_Celsia.Controllers
{
    public class ExcelController : Controller
    {
        private readonly IExcelRepository _dataImportService;

        public ExcelController(IExcelRepository dataImportService)
        {
            _dataImportService = dataImportService;
        }

        // Acción para manejar la carga del archivo
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                try
                {
                    await _dataImportService.ImportDataFromExcelAsync(excelFile);
                    ViewBag.Message = "Archivo importado con éxito.";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error al importar archivo: {ex.Message}";
                }
            }
            else
            {
                ModelState.AddModelError("", "Por favor, selecciona un archivo válido.");
            }

            return RedirectToAction("Index", "Home"); // Asegúrate de que esta vista exista
        }
    }
}