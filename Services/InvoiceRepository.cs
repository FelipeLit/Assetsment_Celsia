using assetsment_Celsia.Data;
using assetsment_Celsia.Interfaces;
using assetsment_Celsia.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace assetsment_Celsia.Services
{
    public class InvoiceRepository : IInvoicesRepository
    {
        private readonly CelsiaContext _context;
        public InvoiceRepository(CelsiaContext context)
        {
            _context = context;
        }

        public Task AddInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task<Client> GetInvoiceByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateInvoiceAsync(Invoice incoice)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Invoice>> GetInvoice(int page, int size)
        {
            var pageNumber = page > 0 ? page : 1;
            var skip = (pageNumber - 1) * size;

            var invoices = await _context.Invoices
                .Skip(skip)
                .Take(size)
                .ToListAsync();

            return invoices;
        }
    }
}