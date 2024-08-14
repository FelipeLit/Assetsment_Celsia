using System.Transactions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using X.PagedList;

namespace assetsment_Celsia.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IPagedList<Transaction>> GetTransactions(int page, int size);
    }
}