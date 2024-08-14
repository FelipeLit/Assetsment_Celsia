using assetsment_Celsia.Models;
using X.PagedList;

namespace assetsment_Celsia.Interfaces
{
    public interface IInvoicesRepository
    {
        Task<IEnumerable<Invoice>> GetInvoice(int page, int size);
    }
}