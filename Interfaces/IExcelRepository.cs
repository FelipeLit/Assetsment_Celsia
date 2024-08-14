namespace assetsment_Celsia.Interfaces
{
    public interface IExcelRepository
    {
        Task ImportDataFromExcelAsync(IFormFile file);
    }
}