using System.Transactions;
using assetsment_Celsia.Data;
using assetsment_Celsia.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace assetsment_Celsia.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CelsiaContext _context;
        public TransactionRepository(CelsiaContext context)
        {
            _context = context;
        }

        public Task<IPagedList<Transaction>> GetTransactions(int page, int size)
        {
            throw new NotImplementedException();
        }

    }
}